using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Log-normal distribution parameters.
    /// </summary>
    public class LognormalDistributionSettings : NormalDistributionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LognormalDistributionSettings"/> class
        /// with zero mean and standard deviation equals 1.
        /// </summary>
        public LognormalDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LognormalDistributionSettings"/> class
        /// with expected value <paramref name="mean"/> and standard deviation <paramref name="std"/>.
        /// </summary>
        /// <param name="mean">Expected value.</param>
        /// <param name="std">Standard deviation.</param>
        public LognormalDistributionSettings(double mean, double std)
            : base(mean, std)
        {
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new LognormalDistribution(Mean, StandardDeviation);
        }
    }
}
