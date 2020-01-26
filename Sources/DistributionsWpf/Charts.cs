using LiveCharts;
using LiveCharts.Defaults;
using RandomAlgebra.Distributions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributionsWpf
{
    public static class Charts
    {
        //public static void PrepareGraph(ZedGraphControl pdf, ZedGraphControl cdf)
        //{
        //    PrepareGraph(pdf, Languages.GetText("PDFTitle"));
        //    PrepareGraph(cdf, Languages.GetText("CDFTitle"));
        //}

        //private static void PrepareGraph(ZedGraphControl control, string name)
        //{
        //    GraphPane pane = control.GraphPane;
        //    pane.CurveList.Clear();
        //    pane.XAxis.Title.IsVisible = false;
        //    pane.YAxis.Title.IsVisible = false;

        //    pane.Title.Text = name;
        //}

        public static void FillChart(ChartValues<ObservablePoint> pdf, ChartValues<ObservablePoint> cdf, BaseDistribution distribution, int length)
        {
            if (distribution == null)
                return;

            double step = (distribution.MaxX - distribution.MinX) / (length - 1);

            FillPoints(pdf, distribution.ProbabilityDensityFunction, distribution.MinX, distribution.MaxX, step, length);
            FillPoints(cdf, distribution.DistributionFunction, distribution.MinX, distribution.MaxX, step, length);
        }

        private static void FillPoints(ChartValues<ObservablePoint> points, Func<double, double> func, double min, double max, double step, int length)
        {
            if (min == max)
            {
                points.Add(new ObservablePoint(min, func.Invoke(min)));
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
                        points.Add(new ObservablePoint(x, y));
                    }
                    else
                    {
                        points.Add(new ObservablePoint(x, 0));
                    }
                }
            }
        }
    }
}
