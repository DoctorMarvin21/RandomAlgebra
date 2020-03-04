using System.Collections.ObjectModel;
using RandomAlgebra.Distributions.Settings;

namespace DistributionsAvalonia
{
    public class Configuration
    {
        public Configuration()
        {
            ExpressionArguments.Add(new ExpressionArgument("A", new UniformDistributionSettings()));
            ExpressionArguments.Add(new ExpressionArgument("B", new NormalDistributionSettings()));
        }

        public string Expression { get; set; } = "A+B*3+1";

        public ObservableCollection<ExpressionArgument> ExpressionArguments { get; }
            = new ObservableCollection<ExpressionArgument>();

        public ExpressionArgument SelectedArgument { get; set; }

        public ObservableCollection<MultivariateExpressionArgument> MultivariateExpressionArguments { get; }
            = new ObservableCollection<MultivariateExpressionArgument>();


        public bool EvaluateRandomAlgebra { get; set; } = true;

        public int Samples { get; set; } = 1000;

        public bool EvaluateMonteCarlo { get; set; } = true;

        public int Experiments { get; set; } = 1000000;

        public int Pockets { get; set; } = 1000;

        public int ChartPoints { get; set; } = 1000;

        public double Probability { get; set; } = 0.95;

        public bool AnalyticalConvolution
        {
            get
            {
                return Optimizations.UseAnalyticalConvolution;
            }
            set
            {
                Optimizations.UseAnalyticalConvolution = value;
            }
        }

        public bool FftConvolution
        {
            get
            {
                return Optimizations.UseFftConvolution;
            }
            set
            {
                Optimizations.UseFftConvolution = value;
            }
        }
    }
}
