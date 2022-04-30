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

        public override double Variance => 0;

        public override double Skewness => 0;

        public override double Mean => value;

        public override double MaxX => value;

        public override double MinX => value;

        public override DistributionType DistributionType => DistributionType.Number;

        // Just for correct step
        public override int Samples => 1;

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

        public override double ProbabilityDensityFunction(double x)
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

        public override double DistributionFunction(double x)
        {
            return ProbabilityDensityFunction(x);
        }

        public override double Quantile(double p)
        {
            if (p < 0 || p > 1)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ProbabilityMustBeInRangeFromZeroToOne);
            }

            return value;
        }

        #endregion

        #region Random algebra

        public override BaseDistribution Sum(BaseDistribution value)
        {
            switch (value.DistributionType)
            {
                case DistributionType.Number:
                    {
                        return Mean + value.Mean;
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

        public override BaseDistribution Difference(BaseDistribution value)
        {
            switch (value.DistributionType)
            {
                case DistributionType.Number:
                    {
                        return Mean - value.Mean;
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

        public override BaseDistribution Product(BaseDistribution value)
        {
            switch (value.DistributionType)
            {
                case DistributionType.Number:
                    {
                        return Mean * value.Mean;
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

        public override BaseDistribution Ratio(BaseDistribution value)
        {
            switch (value.DistributionType)
            {
                case DistributionType.Number:
                    {
                        return Mean / value.Mean;
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

        public override BaseDistribution Power(BaseDistribution value)
        {
            switch (value.DistributionType)
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

        public override BaseDistribution Log(BaseDistribution nBase)
        {
            switch (nBase.DistributionType)
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

        public override BaseDistribution Abs()
        {
            return Math.Abs(Mean);
        }

        public override BaseDistribution Negate()
        {
            return -Mean;
        }

        #endregion
    }
}
