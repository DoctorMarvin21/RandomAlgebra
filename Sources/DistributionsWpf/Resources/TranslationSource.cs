using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace DistributionsWpf
{
    public class TranslationSource : INotifyPropertyChanged
    {
        private CultureInfo currentCulture = CultureInfo.InstalledUICulture;

        private TranslationSource()
        {
        }

        public static TranslationSource Instance { get; } = new TranslationSource();

        public static ResourceManager ResourceManager { get; } =
            new ResourceManager("DistributionsWpf.Resources.Distributions", Assembly.GetExecutingAssembly());

        public string this[string key]
        {
            get
            {
                string translation = GetTranslation(key);
                return translation ?? key;
            }
        }

        public CultureInfo CurrentCulture
        {
            get { return currentCulture; }
            set
            {
                if (currentCulture != value)
                {
                    currentCulture = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                }
            }
        }

        public string GetTranslation(string resourceKey)
        {
            return ResourceManager.GetString(resourceKey, currentCulture) ?? resourceKey;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
