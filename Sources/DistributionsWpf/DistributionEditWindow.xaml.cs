using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace DistributionsWpf
{
    /// <summary>
    /// Логика взаимодействия для EditDistributionWindow.xaml
    /// </summary>
    public partial class DistributionEditWindow : Window
    {
        public DistributionEditWindow()
        {
            InitializeComponent();
        }

        public DistributionEditWindow(Window owner, ExpressionArgument expressionArgument)
        {
            ExpressionArgument = expressionArgument;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public ExpressionArgument ExpressionArgument { get; }

        public void OkClick(object sender, RoutedEventArgs e)
        {
            Close();
            //await OwningWindow.HideMetroDialogAsync(this);
        }
    }
}
