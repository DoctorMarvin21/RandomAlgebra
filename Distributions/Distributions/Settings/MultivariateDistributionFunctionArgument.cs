using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distribuitons
{
    public class MultivariateDistributionFunctionArgument
    {
        public MultivariateDistributionFunctionArgument(string[] arguments, MultivariateDistributionSettings settings)
        {
            Arguments = arguments;
            MultivariateDistributionSettings = settings;
        }

        public string JoinedArguments
        {
            get
            {
                if (Arguments == null)
                    return string.Empty;
                else
                {
                    return string.Join("; ", Arguments);
                }
            }
        }

        public string[] Arguments
        {
            get;
            set;
        }

        public string DistributionName
        {
            get
            {
                return Multilanguage.GetText(MultivariateDistributionSettings.GetType().Name);
            }
        }

        public MultivariateDistributionSettings MultivariateDistributionSettings
        {
            get;
            set;
        }

        public static Dictionary<string[], MultivariateDistributionSettings> CreateDictionary(List<MultivariateDistributionFunctionArgument> functionArguments)
        {
            Dictionary<string[], MultivariateDistributionSettings> keyValuePairs = new Dictionary<string[], MultivariateDistributionSettings>();
            foreach (MultivariateDistributionFunctionArgument arg in functionArguments)
            {
                keyValuePairs.Add(arg.Arguments, arg.MultivariateDistributionSettings);
            }
            return keyValuePairs;
        }
    }
}
