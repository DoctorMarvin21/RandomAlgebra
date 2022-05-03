using System;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Sum of correlated normal distributions counted by equation c1*A+c2*B+... where c is vector of coefficients.
    /// </summary>
    public class MultivariateBasedNormalDistributionSettings : DistributionSettings
    {
        private MultivariateNormalDistributionSettings multivariateNormalDistributionSettings
            = new MultivariateNormalDistributionSettings(new double[] { 0, 0 }, new double[,] { { 1, 0 }, { 0, 1 } });

        private double[] coefficients = new double[] { 1, 1 };

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateBasedNormalDistributionSettings"/> class.
        /// Base constructor that creates sum of two not correlated standard normal distributions.
        /// </summary>
        public MultivariateBasedNormalDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateBasedNormalDistributionSettings"/> class.
        /// Creates sum of correlated normal distributions with parameters set by <paramref name="distributionSettings"/>.
        /// </summary>
        /// <param name="coeff">Coefficients.</param>
        /// <param name="distributionSettings">Parameters of multivariate normal distribution.</param>
        public MultivariateBasedNormalDistributionSettings(double[] coeff, MultivariateNormalDistributionSettings distributionSettings)
        {
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
        public MultivariateNormalDistributionSettings MultivariateNormalDistributionSettings
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
            return multivariateNormalDistributionSettings.GetUnivariateDistributionSettings(Coefficients);
        }
    }
}
