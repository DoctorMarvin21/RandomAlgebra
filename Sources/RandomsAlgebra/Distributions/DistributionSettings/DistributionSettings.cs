using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra;
using RandomAlgebra.Distributions.SpecialDistributions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Distributions settings base class
    /// </summary>
    public abstract class DistributionSettings
    {
        protected virtual void CheckParameters()
        {

        }

        /// <summary>
        /// Returns distribution by current distribution settings
        /// </summary>
        /// <returns>Distribution</returns>
        public BaseDistribution GetDistribution()
        {
            return GetDistributionInternal(ContinuousDistribution.DefaultSamples);
        }

        /// <summary>
        /// Returns distribution by current distribution settings with certain sampling rate
        /// </summary>
        /// <param name="samples">Samples count used in method <see cref="ContinuousDistribution.Discretize"/></param>
        /// <returns>Распределение</returns>
        public BaseDistribution GetDistribution(int samples)
        {
            return GetDistributionInternal(samples);
        }

        /// <summary>
        /// Generates vector of random variables with current distribution
        /// </summary>
        /// <param name="samples">Number of random variables</param>
        /// <param name="rnd">Random source</param>
        /// <returns>Vector of random variables</returns>
        public virtual double[] GenerateRandom(int samples, Random rnd)
        {
            var distr = this.GetUnivariateContinuoisDistribution();
            return distr.Generate(samples, rnd);
        }

        protected virtual BaseDistribution GetDistributionInternal(int samples)
        {
            return new ContinuousDistribution(this, samples);
        }

        internal abstract UnivariateContinuousDistribution GetUnivariateContinuoisDistribution();

        public abstract override string ToString();
    }

    /// <summary>
    /// Uniform distribution settings
    /// </summary>
    public class UniformDistributionSettings : DistributionSettings
    {
        /// <summary>
        /// Base constructor of uniform distrubution with support [-1, 1]
        /// </summary>
        public UniformDistributionSettings()
        {
        }

        /// <summary>
        /// Creates instance of uniform distribution settings with lower bound <paramref name="lowerBound"/> и upper bound <paramref name="upperBound"/>
        /// </summary>
        /// <param name="lowerBound">Lower bound</param>
        /// <param name="upperBound">Upper bound</param>
        public UniformDistributionSettings(double lowerBound, double upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;

            CheckParameters();
        }

        /// <summary>
        /// Lower bound
        /// </summary>
        public double LowerBound { get; set; } = -1;
        /// <summary>
        /// Upper bound
        /// </summary>
        public double UpperBound { get; set; } = 1;

        protected override void CheckParameters()
        {
            if (LowerBound > UpperBound)
            {
                throw new DistributionsArgumentException("Lower bound is greater then upper bound", "Нижняя граница больше верхней границы");
            }
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new UniformContinuousDistribution(LowerBound, UpperBound);
        }

        public override string ToString()
        {
            return $"α = {LowerBound}; β = {UpperBound}";
        }
    }

    /// <summary>
    /// Arcsine distribution settings
    /// </summary>
    public class ArcsineDistributionSettings : UniformDistributionSettings
    {

        /// <summary>
        /// Base constructor of arcsine distribution with support [-1, 1]
        /// </summary>
        public ArcsineDistributionSettings()
        {

        }

        /// <summary>
        /// Creates instance of arcsine distribution settings with lower bound <paramref name="lowerBound"/> и upper bound <paramref name="upperBound"/>
        /// </summary>
        /// <param name="lowerBound">Lower bound</param>
        /// <param name="upperBound">Upper bound</param>
        public ArcsineDistributionSettings(double lowerBound, double upperBound) : base(lowerBound, upperBound)
        {

        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new ArcsineDistribution(LowerBound, UpperBound);
        }
    }

    /// <summary>
    /// Normal distribution settings
    /// </summary>
    public class NormalDistributionSettings : DistributionSettings
    {
        private double _standardDeviation = 1;

        /// <summary>
        /// Base contructor with zero mean and standard deviation equals 1
        /// </summary>
        public NormalDistributionSettings()
        {
        }

        /// <summary>
        /// Creates intance of normal distribution settings with expected value <paramref name="mean"/> and standard deviation <paramref name="std"/>
        /// </summary>
        /// <param name="mean">Expected value</param>
        /// <param name="std">Standard deviation</param>
        public NormalDistributionSettings(double mean, double std)
        {
            Mean = mean;
            _standardDeviation = std;

            CheckParameters();
        }

        /// <summary>
        /// Expected value
        /// </summary>
        public double Mean
        {
            get;
            set;
        } = 0;

        /// <summary>
        /// Standard deviation
        /// </summary>
        public double StandardDeviation
        {
            get
            {
                return _standardDeviation;
            }
            set
            {
                _standardDeviation = value;

                CheckParameters();
            }
        }

        protected override void CheckParameters()
        {
            if (_standardDeviation <= 0)
            {
                throw new DistributionsArgumentException("Standard deviation must be greater then zero", "Стандартное отклонение должно быть больше 0");
            }
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new NormalDistribution(Mean, StandardDeviation);
        }

        public override string ToString()
        {
            return $"μ = {Mean}; σ = {StandardDeviation}";
        }
    }

    /// <summary>
    /// Lognormal distribution parameters
    /// </summary>
    public class LognormalDistributionSettings : NormalDistributionSettings
    {
        /// <summary>
        /// Base contructor with zero mean and standard deviation equals 1
        /// </summary>
        public LognormalDistributionSettings()
        {

        }

        /// <summary>
        /// Creates intance of lognormal distribution settings with expected value <paramref name="mean"/> and standard deviation <paramref name="std"/>
        /// </summary>
        /// <param name="mean">Expected value</param>
        /// <param name="std">Standard deviation</param>
        public LognormalDistributionSettings(double mean, double std) : base(mean, std)
        {
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new LognormalDistribution(Mean, StandardDeviation);
        }
    }

    /// <summary>
    /// Generalized Gaussian, also known as exponential power distribution settings, for shape parameter 2 it is normal distribution, 1 is laplace, and inf is uniform, it is standartized above standard deviation
    /// </summary>
    public class GeneralizedNormalDistributionSettings : NormalDistributionSettings
    {
        private double _shapeParameter = 2;

        /// <summary>
        /// Base constructor with zero mean, scale is one and shape is ten
        /// </summary>
        public GeneralizedNormalDistributionSettings()
        {
        }

        /// <summary>
        /// Create instance of generalized t-distribution with zero mean, scale is one and shape parameter is <paramref name="shapeParameter"/>
        /// </summary>
        /// <param name="shapeParameter">Shape parameter</param>
        public GeneralizedNormalDistributionSettings(double shapeParameter) : base()
        {
            _shapeParameter = shapeParameter;

            CheckParameters();
        }

        /// <summary>
        /// Create instance of generalized t-distribution with expected value <paramref name="mean"/>, scale <paramref name="std"/> and degrees of freedom <paramref name="shapeParameter"/>
        /// </summary>
        /// <param name="mean">Expected value</param>
        /// <param name="std">Standard deviation as scale parameter, standard deviation of resulted generalized t-distribution settings would be  σ * ν/(ν-2)</param>
        /// <param name="shapeParameter">Shape parameter</param>
        public GeneralizedNormalDistributionSettings(double mean, double std, double shapeParameter) : base(mean, std)
        {
            _shapeParameter = shapeParameter;

            CheckParameters();
        }

        /// <summary>
        /// For shape parameter 2 it is normal distribution, 1 is laplace, and inf is uniform
        /// </summary>
        public double ShapeParameter
        {
            get
            {
                return _shapeParameter;
            }
            set
            {
                _shapeParameter = value;

                CheckParameters();
            }
        }

        protected override void CheckParameters()
        {
            base.CheckParameters();

            if (_shapeParameter <= 0)
                throw new DistributionsArgumentException("Shape parameter must be greater then zero", "Стандартное отклонение должно быть больше 0");
        }


        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            double a = StandardDeviation * Math.Sqrt(Accord.Math.Gamma.Function(1d / ShapeParameter) / Accord.Math.Gamma.Function(3d / ShapeParameter));

            return new GeneralizedNormalDistribution(Mean, a, ShapeParameter);
        }

        public override string ToString()
        {
            return base.ToString() + $"; β = {ShapeParameter}";
        }
    }

    /// <summary>
    /// Sum of two correlated normal distributions
    /// </summary>
    public class BivariateBasedNormalDistributionSettings : DistributionSettings
    {
        private double _standardDeviation1 = 1;
        private double _standardDeviation2 = 1;
        private double _correlation = 0;

        /// <summary>
        /// Base constructor that creates sum of two not correlated standard normal distributions
        /// </summary>
        public BivariateBasedNormalDistributionSettings()
        {

        }

        /// <summary>
        /// Creates isntance of sum of two correlated normal distributions with exact parameters
        /// </summary>
        /// <param name="mean1">Expected value of 1-st normal distribution</param>
        /// <param name="mean2">Expected value of 2-nd normal distribution</param>
        /// <param name="std1">Standard deviation of 1-st normal distribution</param>
        /// <param name="std2">Standard deviation of 2-nd normal distribution</param>
        /// <param name="correlation">Correlation between first and second normal distributions</param>
        public BivariateBasedNormalDistributionSettings(double mean1, double mean2, double std1, double std2, double correlation)
        {
            Mean1 = mean1;
            Mean2 = mean2;
            _standardDeviation1 = std1;
            _standardDeviation2 = std2;
            _correlation = correlation;

            CheckParameters();
        }

        /// <summary>
        /// Expected value of 1-st normal distribution
        /// </summary>
        public double Mean1
        {
            get;
            set;
        }

        /// <summary>
        /// Expected value of 2-nd normal distribution
        /// </summary>
        public double Mean2
        {
            get;
            set;
        }

        /// <summary>
        /// Standard deviation of 1-st normal distribution
        /// </summary>
        public double StandardDeviation1
        {
            get
            {
                return _standardDeviation1;
            }
            set
            {
                _standardDeviation1 = value;
                CheckParameters();
            }
        }

        /// <summary>
        /// Standard deviation of 2-nd normal distribution
        /// </summary>
        public double StandardDeviation2
        {
            get
            {
                return _standardDeviation2;
            }
            set
            {
                _standardDeviation2 = value;
                CheckParameters();
            }
        }

        /// <summary>
        /// Correlation between first and second normal distributions
        /// </summary>
        public double Correlation
        {
            get
            {
                return _correlation;
            }
            set
            {
                _correlation = value;
                CheckParameters();
            }
        }

        protected override void CheckParameters()
        {
            if (_standardDeviation1 <= 0)
            {
                throw new DistributionsArgumentException("Standard deviation of 1-st distribution must be greater then zero", "Стандартное отклонение 1 должно быть больше 0");
            }
            if (_standardDeviation2 <= 0)
            {
                throw new DistributionsArgumentException("Standard deviation of 2-nd distribution must be greater then zero", "Стандартное отклонение 2 должно быть больше 0");
            }
            if (_correlation <= -1 || _correlation >= 1)
            {
                throw new DistributionsArgumentException("Correlation must be in range (-1, 1)", "Коэффициент корреляции должен быть в пределах (-1, 1)");
            }
        }

        /// <summary>
        /// Expected value of sum of correlated normal distributions
        /// </summary>
        public double TotalMean
        {
            get
            {
                return Mean1 + Mean2;
            }
        }

        /// <summary>
        /// Standard deviation of sum of correlated normal distributions
        /// </summary>
        public double TotalStandardDeviation
        {
            get
            {
                return Math.Sqrt(Math.Pow(StandardDeviation1, 2) + Math.Pow(StandardDeviation2, 2) + 2 * Correlation * StandardDeviation1 * StandardDeviation2);
            }
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new NormalDistribution(TotalMean, TotalStandardDeviation);
        }

        public override string ToString()
        {
            return $"ρ = {Correlation}; μ = {TotalMean}; σ = {TotalStandardDeviation}";
        }
    }

    /// <summary>
    /// Sum of correlated normal distributions counted by equation c1*A+c2*B+... where c is vector of coeffitients
    /// </summary>
    public class MultivariateBasedNormalDistributionSettings : DistributionSettings
    {
        private MultivariateNormalDistributionSettings _multivariateNormalDistributionSettings = new MultivariateNormalDistributionSettings(new double[] { 0, 0 }, new double[,] { { 1, 0 }, { 0, 1 } });
        private double[] _coefficients = new double[] { 1, 1 };


        /// <summary>
        /// Base constructor that creates sum of two not correlated standard normal distributions
        /// </summary>
        public MultivariateBasedNormalDistributionSettings()
        {
        }

        /// <summary>
        /// returns sum of correlated normal distributions with parameters setted by <paramref name="distributionSettings"/>
        /// </summary>
        /// <param name="coeff">Coeffitients</param>
        /// <param name="distributionSettings">Parameters of mulivariate normal distribution</param>
        public MultivariateBasedNormalDistributionSettings(double[] coeff, MultivariateNormalDistributionSettings distributionSettings)
        {
            _coefficients = coeff;
            _multivariateNormalDistributionSettings = distributionSettings;

            CheckParameters();
        }

        /// <summary>
        /// Cofficients vector that will be applied to the sum of normal distributions
        /// </summary>
        public double[] Coefficients
        {
            get
            {
                return _coefficients;
            }
            set
            {
                _coefficients = value;

                CheckParameters();
            }
        }

        /// <summary>
        /// Parameters of multivariate normal
        /// </summary>
        public MultivariateNormalDistributionSettings MultivariateNormalDistributionSettings
        {
            get
            {
                return _multivariateNormalDistributionSettings;
            }
            set
            {
                _multivariateNormalDistributionSettings = value;

                CheckParameters();
            }
        }

        protected override void CheckParameters()
        {
            if (_multivariateNormalDistributionSettings == null)
                throw new ArgumentNullException(nameof(MultivariateNormalDistributionSettings));

            if (_coefficients == null)
                throw new ArgumentNullException(nameof(Coefficients));
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return GetSettings().GetUnivariateContinuoisDistribution();
        }

        private NormalDistributionSettings GetSettings()
        {
            return _multivariateNormalDistributionSettings.GetUnivariateDistributionSettings(Coefficients);
        }

        public override string ToString()
        {
            var settings = GetSettings();

            return $"D = {_multivariateNormalDistributionSettings.Dimension}; μ = {settings.Mean}; σ = {settings.StandardDeviation}";
        }
    }

    /// <summary>
    /// Generalized t-distribution settings
    /// </summary>
    public class StudentGeneralizedDistributionSettings : NormalDistributionSettings
    {
        private double _degreesOfFreedom = 10;

        /// <summary>
        /// Base constructor with zero mean, scale is one and degrees of freedom is ten
        /// </summary>
        public StudentGeneralizedDistributionSettings()
        {
        }

        /// <summary>
        /// Create instance of generalized t-distribution with zero mean, scale is one and degrees of freedom is <paramref name="degreesOfFreedom"/>
        /// </summary>
        /// <param name="degreesOfFreedom">Degrees of freedom</param>
        public StudentGeneralizedDistributionSettings(double degreesOfFreedom) : base()
        {
            _degreesOfFreedom = degreesOfFreedom;
            
            CheckParameters();
        }

        /// <summary>
        /// Create instance of generalized t-distribution with expected value <paramref name="mean"/>, scale <paramref name="std"/> and degrees of freedom <paramref name="degreesOfFreedom"/>
        /// </summary>
        /// <param name="mean">Expected value</param>
        /// <param name="std">Standard deviation as scale parameter, standard deviation of resulted generalized t-distribution settings would be  σ * ν/(ν-2)</param>
        /// <param name="degreesOfFreedom">Degrees of freedom</param>
        public StudentGeneralizedDistributionSettings(double mean, double std, double degreesOfFreedom) : base(mean, std)
        {
            _degreesOfFreedom = degreesOfFreedom;

            CheckParameters();
        }

        /// <summary>
        /// Degrees of freedom
        /// </summary>
        public double DegreesOfFreedom
        {
            get
            {
                return _degreesOfFreedom;
            }
            set
            {
                _degreesOfFreedom = value;
                CheckParameters();
            }
        }
        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new StudentGeneralizedDistribution(Mean, StandardDeviation, DegreesOfFreedom);
        }

        protected override void CheckParameters()
        {
            base.CheckParameters();

            if (_degreesOfFreedom < 1)
            {
                throw new DistributionsArgumentException("Degrees of freedom must not be less then 1", "Число степеней свободы должно быть не меньше 1");
            }
        }

        public override string ToString()
        {
            return base.ToString() + $"; ν = {DegreesOfFreedom}";
        }
    }

    /// <summary>
    /// Beta distribution settings
    /// </summary>
    public class BetaDistributionSettings : DistributionSettings
    {
        /// <summary>
        /// Base constructor with <see cref="ShapeParameterA"/> 1 and <see cref="ShapeParameterB"/> = 2
        /// </summary>
        public BetaDistributionSettings()
        {
        }

        /// <summary>
        /// Creates instance of Beta distribution whth shape parameters <paramref name="shapeParameterA"/> and <paramref name="shapeParameterB"/>
        /// </summary>
        /// <param name="shapeParameterA">Shape parameter α</param>
        /// <param name="shapeParameterB">Shape parameter β</param>
        public BetaDistributionSettings(double shapeParameterA, double shapeParameterB)
        {
            _shapeParameterA = shapeParameterA;
            _shapeParameterB = shapeParameterB;

            CheckParameters();
        }


        private double _shapeParameterA = 1;
        private double _shapeParameterB = 2;

        /// <summary>
        /// Shape parameter α
        /// </summary>
        public double ShapeParameterA
        {
            get
            {
                return _shapeParameterA;
            }
            set
            {
                _shapeParameterA = value;

                CheckParameters();
            }
        }
        /// <summary>
        /// Shape parameter β
        /// </summary>
        public double ShapeParameterB
        {
            get
            {
                return _shapeParameterB;
            }
            set
            {
                _shapeParameterB = value;

                CheckParameters();
            }
        }

        protected override void CheckParameters()
        {
            if (_shapeParameterA <= 0)
            {
                throw new DistributionsArgumentException("Shape parameter α must be greater then zero", "Параметр формы α должен быть больше 0");
            }

            if (_shapeParameterB <= 0)
            {
                throw new DistributionsArgumentException("Shape parameter β must be greater then zero", "Параметр формы β должен быть больше 0");
            }
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new BetaDistribution(ShapeParameterA, ShapeParameterB);
        }

        public override string ToString()
        {
            return $"α = {ShapeParameterA}; β = {ShapeParameterB}";
        }
    }

    /// <summary>
    /// Gamma distribution settings
    /// </summary>
    public class GammaDistributionSettings : DistributionSettings
    {
        private double _shapeParameter = 1;
        private double _scaleParameter = 1;

        /// <summary>
        /// Base contructor with shape parameter <see cref="ShapeParameter"/> = 1 and <see cref="ScaleParameter"/> = 1
        /// </summary>
        public GammaDistributionSettings()
        {

        }

        /// <summary>
        /// Creates instance of Gamma distribution settings with shape parameter <paramref name="shapeParameter"/> и scale parameter <paramref name="scaleParameter"/>
        /// </summary>
        /// <param name="shapeParameter">Shape parameter k</param>
        /// <param name="scaleParameter">Scale parameter θ</param>
        public GammaDistributionSettings(double shapeParameter, double scaleParameter)
        {

        }

        /// <summary>
        ///Shape parameter k
        /// </summary>
        public double ShapeParameter
        {
            get
            {
                return _shapeParameter;
            }
            set
            {
                _shapeParameter = value;

                CheckParameters();
            }
        }


        /// <summary>
        /// Scale parameter θ
        /// </summary>
        public double ScaleParameter
        {
            get
            {
                return _scaleParameter;
            }
            set
            {
                _scaleParameter = value;

                CheckParameters();
            }
        }

        protected override void CheckParameters()
        {
            if (_shapeParameter <= 0)
            {
                throw new DistributionsArgumentException("Shape parameter must be greater then zero", "Параметр формы должен быть больше 0");
            }

            if (_scaleParameter <= 0)
            {
                throw new DistributionsArgumentException("Scale parameter must be greater then zero", "Параметр масштаба должен быть больше 0");
            }
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new GammaCorrectedDistribution(ScaleParameter, ShapeParameter);
        }

        public override string ToString()
        {
            return $"θ = {ScaleParameter}; k = {ShapeParameter}";
        }
    }

    /// <summary>
    /// Exponential distribution settings
    /// </summary>
    public class ExponentialDistributionSettings : DistributionSettings
    {
        private double _rate = 1;

        /// <summary>
        /// Base contructor with <see cref="Rate"/> = 1
        /// </summary>
        public ExponentialDistributionSettings()
        {

        }

        /// <summary>
        /// Creates instance of exponential distribution with rate = <paramref name="rate"/>
        /// </summary>
        /// <param name="rate">Rate λ</param>
        public ExponentialDistributionSettings(double rate)
        {
            _rate = rate;
            CheckParameters();
        }

        /// <summary>
        /// Rate λ
        /// </summary>
        public double Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
                CheckParameters();
            }
        }

        protected override void CheckParameters()
        {
            if (_rate <= 0)
            {
                throw new DistributionsArgumentException("Rate parameter must be greater then zero", "Интенсивность должна быть больше 0");
            }
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new ExponentialDistribution(Rate);
        }

        public override string ToString()
        {
            return $"λ = {Rate}";
        }
    }

    /// <summary>
    /// Chi distribution settings
    /// </summary>
    public class ChiDistributionSettings : DistributionSettings
    {
        private int _degreesOfFreedom = 1;

        /// <summary>
        /// Base constructor with <see cref="DegreesOfFreedom"/> = 1
        /// </summary>
        public ChiDistributionSettings()
        {

        }

        /// <summary>
        /// Creates instance of Chi distribution with degrees of freedom = <paramref name="degreesOfFreedom"/>
        /// </summary>
        /// <param name="degreesOfFreedom">Degrees of freedom</param>
        public ChiDistributionSettings(int degreesOfFreedom)
        {
            _degreesOfFreedom = degreesOfFreedom;
            CheckParameters();
        }

        /// <summary>
        /// Degrees of freedom
        /// </summary>
        public int DegreesOfFreedom
        {
            get
            {
                return _degreesOfFreedom;
            }
            set
            {
                _degreesOfFreedom = value;

                CheckParameters();
            }
        }

        protected override void CheckParameters()
        {
            if (_degreesOfFreedom < 1)
            {
                throw new DistributionsArgumentException("Degrees of freedom must be greater then zero", "Число степеней свободы должно быть больше нуля");
            }
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new ChiDistribution(DegreesOfFreedom);
        }

        public override string ToString()
        {
            return $"k = {DegreesOfFreedom}";
        }
    }

    /// <summary>
    /// Chi squared distribution settings
    /// </summary>
    public class ChiSquaredDistributionSettings : ChiDistributionSettings
    {
        /// <summary>
        /// Base constructor with <see cref="DegreesOfFreedom"/> = 1
        /// </summary>
        public ChiSquaredDistributionSettings()
        {

        }

        /// <summary>
        /// Creates instance of Chi squared distribution with degrees of freedom = <paramref name="degreesOfFreedom"/>
        /// </summary>
        /// <param name="degreesOfFreedom">Degrees of freedom</param>
        public ChiSquaredDistributionSettings(int degreesOfFreedom)
        {
            DegreesOfFreedom = degreesOfFreedom;
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new ChiSquareDistribution(DegreesOfFreedom);
        }
    }

    /// <summary>
    /// Rayleigh distribution settings
    /// </summary>
    public class RayleighDistributionSettings : DistributionSettings
    {
        private double _scaleParameter = 1;

        /// <summary>
        /// Base constructor with <see cref="ScaleParameter"/> = 1
        /// </summary>
        public RayleighDistributionSettings()
        {
        }

        /// <summary>
        /// Creates instance of Rayleigh distribution settings with scale parameter = <paramref name="scaleParameter"/>
        /// </summary>
        /// <param name="scaleParameter">Scale parameter</param>
        public RayleighDistributionSettings(double scaleParameter)
        {
            _scaleParameter = scaleParameter;
            CheckParameters();
        }

        public double ScaleParameter
        {
            get
            {
                return _scaleParameter;
            }
            set
            {
                _scaleParameter = value;

                CheckParameters();
            }
        }

        protected override void CheckParameters()
        {
            if (_scaleParameter <= 0)
            {
                throw new DistributionsArgumentException("Scale parameter must be greater then zero", "Параметр масштаба должен быть больше 0");
            }
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return new RayleighDistribution(ScaleParameter);
        }

        public override string ToString()
        {
            return $"σ = {ScaleParameter}";
        }
    }

    /// <summary>
    /// Settings of distribution based on <see cref="UnivariateContinuousDistribution"/>, reference for <see cref="Accord.Statistics"/> required
    /// </summary>
    public class CustomDistribution : DistributionSettings
    {
        /// <summary>
        /// Creates instance based on <paramref name="distribution"/>, reference for <see cref="Accord.Statistics"/> required
        /// </summary>
        /// <param name="distribution"></param>
        public CustomDistribution(UnivariateContinuousDistribution distribution)
        {
            Distribution = distribution;
        }

        /// <summary>
        /// Base distrubution, reference for <see cref="Accord.Statistics"/> required
        /// </summary>
        public UnivariateContinuousDistribution Distribution
        {
            get;
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuoisDistribution()
        {
            return Distribution;
        }

        public override string ToString()
        {
            return Distribution.ToString();
        }
    }
}
