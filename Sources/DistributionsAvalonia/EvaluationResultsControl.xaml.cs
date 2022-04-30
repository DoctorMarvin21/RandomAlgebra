using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DistributionsAvalonia
{
    public class EvaluationResultsControl : UserControl
    {
        public EvaluationResultsControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
