using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.Distributions.CustomDistributions;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Generalized t-distribution settings.
    /// </summary>
    public class StudentGeneralizedDistributionSettings : NormalDistributionSettings
    {
        private double degreesOfFreedom = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentGeneralizedDistributionSettings"/> class
        /// with zero mean, scale is one and degrees of freedom is ten.
        /// </summary>
        public StudentGeneralizedDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentGeneralizedDistributionSettings"/> class
        /// with zero mean, scale is one
        /// and degrees of freedom is <paramref name="degreesOfFreedom"/>.
        /// </summary>
        /// <param name="degreesOfFreedom">Degrees of freedom.</param>
        public StudentGeneralizedDistributionSettings(double degreesOfFreedom)
            : base()
        {
            this.degreesOfFreedom = degreesOfFreedom;
            CheckParameters();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentGeneralizedDistributionSettings"/> class.
        /// with expected value <paramref name="mean"/>, scale <paramref name="std"/>
        /// and degrees of freedom <paramref name="degreesOfFreedom"/>.
        /// </summary>
        /// <param name="mean">Expected value.</param>
        /// <param name="std">Standard deviation as scale parameter, standard deviation of resulted generalized
        /// t-distribution settings would be  σ * ν/(ν-2).</param>
        /// <param name="degreesOfFreedom">Degrees of freedom.</param>
        public StudentGeneralizedDistributionSettings(double mean, double std, double degreesOfFreedom)
            : base(mean, std)
        {
            this.degreesOfFreedom = degreesOfFreedom;
            CheckParameters();
        }

        /// <summary>
        /// Degrees of freedom.
        /// </summary>
        public double DegreesOfFreedom
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
            return base.ToString() + $"; ν = {DegreesOfFreedom}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new StudentGeneralizedDistribution(Mean, StandardDeviation, DegreesOfFreedom);
        }

        protected override void CheckParameters()
        {
            base.CheckParameters();

            if (degreesOfFreedom < 1)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.DegreesOfFreedomMustNotBeLessThenOne);
            }
        }
    }
}
