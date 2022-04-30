using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.Distributions.CustomDistributions;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Chi distribution settings.
    /// </summary>
    public class ChiDistributionSettings : DistributionSettings
    {
        private int degreesOfFreedom = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChiDistributionSettings"/> class
        /// with <see cref="DegreesOfFreedom"/> = 1.
        /// </summary>
        public ChiDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChiDistributionSettings"/> class
        /// with degrees of freedom = <paramref name="degreesOfFreedom"/>.
        /// </summary>
        /// <param name="degreesOfFreedom">Degrees of freedom.</param>
        public ChiDistributionSettings(int degreesOfFreedom)
        {
            this.degreesOfFreedom = degreesOfFreedom;
            CheckParameters();
        }

        /// <summary>
        /// Degrees of freedom.
        /// </summary>
        public int DegreesOfFreedom
        {
            get => degreesOfFreedom;
            set
            {
                degreesOfFreedom = value;

                CheckParameters();
            }
        }

        public override string ToString()
        {
            return $"k = {DegreesOfFreedom}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new ChiDistribution(DegreesOfFreedom);
        }

        protected override void CheckParameters()
        {
            if (degreesOfFreedom < 1)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.DegreesOfFreedomMustBeGreaterThenZero);
            }
        }
    }
}
