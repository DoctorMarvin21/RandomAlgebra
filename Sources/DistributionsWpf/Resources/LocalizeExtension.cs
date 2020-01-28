using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DistributionsWpf
{
    public class LocalizeExtension : MarkupExtension
    {

        public string Key { get; }

        public LocalizeExtension(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Binding binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath($"[{Key}]"),
                Source = TranslationSource.Instance,
                FallbackValue = Key
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
