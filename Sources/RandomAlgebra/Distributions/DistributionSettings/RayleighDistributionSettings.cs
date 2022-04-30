using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Rayleigh distribution settings.
    /// </summary>
    public class RayleighDistributionSettings : DistributionSettings
    {
        private double scaleParameter = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="RayleighDistributionSettings"/> class
        /// with <see cref="ScaleParameter"/> = 1.
        /// </summary>
        public RayleighDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RayleighDistributionSettings"/> class
        /// with scale parameter = <paramref name="scaleParameter"/>.
        /// </summary>
        /// <param name="scaleParameter">Scale parameter.</param>
        public RayleighDistributionSettings(double scaleParameter)
        {
            this.scaleParameter = scaleParameter;
            CheckParameters();
        }

        public double ScaleParameter
        {
            get => scaleParameter;
            set
            {
                scaleParameter = value;
                CheckParameters();
            }
        }

        public override string ToString()
        {
            return $"σ = {ScaleParameter}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new RayleighDistribution(ScaleParameter);
        }

        protected override void CheckParameters()
        {
            if (scaleParameter <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ScaleParameterMustBeGreaterThenZero);
            }
        }
    }
}
