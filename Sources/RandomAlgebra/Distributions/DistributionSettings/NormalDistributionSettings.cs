using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Normal distribution settings.
    /// </summary>
    public class NormalDistributionSettings : DistributionSettings
    {
        private double standardDeviation = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalDistributionSettings"/> class
        /// with zero mean and standard deviation equals 1.
        /// </summary>
        public NormalDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalDistributionSettings"/> class
        /// with expected value <paramref name="mean"/> and standard deviation <paramref name="std"/>.
        /// </summary>
        /// <param name="mean">Expected value.</param>
        /// <param name="std">Standard deviation.</param>
        public NormalDistributionSettings(double mean, double std)
        {
            Mean = mean;
            standardDeviation = std;

            CheckParameters();
        }

        /// <summary>
        /// Expected value.
        /// </summary>
        public double Mean { get; set; } = 0;

        /// <summary>
        /// Standard deviation.
        /// </summary>
        public double StandardDeviation
        {
            get => standardDeviation;
            set
            {
                standardDeviation = value;
                CheckParameters();
            }
        }

        public override string ToString()
        {
            return $"μ = {Mean}; σ = {StandardDeviation}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new NormalDistribution(Mean, StandardDeviation);
        }

        protected override void CheckParameters()
        {
            if (standardDeviation <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.StandardDeviationMustBeGreaterThenZero);
            }
        }
    }
}
