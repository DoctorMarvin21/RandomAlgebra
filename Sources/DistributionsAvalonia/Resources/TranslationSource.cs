using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace DistributionsAvalonia
{
    public class TranslationSource
    {
        private Dictionary<string, TranslationData> translations = new Dictionary<string, TranslationData>();
        private CultureInfo currentCulture;

        private TranslationSource()
        {
            CultureInfo invariantEnglish = CultureInfo.GetCultureInfo("en");

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            List<CultureInfo> availableCultures = new List<CultureInfo>();

            for (int i = 0; i < cultures.Length; i++)
            {
                CultureInfo culture = cultures[i];
                try
                {
                    ResourceSet resourceSet = ResourceManager.GetResourceSet(culture, true, false);

                    if (resourceSet != null)
                    {
                        if (culture.Name == string.Empty)
                        {
                            culture = invariantEnglish;
                        }

                        availableCultures.Add(culture);
                    }
                }
                catch
                {
                }
            }

            AvailableCultures = availableCultures.ToArray();

            CultureInfo uiCulture = CultureInfo.InstalledUICulture;

            CultureInfo availableCulture = AvailableCultures.FirstOrDefault(x => x.TwoLetterISOLanguageName == uiCulture.TwoLetterISOLanguageName) ?? invariantEnglish;

            currentCulture = availableCulture;
        }

        public static TranslationSource Instance { get; } = new TranslationSource();

        public ResourceManager ResourceManager { get; } =
            new ResourceManager($"{nameof(DistributionsAvalonia)}.Resources.Distributions", Assembly.GetExecutingAssembly());

        public TranslationData this[string key]
        {
            get
            {
                return GetTranslation(key);
            }
        }

        public CultureInfo[] AvailableCultures { get; }

        public CultureInfo CurrentCulture
        {
            get { return currentCulture; }
            set
            {
                if (value != null && currentCulture != value)
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
