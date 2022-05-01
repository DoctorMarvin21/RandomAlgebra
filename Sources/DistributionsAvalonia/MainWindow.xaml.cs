using System.Globalization;
using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.ComponentModel;
using Avalonia;
using OxyPlot.Avalonia;
using Avalonia.VisualTree;
using System.Linq;

namespace DistributionsAvalonia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static readonly AvaloniaProperty<bool> CanProcessProperty =
            AvaloniaProperty.Register<MainWindow, bool>(nameof(CanProcess), true, false);

        public static readonly AvaloniaProperty<string> EvaluationErrorProperty =
            AvaloniaProperty.Register<MainWindow, string>(nameof(EvaluationError));

        public MainWindow()
        {
            Configuration = new Configuration();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public Configuration Configuration { get; } = new Configuration();

        public DistributionsPair Results { get; } = new DistributionsPair();

        public ChartData PdfChart { get; set; } = new ChartData(ChartDataType.PDF);

        public ChartData CdfChart { get; set; } = new ChartData(ChartDataType.CDF);

        public TranslationSource TranslationSource { get { return TranslationSource.Instance; } }

        private async void Process()
        {
            try
            {
                CanProcess = false;
                EvaluationError = null;

                await Results.ProcessAsync(Configuration);

                PdfChart.Update(Results, Configuration.ChartPoints);
                CdfChart.Update(Results, Configuration.ChartPoints);

                var b = this.FindControl<ContentControl>("PdfPlot").FindControl<Plot>("DDF");


                this.FindControl<ContentControl>("PdfPlot")
                    .GetVisualChildren()
                    .OfType<Plot>()
                    .Single().ResetAllAxes();

                this.FindControl<ContentControl>("CdfPlot")
                    .GetVisualChildren()
                    .OfType<Plot>()
                    .Single().ResetAllAxes();
            }
            catch (Exception ex)
            {
                EvaluationError = ex.Message;
            }
            finally
            {
                CanProcess = true;
            }
        }

        public bool CanProcess
        {
            get { return (bool)GetValue(CanProcessProperty); }
            set { SetValue(CanProcessProperty, value); }
        }

        public string EvaluationError
        {
            get { return (string)GetValue(EvaluationErrorProperty); }
            set { SetValue(EvaluationErrorProperty, value); }
        }

        private void AddExpressionArgument()
        {
            Configuration.ExpressionArguments.Add(new ExpressionArgument(null,
                new RandomAlgebra.Distributions.Settings.UniformDistributionSettings()));
        }

        private void RemoveExpressionArgument()
        {
            if (Configuration.SelectedArgument != null)
            {
                Configuration.ExpressionArguments.Remove(Configuration.SelectedArgument);
            }
            else if (Configuration.ExpressionArguments.Count > 0)
            {
                Configuration.ExpressionArguments.RemoveAt(Configuration.ExpressionArguments.Count - 1);
            }
        }
    }
}
