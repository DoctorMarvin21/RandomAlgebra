using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.Distributions.CustomDistributions;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Gamma distribution settings.
    /// </summary>
    public class GammaDistributionSettings : DistributionSettings
    {
        private double shapeParameter = 1;
        private double scaleParameter = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="GammaDistributionSettings"/> class
        /// with shape parameter <see cref="ShapeParameter"/> = 1 and <see cref="ScaleParameter"/> = 1.
        /// </summary>
        public GammaDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GammaDistributionSettings"/> class
        /// with shape parameter <paramref name="shapeParameter"/> and scale parameter <paramref name="scaleParameter"/>.
        /// </summary>
        /// <param name="shapeParameter">Shape parameter k.</param>
        /// <param name="scaleParameter">Scale parameter θ.</param>
        public GammaDistributionSettings(double shapeParameter, double scaleParameter)
        {
            this.shapeParameter = shapeParameter;
            this.scaleParameter = scaleParameter;
            CheckParameters();
        }

        /// <summary>
        /// Shape parameter k.
        /// </summary>
        public double ShapeParameter
        {
            get => shapeParameter;
            set
            {
                shapeParameter = value;
                CheckParameters();
            }
        }

        /// <summary>
        /// Scale parameter θ.
        /// </summary>
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
            return $"θ = {ScaleParameter}; k = {ShapeParameter}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new GammaCorrectedDistribution(ScaleParameter, ShapeParameter);
        }

        protected override void CheckParameters()
        {
            if (shapeParameter <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ShapeParameterMustBeGreaterThenZero);
            }

            if (scaleParameter <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ScaleParameterMustBeGreaterThenZero);
            }
        }
    }
}
