using Accord.Statistics.Distributions.Univariate;
using RandomsAlgebra.Distributions.Settings;
using RandomsAlgebra.Distributions.SpecialDistributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomsAlgebra.Distributions
{
    /// <summary>
    /// Continuous distribution
    /// </summary>
    public sealed class ContinuousDistribution : BaseDistribution
    {
        internal const int DefaultSamples = 1000;

        double[] _support = null;
        double? _skewness = null;
        int _samples = DefaultSamples;

        #region Constructors
        /// <summary>
        /// Creates continuous distribution by <paramref name="distributionSettings"/>
        /// </summary>
        /// <param name="distributionSettings">Distribution settings</param>
        public ContinuousDistribution(DistributionSettings distributionSettings) : this(distributionSettings?.GetUnivariateContinuoisDistribution())
        {
        }

        /// <summary>
        /// Creates continuous distribution by <paramref name="distributionSettings"/> and samples count
        /// </summary>
        /// <param name="distributionSettings">Distribution settings</param>
        /// <param name="samples">Samples count used in method <see cref="Discretize"/></param>
        public ContinuousDistribution(DistributionSettings distributionSettings, int samples) : this(distributionSettings?.GetUnivariateContinuoisDistribution(), samples)
        {
        }

        internal ContinuousDistribution(UnivariateContinuousDistribution baseDistribution)
        {
			if (baseDistribution == null)
				throw new ArgumentNullException(nameof(baseDistribution));

            BaseDistribution = baseDistribution;
        }

        internal ContinuousDistribution(UnivariateContinuousDistribution baseDistribution, int samples) : this(baseDistribution)
        {
            if (samples < 2)
                throw new DistributionsArgumentException("Samples must be greater than 2", "Число отсчетов должно быть положительным числом больше 2");


            _samples = samples;
        }

        internal ContinuousDistribution(UnivariateContinuousDistribution baseDistribution, int samples, double coeff, double offset) : this(baseDistribution, samples)
        {
            Coefficient = coeff;
            Offset = offset;
        }
        #endregion

        #region Settings
        internal UnivariateContinuousDistribution BaseDistribution
        {
            get;
        }

        internal double Coefficient
        {
            get;
        } = 1;


        internal double Offset
        {
            get;
        }

        #endregion

        #region Sampling
        /// <summary>
        /// Performs sampling to discrete distributions with number of samples <see cref="Samples"/>
        /// </summary>
        /// <returns>Discrete distribution</returns>
        public DiscreteDistribution Discretize()
        {
            return Discretize(_samples);
        }


        /// <summary>
        /// Performs sampling to discrete distributions with number of samples <paramref name="samples"/>
        /// </summary>
        /// <param name="samples">Samples</param>
        /// <returns>Discrete distribution</returns>
        public DiscreteDistribution Discretize(int samples)
        {
            return new DiscreteDistribution(this, samples);
        }

        public static explicit operator DiscreteDistribution(ContinuousDistribution continious)
        {
            return continious.Discretize();
        }

        #endregion

        #region Overrides properties
        internal override DistributionType InnerDistributionType
        {
            get
            {
                return DistributionType.Continious;
            }
        }

        internal override double InnerMinX
        {
            get
            {
                if (_support == null)
                {
                    _support = GetDiscretizationSupport();
                }
                return _support[0];
            }
        }

        internal override double InnerMaxX
        {
            get
            {
                if (_support == null)
                {
                    _support = GetDiscretizationSupport();
                }
                return _support[1];
            }
        }

        internal override int InnerSamples
        {
            get
            {
                return _samples;
            }
        }

        private double[] GetDiscretizationSupport()
        {
            bool minInfinite = double.IsInfinity(BaseDistribution.Support.Min);
            bool maxInfinite = double.IsInfinity(BaseDistribution.Support.Max);

            double tolerance = 1d / Math.Pow(InnerSamples * 10, 2);
            double min = minInfinite ? GetSupport(true, tolerance) : BaseDistribution.Support.Min;
            double max = maxInfinite ? GetSupport(false, tolerance) : BaseDistribution.Support.Max;

            min = min * Coefficient + Offset;
            max = max * Coefficient + Offset;

            if (min > max)
            {
                double temp = min;
                min = max;
                max = temp;
            }

            return new double[] { min, max };
        }

        private double GetSupport(bool min, double tolerance)
        {
            if (min)
                return BaseDistribution.InverseDistributionFunction(tolerance);
            else
                return BaseDistribution.InverseDistributionFunction(1 - tolerance);
        }

        internal override double InnerMean
        {
            get
            {
                return BaseDistribution.Mean * Coefficient + Offset;
            }
        }

        internal override double InnerVariance
        {
            get
            {
                return BaseDistribution.Variance * Math.Pow(Coefficient, 2);
            }
        }

        internal override double InnerSkewness
        {
            get
            {
                //Рассчитаем как интеграл через третий момент
                if (_skewness == null)
                    _skewness = Accord.Math.Integration.InfiniteAdaptiveGaussKronrod.Integrate(ThirdMoment, MinX, MaxX) / Math.Pow(Variance, 3d / 2);

                return _skewness.Value;
            }
        }

        public double ThirdMoment(double x)
        {
            return Math.Pow(x - Mean, 3) * InnerGetPDFYbyX(x);
        }

        #endregion

        #region Overrides functions
        internal override double InnerGetPDFYbyX(double x)
        {
            try
            {
                if (x < InnerMinX || x > InnerMaxX)
                {
                    return 0;
                }
                else if (Coefficient == 1 && Offset == 0)
                {
                    return BaseDistribution.ProbabilityDensityFunction(x);
                }
                else if (Coefficient == 1)
                {
                    return BaseDistribution.ProbabilityDensityFunction(x - Offset);
                }
                else
                {
                    return BaseDistribution.ProbabilityDensityFunction((x - Offset) / Coefficient) / (Math.Abs(Coefficient));
                }
            }
            catch
            {
                return 0;
            }
        }

        internal override double InnerGetCDFYbyX(double x)
        {
            try
            {
                if (x < InnerMinX || x > InnerMaxX)
                    return 0;
                else
                    return BaseDistribution.DistributionFunction((x - Offset) / Coefficient);
            }
            catch
            {
                return 0;
            }
        }

        internal override double InnerQuantile(double p)
        {
            return BaseDistribution.InverseDistributionFunction(p) * Coefficient + Offset;
        }

        #endregion

        #region Randoms math
        internal override BaseDistribution InnerGetSumm(BaseDistribution value)
        {
            switch (value.InnerDistributionType)
            {
                case DistributionType.Continious:
                    {
                        var right = (ContinuousDistribution)value;

                        if (Optimizations.UseContiniousConvolution)
                        {
                            if (BaseDistribution is NormalDistribution && right.BaseDistribution is NormalDistribution)
                            {
                                return ContinuousRandomsMath.ConvolutionOfNormalAndNormal(this, right);
                            }
                            else if (BaseDistribution is UniformContinuousDistribution && right.BaseDistribution is UniformContinuousDistribution)
                            {
                                return ContinuousRandomsMath.ConvolutionOfUniformAndUniform(this, right);
                            }
                            else if (BaseDistribution is NormalDistribution && right.BaseDistribution is UniformContinuousDistribution)
                            {
                                return ContinuousRandomsMath.ConvolutionOfNormalAndUniform(this, right);
                            }
                            else if (BaseDistribution is UniformContinuousDistribution && right.BaseDistribution is NormalDistribution)
                            {
                                return ContinuousRandomsMath.ConvolutionOfNormalAndUniform(right, this);
                            }
                            else if (BaseDistribution is NormalDistribution && right.BaseDistribution is BhattacharjeeDistribution)
                            {
                                return ContinuousRandomsMath.ConvolutionOfNormalAndBhattacharjee(this, right);
                            }
                            else if (BaseDistribution is BhattacharjeeDistribution && right.BaseDistribution is NormalDistribution)
                            {
                                return ContinuousRandomsMath.ConvolutionOfNormalAndBhattacharjee(right, this);
                            }
                            else if (BaseDistribution is StudentGeneralizedDistribution && right.BaseDistribution is UniformContinuousDistribution)
                            {
                                return ContinuousRandomsMath.ConvolutionOfStudentAndUniform(this, right);
                            }
                            else if (BaseDistribution is UniformContinuousDistribution && right.BaseDistribution is StudentGeneralizedDistribution)
                            {
                                return ContinuousRandomsMath.ConvolutionOfStudentAndUniform(right, this);
                            }
                            else
                            {
                                if (Optimizations.UseFFTConvolution)
                                {
                                    return FFT.Convolute(this, right);
                                }
                                else
                                {
                                    return Discretize() + right.Discretize();
                                }
                            }
                        }
                        else
                        {
                            if (Optimizations.UseFFTConvolution)
                            {
                                return FFT.Convolute(this, right);
                            }
                            else
                            {
                                return Discretize() + right.Discretize();
                            }
                        }
                    }
                case DistributionType.Discrete:
                    {
                        if (Optimizations.UseFFTConvolution)
                        {
                            return FFT.Convolute((DiscreteDistribution)value, this);
                        }
                        else
                        {
                            return Discretize() + value;
                        }
                    }
                case DistributionType.Number:
                    {
                        return ContinuousRandomsMath.Add(this, (double)value);
                    }
                default:
                    throw new DistributionsInvalidOperationException();
            }
        }

        internal override BaseDistribution InnerGetDifference(BaseDistribution value)
        {
            switch (value.InnerDistributionType)
            {
                case DistributionType.Continious:
                    {
                        return this + value * -1;
                    }
                case DistributionType.Discrete:
                    {
                        if (Optimizations.UseFFTConvolution)
                        {
                            return FFT.Convolute((DiscreteDistribution)(value * -1), this);
                        }
                        else
                        {
                            return Discretize() - value;
                        }
                    }
                case DistributionType.Number:
                    {
                        return ContinuousRandomsMath.Sub(this, (double)value);
                    }
                default:
                    throw new DistributionsInvalidOperationException();
            }
        }

        internal override BaseDistribution InnerGetProduct(BaseDistribution value)
        {
            switch (value.InnerDistributionType)
            {
                case DistributionType.Continious:
                case DistributionType.Discrete:
                    {
                        return Discretize() * value;
                    }
                case DistributionType.Number:
                    {
                        if (value.InnerMean == 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.MultiplyRandomByZero);

                        return ContinuousRandomsMath.Multiply(this, (double)value);
                    }
                default:
                    throw new DistributionsInvalidOperationException();
            }
        }

        internal override BaseDistribution InnerGetRatio(BaseDistribution value)
        {
            switch (value.InnerDistributionType)
            {
                case DistributionType.Continious:
                case DistributionType.Discrete:
                    {
                        return Discretize() / value;
                    }
                case DistributionType.Number:
                    {
                        if (value.InnerMean == 0)
                             CommonExceptions.ThrowCommonExcepton(CommonExceptionType.DivisionByZero);

                        return ContinuousRandomsMath.Divide(this, (double)value);
                    }
                default:
                    throw new DistributionsInvalidOperationException();
            }
        }

        internal override BaseDistribution InnerGetPower(BaseDistribution value)
        {
            switch (value.InnerDistributionType)
            {
                case DistributionType.Number:
                    {
                        return CommonRandomsMath.Power(this, (double)value);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomsMath.Power(Discretize(), (DiscreteDistribution)value);
                    }
                case DistributionType.Continious:
                    {
                        return DiscreteRandomsMath.Power(Discretize(), (DiscreteDistribution)(ContinuousDistribution)value);
                    }
                default:
                    {
                        throw new DistributionsInvalidOperationException();
                    }
            }
        }

        internal override BaseDistribution InnerGetLog(BaseDistribution nBase)
        {
            switch (nBase.InnerDistributionType)
            {
                case DistributionType.Number:
                    {
                        return CommonRandomsMath.Log(this, (double)nBase);
                    }
                case DistributionType.Continious:
                    {
                        return DiscreteRandomsMath.Log(Discretize(), (DiscreteDistribution)(ContinuousDistribution)nBase);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomsMath.Log(Discretize(), (DiscreteDistribution)nBase);
                    }
                default:
                    {
                        throw new DistributionsInvalidOperationException();
                    }
            }
        }

        internal override BaseDistribution InnerGetAbs()
        {
            return CommonRandomsMath.Abs(this);
        }

        internal override BaseDistribution InnerGetNegate()
        {
            return ContinuousRandomsMath.Negate(this);
        }

        #endregion
    }
}
