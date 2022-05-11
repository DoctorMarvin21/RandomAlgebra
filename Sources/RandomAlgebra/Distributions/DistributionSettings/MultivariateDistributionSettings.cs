using System;
using Accord.Math;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Base class for multivariate distribution settings.
    /// </summary>
    public class MultivariateDistributionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateDistributionSettings"/> class with <paramref name="dimension"/>
        /// dimensions, zero means and identity covariance matrix.
        /// </summary>
        /// <param name="dimension">Number of dimensions.</param>
        /// <param name="settings">Base settings.</param>
        public MultivariateDistributionSettings(int dimension, DistributionSettings settings)
            : this(new InternalParameters { Means = new double[dimension], CovarianceMatrix = GetDefaultCovarianceMatrix(dimension) }, settings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateDistributionSettings"/> class by measured values.
        /// </summary>
        /// <param name="input">Matrix of measured values.</param>
        /// <param name="settings">Base settings.</param>
        public MultivariateDistributionSettings(double[,] input, DistributionSettings settings)
            : this(ProcessInput(input), settings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateDistributionSettings"/> class
        /// by means vector and covariance matrix.
        /// </summary>
        /// <param name="means">Means vector.</param>
        /// <param name="covarianceMatrix">Covariance matrix.</param>
        /// <param name="settings">Base settings.</param>
        public MultivariateDistributionSettings(double[] means, double[,] covarianceMatrix, DistributionSettings settings)
            : this(new InternalParameters { Means = means, CovarianceMatrix = covarianceMatrix }, settings)
        {
            BaseSettings = settings;
        }

        protected MultivariateDistributionSettings(InternalParameters parameters, DistributionSettings settings)
        {
            BaseSettings = settings;

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
        }

        /// <summary>
        /// Vector of means.
        /// </summary>
        public double[] Means { get; }

        /// <summary>
        /// Variance-covariance matrix.
        /// </summary>
        public double[,] CovarianceMatrix { get; }

        /// <summary>
        /// Dimension of covariance matrix.
        /// </summary>
        public int Dimension { get; }

        /// <summary>
        /// Settings of base univariate distribution.
        /// </summary>
        public DistributionSettings BaseSettings { get; }

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

            var left = m1 + (BaseSettings.GetDistribution(samples) * s1);
            var right = m2 + (BaseSettings.GetDistribution(samples) * s2);

            return new CorrelatedPair(left, right, rho);
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

        private static double[,] GetDefaultCovarianceMatrix(int dimension)
        {
            var covarianceMatrix = new double[dimension, dimension];

            for (int i = 0; i < dimension; i++)
            {
                covarianceMatrix[i, i] = 1;
            }

            return covarianceMatrix;
        }

        protected class InternalParameters
        {
            public double[] Means { get; set; }

            public double[,] CovarianceMatrix { get; set; }
        }
    }
}
