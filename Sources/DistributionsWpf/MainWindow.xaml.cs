using System.Windows;
using MahApps.Metro.Controls;
using System.Globalization;
using OxyPlot.Wpf;
using MahApps.Metro.Controls.Dialogs;
using System;

namespace DistributionsWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            Configuration = new Configuration();
            InitializeComponent();
        }

        public Configuration Configuration { get; } = new Configuration();

        public DistributionsPair Results { get; } = new DistributionsPair();

        public ChartData PdfChart { get; set; } = new ChartData(ChartDataType.PDF);

        public ChartData CdfChart { get; set; } = new ChartData(ChartDataType.CDF);

        private async void Process()
        {
            try
            {
                await Results.ProcessAsync(Configuration);

                PdfChart.Update(Results, Configuration.ChartPoints);
                CdfChart.Update(Results, Configuration.ChartPoints);

                PdfPlot.FindChild<Plot>().ResetAllAxes();
                CdfPlot.FindChild<Plot>().ResetAllAxes();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync(TranslationSource.Instance.GetTranslation("Exception"), ex.Message);
            }
        }

        private void Process(object sender, RoutedEventArgs e)
        {
            Process();
        }

        private void LanguageTest(object sender, RoutedEventArgs e)
        {
            TranslationSource.Instance.CurrentCulture = new CultureInfo("en-US");
        }

        private async void DistributionSettingsMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed && e.ClickCount >= 2)
            {
                var argument = ((FrameworkElement)sender).DataContext as ExpressionArgument;
                await this.ShowMetroDialogAsync(new DistributionEditWindow(this, argument));
            }
        }

        private void AddExpressionArgument(object sender, RoutedEventArgs e)
        {
            Configuration.ExpressionArguments.Add(new ExpressionArgument(null,
                new RandomAlgebra.Distributions.Settings.UniformDistributionSettings()));
        }

        private void RemoveExpressionArgument(object sender, RoutedEventArgs e)
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
