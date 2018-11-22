using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomAlgebra.Distributions
{
    public static class CalculationProgress
    {
        public static event EventHandler<WarningEventArgs> Warning;

        internal static void InvokeWarning(string warningEng, string warningRus)
        {
            string message;
            if (CommonExceptions.Locale == "ru")
            {
                message = warningRus;
            }
            else
            {
                message = warningEng;
            }

            Warning?.Invoke(null, new WarningEventArgs(message));
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
