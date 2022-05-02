using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public enum DialogMode
    {
        Add,
        Edit
    }

    public class DistributionsDialogProvider
    {
        public DistributionsDialogProvider(IList<ExpressionArgument> arguments)
        {
            ExpressionArguments = arguments;
        }

        public DialogMode DialogMode { get; set; }

        public bool IsDialogOpen { get; set; }

        public ExpressionArgument ExpressionArgument { get; set; }

        public IList<ExpressionArgument> ExpressionArguments { get; }

        public void AddArgument()
        {
            DialogMode = DialogMode.Add;
            ExpressionArgument = new ExpressionArgument("A", new UniformDistributionSettings());
            IsDialogOpen = true;
        }

        public void EditArgument(ExpressionArgument item)
        {
            DialogMode = DialogMode.Edit;
            ExpressionArgument = item;
            IsDialogOpen = true;
        }

        public void DeleteExpressionArgument(ExpressionArgument item)
        {
            ExpressionArguments.Remove(item);
        }

        public void DialogOK()
        {
            if (DialogMode == DialogMode.Add)
            {
                ExpressionArguments.Add(ExpressionArgument);
            }

            IsDialogOpen = false;
        }

        public void DialogCancel()
        {
            IsDialogOpen = false;
        }
    }
}
