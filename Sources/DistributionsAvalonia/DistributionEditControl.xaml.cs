using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DistributionsAvalonia
{
    public partial class DistributionEditControl : UserControl
    {
        public DistributionEditControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
