using System;
using System.Linq;
using Accord.Math;
using Accord.Math.Decompositions;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Base class for multivariate distribution settings.
    /// </summary>
    public abstract class MultivariateDistributionSettings
    {
        protected MultivariateDistributionSettings(double[,] input)
            : this(ProcessInput(input))
        {
        }

        protected MultivariateDistributionSettings(double[] means, double[,] covarianceMatrix)
            : this(new InternalParameters { Means = means, CovarianceMatrix = covarianceMatrix })
        {
        }

        protected MultivariateDistributionSettings(InternalParameters parameters)
        {
            if (parameters.CovarianceMatrix == null)
            {
                throw new ArgumentNullException(nameof(parameters.CovarianceMatrix));
            }

            if (parameters.Means == null)
            {
                throw new ArgumentNullException(nameof(parameters.Means));
            }

            if (!parameters.CovarianceMatrix.IsSquare())
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.CovarianceMatrixMustBeSquare);
            }

            if (!parameters.CovarianceMatrix.IsSymmetric())
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.CovarianceMatrixMustBeSymmetric);
            }

            if (parameters.Means.Length != parameters.CovarianceMatrix.GetLength(0))
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.VectorOfMeansMustBeEqualToDimension);
            }

            Dimension = parameters.Means.Length;

            CovarianceMatrix = parameters.CovarianceMatrix;
            Means = parameters.Means;

            Chol = new CholeskyDecomposition(CovarianceMatrix);

            if (!Chol.IsPositiveDefinite)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.CovarianceMatrixMustBePositiveDefined);
            }
        }

        /// <summary>
        /// Vector of means.
        /// </summary>
        public double[] Means
        {
            get;
        }

        /// <summary>
        /// Variance-covariance matrix.
        /// </summary>
        public double[,] CovarianceMatrix
        {
            get;
        }

        /// <summary>
        /// Dimension of covariance matrix.
        /// </summary>
        public int Dimension
        {
            get;
        }

        protected CholeskyDecomposition Chol { get; }

        /// <summary>
        /// Generates dimension sized vector of random variables.
        /// </summary>
        /// <param name="rnd">Randoms source.</param>
        /// <returns>Vector of random variables.</returns>
        public double[] GenerateRandom(Random rnd)
        {
            return GenerateRandomInternal(rnd);
        }

        /// <summary>
        /// Returns bivariate pair based on current distribution settings if dimension = 2.
        /// </summary>
        /// <param name="samples">Number of samples for vector calculation.</param>
        /// <returns>Bivariate pair.</returns>
        public CorrelatedPair GetBivariatePair(int samples)
        {
            if (Dimension != 2)
            {
                throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.ForCorrelationPairMultivariateDistributionMustBeTwoDimensional);
            }

            double m1 = Means[0];
            double m2 = Means[1];

            double s1 = Math.Sqrt(CovarianceMatrix[0, 0]);
            double s2 = Math.Sqrt(CovarianceMatrix[1, 1]);
            double rho = CovarianceMatrix[0, 1] / (s1 * s2);

            return GetBivariatePairInternal(m1, m2, s1, s2, rho, samples);
        }

        protected static InternalParameters ProcessInput(double[,] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // https://stattrek.com/matrix-algebra/covariance-matrix
            var rows = input.GetLength(0);

            double[] onesRow = Vector.Create<double>(rows, 1);

            var meansMatrix = onesRow.Outer(onesRow).Dot(input).Divide(rows);

            var deviationScores = input.Subtract(meansMatrix);

            var sumOfSquares = deviationScores.TransposeAndDot(deviationScores);

            var means = onesRow.Dot(input).Divide(rows);

            var covarianceMatrix = sumOfSquares.Divide(rows - 1);

            return new InternalParameters { Means = means, CovarianceMatrix = covarianceMatrix };
        }

        protected abstract double[] GenerateRandomInternal(Random rnd);

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
    /// Multivariate normal distribution that generates random variables vector and sum of normal distributions.
    /// </summary>
    public class MultivariateNormalDistributionSettings : MultivariateDistributionSettings
    {
        private static readonly NormalDistribution BaseNormal = new NormalDistribution();

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateNormalDistributionSettings"/> class by measured values.
        /// </summary>
        /// <param name="input">Matrix of measured values.</param>
        public MultivariateNormalDistributionSettings(double[,] input)
            : base(input)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateNormalDistributionSettings"/> class
        /// by means vector and covariance matrix.
        /// </summary>
        /// <param name="means">Means vector.</param>
        /// <param name="covarianceMatrix">Covariance matrix.</param>
        public MultivariateNormalDistributionSettings(double[] means, double[,] covarianceMatrix)
            : base(means, covarianceMatrix)
        {
        }

        /// <summary>
        /// Generates new normal distribution settings as a sum of correlated normal distributions with coefficients <paramref name="coeffs"/> (c1*A+c2*B+c3*C etc.)
        /// </summary>
        /// <param name="coeffs">Coefficients vector that will be applied to the sum of normal distributions.</param>
        /// <returns>Normal distribution settings instance.</returns>
        public NormalDistributionSettings GetUnivariateDistributionSettings(double[] coeffs)
        {
            if (coeffs == null)
            {
                coeffs = Vector.Ones(Dimension);
            }

            if (coeffs.Length != Dimension)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.VectorOfCoeffitientsMustBeEqualToDimension);
            }

            if (coeffs.All(x => x == 1))
            {
                return new NormalDistributionSettings(Means.Sum(), Math.Sqrt(CovarianceMatrix.Sum()));
            }

            double[,] ltf = Chol.LeftTriangularFactor;

            var weighted = Matrix.Diagonal(coeffs).Dot(ltf);
            var colSumPow = weighted.Transpose().Dot(Vector.Ones(Dimension)).Pow(2);

            double variance = colSumPow.Sum();

            double mean = Elementwise.Multiply(coeffs, Means).Sum();

            return new NormalDistributionSettings(mean, Math.Sqrt(variance));
        }

        protected override double[] GenerateRandomInternal(Random rnd)
        {
            double[,] ltf = Chol.LeftTriangularFactor;
            double[] result = new double[Dimension];
            double[] u = Means;

            for (int i = 0; i < Dimension; i++)
            {
                result[i] = BaseNormal.Generate(rnd);
            }

            result = Matrix.Dot(ltf, result);

            result = Elementwise.Add(result, u);

            return result;
        }

        protected override CorrelatedPair GetBivariatePairInternal(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples)
        {
            return new CorrelatedPair(
                new NormalDistributionSettings(mean1, sigma1).GetDistribution(samples),
                new NormalDistributionSettings(mean2, sigma2).GetDistribution(samples),
                rho);
        }
    }

    /// <summary>
    /// Multivariate t distribution settings that generates random variables vector.
    /// </summary>
    public class MultivariateTDistributionSettings : MultivariateDistributionSettings
    {
        private static readonly NormalDistribution BaseNormal = new NormalDistribution();
        private readonly GammaDistribution baseGamma;
        private readonly ChiSquareDistribution baseChi;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateTDistributionSettings"/> class
        /// by measured value with degrees of freedom N-1.
        /// </summary>
        /// <param name="input">Matrix of measured values.</param>
        public MultivariateTDistributionSettings(double[,] input)
            : this(input, input.GetLength(1) - 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateTDistributionSettings"/> class
        /// by measured value with degrees of freedom N-1.
        /// </summary>
        /// <param name="input">Matrix of measured values.</param>
        /// <param name="degreesOfFreedom">Degrees of freedom.</param>
        public MultivariateTDistributionSettings(double[,] input, double degreesOfFreedom)
            : base(input)
        {
            DegreesOfFreedom = degreesOfFreedom;
            baseGamma = new GammaDistribution(2.0, 0.5 * degreesOfFreedom);
            baseChi = new ChiSquareDistribution((int)degreesOfFreedom);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateTDistributionSettings"/> class
        /// by means vector, covariance matrix and degrees of freedom.
        /// </summary>
        /// <param name="means">Means vector.</param>
        /// <param name="covarianceMatrix">Covariance matrix.</param>
        /// <param name="degreesOfFreedom">Degrees of freedom.</param>
        public MultivariateTDistributionSettings(double[] means, double[,] covarianceMatrix, double degreesOfFreedom)
            : base(means, covarianceMatrix)
        {
            DegreesOfFreedom = degreesOfFreedom;
            baseGamma = new GammaDistribution(2.0, 0.5 * degreesOfFreedom);
            baseChi = new ChiSquareDistribution((int)degreesOfFreedom);
        }

        /// <summary>
        /// Degrees of freedom applied to every dimension.
        /// </summary>
        public double DegreesOfFreedom
        {
            get;
        }

        protected override double[] GenerateRandomInternal(Random rnd)
        {
            double[,] ltf = Chol.LeftTriangularFactor;
            double[] result = new double[Dimension];
            double[] u = Means;

            // TODO: I Don't remember, what happens here. I should check it out.
            // OK. I checked, it doesn't work properly...
            for (int i = 0; i < Dimension; i++)
            {
                result[i] = BaseNormal.Generate(rnd);

                // result[i] = _baseNormal.Generate(rnd) / Math.Sqrt(_baseGamma.Generate(rnd) / DegreesOfFreedom);
            }

            result = Matrix.Dot(ltf, result);

            double[] gammaSqrt = baseGamma.Generate(Dimension, rnd).Divide(DegreesOfFreedom).Sqrt();

            // from MATHlab function MVTRND(C,DF,N).
            result = Elementwise.Divide(result, gammaSqrt);

            // double[] chiSqrt = _baseChi.Generate(Dimension, rnd).Divide(DegreesOfFreedom).Sqrt();

            // result = Elementwise.Divide(result, chiSqrt); //from R func R/rmvt.R
            result = Elementwise.Add(result, u);

            return result;
        }

        protected override CorrelatedPair GetBivariatePairInternal(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples)
        {
            return new CorrelatedPair(
                new StudentGeneralizedDistributionSettings(mean1, sigma1, DegreesOfFreedom).GetDistribution(samples),
                new StudentGeneralizedDistributionSettings(mean2, sigma2, DegreesOfFreedom).GetDistribution(samples),
                rho);
        }
    }
}
