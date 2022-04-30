using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.Distributions.CustomDistributions;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Arcsine distribution settings.
    /// </summary>
    public class ArcsineDistributionSettings : UniformDistributionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArcsineDistributionSettings"/> class with support [-1, 1].
        /// </summary>
        public ArcsineDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArcsineDistributionSettings"/> class
        /// with lower bound <paramref name="lowerBound"/> and upper bound <paramref name="upperBound"/>.
        /// </summary>
        /// <param name="lowerBound">Lower bound.</param>
        /// <param name="upperBound">Upper bound.</param>
        public ArcsineDistributionSettings(double lowerBound, double upperBound)
            : base(lowerBound, upperBound)
        {
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new ArcsineDistribution(LowerBound, UpperBound);
        }
    }
}
