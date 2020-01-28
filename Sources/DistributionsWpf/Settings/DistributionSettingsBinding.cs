using RandomAlgebra.Distributions.Settings;
using System;
using System.ComponentModel;
using System.Reflection;

namespace DistributionsWpf
{
    public class DistributionSettingsBinding : INotifyPropertyChanged
    {
        private readonly ExpressionArgument _owner;
        private readonly DistributionSettings _instance;
        private readonly PropertyInfo _propertyInfo;

        public DistributionSettingsBinding(ExpressionArgument owner, DistributionSettings instance, PropertyInfo propertyInfo)
        {
            _owner = owner;
            _instance = instance;
            _propertyInfo = propertyInfo;

            TranslationSource.Instance.PropertyChanged += TranslationSourcePropertyChanged;
            UpdateName();
        }

        private void TranslationSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateName();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; private set; }

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

        private void UpdateName()
        {
            Name = TranslationSource.Instance.GetTranslation(_propertyInfo.Name);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
