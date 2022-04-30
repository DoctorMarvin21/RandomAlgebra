using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Uniform distribution settings.
    /// </summary>
    public class UniformDistributionSettings : DistributionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniformDistributionSettings"/> class with support [-1, 1].
        /// </summary>
        public UniformDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniformDistributionSettings"/> class with
        /// lower bound <paramref name="lowerBound"/> and upper bound <paramref name="upperBound"/>.
        /// </summary>
        /// <param name="lowerBound">Lower bound.</param>
        /// <param name="upperBound">Upper bound.</param>
        public UniformDistributionSettings(double lowerBound, double upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;

            CheckParameters();
        }

        /// <summary>
        /// Lower bound.
        /// </summary>
        public double LowerBound { get; set; } = -1;

        /// <summary>
        /// Upper bound.
        /// </summary>
        public double UpperBound { get; set; } = 1;

        public override string ToString()
        {
            return $"α = {LowerBound}; β = {UpperBound}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new UniformContinuousDistribution(LowerBound, UpperBound);
        }

        protected override void CheckParameters()
        {
            if (LowerBound > UpperBound)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.LowerBoundIsGreaterThenUpperBound);
            }
        }
    }
}
