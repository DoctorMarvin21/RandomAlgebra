using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System;

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
                Path = $"[{Key}]",
                Source = TranslationSource.Instance,
                FallbackValue = Key
            };

            return binding;
        }
    }
}
