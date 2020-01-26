using RandomAlgebra.Distributions.Settings;
using System;
using System.Reflection;

namespace DistributionsWpf
{
    public class DistributionSettingsBinding
    {
        private readonly DistributionFunctionArgument _owner;
        private readonly DistributionSettings _instance;
        private readonly PropertyInfo _propertyInfo;

        public DistributionSettingsBinding(DistributionFunctionArgument owner, DistributionSettings instance, PropertyInfo propertyInfo)
        {
            _owner = owner;
            _instance = instance;
            _propertyInfo = propertyInfo;
            Description = Resources.GetMessage(propertyInfo.Name) ?? propertyInfo.Name;
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
                _owner.DistributionSettingsChanged();
            }
        }
    }
}
