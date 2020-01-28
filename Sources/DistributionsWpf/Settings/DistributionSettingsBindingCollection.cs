using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using RandomAlgebra.Distributions.Settings;

namespace DistributionsWpf
{
    public class DistributionSettingsBindingCollection : ObservableCollection<DistributionSettingsBinding>
    {
        public DistributionSettingsBindingCollection()
        {
        }

        public void Update(ExpressionArgument source, DistributionSettings settings)
        {
            Clear();

            var properties = settings.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanWrite && x.CanRead);

            foreach (var property in properties)
            {
                Add(new DistributionSettingsBinding(source, settings, property));
            }
        }
    }
}
