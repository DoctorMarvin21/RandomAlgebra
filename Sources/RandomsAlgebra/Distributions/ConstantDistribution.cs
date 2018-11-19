using RandomAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomAlgebra.Distributions
{
    /// <summary>
    /// Mean value with zero variance
    /// </summary>
    public sealed class ConstantDistribution : BaseDistribution
    {
        readonly double _value = 0;

        /// <summary>
        /// Creates instance of <see cref="ConstantDistribution"/> by mean value
        /// </summary>
        /// <param name="value">Constant mean value</param>
        public ConstantDistribution(double value)
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

        internal override int InnerSamples { get { return 1; } }//just for correct step

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
            return _value;
        }
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
