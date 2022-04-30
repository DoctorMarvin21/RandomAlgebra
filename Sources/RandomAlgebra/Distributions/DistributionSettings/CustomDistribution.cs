using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Settings of distribution based on <see cref="UnivariateContinuousDistribution"/>.
    /// </summary>
    public class CustomDistribution : DistributionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDistribution"/> class
        /// based on <paramref name="distribution"/>, reference for <see cref="Accord.Statistics"/> required.
        /// </summary>
        /// <param name="distribution">Base distribution.</param>
        public CustomDistribution(UnivariateContinuousDistribution distribution)
        {
            Distribution = distribution;
        }

        /// <summary>
        /// Base distribution, reference for <see cref="Accord.Statistics"/> required.
        /// </summary>
        public UnivariateContinuousDistribution Distribution { get; }

        public override string ToString()
        {
            return Distribution.ToString();
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return Distribution;
        }
    }
}
