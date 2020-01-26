using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using RandomAlgebra.Distributions.Settings;

namespace DistributionsWpf
{
    public class DistributionSettingsBindingCollection<T> : Collection<DistributionSettingsBinding>
        where T : DistributionSettings, new()
    {
        private readonly T _settings;
        public DistributionSettingsBindingCollection()
        {
            _settings = new T();

            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanWrite && x.CanRead);

            foreach (var property in properties)
            {
                Add(new DistributionSettingsBinding(_settings, property));
            }
        }

        public T SettingsInstance { get; }
    }
}
