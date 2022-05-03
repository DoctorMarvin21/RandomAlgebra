using Microsoft.AspNetCore.Components;
using Plotly.Blazor;
using Plotly.Blazor.Traces;

namespace DistributionsBlazor
{
    public partial class MainComponent : ComponentBase
    {
        private const string RandomAlgebra = "Random Algebra";
        private const string MonteCarlo = "Monte Carlo";

        public MainComponent()
        {
            DistributionsDialogProvider = new DistributionsDialogProvider(Configuration.ExpressionArguments);
        }

        public Configuration Configuration { get; } = new Configuration();

        public DistributionsPair Results { get; } = new DistributionsPair();

        public IList<ITrace> PdfData { get; set; }

        public IList<ITrace> CdfData { get; set; }

        public DistributionsDialogProvider DistributionsDialogProvider { get; }

        public bool IsInProgress { get; set; }

        public bool IsCalculated { get; set; }

        public string EvaluationError { get; set; }

        public async Task Process()
        {
            IsCalculated = false;
            IsInProgress = true;
            EvaluationError = null;

            try
            {
                await Results.ProcessAsync(Configuration);

                PdfChart.Update(Results, Configuration.ChartPoints);
                CdfChart.Update(Results, Configuration.ChartPoints);

                UpdatePdfTrace();
                UpdateCdfTrace();

                IsCalculated = true;
            }
            catch (Exception ex)
            {
                EvaluationError = ex.Message;
                IsCalculated = false;
            }
            finally
            {
                IsInProgress = false;
            }
        }

        private void UpdatePdfTrace()
        {
            var randomAlgebraScatterPdf = new Scatter
            {
                Name = RandomAlgebra,
                Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Lines,
                X = PdfChart.RandomAlgebraX,
                Y = PdfChart.RandomAlgebraY
            };

            var monteCarloScatterPdf = new Scatter
            {
                Name = MonteCarlo,
                Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Lines,
                X = PdfChart.MonteCarloX,
                Y = PdfChart.MonteCarloY
            };

            PdfData = new List<ITrace>
            {
                randomAlgebraScatterPdf,
                monteCarloScatterPdf
            };
        }

        private void UpdateCdfTrace()
        {
            var randomAlgebraScatterCdf = new Scatter
            {
                Name = RandomAlgebra,
                Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Lines,
                X = CdfChart.RandomAlgebraX,
                Y = CdfChart.RandomAlgebraY
            };


            var monteCarloScatterCdf = new Scatter
            {
                Name = MonteCarlo,
                Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Lines,
                X = CdfChart.MonteCarloX,
                Y = CdfChart.MonteCarloY
            };

            CdfData = new List<ITrace>
            {
                randomAlgebraScatterCdf,
                monteCarloScatterCdf
            };
        }

        public ChartData PdfChart { get; set; } = new ChartData(ChartDataType.PDF);

        public ChartData CdfChart { get; set; } = new ChartData(ChartDataType.CDF);
    }
}
