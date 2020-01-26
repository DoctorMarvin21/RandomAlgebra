using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomAlgebra.Distributions
{
    public enum WarningType
    {
        InifinityEliminated
    }

    public static class CalculationProgress
    {
        public static event EventHandler<WarningEventArgs> Warning;

        internal static void InvokeWarning(WarningType warningType)
        {
            string message = ExceptionMessages.GetExceptionMessage(warningType.ToString());
            Warning?.Invoke(null, new WarningEventArgs(message));
        }

        internal static void InvokeWarning(WarningType warningType, params object[] arguments)
        {
            string message = ExceptionMessages.GetExceptionMessage(warningType.ToString());
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
