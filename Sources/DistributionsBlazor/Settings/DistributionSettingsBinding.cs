using RandomAlgebra.Distributions.Settings;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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

        private string value;

        public DistributionSettingsBinding(DistributionSettings instance, PropertyInfo propertyInfo)
        {
            this.instance = instance;
            this.propertyInfo = propertyInfo;
            value = propertyInfo.GetValue(instance).ToString();

            if (propertiesNames.TryGetValue(propertyInfo.Name, out string name))
            {
                Name = name;
            }
            else
            {
                Name = propertyInfo.Name;
            }
        }

        public string Name { get; }

        [NumericValudation]
        public string Value
        {
            get => value;
            set
            {
                this.value = value;

                try
                {
                    var updatedValue = value?.Replace(",", ".");
                    object newValue = Convert.ChangeType(updatedValue, propertyInfo.PropertyType, CultureInfo.InvariantCulture);
                    propertyInfo.SetValue(instance, newValue);
                }
                catch
                {
                }
            }
        }

        private class NumericValudationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var updatedValue = (value as string).Replace(",", ".");

                if (updatedValue is not null && double.TryParse(updatedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    return null;
                }
                else if (updatedValue is null)
                {
                    return new ValidationResult("Value can't be null");
                }
                else if (updatedValue.Length == 0)
                {
                    return new ValidationResult("Value can't be empty");
                }
                else
                {
                    return new ValidationResult("Value has invalid format");
                }
            }

            public override string FormatErrorMessage(string name)
            {
                return base.FormatErrorMessage(name);
            }
        }
    }
}
