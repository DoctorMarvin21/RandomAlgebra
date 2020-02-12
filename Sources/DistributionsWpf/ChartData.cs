using RandomAlgebra.Distributions;
using System;
using System.Collections.Generic;
using OxyPlot;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DistributionsWpf
{
    public enum ChartDataType
    {
        PDF,
        CDF
    }

    public class ChartData
    {
        public ChartData(ChartDataType dataType)
        {
            DataType = dataType;

            Title = TranslationSource.Instance[DataType.ToString()];
        }

        public ChartDataType DataType { get; }

        public TranslationData Title { get; private set; }

        public ObservableCollection<DataPoint> RandomAlgebra { get; }
            = new ObservableCollection<DataPoint>();

        public ObservableCollection<DataPoint> MonteCarlo { get; }
            = new ObservableCollection<DataPoint> ();

        public void Update(DistributionsPair distributionsPair, int length)
        {
            RandomAlgebra.Clear();
            MonteCarlo.Clear();

            if (distributionsPair.RandomAlgebra != null)
            {
                FillData(RandomAlgebra, distributionsPair.RandomAlgebra, length);
            }

            if (distributionsPair.MonteCarlo != null)
            {
                FillData(MonteCarlo, distributionsPair.MonteCarlo, length);
            }
        }

        private void FillData(ObservableCollection<DataPoint> points, BaseDistribution distribution, int length)
        {
            double step = (distribution.MaxX - distribution.MinX) / (length - 1);

            List<DataPoint> temp = new List<DataPoint>();

            if (DataType == ChartDataType.PDF)
            {
                FillPoints(temp, distribution.ProbabilityDensityFunction, distribution.MinX, distribution.MaxX, step, length);
            }
            else if (DataType == ChartDataType.CDF)
            {
                FillPoints(points, distribution.DistributionFunction, distribution.MinX, distribution.MaxX, step, length);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                points.Add(temp[i]);
            }
        }

        private static void FillPoints(ICollection<DataPoint> points, Func<double, double> func, double min, double max, double step, int length)
        {
            if (min == max)
            {
                points.Add(new DataPoint(min, func.Invoke(min)));
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    double x = i * step + min;

                    if (i == length - 1)
                        x = max;

                    var y = func.Invoke(x);
                    if (!double.IsInfinity(y) && !double.IsNaN(y))
                    {
                        points.Add(new DataPoint(x, y));
                    }
                    else
                    {
                        points.Add(new DataPoint(x, 0));
                    }
                }
            }
        }
    }
}
