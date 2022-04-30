using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Beta distribution settings.
    /// </summary>
    public class BetaDistributionSettings : DistributionSettings
    {
        private double shapeParameterA = 1;
        private double shapeParameterB = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetaDistributionSettings"/> class
        /// with <see cref="ShapeParameterA"/> 1 and <see cref="ShapeParameterB"/> = 2.
        /// </summary>
        public BetaDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BetaDistributionSettings"/> class
        /// with shape parameters <paramref name="shapeParameterA"/> and <paramref name="shapeParameterB"/>.
        /// </summary>
        /// <param name="shapeParameterA">Shape parameter α.</param>
        /// <param name="shapeParameterB">Shape parameter β.</param>
        public BetaDistributionSettings(double shapeParameterA, double shapeParameterB)
        {
            this.shapeParameterA = shapeParameterA;
            this.shapeParameterB = shapeParameterB;

            CheckParameters();
        }

        /// <summary>
        /// Shape parameter α.
        /// </summary>
        public double ShapeParameterA
        {
            get => shapeParameterA;
            set
            {
                shapeParameterA = value;

                CheckParameters();
            }
        }

        /// <summary>
        /// Shape parameter β.
        /// </summary>
        public double ShapeParameterB
        {
            get => shapeParameterB;
            set
            {
                shapeParameterB = value;
                CheckParameters();
            }
        }

        public override string ToString()
        {
            return $"α = {ShapeParameterA}; β = {ShapeParameterB}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new BetaDistribution(ShapeParameterA, ShapeParameterB);
        }

        protected override void CheckParameters()
        {
            if (shapeParameterA <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ShapeParameterAMustBeGreaterThenZero);
            }

            if (shapeParameterB <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ShapeParameterBMustBeGreaterThenZero);
            }
        }
    }
}
