using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Distributions settings base class.
    /// </summary>
    public abstract class DistributionSettings
    {
        /// <summary>
        /// Returns text representation of distribution settings.
        /// </summary>
        /// <returns>Text representation of distribution settings.</returns>
        public abstract override string ToString();

        /// <summary>
        /// Returns distribution by current distribution settings.
        /// </summary>
        /// <returns><see cref="BaseDistribution"/> instance.</returns>
        public BaseDistribution GetDistribution()
        {
            return GetDistributionInternal(ContinuousDistribution.DefaultSamples);
        }

        /// <summary>
        /// Returns distribution by current distribution settings with certain sampling rate.
        /// </summary>
        /// <param name="samples">Samples count used in method <see cref="ContinuousDistribution.Discretize"/>.</param>
        /// <returns><see cref="BaseDistribution"/> instance.</returns>
        public BaseDistribution GetDistribution(int samples)
        {
            return GetDistributionInternal(samples);
        }

        internal abstract UnivariateContinuousDistribution GetUnivariateContinuousDistribution();

        protected virtual void CheckParameters()
        {
        }

        protected virtual BaseDistribution GetDistributionInternal(int samples)
        {
            return new ContinuousDistribution(this, samples);
        }
    }
}
