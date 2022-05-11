using RandomAlgebra.Distributions.Settings;
using System.Reflection;

namespace DistributionsBlazor
{
    public class DistributionSettingsSource
    {
        private readonly Func<string, bool> filter;

        public DistributionSettingsSource(DistributionSettings settings, Func<string, bool> filter = null)
        {
            this.filter = filter;
            Settings = settings;
            UpdateBindings();
        }

        public DistributionSettings Settings { get; }

        public IList<DistributionSettingsBinding> Bindings { get; set; }

        protected virtual void UpdateBindings()
        {
            var bindings = new List<DistributionSettingsBinding>();

            var properties = Settings.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanWrite && x.CanRead && filter?.Invoke(x.Name) != false);

            foreach (var property in properties)
            {
                bindings.Add(new DistributionSettingsBinding(Settings, property));
            }

            Bindings = bindings;
        }
    }
}
