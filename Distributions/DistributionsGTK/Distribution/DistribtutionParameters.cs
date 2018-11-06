using System;
using TableBinder;
using System.Collections.Generic;
using RandomsAlgebra.Distributions;
using RandomsAlgebra.Distributions.Settings;
using System.Collections;
using System.Linq;

namespace DistributionsGTK
{
	public class DistribtutionParameters
	{
		string _settingName = string.Empty;

		public DistribtutionParameters(string argument, DistributionSettings settings)
		{
			if (argument == null)
				throw new ArgumentNullException(nameof(argument));

			if (settings == null)
				throw new ArgumentNullException(nameof(settings));


			Argument = argument;

			_settingName = BinderDistributionSettingsTypeComboAttribute.Types.FirstOrDefault(x => x.Value == settings.GetType()).Key;

			Settings = settings;
		}

		[BinderTitle("Аргумент")]
		public string Argument
		{
			get;
			set;
		}

		[BinderHidden]
		public DistributionSettings Settings
		{
			get;
			set;
		}

		[BinderDistributionSettingsTypeCombo]
		[BinderTitle("Тип распределения")]
		public string TypeName
		{
			get
			{
				return _settingName;
			}
			set
			{
				_settingName = value;


				Type type;

				if (BinderDistributionSettingsTypeComboAttribute.Types.TryGetValue(_settingName, out type))
				{
					Settings = Activator.CreateInstance(type) as DistributionSettings;
				}
			}
		}

		[BinderTitle("Параметры")]
		public string Display
		{
			get
			{
				return Settings.ToString();
			}
		}


		public static Dictionary<string, DistributionBase> GetDictionaryDistributions(IEnumerable<DistribtutionParameters> parameters, int samples, double tolerance, Optimizations optimizations)
		{
			Dictionary<string, DistributionBase> dic = new Dictionary<string, DistributionBase>();


			foreach (var par in parameters)
			{
				dic.Add(par.Argument, par.Settings.GetDistribution(samples, tolerance, optimizations));
			}

			return dic;
		}

		public static Dictionary<string, DistributionSettings> GetDictionaryDistributionSettings(IEnumerable<DistribtutionParameters> parameters)
		{
			Dictionary<string, DistributionSettings> dic = new Dictionary<string, DistributionSettings>();


			foreach (var par in parameters)
			{
				dic.Add(par.Argument, par.Settings);
			}

			return dic;

		}
	}

	public class BinderDistributionSettingsTypeComboAttribute : BinderComboBoxAttribute
	{
		public static Dictionary<string, Type> _distributionSettingsTypes = new Dictionary<string, Type>()
		{
			{ "Нормальное", typeof(NormalDistributionSettings) },
			{ "Равномерное", typeof(UniformDistributionSettings) },
		};

		public override string[] GetValues()
		{
			return _distributionSettingsTypes.Keys.ToArray();
		}

		public static Dictionary<string, Type> Types
		{
			get
			{
				return _distributionSettingsTypes;
			}
		}
	}
}
