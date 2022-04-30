using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Exponential distribution settings.
    /// </summary>
    public class ExponentialDistributionSettings : DistributionSettings
    {
        private double rate = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialDistributionSettings"/> class
        /// with <see cref="Rate"/> = 1.
        /// </summary>
        public ExponentialDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialDistributionSettings"/> class
        /// with rate = <paramref name="rate"/>.
        /// </summary>
        /// <param name="rate">Rate λ.</param>
        public ExponentialDistributionSettings(double rate)
        {
            this.rate = rate;
            CheckParameters();
        }

        /// <summary>
        /// Rate λ.
        /// </summary>
        public double Rate
        {
            get => rate;
            set
            {
                rate = value;
                CheckParameters();
            }
        }

        public override string ToString()
        {
            return $"λ = {Rate}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new ExponentialDistribution(Rate);
        }

        protected override void CheckParameters()
        {
            if (rate <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.RateParameterMustBeGreaterThenZero);
            }
        }
    }
}
