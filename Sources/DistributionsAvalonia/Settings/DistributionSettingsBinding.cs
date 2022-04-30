using RandomAlgebra.Distributions.Settings;
using System;
using System.Reflection;

namespace DistributionsAvalonia
{
    public class DistributionSettingsBinding
    {
        private readonly ExpressionArgument _owner;
        private readonly DistributionSettings _instance;
        private readonly PropertyInfo _propertyInfo;

        public DistributionSettingsBinding(ExpressionArgument owner, DistributionSettings instance, PropertyInfo propertyInfo)
        {
            _owner = owner;
            _instance = instance;
            _propertyInfo = propertyInfo;

            Name = TranslationSource.Instance[_propertyInfo.Name];
        }

        public TranslationData Name { get; private set; }

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
