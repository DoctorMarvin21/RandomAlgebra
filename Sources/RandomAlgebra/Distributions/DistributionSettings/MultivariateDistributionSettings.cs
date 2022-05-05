using System;
using Accord.Math;
using Accord.Math.Decompositions;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Base class for multivariate distribution settings.
    /// </summary>
    public abstract class MultivariateDistributionSettings
    {
        protected MultivariateDistributionSettings(int dimension)
            : this(new InternalParameters { Means = new double[dimension], CovarianceMatrix = GetDefaultCovarianceMatrix(dimension) })
        {
        }

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
        public double[] Means { get; }

        /// <summary>
        /// Variance-covariance matrix.
        /// </summary>
        public double[,] CovarianceMatrix { get; }

        /// <summary>
        /// Dimension of covariance matrix.
        /// </summary>
        public int Dimension { get; }

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
