using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distribuitons
{
    public static class Multilanguage
    {
        private static Dictionary<string, Translations> _dic = new Dictionary<string, Translations>
        {
            { "FormDistributionsName", new Translations("Distributions", "Распределения") },
            { "FormCommonDistributionSettingsName", new Translations("Distribution settings", "Настройки распределения") },
            { "ButtonOkName", new Translations("OK", "OK") },
            { "ButtonCancelName", new Translations("Cancel", "Отмена") },
            { "FormOptimizationsName", new Translations("Optimizations", "Оптимизации") },
            { "FormOptimizationsCheckContinuous", new Translations("Analytical convolution (if possible)", "Аналитическая свёртка (по возможности)") },
            { "FormOptimizationsCheckFFT", new Translations("FFT convolution", "БПФ свёртка") },

            { nameof(MultivariateNormalDistributionSettings), new Translations("Normal", "Нормальное")  },
            { nameof(MultivariateTDistributionSettings), new Translations("t-distribution", "Стьюдента")  },

            { nameof(UniformDistributionSettings), new Translations("Uniform", "Равномерное") },
            { nameof(ArcsineDistributionSettings), new Translations("Arcsine", "Арксинусное") },
            { nameof(UniformDistributionSettings.LowerBound), new Translations("Lower bound", "Нижняя граница") },
            { nameof(UniformDistributionSettings.UpperBound), new Translations("Upper bound", "Верхняя граница") },
            { nameof(NormalDistributionSettings), new Translations("Normal", "Нормальное") },
            { nameof(LognormalDistributionSettings), new Translations("Lognormal", "Логнормальное") },
            { nameof(NormalDistributionSettings.Mean), new Translations("Expected value", "Мат. ожидание") },
            { nameof(NormalDistributionSettings.StandardDeviation), new Translations("Standard deviation", "Стандартное отклонение") },
            { nameof(BivariateBasedNormalDistributionSettings), new Translations("Two dimensional normal", "Двумерное нормальное") },
            { nameof(BivariateBasedNormalDistributionSettings.Mean1), new Translations("Expected value 1", "Мат. ожидание 1") },
            { nameof(BivariateBasedNormalDistributionSettings.Mean2), new Translations("Expected value 2", "Мат. ожидание 2") },
            { nameof(BivariateBasedNormalDistributionSettings.StandardDeviation1), new Translations("Standard deviation 1", "Ст. отклонение 1") },
            { nameof(BivariateBasedNormalDistributionSettings.StandardDeviation2), new Translations("Standard deviation 2", "Ст. отклонение 2") },
            { nameof(BivariateBasedNormalDistributionSettings.Correlation), new Translations("Correlation", "Коэффициент корреляции") },
            { nameof(MultivariateBasedNormalDistributionSettings), new Translations("Sum of correlated normal", "Многомерное нормальное") },
            { nameof(StudentGeneralizedDistributionSettings), new Translations("t-distribution", "Стьюдента") },
            { nameof(StudentGeneralizedDistributionSettings.DegreesOfFreedom), new Translations("Degrees of freedom", "Степеней свободы") },
            { nameof(BetaDistributionSettings), new Translations("Beta", "Бета") },
            { nameof(BetaDistributionSettings.ShapeParameterA), new Translations("Shape parameter α", "Параметр формы α") },
            { nameof(BetaDistributionSettings.ShapeParameterB), new Translations("Shape parameter β", "Параметр формы β") },
            { nameof(GammaDistributionSettings), new Translations("Gamma", "Гамма") },
            { nameof(GammaDistributionSettings.ShapeParameter), new Translations("Shape parameter", "Параметр формы") },
            { nameof(GammaDistributionSettings.ScaleParameter), new Translations("Scale parameter", "Параметр масштаба") },
            { nameof(ExponentialDistributionSettings), new Translations("Exponential", "Экспоненциальное") },
            { nameof(ExponentialDistributionSettings.Rate), new Translations("Rate λ", "Интенсивность λ") },
            { nameof(ChiDistributionSettings), new Translations("Chi", "Хи") },
            { nameof(ChiSquaredDistributionSettings), new Translations("Chi-squared", "Хи-квадрат") },
            { nameof(RayleighDistributionSettings), new Translations("Rayleigh", "Рэлея") }
        };



        public static string GetText(string arg)
        {
            if (_dic.TryGetValue(arg, out var lang))
            {
                return lang.GetText();
            }
            else
            {
                return arg;
            }
        }



        private class Translations
        {
            public Translations(string eng, string rus)
            {
                Eng = eng;
                Rus = rus;
            }

            public string Rus
            {
                get;
                set;
            }

            public string Eng
            {
                get;
                set;
            }

            private static string Locale
            {
                get
                {
                    //return "en";
                    return System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                }
            }

            public string GetText()
            {
                if (Locale == "ru")
                {
                    return Rus;
                }
                else
                {
                    return Eng;
                }
            }
        }
    }
}
