using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DistributionsAvalonia
{
    public class EvaluationSettings : Window
    {
        public EvaluationSettings()
        {
            InitializeComponent();
        }

        public EvaluationSettings(Configuration configuration)
            : this()
        {
            DataContext = configuration;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
