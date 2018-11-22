using RandomAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distributions
{
    public static class Languages
    {
        private static Dictionary<string, Translations> _dic = new Dictionary<string, Translations>
        {
            { "GroupCommonParameters", new Translations("Common parameters", "Общие параметры") },
            { "GroupResults", new Translations("Results", "Результаты") },
            { "GroupDistributions", new Translations("Distributions", "Распределения") },
            { "GroupMultivariate", new Translations("Multivariate distributions", "Многомерные распределения") },
            { "GroupMatrixParameters", new Translations("Matrix parameters", "Параметры матрицы") },
            { "GroupDistributionParameters", new Translations("Distribution parameters", "Параметры распределения") },
            { "GroupWarnings", new Translations("Warnings", "Предупреждения") },

            { "TableCoeffitients", new Translations("Distribution parameters", "Параметры распределения") },
            { "TableMatrix", new Translations("Covariance matrix", "Параметры распределения") },
            { "TableMeans", new Translations("Mean values", "Средние значения") },

            { "ButtonEvaluate", new Translations("Evaluate", "Расчёт") },
            { "ButtonBuildTables", new Translations("Build tables", "Сформировать таблицы") },
            { "ButtonOk", new Translations("OK", "OK") },
            { "ButtonCancel", new Translations("Cancel", "Отмена") },
            { "ButtonAdd", new Translations("Add", "Добавить")  },
            { "ButtonRemove", new Translations("Remove", "Удалить")  },
            { "CheckContinuous", new Translations("Analytical convolution (if possible)", "Аналитическая свёртка (по возможности)") },
            { "CheckFFT", new Translations("FFT convolution", "БПФ свёртка") },
            { "LabelModel", new Translations("Model", "Модель") },
            { "LabelSamplesCount", new Translations("Samples", "Число отсчётов") },
            { "LabelExperimentsCount", new Translations("Experiments", "Экспериментов") },
            { "LabelPocketsCount", new Translations("Pockets (for charts)", "Карманов (для графиков)") },
            { "LabelProbablility", new Translations("Probability, p", "Вероятность, p") },
            { "LabelChartPoints", new Translations("Chart points", "Точек графика") },

            { "PDFTitle", new Translations("Density Function", "Плотность вероятности") },
            { "CDFTitle", new Translations("Distribution Function", "Функция распределения") },

            { "Argument", new Translations("Argument", "Аргумент") },
            { "Arguments", new Translations("Arguments", "Аргументы") },
            { "Distribution", new Translations("Distribution", "Распределениe") },
            { "Distributions", new Translations("Distributions", "Распределения") },
            { "Dimensions", new Translations("Dimensions", "Измерений") },
            { "DistributionType", new Translations("Distribution type", "Вид распределения") },
            { "MultivariateDistribution", new Translations("Multivariate distribution", "Многомерное распределение") },
            { "DistributionSettings", new Translations("Distribution settings", "Настройки распределения") },
            { "RandomsAlgebra", new Translations("Randoms algebra", "Алгебра сл. величин") },
            { "MonteCarlo", new Translations("Monte-Carlo", "Монте-Карло") },
            { "Optimizations", new Translations("Optimizations", "Оптимизации") },
            { "Export", new Translations("Export", "Экспорт") },


            { "ColumnParameterName", new Translations("Parameter", "Параметр") },
            { "ColumnPersentRatio", new Translations("Ratio, %", "Соотношение, %") },

            { "Exception", new Translations("Error", "Ошибка") },
            { "ExceptionCoeffitientsMissing", new Translations("Coeffitients missing", "Коэффициенты не сгенерированы") },
            { "ExceptionMaxtrixMissing", new Translations("Covariance matrix missing", "Матрица ковариации не сформирована") },
            { "ExceptionArgumentsMissing", new Translations("Arguments missing", "Аргументы не сгенерированы") },


            { nameof(MultivariateNormalDistributionSettings), new Translations("Normal", "Нормальное")  },
            { nameof(MultivariateTDistributionSettings), new Translations("t-distribution", "Стьюдента")  },
            { nameof(UniformDistributionSettings), new Translations("Uniform", "Равномерное") },
            { nameof(ArcsineDistributionSettings), new Translations("Arcsine", "Арксинусное") },
            { nameof(UniformDistributionSettings.LowerBound), new Translations("Lower bound", "Нижняя граница") },
            { nameof(UniformDistributionSettings.UpperBound), new Translations("Upper bound", "Верхняя граница") },
            { nameof(NormalDistributionSettings), new Translations("Normal", "Нормальное") },
            { nameof(LognormalDistributionSettings), new Translations("Lognormal", "Логнормальное") },
            { nameof(GeneralizedNormalDistributionSettings), new Translations("Generalized normal", "Обобщенное нормальное") },
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
