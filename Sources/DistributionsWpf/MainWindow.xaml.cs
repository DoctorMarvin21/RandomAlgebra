using System.Globalization;
using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace DistributionsWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
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

        private async void Process()
        {
            try
            {
                await Results.ProcessAsync(Configuration);

                PdfChart.Update(Results, Configuration.ChartPoints);
                CdfChart.Update(Results, Configuration.ChartPoints);

                //this.FindControl<ContentControl>("PdfPlot").

                //PdfPlot.FindChild<Plot>().ResetAllAxes();
                //CdfPlot.FindChild<Plot>().ResetAllAxes();
            }
            catch (Exception ex)
            {
                //await this.ShowMessageAsync(TranslationSource.Instance.GetTranslation("Exception"), ex.Message);
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


        public async void OnDistributionSettingsDoubleTapped(object sender, RoutedEventArgs e)
        {
            var argument = ((TextBlock)sender).DataContext as ExpressionArgument;

            DistributionEditWindow editWindow = new DistributionEditWindow(this, argument);
            await editWindow.ShowDialog(this);
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
