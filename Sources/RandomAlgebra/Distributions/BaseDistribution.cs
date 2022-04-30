using System;

namespace RandomAlgebra.Distributions
{
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

    /// <summary>
    /// Base class for distribution.
    /// </summary>
    public abstract class BaseDistribution
    {
        private double? step;

        #region Public properties

        /// <summary>
        /// Minimum of random.
        /// </summary>
        public abstract double MinX { get; }

        /// <summary>
        /// Maximum of random.
        /// </summary>
        public abstract double MaxX { get; }

        /// <summary>
        /// Samples count.
        /// </summary>
        public abstract int Samples { get; }

        /// <summary>
        /// Step of sampling.
        /// </summary>
        public double Step
        {
            get
            {
                if (step == null)
                {
                    step = (MaxX - MinX) / (Samples - 1);

                    if (step < 0)
                    {
                        throw new DistributionsArgumentException(DistributionsArgumentExceptionType.NegativeStep);
                    }
                }

                return step.Value;
            }
        }

        /// <summary>
        /// Valiance, the second moment.
        /// </summary>
        public abstract double Variance { get; }

        /// <summary>
        /// Skewness, the third moment.
        /// </summary>
        public abstract double Skewness { get; }

        /// <summary>
        /// Expected value.
        /// </summary>
        public abstract double Mean { get; }

        /// <summary>
        /// Standard deviation, root of variance.
        /// </summary>
        public double StandardDeviation => Math.Sqrt(Variance);

        /// <summary>
        /// DistributionType: continuous (<see cref="ContinuousDistribution"/>),
        /// discrete (<see cref="DiscreteDistribution"/>) of number with zero variance (<see cref="ConstantDistribution"/>).
        /// </summary>
        public abstract DistributionType DistributionType { get; }

        #endregion

        #region Operators overriding

        public static implicit operator BaseDistribution(double value)
        {
            return new ConstantDistribution(value);
        }

        public static explicit operator double(BaseDistribution value)
        {
            return value.Mean;
        }

        public static BaseDistribution operator +(BaseDistribution left, BaseDistribution right)
        {
            return left.Sum(right);
        }

        public static BaseDistribution operator -(BaseDistribution left, BaseDistribution right)
        {
            return left.Difference(right);
        }

        public static BaseDistribution operator *(BaseDistribution left, BaseDistribution right)
        {
            return left.Product(right);
        }

        public static BaseDistribution operator /(BaseDistribution left, BaseDistribution right)
        {
            return left.Ratio(right);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Upper quantile as (p+1)/2.
        /// </summary>
        /// <param name="p">Probability in range [0, 1].</param>
        /// <returns>Upper quantile.</returns>
        public double QuantileUpper(double p)
        {
            double pc = (p + 1) / 2d;
            return Quantile(pc);
        }

        /// <summary>
        /// Lower quantile as (1-p)/2.
        /// </summary>
        /// <param name="p">Probability in range [0, 1].</param>
        /// <returns>Lower quantile.</returns>
        public double QuantileLower(double p)
        {
            double pc = (1 - p) / 2d;
            return Quantile(pc);
        }

        /// <summary>
        /// Mean between upper and lower quantiles.
        /// </summary>
        /// <param name="p">Probability in range [0, 1].</param>
        /// <returns>Quantile range.</returns>
        public double QuantileRange(double p)
        {
            double pHigh = (p + 1) / 2d;
            double pLow = (1 - p) / 2d;

            return (Quantile(pHigh) - Quantile(pLow)) / 2d;
        }

        /// <summary>
        /// One-sided quantile (confidence interval).
        /// </summary>
        /// <param name="p">Probability in range [0, 1].</param>
        /// <returns>One-sided quantile.</returns>
        public abstract double Quantile(double p);

        /// <summary>
        /// Returns value of probability density function in point x,
        /// if can't find exact value, returns linear interpolated,
        /// if argument out of range between <see cref="MinX"/> and <see cref="MaxX"/> returns zero.
        /// </summary>
        /// <param name="x">Argument of probability density function.</param>
        /// <returns>Probability density.</returns>
        public abstract double ProbabilityDensityFunction(double x);

        /// <summary>
        /// Returns value of distribution function in point x,
        /// if can't find exact value, returns linear interpolated,
        /// if argument out of range between <see cref="MinX"/> and <see cref="MaxX"/> returns zero.
        /// </summary>
        /// <param name="x">Argument of distribution function.</param>
        /// <returns>Distribution.</returns>
        public abstract double DistributionFunction(double x);

        /// <summary>
        /// Sum (convolution) of distributions.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Sum result.</returns>
        public abstract BaseDistribution Sum(BaseDistribution value);

        /// <summary>
        /// Difference between distributions.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Difference result.</returns>
        public abstract BaseDistribution Difference(BaseDistribution value);

        /// <summary>
        /// Distributions product.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Product result.</returns>
        public abstract BaseDistribution Product(BaseDistribution value);

        /// <summary>
        /// Distributions ratio.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Ratio result.</returns>
        public abstract BaseDistribution Ratio(BaseDistribution value);

        /// <summary>
        /// Returns distribution in power of <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Power result.</returns>
        public abstract BaseDistribution Power(BaseDistribution value);

        /// <summary>
        /// Logarithm with base <paramref name="nBase"/>.
        /// </summary>
        /// <param name="nBase">Base of logarithm.</param>
        /// <returns>Log result.</returns>
        public abstract BaseDistribution Log(BaseDistribution nBase);

        /// <summary>
        /// Absolute value of distribution.
        /// </summary>
        /// <returns>Absolute value.</returns>
        public abstract BaseDistribution Abs();

        /// <summary>
        /// Product of distribution and -1.
        /// </summary>
        /// <returns>Product result.</returns>
        public abstract BaseDistribution Negate();

        #endregion
    }
}
