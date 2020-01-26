using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RandomAlgebra.Distributions.Settings;
using RandomAlgebra.Distributions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using MahApps.Metro.Controls;

namespace DistributionsWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private DistributionsPair _distributionsPair = new DistributionsPair();

        public MainWindow()
        {
            Expression = "A+B*3+1";
            DistributionFunctionArguments.Add(new DistributionFunctionArgument("A", new UniformDistributionSettings()));
            DistributionFunctionArguments.Add(new DistributionFunctionArgument("B", new NormalDistributionSettings()));

            PdfCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Randoms Algebra",
                    PointGeometry = null,
                    LineSmoothness = 0
                },
                new LineSeries
                {
                    Title = "Monte Carlo",
                    PointGeometry = null,
                    LineSmoothness = 0
                }
            };

            CdfCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Randoms Algebra",
                    PointGeometry = null,
                    LineSmoothness = 0
                },
                new LineSeries
                {
                    Title = "Monte Carlo",
                    PointGeometry = null,
                    LineSmoothness = 0
                }
            };

            InitializeComponent();
        }

        public string Expression { get; set; }

        public ObservableCollection<DistributionFunctionArgument> DistributionFunctionArguments { get; }
            = new ObservableCollection<DistributionFunctionArgument>();

        public ObservableCollection<DistributionParameters> DistributionParameters { get; }
            = new ObservableCollection<DistributionParameters>();

        public SeriesCollection PdfCollection { get; }

        public SeriesCollection CdfCollection { get; }

        private void Process()
        {
#if !DEBUG
            try
#endif
            {
                //_warningsSource.Clear();

                string expression = Expression;
                bool evaluateRandomsAlgebra = true;
                bool evaluateMonteCarlo = true;

                int samples = 1000;
                int experiments = 1000000;
                int pockets = 1000;

                var univariate = DistributionFunctionArgument.CreateDictionary(DistributionFunctionArguments);


                var multivariate = MultivariateDistributionFunctionArgument.CreateDictionary(new List<MultivariateDistributionFunctionArgument>());
                if (multivariate.Count == 0)
                    multivariate = null;

                Stopwatch sw = Stopwatch.StartNew();

                if (evaluateRandomsAlgebra)
                {
                    sw.Restart();
                    _distributionsPair.RandomsAlgebra = DistributionManager.RandomsAlgebraDistribution(expression, univariate, multivariate, samples);

                    if (_distributionsPair.RandomsAlgebra is ContinuousDistribution continuous)
                    {
                        //_distributionsPair.RandomsAlgebra = continuous.Discretize();
                    }


                    sw.Stop();

                    _distributionsPair.RandomsAlgebraTime = sw.Elapsed;
                }
                else
                {
                    _distributionsPair.RandomsAlgebra = null;
                    _distributionsPair.RandomsAlgebraTime = null;
                }

                if (evaluateMonteCarlo)
                {
                    sw.Restart();
                    _distributionsPair.MonteCarlo = DistributionManager.MonteCarloDistribution(expression, univariate, multivariate, experiments, pockets);
                    sw.Stop();

                    _distributionsPair.MonteCarloTime = sw.Elapsed;
                }
                else
                {
                    _distributionsPair.MonteCarlo = null;
                    _distributionsPair.MonteCarloTime = null;
                }

                FillResults();
                FillCharts();
            }
#if !DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Languages.GetText("Exception"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
        }

        private void FillResults()
        {
            DistributionParameters.Clear();

            foreach (DistributionParameters parameter in DistributionManager.GetParameters(_distributionsPair, 0.95))
            {
                DistributionParameters.Add(parameter);
            }
        }

        private void FillCharts()
        {
            var a = new ChartValues<ObservablePoint>();
            var b = new ChartValues<ObservablePoint>();
            var c = new ChartValues<ObservablePoint>();
            var d = new ChartValues<ObservablePoint>();

            Charts.FillChart(a, b, _distributionsPair.RandomsAlgebra, 100);
            Charts.FillChart(c, d, _distributionsPair.MonteCarlo, 100);

            PdfCollection[0].Values = a;
            PdfCollection[1].Values = c;

            CdfCollection[0].Values = b;
            CdfCollection[1].Values = d;

            //Charts.PrepareGraph(zedDistrPDF, zedDistrCDF);
            //Charts.AddCharts(zedDistrPDF, zedDistrCDF, _distributionsPair, (int)numericChartPoints.Value);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process();
        }
    }
}
