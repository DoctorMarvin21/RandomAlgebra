using System;
using System.Collections.Generic;
using TableBinder;
using Charts;
using RandomsAlgebra.Distributions;
using RandomsAlgebra.Distributions.Settings;
using RandomsAlgebra.DistributionsEvaluation;
using Gtk;

namespace DistributionsGTK
{
	public partial class TransformWindow : Gtk.Window
	{
		Binder<DistribtutionParameters> _univariateParameters;
		ChartsDrawer _drawerPDF;
		ChartsDrawer _drawerCDF;

		public TransformWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();

			checkMath.Clicked += OnCheckMathClicked;
			checkMonteCarlo.Clicked += OnCheckMonteCarloClicked;
			propsMath.Sensitive = checkMath.Active;
			propsMonteCarlo.Sensitive = checkMonteCarlo.Active;

			_univariateParameters = new Binder<DistribtutionParameters>(treeUnivariateSettings);
			_univariateParameters.AddValue(new DistribtutionParameters("A", new NormalDistributionSettings()));
			_univariateParameters.AddValue(new DistribtutionParameters("B", new UniformDistributionSettings()));

			_drawerPDF = new ChartsDrawer(chartPDF);
			_drawerCDF = new ChartsDrawer(chartCDF);

			this.DeleteEvent += OnDeleteEvent;
		}

		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal = true;
		}

		protected void OnCheckMathClicked(object sender, EventArgs e)
		{
			propsMath.Sensitive = checkMath.Active; 
		}

		protected void OnCheckMonteCarloClicked(object sender, EventArgs e)
		{
			propsMonteCarlo.Sensitive = checkMonteCarlo.Active;
		}

		protected void OnBtnSolveClicked(object sender, EventArgs e)
		{

			try
			{
				Evaluate();
			}
			catch (Exception ex)
			{
				CommonInterface.ShowException(this, ex);
			}
		}

		private void Evaluate()
		{
			DistributionsEvaluator evaluator = new DistributionsEvaluator(txtEquation.Text);


			DistributionBase resultMath = null;
			DistributionBase resultMonteCarlo = null;

			_drawerPDF.Pane.Lines.Clear();
			_drawerCDF.Pane.Lines.Clear();

			var rawData = _univariateParameters.GetData();

			if (checkMath.Active)
			{
				var distributionsDic = DistribtutionParameters.GetDictionaryDistributions(rawData, (int)numSamples.Value, numSensitivity.Value, new Optimizations());
				resultMath = evaluator.EvaluateDistributions(distributionsDic);
				var linePDF = CreateLine(false);
				var lineCDF = CreateLine(false);
				AddChart(linePDF, resultMath, false, 1000);
				AddChart(lineCDF, resultMath, true, 1000);
			}

			if (checkMonteCarlo.Active)
			{
				var settingsDic = DistribtutionParameters.GetDictionaryDistributionSettings(rawData);
				resultMonteCarlo = new MonteCarloDistribution(evaluator, settingsDic, (int)numRandoms.Value, (int)numPockets.Value);

				var linePDF = CreateLine(true);
				var lineCDF = CreateLine(true);

				AddChart(linePDF, resultMonteCarlo, false, 1000);
				AddChart(lineCDF, resultMonteCarlo, true, 1000);
			}
		}


		private ChartLine CreateLine(bool monteCarlo)
		{
			var line = new ChartLine();
			if (monteCarlo)
				line.Color = new Cairo.Color(255, 0, 0);
			else
				line.Color = new Cairo.Color(0, 0, 255);

			line.LineWidth = 0.75;
			return line;
		}


		private void AddChart(ChartLine line, DistributionBase distr, bool cdf, int len)
		{
			double min = distr.MinX;
			double max = distr.MaxX;

			double step = (distr.MaxX - distr.MinX) / (len - 1);

			for (int i = 0; i < len; i++)
			{
				double x = min + step * i;

				if (cdf)
					line.Points.Add(new Cairo.PointD(x, distr.DistributionFunction(x)));
				else
					line.Points.Add(new Cairo.PointD(x, distr.ProbabilityDensityFunction(x)));
			}

			if (cdf)
			{
				_drawerCDF.Pane.Lines.Add(line);
				_drawerCDF.Redraw();
			}
			else
			{
				_drawerPDF.Pane.Lines.Add(line);
				_drawerPDF.Redraw();
			}
		}

		protected void OnTreeUnivariateSettingsRowActivated(object o, RowActivatedArgs args)
		{
			PropertiesEditorDialog editor = new PropertiesEditorDialog();
			editor.ParentWindow = this.GdkWindow;
			editor.Run();
			editor.Destroy();
			//TODO: process double click!
			//_univariateParameters.
			//args.Column
		}
	}
}
