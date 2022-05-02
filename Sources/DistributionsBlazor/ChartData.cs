using RandomAlgebra.Distributions;

namespace DistributionsBlazor
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

            Title = DataType.ToString();
        }

        public ChartDataType DataType { get; }

        public string Title { get; private set; }

        public List<object> RandomAlgebraX { get; }
            = new List<object>();

        public List<object> RandomAlgebraY { get; }
            = new List<object>();

        public List<object> MonteCarloX { get; }
            = new List<object>();

        public List<object> MonteCarloY { get; }
            = new List<object>();

        public void Update(DistributionsPair distributionsPair, int length)
        {
            RandomAlgebraX.Clear();
            MonteCarloX.Clear();

            RandomAlgebraY.Clear();
            MonteCarloY.Clear();

            if (distributionsPair.RandomAlgebra != null)
            {
                FillData(RandomAlgebraX, RandomAlgebraY, distributionsPair.RandomAlgebra, length);
            }

            if (distributionsPair.MonteCarlo != null)
            {
                FillData(MonteCarloX, MonteCarloY, distributionsPair.MonteCarlo, length);
            }
        }

        private void FillData(List<object> pointsX, List<object> pointsY, BaseDistribution distribution, int length)
        {
            double step = (distribution.MaxX - distribution.MinX) / (length - 1);

            if (DataType == ChartDataType.PDF)
            {
                FillPoints(pointsX, pointsY, distribution.ProbabilityDensityFunction, distribution.MinX, distribution.MaxX, step, length);
            }
            else if (DataType == ChartDataType.CDF)
            {
                FillPoints(pointsX, pointsY, distribution.DistributionFunction, distribution.MinX, distribution.MaxX, step, length);
            }
        }

        private static void FillPoints(List<object> pointsX, List<object> pointsY, Func<double, double> func, double min, double max, double step, int length)
        {
            if (min == max)
            {
                pointsX.Add(min);
                pointsY.Add(func.Invoke(min));
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
                        pointsX.Add(x);
                        pointsY.Add(y);
                    }
                    else
                    {
                        pointsX.Add(x);
                        pointsY.Add(0);
                    }
                }
            }
        }
    }
}
