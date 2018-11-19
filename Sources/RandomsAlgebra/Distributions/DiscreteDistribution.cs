using RandomAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace RandomAlgebra.Distributions
{
    /// <summary>
    /// Discrete distribution as sampling of continuous distribution
    /// </summary>
    public class DiscreteDistribution : BaseDistribution
    {
        readonly double _min = 0;
        readonly int _length = 0;
        readonly double _max = 0;
        double[] _cdfCoordinates = null;
        ReadOnlyCollection<double> _xCoordinatesReadonly = null;
        ReadOnlyCollection<double> _yCoordinatesReadonly = null;
        ReadOnlyCollection<double> _cdfCoordinatesReadonly = null;
        double? _mean = null;
        double? _variance = null;
        double? _skewness = null;

        #region Constructors

        /// <summary>
        /// Creates instance of <see cref="DiscreteDistribution"/> by <paramref name="continiousDistribution"/> by sampling
        /// </summary>
        /// <param name="continiousDistribution">Sampled <see cref="ContinuousDistribution"/></param>
        public DiscreteDistribution(ContinuousDistribution continiousDistribution) : this(continiousDistribution, continiousDistribution.InnerSamples)
        {

        }

        /// <summary>
        /// Creates instance of <see cref="DiscreteDistribution"/> by <paramref name="continiousDistribution"/> by sampling with <paramref name="samples"/> count
        /// </summary>
        /// <param name="continiousDistribution">Sampled <see cref="ContinuousDistribution"/></param>
        /// <param name="samples">Samples count</param>
        public DiscreteDistribution(ContinuousDistribution continiousDistribution, int samples) : this(DiscretizeContinious(continiousDistribution, samples))
        {

        }

        internal DiscreteDistribution(double[] xCoordinates, double[] yCoordinates, double[] cdfCoordinates = null) : this(new PrivateCoordinates { XCoordinates = xCoordinates, YCoordinates = yCoordinates, CDFCoordinates = cdfCoordinates })
        {
        }

        private DiscreteDistribution(PrivateCoordinates coordinates)
        {

            if (coordinates.XCoordinates == null)
                throw new ArgumentNullException(nameof(coordinates.XCoordinates));
            if (coordinates.YCoordinates == null)
                throw new ArgumentNullException(nameof(coordinates.YCoordinates));

            if (coordinates.XCoordinates.Length < 3)
                throw new DistributionsArgumentException("Length of arguments must be greater then 2", "Длина массива аргументов должна быть больше 2");
            if (coordinates.YCoordinates.Length < 3)
                throw new DistributionsArgumentException("Length of values must be greater then 2", "Длина массива значений должна быть больше 2");

            if (coordinates.XCoordinates.Length != coordinates.YCoordinates.Length)
                throw new DistributionsArgumentException("Length of arguments not equal to length of values", "Несовпадение длин массивов аргументов и значений");

            ReplaceInvalidValues(coordinates.YCoordinates);

            if (!coordinates.FromContinuous && coordinates.CDFCoordinates == null)
                coordinates = Resample(coordinates);

            XCoordinatesInternal = coordinates.XCoordinates;
            YCoordinatesInternal = coordinates.YCoordinates;
            _cdfCoordinates = coordinates.CDFCoordinates;


            _length = XCoordinatesInternal.Length;
            _min = XCoordinatesInternal[0];
            _max = XCoordinatesInternal[_length - 1];

            if (coordinates.CDFCoordinates == null)
                Normalize(YCoordinatesInternal, Step);
        }

        private static PrivateCoordinates Resample(PrivateCoordinates coordinates)
        {
            int length = coordinates.XCoordinates.Length;

            double min = coordinates.XCoordinates[0];
            double max = coordinates.XCoordinates[length - 1];

            double step = (max - min) / (length - 1);

            double k = Normalize(coordinates.YCoordinates, step);

            double[] cdf = GetCDF(coordinates.YCoordinates, step);
            double tolerance = 1d / Math.Pow(length * 10, 2);

            var minI = 0;
            var maxI = length - 1;

            for (int i = 0; i < length - 1; i++)
            {
                var d = cdf[i];

                if (d < tolerance)
                    minI = i;
                if (d > 1 - tolerance)
                {
                    maxI = i;
                    break;
                }
            }

            min = coordinates.XCoordinates[minI];
            max = coordinates.XCoordinates[maxI - 1];

            int r = maxI - minI;
            double[] newX = CommonRandomMath.GenerateXAxis(min, max, length, out step);
            double[] newY = new double[r];

            for (int i = 0; i < r; i++)
            {
                newY[i] = coordinates.YCoordinates[i + minI];
            }
            newY = CommonRandomMath.Resample(newY, length);

            return new PrivateCoordinates { XCoordinates = newX, YCoordinates = newY };
        }
        #endregion

        #region Constructor functions
        private static double Normalize(double[] yCoordinates, double step)
        {
            int l = yCoordinates.Length;

            double scale = 0;
            double d = 0;

            for (int i = 0; i < l; i++)
            {
                d = yCoordinates[i];

                if (i == 0 || i == l - 1)
                    d /= 2d;

                scale += d;
            }
            scale *= step;

            for (int i = 0; i < l; i++)
            {
                yCoordinates[i] = yCoordinates[i] / scale;
            }

            return scale;
        }

        private static void ReplaceInvalidValues(double[] arr)
        {
            int l = arr.Length;

            for (int i = 0; i < l; i++)
            {
                double value = arr[i];

                if (double.IsInfinity(value) || double.IsNaN(value))
                {
                    if (i == 0)
                    {
                        arr[0] = 2 * arr[1] - arr[2];
                    }
                    else if (i == l - 1)
                    {
                        arr[l - 1] = 2 * arr[l - 2] - arr[i - 3];
                    }
                    else
                    {
                        if (double.IsInfinity(arr[i + 1]) || double.IsNaN(arr[i + 1]))
                        {
                            arr[i] = 0;
                        }
                        else
                        {
                            arr[i] = (arr[i - 1] + arr[i + 1]) / 2;
                        }
                    }

                }
            }
        }

        private static PrivateCoordinates DiscretizeContinious(ContinuousDistribution continiousDistribution, int samples)
        {
            if (samples < 2)
                throw new DistributionsArgumentException("Samples count must be greater then 2", "Число отсчётов должно быть больше 2");

            if (continiousDistribution == null)
                throw new ArgumentNullException(nameof(continiousDistribution));
			double step;
            double[] xAxis = CommonRandomMath.GenerateXAxis(continiousDistribution.InnerMinX, continiousDistribution.InnerMaxX, samples, out step);

            double[] discrete = new double[samples];

            for (int i = 0; i < samples; i++)
            {
                double x = xAxis[i];
                discrete[i] = continiousDistribution.InnerGetPDFYbyX(x);
            }

            return new PrivateCoordinates { XCoordinates = xAxis, YCoordinates = discrete, FromContinuous = true };
        }
        #endregion

        #region Coordinates
        /// <summary>
        /// Arguments of distribution function and probability density
        /// </summary>
        public ReadOnlyCollection<double> FunctionArguments
        {
            get
            {
                if (_xCoordinatesReadonly == null)
                {
                    _xCoordinatesReadonly = new ReadOnlyCollection<double>(XCoordinatesInternal);
                }
                return _xCoordinatesReadonly;
            }
        }
        /// <summary>
        /// Sampled values of probability density by arguments <see cref="FunctionArguments"/>
        /// </summary>
        public ReadOnlyCollection<double> ProbabilityDensityValues
        {
            get
            {
                if (_yCoordinatesReadonly == null)
                {
                    _yCoordinatesReadonly = new ReadOnlyCollection<double>(YCoordinatesInternal);
                }
                return _yCoordinatesReadonly;
            }
        }

        /// <summary>
        /// Sampled values of distribution function by arguments <see cref="FunctionArguments"/>
        /// </summary>
        public ReadOnlyCollection<double> CumulativeDistributionValues
        {
            get
            {
                if (_cdfCoordinatesReadonly == null)
                {
                    _cdfCoordinatesReadonly = new ReadOnlyCollection<double>(CDFCoordinatesInternal);
                }

                return _cdfCoordinatesReadonly;
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
                if (_cdfCoordinates == null)
                {
                    _cdfCoordinates = GetCDF(YCoordinatesInternal, Step);
                }

                return _cdfCoordinates;
            }
        }


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

        #endregion

        #region Override settings
        internal override double InnerMinX
        {
            get
            {
                return _min;
            }
        }

        internal override double InnerMaxX
        {
            get
            {
                return _max;
            }
        }

        internal override double InnerMean
        {
            get
            {
                if (_mean == null)
                {
                    _mean = GetMoment(1, 0);
                }

                return _mean.Value;
            }
        }

        internal override double InnerVariance
        {
            get
            {
                if (_variance == null)
                {
                    _variance = GetMoment(2, InnerMean);
                }

                return _variance.Value;
            }
        }

        internal override double InnerSkewness
        {
            get
            {
                if (_skewness == null)
                {
                    double m3 = GetMoment(3, InnerMean);
                    double s3 = Math.Pow(Variance, 3d / 2d);
                    _skewness = m3 / s3;
                }

                return _skewness.Value;
            }
        }

        private double GetMoment(double k, double m)
        {
            double[] xCoordinates = XCoordinatesInternal;
            double[] yCoordinates = YCoordinatesInternal;

            double var = 0;
            double d = 0;
            for (int i = 0; i < _length; i++)
            {
                d = Math.Pow(xCoordinates[i] - m, k) * yCoordinates[i];

                if (i == 0 || i == _length - 1)
                    d /= 2d;

                var += d;

            }
            return var * Step;
        }

        internal override DistributionType InnerDistributionType
        {
            get
            {
                return DistributionType.Discrete;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double GetYByX(double x, double[] coordinates)
        {
            if (x < _min || x > _max)
                return 0;

            double ind = (x - _min) / Step;

            int intInt = (int)ind;
            double k = ind - intInt;

            if (k == 0)
            {
                return coordinates[intInt];
            }
            else
            {
                //да, такое может быть, double хитёр
                if (intInt < 0 || (intInt >= _length - 1))
                    return 0;
                else
                {
                    double min = coordinates[intInt];
                    double max = coordinates[intInt + 1];
                    return (max - min) * k + min;
                }
            }
        }

        internal override double InnerQuantile(double p)
        {
            double[] cdf = CDFCoordinatesInternal;
            double[] xCoordinates = XCoordinatesInternal;

            if (p < cdf[0])
                return double.NegativeInfinity;
            else if (p > cdf[_length - 1])
                return double.PositiveInfinity;
            else
            {
                for (int i = 0; i < _length - 1; i++)
                {
                    //линейное уравнение для аппроксимации
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

        internal override int InnerSamples
        {
            get
            {
                return _length;
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
                        if (Optimizations.UseFFTConvolution)
                        {
                            return FFT.Convolute(this, (ContinuousDistribution)value);
                        }
                        else
                        {
                            return DiscreteRandomMath.Add(this, (DiscreteDistribution)(ContinuousDistribution)value);
                        }
                    }
                default:
                    throw new DistributionsInvalidOperationException();
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
                        if (Optimizations.UseFFTConvolution)
                        {
                            return FFT.Convolute(this, (ContinuousDistribution)(value * -1));
                        }
                        else
                        {
                            return DiscreteRandomMath.Sub(this, (DiscreteDistribution)(ContinuousDistribution)value);
                        }
                    }
                default:
                    throw new DistributionsInvalidOperationException();
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
                    throw new DistributionsInvalidOperationException();
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
                    throw new DistributionsInvalidOperationException();
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

        private class PrivateCoordinates
        {
            public double[] XCoordinates { get; set; }
            public double[] YCoordinates { get; set; }
            public double[] CDFCoordinates { get; set; }
            public bool FromContinuous { get; set; }
        }
    }
}