using System;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Generalized Gaussian, also known as exponential power distribution settings,
    /// for shape parameter 2 it is normal distribution, 1 is Laplace, and inf is uniform,
    /// it is standardized above standard deviation.
    /// </summary>
    public class GeneralizedNormalDistributionSettings : NormalDistributionSettings
    {
        private double shapeParameter = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralizedNormalDistributionSettings"/> class
        /// with zero mean, scale is one and shape is ten.
        /// </summary>
        public GeneralizedNormalDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralizedNormalDistributionSettings"/> class
        /// with zero mean, scale is one and shape parameter is <paramref name="shapeParameter"/>.
        /// </summary>
        /// <param name="shapeParameter">Shape parameter.</param>
        public GeneralizedNormalDistributionSettings(double shapeParameter)
            : base()
        {
            this.shapeParameter = shapeParameter;
            CheckParameters();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralizedNormalDistributionSettings"/> class
        /// with expected value <paramref name="mean"/>, scale <paramref name="std"/>
        /// and degrees of freedom <paramref name="shapeParameter"/>.
        /// </summary>
        /// <param name="mean">Expected value.</param>
        /// <param name="std">Standard deviation as scale parameter, standard deviation of resulted
        /// generalized t-distribution settings would be  σ * ν/(ν-2).</param>
        /// <param name="shapeParameter">Shape parameter.</param>
        public GeneralizedNormalDistributionSettings(double mean, double std, double shapeParameter)
            : base(mean, std)
        {
            this.shapeParameter = shapeParameter;
            CheckParameters();
        }

        /// <summary>
        /// For shape parameter 2 it is normal distribution, 1 is Laplace, and inf is uniform.
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

        public override string ToString()
        {
            return base.ToString() + $"; β = {ShapeParameter}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            double scale = StandardDeviation * Math.Sqrt(Gamma.Function(1d / ShapeParameter) /
                Gamma.Function(3d / ShapeParameter));

            return new GeneralizedNormalDistribution(Mean, scale, ShapeParameter);
        }

        protected override void CheckParameters()
        {
            base.CheckParameters();

            if (shapeParameter <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ShapeParameterMustBeGreaterThenZero);
            }
        }
    }
}
