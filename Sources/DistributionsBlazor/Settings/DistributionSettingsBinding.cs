using RandomAlgebra.Distributions.Settings;
using System.Reflection;

namespace DistributionsBlazor
{
    public class DistributionSettingsBinding
    {
        private static readonly Dictionary<string, string> propertiesNames = new Dictionary<string, string>
        {
            { "DegreesOfFreedom", "Degrees of freedom" },
            { "LowerBound", "Lower bound" },
            { "Mean", "Expected value" },
            { "Mean1", "Expected value 1" },
            { "Mean2", "Expected value 2" },
            { "Rate", "Rate λ" },
            { "ScaleParameter", "Scale parameter" },
            { "ShapeParameter", "Shape parameter" },
            { "ShapeParameterA", "Shape parameter α" },
            { "ShapeParameterB", "Shape parameter β" },
            { "StandardDeviation", "Standard deviation" },
            { "StandardDeviation1", "Standard deviation 1" },
            { "StandardDeviation2", "Standard deviation 2" },
            { "UpperBound", "Upper bound" }
        };

        private readonly DistributionSettings instance;
        private readonly PropertyInfo propertyInfo;

        public DistributionSettingsBinding(DistributionSettings instance, PropertyInfo propertyInfo)
        {
            this.instance = instance;
            this.propertyInfo = propertyInfo;

            if (propertiesNames.TryGetValue(propertyInfo.Name, out string name))
            {
                Name = name;
            }
            else
            {
                Name = propertyInfo.Name;
            }
        }

        public string Name { get; private set; }

        public string Value
        {
            get
            {
                return propertyInfo.GetValue(instance).ToString();
            }
            set
            {
                object newValue = Convert.ChangeType(value, propertyInfo.PropertyType);
                propertyInfo.SetValue(instance, newValue);
            }
        }
    }
}
