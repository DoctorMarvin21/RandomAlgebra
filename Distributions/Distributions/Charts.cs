using RandomsAlgebra.Distributions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace Distributions
{
    public static class Charts
    {
        public static void PrepareGraph(ZedGraphControl pdf, ZedGraphControl cdf)
        {
            PrepareGraph(pdf, Languages.GetText("PDFTitle"));
            PrepareGraph(cdf, Languages.GetText("CDFTitle"));
        }

        private static void PrepareGraph(ZedGraphControl control, string name)
        {
            GraphPane pane = control.GraphPane;
            pane.CurveList.Clear();
            pane.XAxis.Title.IsVisible = false;
            pane.YAxis.Title.IsVisible = false;

            pane.Title.Text = name;
        }

        public static void AddCharts(ZedGraphControl pdf, ZedGraphControl cdf, DistributionsPair distributions, int length)
        {
            AddChart(pdf, cdf, distributions.RandomsAlgebra, Languages.GetText("RandomsAlgebra"), Color.Blue, length);
            AddChart(pdf, cdf, distributions.MonteCarlo, Languages.GetText("MonteCarlo"), Color.Red, length);

            InvalidateChart(pdf);
            InvalidateChart(cdf);
        }

        private static void AddChart(ZedGraphControl pdf, ZedGraphControl cdf, BaseDistribution distribution, string name, Color color, int length)
        {
            if (distribution == null)
                return;

            double step = (distribution.MaxX - distribution.MinX) / (length - 1);

            var pointsPDF = GetPoints(distribution.ProbabilityDensityFunction, distribution.MinX, distribution.MaxX, step);
            var pointsCDF = GetPoints(distribution.DistributionFunction, distribution.MinX, distribution.MaxX, step);


            pdf.GraphPane.AddCurve(name, pointsPDF, color, SymbolType.None);
            cdf.GraphPane.AddCurve(name, pointsCDF, color, SymbolType.None);
        }

        private static void InvalidateChart(ZedGraphControl control)
        {
            var pane = control.GraphPane;

            pane.XAxis.Scale.MinAuto = false;
            pane.XAxis.Scale.MaxAuto = false;

            pane.XAxis.Scale.Min = pane.CurveList.Min(x => ((PointPairList)x.Points).Min(y => y.X));
            pane.XAxis.Scale.Max = pane.CurveList.Max(x => ((PointPairList)x.Points).Max(y => y.X));

            pane.YAxis.Scale.MinAuto = false;
            pane.YAxis.Scale.MaxAuto = false;

            pane.YAxis.Scale.Min = 0;
            pane.YAxis.Scale.Max = pane.CurveList.Min(x => ((PointPairList)x.Points).Max(y => y.Y)) * 1.1d;

            control.AxisChange();

            control.Invalidate();
        }

        private static PointPairList GetPoints(Func<double, double> func, double min, double max, double step)
        {
            PointPairList points = new PointPairList();

            for (double x = min; x < max; x += step)
            {
                var y = func.Invoke(x);
                if (!double.IsInfinity(y) && !double.IsNaN(y))
                {
                    points.Add(x, y);
                }
                else
                {
                    points.Add(x, 0);
                }
            }

            return points;
        }
    }
}
