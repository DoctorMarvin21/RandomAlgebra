using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public class Configuration
    {
        public Configuration()
        {
            ExpressionArguments.Add(new ExpressionArgument("A", new UniformDistributionSettings()));
            ExpressionArguments.Add(new ExpressionArgument("B", new NormalDistributionSettings()));
        }

        public string Expression { get; set; } = "A+B*3+1";

        public IList<ExpressionArgument> ExpressionArguments { get; }
            = new List<ExpressionArgument>();

        public ExpressionArgument SelectedArgument { get; set; }

        public IList<MultivariateExpressionArgument> MultivariateExpressionArguments { get; }
            = new List<MultivariateExpressionArgument>();


        public bool EvaluateRandomAlgebra { get; set; } = true;

        public int Samples { get; set; } = 1000;

        public bool EvaluateMonteCarlo { get; set; } = true;

        public int Experiments { get; set; } = 1000000;

        public int Pockets { get; set; } = 1000;

        public int ChartPoints { get; set; } = 1000;

        public double Probability { get; set; } = 0.95;
    }
}
