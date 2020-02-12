using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace DistributionsWpf
{
    public class TranslationSource
    {
        private Dictionary<string, TranslationData> translations = new Dictionary<string, TranslationData>();


        private CultureInfo currentCulture = CultureInfo.InstalledUICulture;

        private TranslationSource()
        {
        }

        public static TranslationSource Instance { get; } = new TranslationSource();

        public static ResourceManager ResourceManager { get; } =
            new ResourceManager("DistributionsWpf.Resources.Distributions", Assembly.GetExecutingAssembly());

        public TranslationData this[string key]
        {
            get
            {
                return GetTranslation(key);
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

                    foreach (TranslationData translation in translations.Values)
                    {
                        translation.Value = ResourceManager.GetString(translation.Key, currentCulture) ?? translation.Key;
                    }
                }
            }
        }

        private TranslationData GetTranslation(string resourceKey)
        {
            if (translations.TryGetValue(resourceKey, out TranslationData translation))
            {
                return translation;
            }
            else
            {
                string text = ResourceManager.GetString(resourceKey, currentCulture) ?? resourceKey;
                translation = new TranslationData(resourceKey, text);
                translations.Add(resourceKey, translation);
            }

            return translation;
        }
    }

    public class TranslationData : INotifyPropertyChanged
    {
        private string value;

        public TranslationData(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
