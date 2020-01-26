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

namespace DistributionsWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UniformDistributionSettings uniformDistributionSettings = new UniformDistributionSettings(-1, 1);

            ContinuousDistribution distribution = new ContinuousDistribution(uniformDistributionSettings);

            try
            {
                var p = distribution / 0;
            }
            catch (Exception ex)
            {

            }
        }

        public DistributionSettingsBindingCollection<UniformDistributionSettings> SettingsBindings { get; }
            = new DistributionSettingsBindingCollection<UniformDistributionSettings>();
    }
}
