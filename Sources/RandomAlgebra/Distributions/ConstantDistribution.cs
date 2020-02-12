using System;

namespace RandomAlgebra.Distributions
{
    /// <summary>
    /// Mean value with zero variance.
    /// </summary>
    public sealed class ConstantDistribution : BaseDistribution
    {
        private readonly double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantDistribution"/> class
        /// by mean value.
        /// </summary>
        /// <param name="value">Constant mean value.</param>
        public ConstantDistribution(double value)
        {
            this.value = value;
        }

        #region Overriding parameters

        internal override double InnerVariance { get { return 0; } }

        internal override double InnerSkewness { get { return 0; } }

        internal override double InnerMean { get { return value; } }

        internal override double InnerMaxX { get { return value; } }

        internal override double InnerMinX { get { return value; } }

        internal override DistributionType InnerDistributionType { get { return DistributionType.Number; } }

        // Just for correct step
        internal override int InnerSamples { get { return 1; } }

        #endregion

        #region Operators

        public static implicit operator ConstantDistribution(double value)
        {
            return new ConstantDistribution(value);
        }

        public static explicit operator double(ConstantDistribution value)
        {
            return value.Mean;
        }

        #endregion

        #region Overriding functions

        internal override double InnerGetPDFYbyX(double x)
        {
            if (x == value)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        internal override double InnerGetCDFYbyX(double x)
        {
            return InnerGetPDFYbyX(x);
        }

        internal override double InnerQuantile(double p)
        {
            return value;
        }

        #endregion

        #region Random algebra

        internal override BaseDistribution InnerGetSumm(BaseDistribution value)
        {
            switch (value.InnerDistributionType)
            {
                case DistributionType.Number:
                    {
                        return Mean + value.InnerMean;
                    }
                case DistributionType.Continious:
                    {
                        return ContinuousRandomMath.Add((ContinuousDistribution)value, Mean);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomMath.Add((DiscreteDistribution)value, Mean);
                    }
                default:
                    {
                        throw new DistributionsInvalidOperationException();
                    }
            }
        }

        internal override BaseDistribution InnerGetDifference(BaseDistribution value)
        {
            switch (value.InnerDistributionType)
            {
                case DistributionType.Number:
                    {
                        return Mean - value.InnerMean;
                    }
                case DistributionType.Continious:
                    {
                        return ContinuousRandomMath.Sub(Mean, (ContinuousDistribution)value);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomMath.Sub(Mean, (DiscreteDistribution)value);
                    }
                default:
                    {
                        throw new DistributionsInvalidOperationException();
                    }
            }
        }

        internal override BaseDistribution InnerGetProduct(BaseDistribution value)
        {
            switch (value.InnerDistributionType)
            {
                case DistributionType.Number:
                    {
                        return Mean * value.InnerMean;
                    }
                case DistributionType.Continious:
                    {
                        return ContinuousRandomMath.Multiply((ContinuousDistribution)value, Mean);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomMath.Multiply((DiscreteDistribution)value, Mean);
                    }
                default:
                    {
                        throw new DistributionsInvalidOperationException();
                    }
            }
        }

        internal override BaseDistribution InnerGetRatio(BaseDistribution value)
        {
            switch (value.InnerDistributionType)
            {
                case DistributionType.Number:
                    {
                        return Mean / value.InnerMean;
                    }
                case DistributionType.Continious:
                case DistributionType.Discrete:
                    {
                        return CommonRandomMath.Divide(Mean, value);
                    }
                default:
                    {
                        throw new DistributionsInvalidOperationException();
                    }
            }
        }

        internal override BaseDistribution InnerGetPower(BaseDistribution value)
        {
            switch (value.InnerDistributionType)
            {
                case DistributionType.Number:
                    {
                        return Math.Pow(Mean, (double)value);
                    }
                case DistributionType.Discrete:
                case DistributionType.Continious:
                    {
                        return CommonRandomMath.Power(Mean, value);
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
                        return Math.Log(Mean, (double)nBase);
                    }
                case DistributionType.Discrete:
                case DistributionType.Continious:
                    {
                        return CommonRandomMath.Log(Mean, nBase);
                    }
                default:
                    {
                        throw new DistributionsInvalidOperationException();
                    }
            }
        }

        internal override BaseDistribution InnerGetAbs()
        {
            return Math.Abs(Mean);
        }

        internal override BaseDistribution InnerGetNegate()
        {
            return -Mean;
        }

        #endregion
    }
}
