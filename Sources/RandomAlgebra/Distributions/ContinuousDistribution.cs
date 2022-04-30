using System;
using Accord.Math.Integration;
using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.Distributions.CustomDistributions;
using RandomAlgebra.Distributions.Settings;

namespace RandomAlgebra.Distributions
{
    /// <summary>
    /// Continuous distribution.
    /// </summary>
    public sealed class ContinuousDistribution : BaseDistribution
    {
        internal const int DefaultSamples = 1000;

        private readonly int samples = DefaultSamples;
        private double[] support;
        private double? skewness;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuousDistribution"/> class
        /// by <paramref name="distributionSettings"/>.
        /// </summary>
        /// <param name="distributionSettings">Distribution settings.</param>
        public ContinuousDistribution(DistributionSettings distributionSettings)
            : this(distributionSettings?.GetUnivariateContinuousDistribution())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuousDistribution"/> class.
        /// by <paramref name="distributionSettings"/> and samples count.
        /// </summary>
        /// <param name="distributionSettings">Distribution settings.</param>
        /// <param name="samples">Samples count used in method <see cref="Discretize"/>.</param>
        public ContinuousDistribution(DistributionSettings distributionSettings, int samples)
            : this(distributionSettings?.GetUnivariateContinuousDistribution(), samples)
        {
        }

        internal ContinuousDistribution(UnivariateContinuousDistribution baseDistribution)
        {
            BaseDistribution = baseDistribution ?? throw new ArgumentNullException(nameof(baseDistribution));
        }

        internal ContinuousDistribution(UnivariateContinuousDistribution baseDistribution, int samples)
            : this(baseDistribution)
        {
            if (samples < 2)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.SamplesNumberMustBeGreaterThenTwo);
            }

            this.samples = samples;
        }

        internal ContinuousDistribution(UnivariateContinuousDistribution baseDistribution, int samples, double coeff, double offset)
            : this(baseDistribution, samples)
        {
            Coefficient = coeff;
            Offset = offset;
        }

        #endregion

        #region Overrides properties

        public override DistributionType DistributionType => DistributionType.Continious;

        public override double MinX
        {
            get
            {
                if (support == null)
                {
                    support = GetDiscretizationSupport();
                }

                return support[0];
            }
        }

        public override double MaxX
        {
            get
            {
                if (support == null)
                {
                    support = GetDiscretizationSupport();
                }

                return support[1];
            }
        }

        public override int Samples
        {
            get
            {
                return samples;
            }
        }

        public override double Mean
        {
            get
            {
                return (BaseDistribution.Mean * Coefficient) + Offset;
            }
        }

        public override double Variance
        {
            get
            {
                return BaseDistribution.Variance * Math.Pow(Coefficient, 2);
            }
        }

        public override double Skewness
        {
            get
            {
                // 3-d moment integration
                if (skewness == null)
                {
                    skewness = InfiniteAdaptiveGaussKronrod.Integrate(ThirdMoment, MinX, MaxX) / Math.Pow(Variance, 3d / 2);
                }

                return skewness.Value;
            }
        }

        #endregion

        #region Settings

        internal UnivariateContinuousDistribution BaseDistribution { get; }

        internal double Coefficient { get; } = 1;

        internal double Offset { get; }

        #endregion

        #region Sampling

        public static explicit operator DiscreteDistribution(ContinuousDistribution continious)
        {
            return continious.Discretize();
        }

        /// <summary>
        /// Performs sampling to discrete distributions with number of samples <see cref="Samples"/>.
        /// </summary>
        /// <returns>Discrete distribution.</returns>
        public DiscreteDistribution Discretize()
        {
            return Discretize(samples);
        }

        /// <summary>
        /// Performs sampling to discrete distributions with number of samples <paramref name="samples"/>.
        /// </summary>
        /// <param name="samples">Samples.</param>
        /// <returns>Discrete distribution.</returns>
        public DiscreteDistribution Discretize(int samples)
        {
            return new DiscreteDistribution(this, samples);
        }

        #endregion

        #region Overrides functions

        public override double ProbabilityDensityFunction(double x)
        {
            try
            {
                if (x < MinX || x > MaxX)
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
                    return BaseDistribution.ProbabilityDensityFunction((x - Offset) / Coefficient) / Math.Abs(Coefficient);
                }
            }
            catch
            {
                return 0;
            }
        }

        public override double DistributionFunction(double x)
        {
            try
            {
                if (x < MinX || x > MaxX)
                {
                    return 0;
                }
                else
                {
                    return BaseDistribution.DistributionFunction((x - Offset) / Coefficient);
                }
            }
            catch
            {
                return 0;
            }
        }

        public override double Quantile(double p)
        {
            if (p < 0 || p > 1)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ProbabilityMustBeInRangeFromZeroToOne);
            }

            return (BaseDistribution.InverseDistributionFunction(p) * Coefficient) + Offset;
        }

        #endregion

        #region Random algebra

        public override BaseDistribution Sum(BaseDistribution value)
        {
            switch (value.DistributionType)
            {
                case DistributionType.Continious:
                    {
                        var right = (ContinuousDistribution)value;

                        if (BaseDistribution is NormalDistribution && right.BaseDistribution is NormalDistribution)
                        {
                            return ContinuousRandomMath.ConvolutionOfNormalAndNormal(this, right);
                        }
                        else if (BaseDistribution is UniformContinuousDistribution && right.BaseDistribution is UniformContinuousDistribution)
                        {
                            return ContinuousRandomMath.ConvolutionOfUniformAndUniform(this, right);
                        }
                        else if (BaseDistribution is NormalDistribution && right.BaseDistribution is UniformContinuousDistribution)
                        {
                            return ContinuousRandomMath.ConvolutionOfNormalAndUniform(this, right);
                        }
                        else if (BaseDistribution is UniformContinuousDistribution && right.BaseDistribution is NormalDistribution)
                        {
                            return ContinuousRandomMath.ConvolutionOfNormalAndUniform(right, this);
                        }
                        else if (BaseDistribution is NormalDistribution && right.BaseDistribution is BhattacharjeeDistribution)
                        {
                            return ContinuousRandomMath.ConvolutionOfNormalAndBhattacharjee(this, right);
                        }
                        else if (BaseDistribution is BhattacharjeeDistribution && right.BaseDistribution is NormalDistribution)
                        {
                            return ContinuousRandomMath.ConvolutionOfNormalAndBhattacharjee(right, this);
                        }
                        else if (BaseDistribution is StudentGeneralizedDistribution && right.BaseDistribution is UniformContinuousDistribution)
                        {
                            return ContinuousRandomMath.ConvolutionOfStudentAndUniform(this, right);
                        }
                        else if (BaseDistribution is UniformContinuousDistribution && right.BaseDistribution is StudentGeneralizedDistribution)
                        {
                            return ContinuousRandomMath.ConvolutionOfStudentAndUniform(right, this);
                        }
                        else
                        {
                            return Discretize() + right.Discretize();
                        }
                    }
                case DistributionType.Discrete:
                    {
                        return Discretize() + value;
                    }
                case DistributionType.Number:
                    {
                        return ContinuousRandomMath.Add(this, (double)value);
                    }
                default:
                    throw new DistributionsInvalidOperationException();
            }
        }

        public override BaseDistribution Difference(BaseDistribution value)
        {
            switch (value.DistributionType)
            {
                case DistributionType.Continious:
                    {
                        return this + (value * -1);
                    }
                case DistributionType.Discrete:
                    {
                        return Discretize() - value;
                    }
                case DistributionType.Number:
                    {
                        return ContinuousRandomMath.Sub(this, (double)value);
                    }
                default:
                    throw new DistributionsInvalidOperationException();
            }
        }

        public override BaseDistribution Product(BaseDistribution value)
        {
            switch (value.DistributionType)
            {
                case DistributionType.Continious:
                case DistributionType.Discrete:
                    {
                        return Discretize() * value;
                    }
                case DistributionType.Number:
                    {
                        if (value.Mean == 0)
                        {
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.MultiplyRandomByZero);
                        }

                        return ContinuousRandomMath.Multiply(this, (double)value);
                    }
                default:
                    throw new DistributionsInvalidOperationException();
            }
        }

        public override BaseDistribution Ratio(BaseDistribution value)
        {
            switch (value.DistributionType)
            {
                case DistributionType.Continious:
                case DistributionType.Discrete:
                    {
                        return Discretize() / value;
                    }
                case DistributionType.Number:
                    {
                        if (value.Mean == 0)
                        {
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.DivisionByZero);
                        }

                        return ContinuousRandomMath.Divide(this, (double)value);
                    }
                default:
                    throw new DistributionsInvalidOperationException();
            }
        }

        public override BaseDistribution Power(BaseDistribution value)
        {
            switch (value.DistributionType)
            {
                case DistributionType.Number:
                    {
                        return CommonRandomMath.Power(this, (double)value);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomMath.Power(Discretize(), (DiscreteDistribution)value);
                    }
                case DistributionType.Continious:
                    {
                        return DiscreteRandomMath.Power(Discretize(), (DiscreteDistribution)(ContinuousDistribution)value);
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
                        return CommonRandomMath.Log(this, (double)nBase);
                    }
                case DistributionType.Continious:
                    {
                        return DiscreteRandomMath.Log(Discretize(), (DiscreteDistribution)(ContinuousDistribution)nBase);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomMath.Log(Discretize(), (DiscreteDistribution)nBase);
                    }
                default:
                    {
                        throw new DistributionsInvalidOperationException();
                    }
            }
        }

        public override BaseDistribution Abs()
        {
            return CommonRandomMath.Abs(this);
        }

        public override BaseDistribution Negate()
        {
            return ContinuousRandomMath.Negate(this);
        }

        #endregion

        #region Private functions

        private double ThirdMoment(double x)
        {
            return Math.Pow(x - Mean, 3) * ProbabilityDensityFunction(x);
        }

        private double[] GetDiscretizationSupport()
        {
            bool minInfinite = double.IsInfinity(BaseDistribution.Support.Min);
            bool maxInfinite = double.IsInfinity(BaseDistribution.Support.Max);

            double tolerance = CommonRandomMath.GetTolerance(Samples);
            double min = minInfinite ? GetSupport(true, tolerance) : BaseDistribution.Support.Min;
            double max = maxInfinite ? GetSupport(false, tolerance) : BaseDistribution.Support.Max;

            min = (min * Coefficient) + Offset;
            max = (max * Coefficient) + Offset;

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
            {
                return BaseDistribution.InverseDistributionFunction(tolerance);
            }
            else
            {
                return BaseDistribution.InverseDistributionFunction(1 - tolerance);
            }
        }

        #endregion
    }
}
