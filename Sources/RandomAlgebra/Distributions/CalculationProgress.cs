using System;

namespace RandomAlgebra.Distributions
{
    // TODO: Add documentation to this class and rework.
    public enum WarningType
    {
        InifinityEliminated
    }

    public static class CalculationProgress
    {
        public static event EventHandler<WarningEventArgs> Warning;

        internal static void InvokeWarning(WarningType warningType)
        {
            string message = Resources.GetMessage(warningType.ToString());
            Warning?.Invoke(null, new WarningEventArgs(message));
        }

        internal static void InvokeWarning(WarningType warningType, params object[] arguments)
        {
            string message = Resources.GetMessage(warningType.ToString());
            Warning?.Invoke(null, new WarningEventArgs(string.Format(message, arguments)));
        }
    }

    public class WarningEventArgs : EventArgs
    {
        internal WarningEventArgs(string message)
        {
            Message = message;
        }

        public string Message
        {
            get;
        }
    }
}
