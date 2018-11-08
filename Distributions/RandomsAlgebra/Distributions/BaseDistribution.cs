using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomsAlgebra.Distributions
{
    /// <summary>
    /// Base class for distribution
    /// </summary>
    public abstract class BaseDistribution
    {

        #region Abstracts
        internal abstract double InnerGetPDFYbyX(double x);
        internal abstract double InnerGetCDFYbyX(double x);


        internal abstract double InnerMinX { get; }
        internal abstract double InnerMaxX { get; }

        internal abstract double InnerMean { get; }
        internal abstract double InnerVariance { get; }
        internal abstract double InnerSkewness { get; }
        internal abstract int InnerSamples { get; }

        internal abstract double InnerQuantile(double p);

        internal abstract DistributionType InnerDistributionType { get; }
        internal abstract BaseDistribution InnerGetSumm(BaseDistribution value);
        internal abstract BaseDistribution InnerGetDifference(BaseDistribution value);
        internal abstract BaseDistribution InnerGetProduct(BaseDistribution value);
        internal abstract BaseDistribution InnerGetRatio(BaseDistribution value);
        internal abstract BaseDistribution InnerGetPower(BaseDistribution value);
        internal abstract BaseDistribution InnerGetLog(BaseDistribution nBase);
        internal abstract BaseDistribution InnerGetAbs();
        internal abstract BaseDistribution InnerGetNegate();

        #endregion

        #region Operators overriding
        public static BaseDistribution operator +(BaseDistribution left, BaseDistribution right)
        {
            return left.InnerGetSumm(right);
        }
        public static BaseDistribution operator -(BaseDistribution left, BaseDistribution right)
        {
            return left.InnerGetDifference(right);
        }

        public static BaseDistribution operator *(BaseDistribution left, BaseDistribution right)
        {
            return left.InnerGetProduct(right);
        }

        public static BaseDistribution operator /(BaseDistribution left, BaseDistribution right)
        {
            return left.InnerGetRatio(right);
        }

        public static implicit operator BaseDistribution(double value)
        {
            return new NumberDistribution(value);
        }
        public static explicit operator double(BaseDistribution value)
        {
            return value.Mean;
        }
        #endregion

        #region Publics
        double? _step = null;

        /// <summary>
        /// Minimum of random
        /// </summary>
        public double MinX { get { return InnerMinX; } }

        /// <summary>
        /// Maximum of random
        /// </summary>
        public double MaxX { get { return InnerMaxX; } }

        /// <summary>
        /// Samples count
        /// </summary>
        public int Samples { get { return InnerSamples; } }

        /// <summary>
        /// Step of sampling
        /// </summary>
        public double Step
        {
            get
            {
                if (_step == null)
                {
                    _step = (InnerMaxX - InnerMinX) / (InnerSamples - 1);

                    if (_step < 0)
                        throw new DistributionsArgumentException("Negative step", "Отрицательный шаг");
                }

                return _step.Value;
            }
        }

        /// <summary>
        /// Valiance, the second moment
        /// </summary>
        public double Variance { get { return InnerVariance; } }

        /// <summary>
        /// Skewness, the third moment
        /// </summary>
        public double Skewness { get { return InnerSkewness; } }

        /// <summary>
        /// Expected value
        /// </summary>
        public double Mean { get { return InnerMean; } }

        /// <summary>
        /// Standard deviation, root of variance
        /// </summary>
        public double StandardDeviation { get { return Math.Sqrt(InnerVariance); } }

        /// <summary>
        /// DistributionType: contiuuous (<see cref="ContinuousDistribution"/>), discrete (<see cref="DiscreteDistribution"/>) of number with zero variance (<see cref="NumberDistribution"/>)
        /// </summary>
        public DistributionType DistributionType { get { return InnerDistributionType; } }

        /// <summary>
        /// Upper quantile as (p+1)/2
        /// </summary>
        /// <param name="p">Probability in range [0, 1]</param>
        /// <returns>Upper quantile</returns>
        public double QuantileUpper(double p)
        {

            double pc = (p + 1) / 2d;
            return Quantile(pc);
        }

        /// <summary>
        /// Lower quantile as (1-p)/2
        /// </summary>
        /// <param name="p">Probability in range [0, 1]</param>
        /// <returns>Lower quantile</returns>
        public double QuantileLower(double p)
        {

            double pc = (1 - p) / 2d;
            return Quantile(pc);
        }

        /// <summary>
        /// Mean between upper and lower quantiles
        /// </summary>
        /// <param name="p">Probability in range [0, 1]</param>
        /// <returns>Quantile range</returns>
        public double QuantileRange(double p)
        {
            double pHigh = (p + 1) / 2d;
            double pLow = (1 - p) / 2d;

            return (Quantile(pHigh) - Quantile(pLow)) / 2d;
        }

        /// <summary>
        /// One-sided quantile (confidence interval)
        /// </summary>
        /// <param name="p">Probability in range [0, 1]</param>
        /// <returns>One-sided quantile</returns>
        public double Quantile(double p)
        {
            if (p < 0 || p > 1)
                throw new DistributionsArgumentException("Probability must be in range [0, 1]", "Вероятность должна находиться в пределах от 0 до 1");

            return InnerQuantile(p);
        }

        /// <summary>
        /// Returns value of probability density function in point x,
        /// if can't find exact value, returns linear interpolated,
        /// if argument out of range between <see cref="MinX"/> and <see cref="MaxX"/> returns zero
        /// </summary>
        /// <param name="x">Argument of probability density function</param>
        /// <returns>Probability density</returns>
        public double ProbabilityDensityFunction(double x) { return InnerGetPDFYbyX(x); }

        /// <summary>
        /// Returns value of distribution function in point x,
        /// if can't find exact value, returns linear interpolated,
        /// if argument out of range between <see cref="MinX"/> and <see cref="MaxX"/> returns zero
        /// </summary>
        /// <param name="x">Argument of distribution function</param>
        /// <returns>Distribution</returns>
        public double DistributionFunction(double x) { return InnerGetCDFYbyX(x); }

        /// <summary>
        /// Sum (convolution) of distributions, result of function depends on parameters of optimization
        /// <see cref="Optimizations.UseContiniousConvolution"/> and <see cref="Optimizations.UseFFTConvolution"/>
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Sum result</returns>
        public BaseDistribution Sum(BaseDistribution value) { return InnerGetSumm(value); }

        /// <summary>
        /// Difference between distributions, result of function depends on parameters of optimization
        /// <see cref="Optimizations.UseContiniousConvolution"/> and <see cref="Optimizations.UseFFTConvolution"/>
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Difference result</returns>
        public BaseDistribution Difference(BaseDistribution value) { return InnerGetDifference(value); }

        /// <summary>
        /// Distributions product
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Product result</returns>
        public BaseDistribution Product(BaseDistribution value) { return InnerGetProduct(value); }

        /// <summary>
        /// Distributions ratio
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Ratio result</returns>
        public BaseDistribution Ratio(BaseDistribution value) { return InnerGetRatio(value); }

        /// <summary>
        /// Returns distribution in power of <paramref name="value"/>
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Power result</returns>
        public BaseDistribution Power(BaseDistribution value) { return InnerGetPower(value); }

        /// <summary>
        /// Logarithm with base <paramref name="nBase"/>
        /// </summary>
        /// <param name="nBase">Base of logarithm</param>
        /// <returns>Log result</returns>
        public BaseDistribution Log(BaseDistribution nBase) { return InnerGetLog(nBase); }

        /// <summary>
        /// Absolute value of distribution
        /// </summary>
        /// <returns>Absolute value</returns>
        public BaseDistribution Abs() { return InnerGetAbs(); }

        /// <summary>
        /// Product of distribution and -1
        /// </summary>
        /// <returns>Product result</returns>
        public BaseDistribution Negate() { return InnerGetNegate(); }
        #endregion
    }

    public enum DistributionType
    {
        /// <summary>
        /// Discrete distribution
        /// </summary>
        Discrete,
        /// <summary>
        /// Continuous distribution
        /// </summary>
        Continious,
        /// <summary>
        /// Value with zero variance
        /// </summary>
        Number
    }
}
