using System;
using System.Linq;
using Accord.Math;
using Accord.Math.Decompositions;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Sum of correlated normal distributions counted by equation c1*A+c2*B+... where c is vector of coefficients.
    /// </summary>
    public class MultivariateBasedNormalDistributionSettings : DistributionSettings
    {
        private MultivariateDistributionSettings multivariateNormalDistributionSettings;

        private double[] coefficients = new double[] { 1, 1 };

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateBasedNormalDistributionSettings"/> class.
        /// Base constructor that creates sum of two not correlated standard normal distributions.
        /// </summary>
        public MultivariateBasedNormalDistributionSettings()
        {
            multivariateNormalDistributionSettings = new MultivariateDistributionSettings(2, new NormalDistributionSettings());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateBasedNormalDistributionSettings"/> class.
        /// Creates sum of correlated normal distributions with parameters set by <paramref name="distributionSettings"/>.
        /// </summary>
        /// <param name="coeff">Coefficients.</param>
        /// <param name="distributionSettings">Parameters of multivariate normal distribution.</param>
        public MultivariateBasedNormalDistributionSettings(double[] coeff, MultivariateDistributionSettings distributionSettings)
        {
            // TODO: check for normality
            coefficients = coeff;
            multivariateNormalDistributionSettings = distributionSettings;

            CheckParameters();
        }

        /// <summary>
        /// Coefficients vector that will be applied to the sum of normal distributions.
        /// </summary>
        public double[] Coefficients
        {
            get => coefficients;
            set
            {
                coefficients = value;
                CheckParameters();
            }
        }

        /// <summary>
        /// Parameters of multivariate normal.
        /// </summary>
        public MultivariateDistributionSettings MultivariateNormalDistributionSettings
        {
            get => multivariateNormalDistributionSettings;
            set
            {
                multivariateNormalDistributionSettings = value;

                CheckParameters();
            }
        }

        public override string ToString()
        {
            try
            {
                var settings = GetSettings();
                return $"D = {multivariateNormalDistributionSettings.Dimension}; μ = {settings.Mean}; σ = {settings.StandardDeviation}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return GetSettings().GetUnivariateContinuousDistribution();
        }

        protected override void CheckParameters()
        {
            if (multivariateNormalDistributionSettings == null)
            {
                throw new ArgumentNullException(nameof(MultivariateNormalDistributionSettings));
            }

            if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(Coefficients));
            }
        }

        private NormalDistributionSettings GetSettings()
        {
            if (Coefficients.Length != MultivariateNormalDistributionSettings.Dimension)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.VectorOfCoeffitientsMustBeEqualToDimension);
            }

            if (Coefficients.All(x => x == 1))
            {
                return new NormalDistributionSettings(MultivariateNormalDistributionSettings.Means.Sum(), Math.Sqrt(MultivariateNormalDistributionSettings.CovarianceMatrix.Sum()));
            }

            var chol = new CholeskyDecomposition(MultivariateNormalDistributionSettings.CovarianceMatrix);

            double[,] ltf = chol.LeftTriangularFactor;

            var weighted = Matrix.Diagonal(Coefficients).Dot(ltf);
            var colSumPow = weighted.Transpose().Dot(Vector.Ones(MultivariateNormalDistributionSettings.Dimension)).Pow(2);

            double variance = colSumPow.Sum();

            double mean = Elementwise.Multiply(Coefficients, MultivariateNormalDistributionSettings.Means).Sum();

            return new NormalDistributionSettings(mean, Math.Sqrt(variance));
        }
    }
}
