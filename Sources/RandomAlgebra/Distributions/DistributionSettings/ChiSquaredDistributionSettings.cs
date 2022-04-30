using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Chi squared distribution settings.
    /// </summary>
    public class ChiSquaredDistributionSettings : ChiDistributionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChiSquaredDistributionSettings"/> class
        /// with <see cref="DegreesOfFreedom"/> = 1.
        /// </summary>
        public ChiSquaredDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChiSquaredDistributionSettings"/> class.
        /// with degrees of freedom = <paramref name="degreesOfFreedom"/>.
        /// </summary>
        /// <param name="degreesOfFreedom">Degrees of freedom.</param>
        public ChiSquaredDistributionSettings(int degreesOfFreedom)
            : base(degreesOfFreedom)
        {
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new ChiSquareDistribution(DegreesOfFreedom);
        }
    }
}
