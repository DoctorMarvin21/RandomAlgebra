using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomsAlgebra.Distributions
{
    /// <summary>
    /// Mean value with zero variance
    /// </summary>
    public sealed class NumberDistribution : BaseDistribution
    {
        readonly double _value = 0;

        /// <summary>
        /// Creates instance of <see cref="NumberDistribution"/> by mean value
        /// </summary>
        /// <param name="value">Constant mean value</param>
        public NumberDistribution(double value)
        {
            _value = value;
        }

        #region Overriding parameters and functions
        internal override double InnerVariance { get { return 0; } }

        internal override double InnerSkewness { get { return 0; } }

        internal override double InnerMean { get { return _value; } }

        internal override double InnerMaxX { get { return _value; } }

        internal override double InnerMinX { get { return _value; } }

        internal override DistributionType InnerDistributionType { get { return DistributionType.Number; } }

        internal override int InnerSamples { get { return 1; } }//just one sample that is mean

        internal override double InnerGetPDFYbyX(double x)
        {
            if (x == _value)
                return 1;
            else
                return 0;
        }

        internal override double InnerGetCDFYbyX(double x)
        {
            return InnerGetPDFYbyX(x);
        }

        internal override double InnerQuantile(double p)
        {
            if (p == 1)
                return 1;
            else
                return 0;
        }
        #endregion

        #region Operators
        public static implicit operator NumberDistribution(double value)
        {
            return new NumberDistribution(value);
        }

        public static explicit operator double(NumberDistribution value)
        {
            return value.Mean;
        }
        #endregion

        #region Randoms algebra
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
                        return ContinuousRandomsMath.Add((ContinuousDistribution)value, Mean);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomsMath.Add((DiscreteDistribution)value, Mean);
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
                        return ContinuousRandomsMath.Sub(Mean, (ContinuousDistribution)value);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomsMath.Sub(Mean, (DiscreteDistribution)value);
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
                        return ContinuousRandomsMath.Multiply((ContinuousDistribution)value, Mean);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomsMath.Multiply((DiscreteDistribution)value, Mean);
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
                        return CommonRandomsMath.Divide(Mean, value);
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
                        return CommonRandomsMath.Power(Mean, value);
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
                        return CommonRandomsMath.Log(Mean, nBase);
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
