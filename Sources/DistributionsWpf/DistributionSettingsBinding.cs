using RandomAlgebra.Distributions.Settings;
using System;
using System.Reflection;

namespace DistributionsWpf
{
    public class DistributionSettingsBinding
    {
        private readonly DistributionSettings _instance;
        private readonly PropertyInfo _propertyInfo;

        public DistributionSettingsBinding(DistributionSettings instance, PropertyInfo propertyInfo)
        {

            _instance = instance;
            _propertyInfo = propertyInfo;
            Description = propertyInfo.Name;
        }

        public string Description
        {
            get;
        }

        public string Value
        {
            get
            {
                return _propertyInfo.GetValue(_instance).ToString();
            }
            set
            {
                object newValue = Convert.ChangeType(value, _propertyInfo.PropertyType);
                _propertyInfo.SetValue(_instance, newValue);
            }
        }
    }
}
