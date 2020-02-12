using System;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.Distributions.SpecialDistributions;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Distributions settings base class.
    /// </summary>
    public abstract class DistributionSettings
    {
        /// <summary>
        /// Returns text representation of distribution settings.
        /// </summary>
        /// <returns>Text representation of distribution settings.</returns>
        public abstract override string ToString();

        /// <summary>
        /// Returns distribution by current distribution settings.
        /// </summary>
        /// <returns><see cref="BaseDistribution"/> instance.</returns>
        public BaseDistribution GetDistribution()
        {
            return GetDistributionInternal(ContinuousDistribution.DefaultSamples);
        }

        /// <summary>
        /// Returns distribution by current distribution settings with certain sampling rate.
        /// </summary>
        /// <param name="samples">Samples count used in method <see cref="ContinuousDistribution.Discretize"/>.</param>
        /// <returns><see cref="BaseDistribution"/> instance.</returns>
        public BaseDistribution GetDistribution(int samples)
        {
            return GetDistributionInternal(samples);
        }

        /// <summary>
        /// Generates vector of random variables with current distribution.
        /// </summary>
        /// <param name="samples">Number of random variables.</param>
        /// <param name="rnd">Random source.</param>
        /// <returns>Vector of random variables.</returns>
        public virtual double[] GenerateRandom(int samples, Random rnd)
        {
            var distr = GetUnivariateContinuoisDistribution();
            return distr.Generate(samples, rnd);
        }

        internal abstract UnivariateContinuousDistribution GetUnivariateContinuoisDistribution();

        protected virtual void CheckParameters()
        {
        }

        protected virtual BaseDistribution GetDistributionInternal(int samples)
        {
            return new ContinuousDistribution(this, samples);
        }
    }

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

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
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

    /// <summary>
    /// Arcsine distribution settings.
    /// </summary>
    public class ArcsineDistributionSettings : UniformDistributionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArcsineDistributionSettings"/> class with support [-1, 1].
        /// </summary>
        public ArcsineDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArcsineDistributionSettings"/> class
        /// with lower bound <paramref name="lowerBound"/> and upper bound <paramref name="upperBound"/>.
        /// </summary>
        /// <param name="lowerBound">Lower bound.</param>
        /// <param name="upperBound">Upper bound.</param>
        public ArcsineDistributionSettings(double lowerBound, double upperBound)
            : base(lowerBound, upperBound)
        {
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new ArcsineDistribution(LowerBound, UpperBound);
        }
    }

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
        public double Mean
        {
            get;
            set;
        } = 0;

        /// <summary>
        /// Standard deviation.
        /// </summary>
        public double StandardDeviation
        {
            get
            {
                return standardDeviation;
            }

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

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
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

    /// <summary>
    /// Log-normal distribution parameters.
    /// </summary>
    public class LognormalDistributionSettings : NormalDistributionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LognormalDistributionSettings"/> class
        /// with zero mean and standard deviation equals 1.
        /// </summary>
        public LognormalDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LognormalDistributionSettings"/> class
        /// with expected value <paramref name="mean"/> and standard deviation <paramref name="std"/>.
        /// </summary>
        /// <param name="mean">Expected value.</param>
        /// <param name="std">Standard deviation.</param>
        public LognormalDistributionSettings(double mean, double std)
            : base(mean, std)
        {
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new LognormalDistribution(Mean, StandardDeviation);
        }
    }

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
            get
            {
                return shapeParameter;
            }

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

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            double a = StandardDeviation * Math.Sqrt(Gamma.Function(1d / ShapeParameter) /
                Gamma.Function(3d / ShapeParameter));

            return new GeneralizedNormalDistribution(Mean, a, ShapeParameter);
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

    /// <summary>
    /// Sum of two correlated normal distributions.
    /// </summary>
    public class BivariateBasedNormalDistributionSettings : DistributionSettings
    {
        private double standardDeviation1 = 1;
        private double standardDeviation2 = 1;
        private double correlation = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="BivariateBasedNormalDistributionSettings"/> class.
        /// Base constructor that creates sum of two not correlated standard normal distributions.
        /// </summary>
        public BivariateBasedNormalDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BivariateBasedNormalDistributionSettings"/> class.
        /// Creates instance of sum of two correlated normal distributions with exact parameters.
        /// </summary>
        /// <param name="mean1">Expected value of 1-st normal distribution.</param>
        /// <param name="mean2">Expected value of 2-nd normal distribution.</param>
        /// <param name="std1">Standard deviation of 1-st normal distribution.</param>
        /// <param name="std2">Standard deviation of 2-nd normal distribution.</param>
        /// <param name="correlation">Correlation between first and second normal distributions.</param>
        public BivariateBasedNormalDistributionSettings(double mean1, double mean2, double std1, double std2, double correlation)
        {
            Mean1 = mean1;
            Mean2 = mean2;
            standardDeviation1 = std1;
            standardDeviation2 = std2;
            this.correlation = correlation;

            CheckParameters();
        }

        /// <summary>
        /// Expected value of 1-st normal distribution.
        /// </summary>
        public double Mean1
        {
            get;
            set;
        }

        /// <summary>
        /// Expected value of 2-nd normal distribution.
        /// </summary>
        public double Mean2
        {
            get;
            set;
        }

        /// <summary>
        /// Standard deviation of 1-st normal distribution.
        /// </summary>
        public double StandardDeviation1
        {
            get
            {
                return standardDeviation1;
            }

            set
            {
                standardDeviation1 = value;
                CheckParameters();
            }
        }

        /// <summary>
        /// Standard deviation of 2-nd normal distribution.
        /// </summary>
        public double StandardDeviation2
        {
            get
            {
                return standardDeviation2;
            }

            set
            {
                standardDeviation2 = value;
                CheckParameters();
            }
        }

        /// <summary>
        /// Correlation between first and second normal distributions.
        /// </summary>
        public double Correlation
        {
            get
            {
                return correlation;
            }

            set
            {
                correlation = value;
                CheckParameters();
            }
        }

        /// <summary>
        /// Expected value of sum of correlated normal distributions.
        /// </summary>
        public double TotalMean
        {
            get
            {
                return Mean1 + Mean2;
            }
        }

        /// <summary>
        /// Standard deviation of sum of correlated normal distributions.
        /// </summary>
        public double TotalStandardDeviation
        {
            get
            {
                return Math.Sqrt(Math.Pow(StandardDeviation1, 2) + Math.Pow(StandardDeviation2, 2) +
                    (2 * Correlation * StandardDeviation1 * StandardDeviation2));
            }
        }

        public override string ToString()
        {
            return $"ρ = {Correlation}; μ = {TotalMean}; σ = {TotalStandardDeviation}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new NormalDistribution(TotalMean, TotalStandardDeviation);
        }

        protected override void CheckParameters()
        {
            if (standardDeviation1 <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.StandardDeviationOfFirstDistributionMustBeGreaterThenZero);
            }
            if (standardDeviation2 <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.StandardDeviationOfSecondDistributionMustBeGreaterThenZero);
            }
            if (correlation <= -1 || correlation >= 1)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.CorrelationMustBeInRangeFromMinusOneToOne);
            }
        }
    }

    /// <summary>
    /// Sum of correlated normal distributions counted by equation c1*A+c2*B+... where c is vector of coefficients.
    /// </summary>
    public class MultivariateBasedNormalDistributionSettings : DistributionSettings
    {
        private MultivariateNormalDistributionSettings multivariateNormalDistributionSettings
            = new MultivariateNormalDistributionSettings(new double[] { 0, 0 }, new double[,] { { 1, 0 }, { 0, 1 } });

        private double[] coefficients = new double[] { 1, 1 };

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateBasedNormalDistributionSettings"/> class.
        /// Base constructor that creates sum of two not correlated standard normal distributions.
        /// </summary>
        public MultivariateBasedNormalDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateBasedNormalDistributionSettings"/> class.
        /// Creates sum of correlated normal distributions with parameters set by <paramref name="distributionSettings"/>.
        /// </summary>
        /// <param name="coeff">Coefficients.</param>
        /// <param name="distributionSettings">Parameters of multivariate normal distribution.</param>
        public MultivariateBasedNormalDistributionSettings(double[] coeff, MultivariateNormalDistributionSettings distributionSettings)
        {
            coefficients = coeff;
            multivariateNormalDistributionSettings = distributionSettings;

            CheckParameters();
        }

        /// <summary>
        /// Coefficients vector that will be applied to the sum of normal distributions.
        /// </summary>
        public double[] Coefficients
        {
            get
            {
                return coefficients;
            }

            set
            {
                coefficients = value;

                CheckParameters();
            }
        }

        /// <summary>
        /// Parameters of multivariate normal.
        /// </summary>
        public MultivariateNormalDistributionSettings MultivariateNormalDistributionSettings
        {
            get
            {
                return multivariateNormalDistributionSettings;
            }

            set
            {
                multivariateNormalDistributionSettings = value;

                CheckParameters();
            }
        }

        public override string ToString()
        {
            var settings = GetSettings();

            return $"D = {multivariateNormalDistributionSettings.Dimension}; μ = {settings.Mean}; σ = {settings.StandardDeviation}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return GetSettings().GetUnivariateContinuoisDistribution();
        }

        protected override void CheckParameters()
        {
            if (multivariateNormalDistributionSettings == null)
            {
                throw new ArgumentNullException(nameof(MultivariateNormalDistributionSettings));
            }

            if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(Coefficients));
            }
        }

        private NormalDistributionSettings GetSettings()
        {
            return multivariateNormalDistributionSettings.GetUnivariateDistributionSettings(Coefficients);
        }
    }

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
            get
            {
                return degreesOfFreedom;
            }

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

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
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
            get
            {
                return shapeParameterA;
            }

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
            get
            {
                return shapeParameterB;
            }

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

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
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
            get
            {
                return shapeParameter;
            }

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
            get
            {
                return scaleParameter;
            }

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

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
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
            get
            {
                return rate;
            }

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

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
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
            get
            {
                return degreesOfFreedom;
            }

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

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
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

    /// <summary>
    /// Chi squared distribution settings.
    /// </summary>
    public class ChiSquaredDistributionSettings : ChiDistributionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChiSquaredDistributionSettings"/> class
        /// with <see cref="DegreesOfFreedom"/> = 1.
        /// </summary>
        public ChiSquaredDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChiSquaredDistributionSettings"/> class.
        /// with degrees of freedom = <paramref name="degreesOfFreedom"/>.
        /// </summary>
        /// <param name="degreesOfFreedom">Degrees of freedom.</param>
        public ChiSquaredDistributionSettings(int degreesOfFreedom)
            : base(degreesOfFreedom)
        {
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new ChiSquareDistribution(DegreesOfFreedom);
        }
    }

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
            get
            {
                return scaleParameter;
            }

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

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
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

    /// <summary>
    /// Settings of distribution based on <see cref="UnivariateContinuousDistribution"/>,
    /// reference for <see cref="Accord.Statistics"/> required.
    /// </summary>
    public class CustomDistribution : DistributionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDistribution"/> class
        /// based on <paramref name="distribution"/>, reference for <see cref="Accord.Statistics"/> required.
        /// </summary>
        /// <param name="distribution">Base distribution, reference for <see cref="Accord.Statistics"/> required.</param>
        public CustomDistribution(UnivariateContinuousDistribution distribution)
        {
            Distribution = distribution;
        }

        /// <summary>
        /// Base distribution, reference for <see cref="Accord.Statistics"/> required.
        /// </summary>
        public UnivariateContinuousDistribution Distribution
        {
            get;
        }

        public override string ToString()
        {
            return Distribution.ToString();
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return Distribution;
        }
    }
}
