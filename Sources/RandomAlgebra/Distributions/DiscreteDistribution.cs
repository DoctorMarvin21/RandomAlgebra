using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RandomAlgebra.Distributions.Settings;

namespace RandomAlgebra.Distributions
{
    /// <summary>
    /// Discrete distribution as sampling of continuous distribution.
    /// </summary>
    public class DiscreteDistribution : BaseDistribution
    {
        private readonly double min;
        private readonly double max;
        private readonly int length;
        private double[] cdfCoordinates;
        private ReadOnlyCollection<double> xCoordinatesReadonly;
        private ReadOnlyCollection<double> yCoordinatesReadonly;
        private ReadOnlyCollection<double> cdfCoordinatesReadonly;
        private double? mean;
        private double? variance;
        private double? skewness;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscreteDistribution"/> class
        /// by <paramref name="continiousDistribution"/> by sampling.
        /// </summary>
        /// <param name="continiousDistribution">Sampled <see cref="ContinuousDistribution"/>.</param>
        public DiscreteDistribution(ContinuousDistribution continiousDistribution)
            : this(continiousDistribution, continiousDistribution.InnerSamples)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscreteDistribution"/> class
        /// by <paramref name="continiousDistribution"/> by sampling with <paramref name="samples"/> count.
        /// </summary>
        /// <param name="continiousDistribution">Sampled <see cref="ContinuousDistribution"/>.</param>
        /// <param name="samples">Samples count.</param>
        public DiscreteDistribution(ContinuousDistribution continiousDistribution, int samples)
            : this(DiscretizeContinious(continiousDistribution, samples))
        {
        }

        internal DiscreteDistribution(double[] xCoordinates, double[] yCoordinates, double[] cdfCoordinates = null)
            : this(new PrivateCoordinates { XCoordinates = xCoordinates, PDFCoordinates = yCoordinates, CDFCoordinates = cdfCoordinates })
        {
        }

        private DiscreteDistribution(PrivateCoordinates coordinates)
        {
            if (coordinates.XCoordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates.XCoordinates));
            }

            if (coordinates.PDFCoordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates.PDFCoordinates));
            }

            if (coordinates.XCoordinates.Length < 3)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.LengthOfArgumentsMustBeGreaterThenTwo);
            }

            if (coordinates.PDFCoordinates.Length < 3)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.LengthOfValuesMustBeGreaterThenTwo);
            }

            if (coordinates.XCoordinates.Length != coordinates.PDFCoordinates.Length)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.LengthOfArgumentsMustBeEqualToLengthOfValues);
            }

            if (!coordinates.FromContinuous && coordinates.CDFCoordinates == null)
            {
                coordinates = Resample(coordinates);
            }

            XCoordinatesInternal = coordinates.XCoordinates;
            YCoordinatesInternal = coordinates.PDFCoordinates;
            cdfCoordinates = coordinates.CDFCoordinates;

            length = XCoordinatesInternal.Length;
            min = XCoordinatesInternal[0];
            max = XCoordinatesInternal[length - 1];

            if (coordinates.CDFCoordinates == null)
            {
                bool gotInf = AnalyzeInfinities(YCoordinatesInternal, Step);
                if (!coordinates.FromContinuous || gotInf)
                {
                    Normalize(YCoordinatesInternal, Step);
                }
            }
        }
        #endregion

        #region Coordinates

        /// <summary>
        /// Arguments of distribution function and probability density.
        /// </summary>
        public ReadOnlyCollection<double> FunctionArguments
        {
            get
            {
                if (xCoordinatesReadonly == null)
                {
                    xCoordinatesReadonly = new ReadOnlyCollection<double>(XCoordinatesInternal);
                }
                return xCoordinatesReadonly;
            }
        }

        /// <summary>
        /// Sampled values of probability density by arguments <see cref="FunctionArguments"/>.
        /// </summary>
        public ReadOnlyCollection<double> ProbabilityDensityValues
        {
            get
            {
                if (yCoordinatesReadonly == null)
                {
                    yCoordinatesReadonly = new ReadOnlyCollection<double>(YCoordinatesInternal);
                }
                return yCoordinatesReadonly;
            }
        }

        /// <summary>
        /// Sampled values of distribution function by arguments <see cref="FunctionArguments"/>.
        /// </summary>
        public ReadOnlyCollection<double> CumulativeDistributionValues
        {
            get
            {
                if (cdfCoordinatesReadonly == null)
                {
                    cdfCoordinatesReadonly = new ReadOnlyCollection<double>(CDFCoordinatesInternal);
                }

                return cdfCoordinatesReadonly;
            }
        }

        internal double[] XCoordinatesInternal
        {
            get;
        }

        internal double[] YCoordinatesInternal
        {
            get;
        }

        internal double[] CDFCoordinatesInternal
        {
            get
            {
                if (cdfCoordinates == null)
                {
                    cdfCoordinates = GetCDF(YCoordinatesInternal, Step);
                }

                return cdfCoordinates;
            }
        }

        #endregion

        #region Override settings

        internal override double InnerMinX
        {
            get
            {
                return min;
            }
        }

        internal override double InnerMaxX
        {
            get
            {
                return max;
            }
        }

        internal override double InnerMean
        {
            get
            {
                if (mean == null)
                {
                    mean = GetMoment(1, 0);
                }

                return mean.Value;
            }
        }

        internal override double InnerVariance
        {
            get
            {
                if (variance == null)
                {
                    variance = GetMoment(2, InnerMean);
                }

                return variance.Value;
            }
        }

        internal override double InnerSkewness
        {
            get
            {
                if (skewness == null)
                {
                    double m3 = GetMoment(3, InnerMean);
                    double s3 = Math.Pow(Variance, 3d / 2d);
                    skewness = m3 / s3;
                }

                return skewness.Value;
            }
        }

        internal override DistributionType InnerDistributionType
        {
            get
            {
                return DistributionType.Discrete;
            }
        }

        internal override int InnerSamples
        {
            get
            {
                return length;
            }
        }

        #endregion

        #region Override functions

        internal override double InnerGetPDFYbyX(double x)
        {
            return GetYByX(x, YCoordinatesInternal);
        }

        internal override double InnerGetCDFYbyX(double x)
        {
            return GetYByX(x, CDFCoordinatesInternal);
        }

        internal override double InnerQuantile(double p)
        {
            double[] cdf = CDFCoordinatesInternal;
            double[] xCoordinates = XCoordinatesInternal;

            if (p < cdf[0])
            {
                return double.NegativeInfinity;
            }
            else if (p > cdf[length - 1])
            {
                return double.PositiveInfinity;
            }
            else
            {
                for (int i = 0; i < length - 1; i++)
                {
                    double x0 = cdf[i];
                    double x1 = cdf[i + 1];

                    if (x0 <= p && x1 >= p)
                    {
                        double y0 = xCoordinates[i];
                        double y1 = xCoordinates[i + 1];
                        double result = CommonRandomMath.InterpolateLinear(x0, x1, y0, y1, p);
                        return result;
                    }
                }

                return double.NaN;
            }
        }

        #endregion

        #region Random algebra

        internal override BaseDistribution InnerGetSumm(BaseDistribution value)
        {
            switch (value.InnerDistributionType)
            {
                case DistributionType.Number:
                    {
                        return DiscreteRandomMath.Add(this, (double)value);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomMath.Add(this, (DiscreteDistribution)value);
                    }
                case DistributionType.Continious:
                    {
                        if (Optimizations.UseFftConvolution)
                        {
                            return FFT.Convolute(this, (ContinuousDistribution)value);
                        }
                        else
                        {
                            return DiscreteRandomMath.Add(this, (DiscreteDistribution)(ContinuousDistribution)value);
                        }
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
                        return DiscreteRandomMath.Sub(this, (double)value);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomMath.Sub(this, (DiscreteDistribution)value);
                    }
                case DistributionType.Continious:
                    {
                        if (Optimizations.UseFftConvolution)
                        {
                            return FFT.Convolute(this, (ContinuousDistribution)(value * -1));
                        }
                        else
                        {
                            return DiscreteRandomMath.Sub(this, (DiscreteDistribution)(ContinuousDistribution)value);
                        }
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
                        return DiscreteRandomMath.Multiply(this, (double)value);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomMath.Multiply(this, (DiscreteDistribution)value);
                    }
                case DistributionType.Continious:
                    {
                        return DiscreteRandomMath.Multiply(this, (DiscreteDistribution)(ContinuousDistribution)value);
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
                        return DiscreteRandomMath.Divide(this, (double)value);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomMath.Divide(this, (DiscreteDistribution)value);
                    }
                case DistributionType.Continious:
                    {
                        return DiscreteRandomMath.Divide(this, (DiscreteDistribution)(ContinuousDistribution)value);
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
                        return CommonRandomMath.Power(this, (double)value);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomMath.Power(this, (DiscreteDistribution)value);
                    }
                case DistributionType.Continious:
                    {
                        return DiscreteRandomMath.Power(this, (DiscreteDistribution)(ContinuousDistribution)value);
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
                        return CommonRandomMath.Log(this, (double)nBase);
                    }
                case DistributionType.Continious:
                    {
                        return DiscreteRandomMath.Log(this, (DiscreteDistribution)(ContinuousDistribution)nBase);
                    }
                case DistributionType.Discrete:
                    {
                        return DiscreteRandomMath.Log(this, (DiscreteDistribution)nBase);
                    }
                default:
                    {
                        throw new DistributionsInvalidOperationException();
                    }
            }
        }

        internal override BaseDistribution InnerGetAbs()
        {
            return CommonRandomMath.Abs(this);
        }

        internal override BaseDistribution InnerGetNegate()
        {
            return DiscreteRandomMath.Negate(this);
        }

        #endregion

        #region Constructor functions

        private static PrivateCoordinates Resample(PrivateCoordinates coordinates)
        {
            var pdf = coordinates.PDFCoordinates;
            int length = coordinates.XCoordinates.Length;

            double min = coordinates.XCoordinates[0];
            double max = coordinates.XCoordinates[length - 1];

            double step = (max - min) / (length - 1);

            AnalyzeInfinities(pdf, step);
            Normalize(pdf, step);

            double[] cdf = GetCDF(pdf, step);
            double tolerance = CommonRandomMath.GetTolerance(length);

            var minI = 0;
            var maxI = length - 1;

            for (int i = 0; i < length - 1; i++)
            {
                var c = cdf[i];

                if (c < tolerance)
                {
                    if (pdf[i] == 0)
                    {
                        minI = i + 1;
                    }
                    else
                    {
                        minI = i;
                    }
                }
                if (c > 1 - tolerance)
                {
                    if (pdf[i] == 0)
                    {
                        maxI = i - 1;
                    }
                    else
                    {
                        maxI = i;
                    }
                    break;
                }
            }

            min = coordinates.XCoordinates[minI];
            max = coordinates.XCoordinates[maxI];

            int r = maxI - minI + 1;

            if (r != length)
            {
                double[] newX = CommonRandomMath.GenerateXAxis(min, max, length, out _);
                double[] newY = new double[r];

                for (int i = 0; i < r; i++)
                {
                    newY[i] = coordinates.PDFCoordinates[i + minI];
                }
                newY = CommonRandomMath.Resample(newY, length);

                return new PrivateCoordinates { XCoordinates = newX, PDFCoordinates = newY };
            }
            else
            {
                return coordinates;
            }
        }

        private static bool AnalyzeInfinities(double[] yCoordinates, double step)
        {
            bool gotInfinities = false;
            int l = yCoordinates.Length;

            // Replace NaN, find scale, find infinities
            double scale = 0;

            List<int> infIndexes = new List<int>();
            List<double> weights = new List<double>();
            int trianglesCount = 0;

            for (int i = 0; i < l; i++)
            {
                double value = yCoordinates[i];

                if (double.IsNaN(value))
                {
                    // TODO: interpolation?
                    yCoordinates[i] = 0;
                }
                else if (double.IsInfinity(value))
                {
                    yCoordinates[i] = 0;
                    infIndexes.Add(i);
                    if (i == 0 || i == l - 1)
                    {
                        trianglesCount++;

                        if (i == 0)
                        {
                            weights.Add(yCoordinates[i + 1]);
                        }
                        else
                        {
                            weights.Add(yCoordinates[i - 1]);
                        }
                    }
                    else
                    {
                        trianglesCount += 2;
                        weights.Add(yCoordinates[i - 1] + yCoordinates[i + 1]);
                    }
                }
                else
                {
                    if (i == 0 || i == l - 1)
                    {
                        value /= 2d;
                    }

                    scale += value;
                }
            }
            scale *= step;

            if (trianglesCount > 0)
            {
                gotInfinities = true;
                CalculationProgress.InvokeWarning(WarningType.InifinityEliminated, infIndexes.Count);
            }

            if (scale < 1)
            {
                double area = (1 - scale) / trianglesCount;

                double h = (2 * area) / step / weights.Sum();

                for (int i = 0; i < infIndexes.Count; i++)
                {
                    int infIndex = infIndexes[i];
                    double value = h * weights[i];

                    if (infIndex == 0)
                    {
                        double vNext = yCoordinates[1];

                        if (value < vNext)
                        {
                            value = vNext;
                        }
                    }
                    else if (infIndex == l - 1)
                    {
                        double vPrev = yCoordinates[l - 1];

                        if (value < vPrev)
                        {
                            value = vPrev;
                        }
                    }
                    else
                    {
                        double vPrev = yCoordinates[infIndex - 1];
                        double vNext = yCoordinates[infIndex + 1];
                        double vMean = (vNext + vPrev) / 2d;

                        if (value < vPrev || value < vNext)
                        {
                            value = vMean;
                        }
                    }

                    yCoordinates[infIndex] = value;
                }
            }

            return gotInfinities;
        }

        private static void Normalize(double[] yCoordinates, double step)
        {
            int l = yCoordinates.Length;
            double scale = 0;

            for (int i = 0; i < l; i++)
            {
                double value = yCoordinates[i];

                if (i == 0 || i == l - 1)
                {
                    value /= 2d;
                }

                scale += value;
            }
            scale *= step;

            for (int i = 0; i < l; i++)
            {
                yCoordinates[i] = yCoordinates[i] / scale;
            }
        }

        private static PrivateCoordinates DiscretizeContinious(ContinuousDistribution continiousDistribution, int samples)
        {
            if (samples < 2)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.SamplesNumberMustBeGreaterThenTwo);
            }

            if (continiousDistribution == null)
            {
                throw new ArgumentNullException(nameof(continiousDistribution));
            }

            double[] xAxis = CommonRandomMath.GenerateXAxis(continiousDistribution.InnerMinX, continiousDistribution.InnerMaxX, samples, out _);

            double[] pdf = new double[samples];

            for (int i = 0; i < samples; i++)
            {
                double x = xAxis[i];
                pdf[i] = continiousDistribution.InnerGetPDFYbyX(x);
            }

            return new PrivateCoordinates { XCoordinates = xAxis, PDFCoordinates = pdf, FromContinuous = true };
        }

        #endregion

        #region Private functions

        private static double[] GetCDF(double[] yCoordinates, double step)
        {
            int length = yCoordinates.Length;

            double[] result = new double[length];

            double y = 0;
            double halfStep = step / 2d;

            for (int i = 1; i < length; i++)
            {
                y += (yCoordinates[i] + yCoordinates[i - 1]) * halfStep;

                result[i] = y;
            }

            return result;
        }

        private double GetMoment(double k, double m)
        {
            double[] xCoordinates = XCoordinatesInternal;
            double[] yCoordinates = YCoordinatesInternal;

            double var = 0;

            for (int i = 0; i < length; i++)
            {
                double d = Math.Pow(xCoordinates[i] - m, k) * yCoordinates[i];

                if (i == 0 || i == length - 1)
                {
                    d /= 2d;
                }

                var += d;
            }

            return var * Step;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double GetYByX(double x, double[] coordinates)
        {
            if (x < min || x > max)
            {
                return 0;
            }

            double ind = (x - min) / Step;

            int indInt = (int)ind;

            if (indInt < 0)
            {
                return coordinates[0];
            }
            else if (indInt >= length - 1)
            {
                return coordinates[length - 1];
            }
            else
            {
                double k = ind - indInt;

                if (k == 0)
                {
                    return coordinates[indInt];
                }
                else
                {
                    double min = coordinates[indInt];
                    double max = coordinates[indInt + 1];
                    return ((max - min) * k) + min;
                }
            }
        }

        #endregion

        private class PrivateCoordinates
        {
            public double[] XCoordinates { get; set; }

            public double[] PDFCoordinates { get; set; }

            public double[] CDFCoordinates { get; set; }

            public bool FromContinuous { get; set; }
        }
    }
}