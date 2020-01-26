using Accord.Math;
using Accord.Math.Decompositions;
using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.DistributionsEvaluation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Base class for multivariate distribution settings
    /// </summary>
    public abstract class MultivariateDistributionSettings
    {
        protected readonly CholeskyDecomposition _chol = null;

        protected MultivariateDistributionSettings(double[,] input) : this(ProcessInput(input))
        {

        }

        protected MultivariateDistributionSettings(double[] means, double[,] covarianceMatrix) : this(new InternalParameters { Means = means, CovarianceMatrix = covarianceMatrix })
        {

        }

        protected static InternalParameters ProcessInput(double[,] input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            //https://stattrek.com/matrix-algebra/covariance-matrix 

            var rows = input.GetLength(0);

            double[] onesRow = Vector.Create<double>(rows, 1);

            var meansMatrix = onesRow.Outer(onesRow).Dot(input).Divide(rows);

            var deviationScores = input.Subtract(meansMatrix);

            var sumOfSquares = deviationScores.TransposeAndDot(deviationScores);

            var means = onesRow.Dot(input).Divide(rows);

            var covarianceMatrix = sumOfSquares.Divide(rows - 1);

            return new InternalParameters { Means = means, CovarianceMatrix = covarianceMatrix };
        }

        protected MultivariateDistributionSettings(InternalParameters parameters)
        {
            if (parameters.CovarianceMatrix == null)
                throw new ArgumentNullException(nameof(parameters.CovarianceMatrix));

            if (parameters.Means == null)
                throw new ArgumentNullException(nameof(parameters.Means));

            if (!parameters.CovarianceMatrix.IsSquare())
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.CovarianceMatrixMustBeSquare);

            if (!parameters.CovarianceMatrix.IsSymmetric())
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.CovarianceMatrixMustBeSymmetric);

            if (parameters.Means.Length != parameters.CovarianceMatrix.GetLength(0))
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.VectorOfMeansMustBeEqualToDimension);

            Dimension = parameters.Means.Length;

            CovarianceMatrix = parameters.CovarianceMatrix;
            Means = parameters.Means;

            _chol = new CholeskyDecomposition(CovarianceMatrix);

            if (!_chol.IsPositiveDefinite)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.CovarianceMatrixMustBePositiveDefined);
            }
        }

        /// <summary>
        /// Generates dimension sized vector of random varibles
        /// </summary>
        /// <param name="rnd">Ranoms source</param>
        /// <returns>Vector of random varibles</returns>
        public double[] GenerateRandom(Random rnd)
        {
            return GenerateRandomInternal(rnd);
        }

        protected abstract double[] GenerateRandomInternal(Random rnd);

        /// <summary>
        /// Vector of means
        /// </summary>
        public double[] Means
        {
            get;
        }

        /// <summary>
        /// Variance-covariance matrix
        /// </summary>
        public double[,] CovarianceMatrix
        {
            get;
        }

        /// <summary>
        /// Dimension of covariance matrix
        /// </summary>
        public int Dimension
        {
            get;
        }

        /// <summary>
        /// Returns bivariate pair based on current distribution settings if dimension = 2
        /// </summary>
        /// <param name="samples">Number of samples for verctor calculation</param>
        /// <returns>Bivariate pair</returns>
        public CorrelatedPair GetBivariatePair(int samples)
        {
            if (Dimension != 2)
                throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.ForCorrelationPairMultivariateDistributionMustBeTwoDimensional);

            double m1 = Means[0];
            double m2 = Means[1];

            double s1 = Math.Sqrt(CovarianceMatrix[0, 0]);
            double s2 = Math.Sqrt(CovarianceMatrix[1, 1]);
            double rho = CovarianceMatrix[0, 1] / (s1 * s2);

            return GetBivariatePairInternal(m1, m2, s1, s2, rho, samples);
        }

        protected abstract CorrelatedPair GetBivariatePairInternal(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples);

        protected class InternalParameters
        {
            public double[] Means
            {
                get;
                set;
            }

            public double[,] CovarianceMatrix
            {
                get;
                set;
            }
        }

    }

    /// <summary>
    /// Multivariate normal distribution that generates random variables vector and sum of normal distributions
    /// </summary>
    public class MultivariateNormalDistributionSettings : MultivariateDistributionSettings
    {
        static NormalDistribution _baseNormal = new NormalDistribution();

        /// <summary>
        /// Creates new instance of multivariate normal distribution by measured value
        /// </summary>
        /// <param name="input">Matrix of measured values</param>
        public MultivariateNormalDistributionSettings(double[,] input) : base(input)
        {

        }
        /// <summary>
        /// Creates new instance of multivariate normal distribution by means vector and covariance matrix
        /// </summary>
        /// <param name="means">Means vector</param>
        /// <param name="covarianceMatrix">Covariance matrix</param>
        public MultivariateNormalDistributionSettings(double[] means, double[,] covarianceMatrix) : base(means, covarianceMatrix)
        {
        }

        protected override double[] GenerateRandomInternal(Random rnd)
        {
            double[,] ltf = _chol.LeftTriangularFactor;
            double[] result = new double[Dimension];
            double[] u = Means;

            for (int i = 0; i < Dimension; i++)
            {
                result[i] = _baseNormal.Generate(rnd);
            }

            result = Matrix.Dot(ltf, result);

            result = Elementwise.Add(result, u);

            return result;
        }

        /// <summary>
        /// Generates new normal distribution settings as a sum of correlated normal distributions with coefficients <paramref name="coeffs"/> (c1*A+c2*B+c3*C etc.)
        /// </summary>
        /// <param name="coeffs">Cofficients vector that will be applied to the sum of normal distributions</param>
        /// <returns></returns>
        public NormalDistributionSettings GetUnivariateDistributionSettings(double[] coeffs)
        {
            if (coeffs == null)
                coeffs = Vector.Ones(Dimension);

            if (coeffs.Length != Dimension)
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.VectorOfCoeffitientsMustBeEqualToDimension);

            if (coeffs.All(x => x == 1))
                return new NormalDistributionSettings(Means.Sum(), Math.Sqrt(CovarianceMatrix.Sum()));

            double[,] ltf = _chol.LeftTriangularFactor;

            var weighted = Matrix.Diagonal(coeffs).Dot(ltf);
            var colSumPow = weighted.Transpose().Dot(Vector.Ones(Dimension)).Pow(2);

            double variance = colSumPow.Sum();

            double mean = Elementwise.Multiply(coeffs, Means).Sum();

            return new NormalDistributionSettings(mean, Math.Sqrt(variance));
        }

        protected override CorrelatedPair GetBivariatePairInternal(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples)
        {
            return new CorrelatedPair(new NormalDistributionSettings(mean1, sigma1).GetDistribution(samples), new NormalDistributionSettings(mean2, sigma2).GetDistribution(samples), rho);
        }
    }

    /// <summary>
    /// Multivariate t distribution settings that generates random variables vector
    /// </summary>
    public class MultivariateTDistributionSettings : MultivariateDistributionSettings
    {
        static NormalDistribution _baseNormal = new NormalDistribution();
        readonly GammaDistribution _baseGamma;
        readonly ChiSquareDistribution _baseChi;

        /// <summary>
        /// Creates new instance of multivariate t distribution by measured value with degrees of freedom N-1
        /// </summary>
        /// <param name="input">Matrix of measured values</param>
        public MultivariateTDistributionSettings(double[,] input) : this(input, input.GetLength(1) - 1)
        {

        }


        /// <summary>
        /// Creates new instance of multivariate t distribution by measured value with degrees of freedom N-1
        /// </summary>
        /// <param name="input">Matrix of measured values</param>
        public MultivariateTDistributionSettings(double[,] input, double degreesOfFreedom) : base(input)
        {
            DegreesOfFreedom = degreesOfFreedom;
            _baseGamma = new GammaDistribution(2.0, 0.5 * degreesOfFreedom);
            _baseChi = new ChiSquareDistribution((int)degreesOfFreedom);
        }

        /// <summary>
        /// Creates new instance of multivariate t distribution by means vector, covariance matrix and degrees of freedom
        /// </summary>
        /// <param name="means">Means vector</param>
        /// <param name="covarianceMatrix">Covariance matrix</param>
        /// <param name="degreesOfFreedom">Degrees of freedom</param>
        public MultivariateTDistributionSettings(double[] means, double[,] covarianceMatrix, double degreesOfFreedom) : base(means, covarianceMatrix)
        {
            DegreesOfFreedom = degreesOfFreedom;
            _baseGamma = new GammaDistribution(2.0, 0.5 * degreesOfFreedom);
            _baseChi = new ChiSquareDistribution((int)degreesOfFreedom);
        }


        /// <summary>
        /// Degrees of freedom applied to every dimesion
        /// </summary>
        public double DegreesOfFreedom
        {
            get;
        }

        protected override double[] GenerateRandomInternal(Random rnd)
        {
            double[,] ltf = _chol.LeftTriangularFactor;
            double[] result = new double[Dimension];
            double[] u = Means;

            for (int i = 0; i < Dimension; i++)
            {
                result[i] = _baseNormal.Generate(rnd);
                //result[i] = _baseNormal.Generate(rnd) / Math.Sqrt(_baseGamma.Generate(rnd) / DegreesOfFreedom);
            }

            result = Matrix.Dot(ltf, result);

            double[] gammaSqrt = _baseGamma.Generate(Dimension, rnd).Divide(DegreesOfFreedom).Sqrt();
            result = Elementwise.Divide(result, gammaSqrt);//from mathlab func MVTRND(C,DF,N)

            //double[] chiSqrt = _baseChi.Generate(Dimension, rnd).Divide(DegreesOfFreedom).Sqrt();
            //result = Elementwise.Divide(result, chiSqrt); //from R func R/rmvt.R



            result = Elementwise.Add(result, u);

            return result;
        }

        protected override CorrelatedPair GetBivariatePairInternal(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples)
        {
            return new CorrelatedPair(new StudentGeneralizedDistributionSettings(mean1, sigma1, DegreesOfFreedom).GetDistribution(samples), new StudentGeneralizedDistributionSettings(mean2, sigma2, DegreesOfFreedom).GetDistribution(samples), rho);
        }
    }
}
