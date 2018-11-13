using Accord.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RandomAlgebra.Distributions
{
    internal static class FFT
    {
        public const double MaxStepRate = 100;

        public static DiscreteDistribution Convolute(ContinuousDistribution left, ContinuousDistribution right)
        {
            double rangeLeft = left.InnerMaxX - left.InnerMinX;
            double rangeRight = right.InnerMaxX - right.InnerMinX;

            if (rangeLeft < rangeRight)
            {
                return Convolute(left.Discretize(), right);
            }
            else
            {
                return Convolute(right.Discretize(), left);
            }
        }

        public static DiscreteDistribution Convolute(DiscreteDistribution left, ContinuousDistribution right)
        {
            double step = left.Step;
            double samples = (right.InnerMaxX - right.InnerMinX) / step + 1d;

            //when steps differs too much it would be much slower
            if (samples / MaxStepRate > left.Samples)
                return DiscreteRandomMath.Add(left, right.Discretize(left.Samples));
            else
            {
                var rightDiscrete = right.Discretize((int)samples);
                return Convolute(left, rightDiscrete, left.InnerSamples, false);
            }
        }

        public static DiscreteDistribution Convolute(DiscreteDistribution left, DiscreteDistribution right, int resultSamples, bool recountStep = true)
        {
            double step;
            double[] leftY, rightY;

            if (recountStep && left.Step != right.Step)
            {
                if (left.Step > right.Step)
                {
                    leftY = CommonRandomMath.Resample(left.YCoordinatesInternal, (int)(left.Step / right.Step * left.InnerSamples));
                    rightY = right.YCoordinatesInternal;
                    step = right.Step;
                }
                else
                {
                    rightY = CommonRandomMath.Resample(right.YCoordinatesInternal, (int)(right.Step / left.Step * right.InnerSamples));
                    leftY = left.YCoordinatesInternal;
                    step = left.Step;
                }
            }
            else
            {
                step = left.Step;
                leftY = left.YCoordinatesInternal;
                rightY = right.YCoordinatesInternal;
            }

            int minPaddedLength = leftY.Length + rightY.Length - 1;

            //original transform supports not only pow of 2, but in ths case if would be faster
            int paddedLength = UpperPowerOfTwo(minPaddedLength);


            double[] result = new double[minPaddedLength];

            Complex[] complexResult = new Complex[paddedLength];

            Complex[] complexLeft = new Complex[paddedLength];
            Complex[] complexRight = new Complex[paddedLength];

            Parallel.Invoke(() =>
            {

                for (int i = 0; i < leftY.Length; i++)
                {
                    complexLeft[i] = leftY[i];
                }

                Accord.Math.Transforms.FourierTransform2.FFT(complexLeft, FourierTransform.Direction.Forward);
            },
            () =>
            {
                for (int i = 0; i < rightY.Length; i++)
                {
                    complexRight[i] = rightY[i];
                }

                Accord.Math.Transforms.FourierTransform2.FFT(complexRight, FourierTransform.Direction.Forward);
            });

            for (int i = 0; i < paddedLength; i++)
            {
                complexResult[i] = complexLeft[i] * complexRight[i];
            }

            Accord.Math.Transforms.FourierTransform2.FFT(complexResult, FourierTransform.Direction.Backward);

            for (int i = 0; i < minPaddedLength; i++)
            {
                result[i] = complexResult[i].Real * step;
            }

            result = CommonRandomMath.Resample(result, resultSamples);

            double minX = right.InnerMinX + left.InnerMinX;
            double maxX = right.InnerMaxX + left.InnerMaxX;

            double[] xCoordinates = CommonRandomMath.GenerateXAxis(minX, maxX, result.Length, out step);

            return new DiscreteDistribution(xCoordinates, result);
        }

    public static int UpperPowerOfTwo(int v)
        {
            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++;
            return v;

        }
    }
}
