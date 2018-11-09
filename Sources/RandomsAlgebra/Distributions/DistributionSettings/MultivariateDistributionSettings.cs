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
                throw new DistributionsArgumentException("Covariance matrix is not square", "Матрица ковариации не квадратная");

            if (!parameters.CovarianceMatrix.IsSymmetric())
                throw new DistributionsArgumentException("Covariance matrix is asymmetric", "Матрица ковариации не симметричная");

            if (parameters.Means.Length != parameters.CovarianceMatrix.GetLength(0))
                throw new DistributionsArgumentException("Vector of means length is not equals to dimension of covariance matrix", "Длина вектора средних значений не совпадает с размерностью матрицы");

            Dimension = parameters.Means.Length;

            CovarianceMatrix = parameters.CovarianceMatrix;
            Means = parameters.Means;

            _chol = new CholeskyDecomposition(CovarianceMatrix);

            if (!_chol.IsPositiveDefinite)
            {
                throw new DistributionsArgumentException("Covariance matrix is not positive-definite", "Матрица ковариации не является положительно определенной");
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
                throw new DistributionsArgumentException("Vector of coefficients length is not equals to dimension of covariance matrix", "Длина вектора средних значений не совпадает с размерностью матрицы");

            if (coeffs.All(x => x == 1))
                return new NormalDistributionSettings(Means.Sum(), Math.Sqrt(CovarianceMatrix.Sum()));

            double[,] ltf = _chol.LeftTriangularFactor;

            var weighted = Matrix.Diagonal(coeffs).Dot(ltf);
            var colSumPow = weighted.Transpose().Dot(Vector.Ones(Dimension)).Pow(2);

            double variance = colSumPow.Sum();

            double mean = Elementwise.Multiply(coeffs, Means).Sum();

            return new NormalDistributionSettings(mean, Math.Sqrt(variance));
        }
    }

    /// <summary>
    /// Multivariate t distribution settings that generates random variables vector
    /// </summary>
    public class MultivariateTDistributionSettings : MultivariateDistributionSettings
    {
        static NormalDistribution _baseNormal = new NormalDistribution();
        readonly GammaDistribution _baseGamma;

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
            //from mathlab func MVTRND(C,DF,N)

            double[,] ltf = _chol.LeftTriangularFactor;
            double[] result = new double[Dimension];
            double[] u = Means;

            for (int i = 0; i < Dimension; i++)
            {
                result[i] = _baseNormal.Generate(rnd);
            }

            result = Matrix.Dot(ltf, result);

            double[] gammaSqrt = _baseGamma.Generate(Dimension, rnd).Divide(DegreesOfFreedom).Sqrt();

            result = Elementwise.Divide(result, gammaSqrt);

            result = Elementwise.Add(result, u);

            return result;
        }
    }
}
