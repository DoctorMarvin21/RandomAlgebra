using DistributionsAvalonia;
using Microsoft.AspNetCore.Components;
using Plotly.Blazor;
using Plotly.Blazor.Traces;

namespace DistributionsBlazor
{
    public partial class MainComponent : ComponentBase
    {
        private const string RandomAlgebra = "Random Algebra";
        private const string MonteCarlo = "Monte Carlo";

        public ExpressionArgument EditedExpressionArgument { get; set; }

        public PlotlyChart PdfView { get; set; }

        public PlotlyChart CdfView { get; set; }

        public Configuration Configuration { get; } = new Configuration();

        public DistributionsPair Results { get; } = new DistributionsPair();

        public bool IsDistributionEditDialogOpen { get; set; }

        public bool IsInProgress { get; set; }

        public bool IsCalculated { get; set; }

        public string EvaluationError { get; set; }

        public async Task Process()
        {
            IsInProgress = true;
            EvaluationError = null;

            try
            {
                await Results.ProcessAsync(Configuration);

                PdfChart.Update(Results, Configuration.ChartPoints);
                CdfChart.Update(Results, Configuration.ChartPoints);

                foreach (var trace in PdfView.Data.ToArray())
                {
                    await PdfView.DeleteTrace(trace);
                }

                foreach (var trace in CdfView.Data.ToArray())
                {
                    await CdfView.DeleteTrace(trace);
                }

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

                await PdfView.AddTrace(randomAlgebraScatterPdf);
                await PdfView.AddTrace(monteCarloScatterPdf);

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

                await CdfView.AddTrace(randomAlgebraScatterCdf);
                await CdfView.AddTrace(monteCarloScatterCdf);

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

        public ChartData PdfChart { get; set; } = new ChartData(ChartDataType.PDF);

        public ChartData CdfChart { get; set; } = new ChartData(ChartDataType.CDF);

        public void EditExpressionArgument(ExpressionArgument item)
        {
            EditedExpressionArgument = item;
            IsDistributionEditDialogOpen = true;
        }

        public void DeleteExpressionArgument(ExpressionArgument item)
        {
            Configuration.ExpressionArguments.Remove(item);
        }

        public async Task AddExpressionArgument()
        {
            await Task.CompletedTask;
        }
    }
}
