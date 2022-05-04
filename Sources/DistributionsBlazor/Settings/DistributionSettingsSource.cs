using RandomAlgebra.Distributions.Settings;
using System.Reflection;

namespace DistributionsBlazor
{
    public class DistributionSettingsSource
    {
        public DistributionSettingsSource(DistributionSettings settings)
        {
            Settings = settings;
            UpdateBindings();
        }

        public DistributionSettings Settings { get; }

        public IList<DistributionSettingsBinding> Bindings { get; set; }

        protected virtual void UpdateBindings()
        {
            var bindings = new List<DistributionSettingsBinding>();

            var properties = Settings.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanWrite && x.CanRead);

            foreach (var property in properties)
            {
                bindings.Add(new DistributionSettingsBinding(Settings, property));
            }

            Bindings = bindings;
        }
    }
}
