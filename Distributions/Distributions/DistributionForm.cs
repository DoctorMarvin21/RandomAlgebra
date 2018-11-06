using RandomsAlgebra;
using RandomsAlgebra.DistributionsEvaluation;
using RandomsAlgebra.Distributions;
using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace Distribuitons
{
    public partial class DistributionForm : Form
    {
        List<DistributionFunctionArgument> _argumentsUnivariate = new List<DistributionFunctionArgument>();

        BindingSource _argumentsSource = new BindingSource();
        DataTable _results = new DataTable();

        List<MultivariateDistributionFunctionArgument> _argumentsMultivariate = new List<MultivariateDistributionFunctionArgument>();
        BindingSource _argumentsMultivariateSource = new BindingSource();

        public DistributionForm()
        {
            InitializeComponent();

            _results.Columns.Add("Параметр", typeof(string));
            _results.Columns.Add("Композиционный", typeof(double));
            _results.Columns.Add("Монте-Карло", typeof(double));
            _results.Columns.Add("Соотношение, %", typeof(double));

            dataGridResults.DataSource = _results;

            dataGridResults.Columns[0].Width = 50;
            dataGridResults.Columns[1].Width = 70;
            dataGridResults.Columns[2].Width = 70;
            dataGridResults.Columns[3].Width = 70;

            txtFunction.Text = "A";
            //txtFunction.Text = "(A+10)_(B+10)";
            _argumentsUnivariate.Add(new DistributionFunctionArgument("A", new ChiDistributionSettings()));
            _argumentsUnivariate.Add(new DistributionFunctionArgument("B", new UniformDistributionSettings()));
            _argumentsUnivariate.Add(new DistributionFunctionArgument("C", new UniformDistributionSettings()));

            dataGridDistributions.AutoGenerateColumns = false;
            dataGridMultivariateDistributions.AutoGenerateColumns = false;

            var argumentColumn = new DataGridViewTextBoxColumn();
            argumentColumn.Width = 50;
            argumentColumn.HeaderText = "Аргумент";
            argumentColumn.DataPropertyName = nameof(DistributionFunctionArgument.Argument);
            dataGridDistributions.Columns.Add(argumentColumn);

            var distributionTypeColumn = new DataGridViewComboBoxColumn();
            distributionTypeColumn.HeaderText = "Тип распределения";
            distributionTypeColumn.DataPropertyName = nameof(DistributionFunctionArgument.SettingsType);
            distributionTypeColumn.DataSource = DisplayNameAndSettingType.DisplayNames;
            distributionTypeColumn.DisplayMember = nameof(DisplayNameAndSettingType.Name);
            distributionTypeColumn.ValueMember = nameof(DisplayNameAndSettingType.SettingsType);
            dataGridDistributions.Columns.Add(distributionTypeColumn);

            var distributionSettingsColumn = new DataGridViewTextBoxColumn();
            distributionSettingsColumn.ReadOnly = true;
            distributionSettingsColumn.HeaderText = "Параметры";
            distributionSettingsColumn.DataPropertyName = nameof(DistributionFunctionArgument.DistributionSettings);
            dataGridDistributions.Columns.Add(distributionSettingsColumn);


            var joinedArgumentsColumn = new DataGridViewTextBoxColumn();
            joinedArgumentsColumn.Width = 100;
            joinedArgumentsColumn.ReadOnly = true;
            joinedArgumentsColumn.HeaderText = "Аргументы";
            joinedArgumentsColumn.DataPropertyName = nameof(MultivariateDistributionFunctionArgument.JoinedArguments);
            dataGridMultivariateDistributions.Columns.Add(joinedArgumentsColumn);

            var multivariateDistributionTypeColumn = new DataGridViewTextBoxColumn();
            multivariateDistributionTypeColumn.Width = 100;
            multivariateDistributionTypeColumn.ReadOnly = true;
            multivariateDistributionTypeColumn.HeaderText = "Тип распределения";
            multivariateDistributionTypeColumn.DataPropertyName = nameof(MultivariateDistributionFunctionArgument.DistributionName);
            dataGridMultivariateDistributions.Columns.Add(multivariateDistributionTypeColumn);

            _argumentsSource.DataSource = _argumentsUnivariate;
            dataGridDistributions.DataSource = _argumentsSource;

            _argumentsMultivariateSource.DataSource = _argumentsMultivariate;
            dataGridMultivariateDistributions.DataSource = _argumentsMultivariateSource;

            //dataGridResults.DataSource = Test.TestData();
        }

        private void DrawGraphConv(ZedGraphControl control, BaseDistribution[] functions, bool cdf)
        {
            int length = (int)numericGraphPoints.Value;

            var distr1 = functions[0];
            var distr2 = functions[1];
            // Получим панель для рисования
            GraphPane pane = control.GraphPane;
            if (cdf)
            {
                pane.Title.Text = "Функция распределения";
            }
            else
            {
                pane.Title.Text = "Плотность вероятности";
            }

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList listMath = new PointPairList();
            PointPairList listRandoms = new PointPairList();

            if (distr1 != null)
            {
                double step = (distr1.MaxX - distr1.MinX) / (length - 1);

                for (double x = distr1.MinX; x < distr1.MaxX; x += step)
                {
                    if (cdf)
                    {
                        var d = distr1.DistributionFunction(x);
                        if (!double.IsInfinity(d) && !double.IsNaN(d))
                        {
                            listMath.Add(x, d);
                        }
                        else
                        {
                            listMath.Add(x, 0);
                        }
                    }
                    else
                    {
                        var d = distr1.ProbabilityDensityFunction(x);
                        if (!double.IsInfinity(d) && !double.IsNaN(d))
                        {
                            listMath.Add(x, d);
                        }
                        else
                        {
                            listMath.Add(x, 0);
                        }
                    }
                }
                var curveMath = pane.AddCurve("Композиционный", listMath, Color.Blue, SymbolType.None);
            }

            if (distr2 != null)
            {
                double step = (distr2.MaxX - distr2.MinX) / (length - 1);

                for (double x = distr2.MinX; x < distr2.MaxX; x += step)
                {
                    if (cdf)
                    {
                        var d = distr2.DistributionFunction(x);
                        if (!double.IsInfinity(d) && !double.IsNaN(d))
                        {
                            listRandoms.Add(x, d);
                        }
                        else
                        {
                            listRandoms.Add(x, 0);
                        }
                    }
                    else
                    {
                        var d = distr2.ProbabilityDensityFunction(x);
                        if (!double.IsInfinity(d) && !double.IsNaN(d))
                        {
                            listRandoms.Add(x, d);
                        }
                        else
                        {
                            listRandoms.Add(x, 0);
                        }
                    }
                }

                var curveMonteCarlo = pane.AddCurve("Монте-Карло", listRandoms, Color.Red, SymbolType.None);
            }

            if (!(distr1 == null && distr2 == null))
            {
                pane.XAxis.Scale.MinAuto = false;
                pane.XAxis.Scale.MaxAuto = false;

                pane.XAxis.Scale.Min = Math.Min((double)(distr1?.MinX ?? distr2.MinX), (double)(distr2?.MinX ?? distr1.MinX));
                pane.XAxis.Scale.Max = Math.Max((double)(distr1?.MaxX ?? distr2.MaxX), (double)(distr2?.MaxX ?? distr1.MaxX));

                pane.YAxis.Scale.MinAuto = false;
                pane.YAxis.Scale.MaxAuto = false;

                pane.YAxis.Scale.Min = 0;

                double? max1 = null;
                double? max2 = null;

                if (cdf)
                {
                    max1 = 1;
                    max2 = 1;
                }
                else
                {
                    if (listMath.Count > 0)
                        max1 = listMath.Max(x => x.Y);
                    if (listRandoms.Count > 0)
                        max2 = listRandoms.Max(x => x.Y);
                }


                pane.YAxis.Scale.Max = Math.Max(max1 ?? max2.Value, max2 ?? max1.Value) * 1.1;


            }

            control.AxisChange();

            control.Invalidate();
        }
        private void btnFunction_Click(object sender, EventArgs e)
        {
            Process();
        }
        private void txtFunction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Process();
            }
        }

        #region Process Function

        private void Process()
        {
#if !DEBUG
            try
#endif
            {
                string formula = txtFunction.Text;

                int samples = (int)numericSamplesCount.Value;
                double tolerance = (double)numericTolerance.Value;
                int randomValues = (int)numericRandomValues.Value;
                int pockets = (int)numericPockets.Value;
                var distr = DistributionFunctionArgument.CreateDictionary(_argumentsUnivariate);

                var mult = MultivariateDistributionFunctionArgument.CreateDictionary(_argumentsMultivariate);
                if (mult.Count == 0)
                    mult = null;

                BaseDistribution[] data = RecountData(formula, distr, mult, samples, tolerance, randomValues, pockets, checkUseRandomsMath.Checked, checkUseMonteCarloMethod.Checked);
                DrawGraphConv(zedDistrPDF, data, false);
                DrawGraphConv(zedDistrCDF, data, true);
            }
#if !DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
        }

        private BaseDistribution[] RecountData(string formula, Dictionary<string, DistributionSettings> distributions, Dictionary<string[], MultivariateDistributionSettings> multivariate, int samplesNumber, double tolerance, int randomValuesCount, int pockets, bool useRandomsMath, bool useMonteCarlo)
        {
            DistributionsEvaluator evaluator = new DistributionsEvaluator(formula);

            Stopwatch sw = new Stopwatch();

            BaseDistribution resultMath = null;
            TimeSpan? mathTime = null;

            if (useRandomsMath)
            {
                if (multivariate != null)
                    throw new ArgumentException("Построение расчётным методом при наличии многомерных распределений невозможно");


                sw.Start();

                var dictionaryMath = new Dictionary<string, BaseDistribution>();


                foreach (var kvp in distributions)
                {
                    dictionaryMath.Add(kvp.Key, kvp.Value.GetDistribution(samplesNumber));
                }
                resultMath = evaluator.EvaluateDistributions(dictionaryMath);

                sw.Stop();

                mathTime = sw.Elapsed;
            }            

            MonteCarloDistribution resultRandoms = null;
            TimeSpan? randomTime = null;

            if (useMonteCarlo)
            {
                sw.Restart();
                resultRandoms = new MonteCarloDistribution(evaluator, distributions, multivariate, randomValuesCount, pockets);

                sw.Stop();
                randomTime = sw.Elapsed;
            }


            ShowDistributionParameters(resultMath, resultRandoms, mathTime, randomTime);

            return new BaseDistribution[] { resultMath, resultRandoms };
        }

        private void ShowDistributionParameters(BaseDistribution randomsMath, MonteCarloDistribution monteCarlo, TimeSpan? randomsMathTime, TimeSpan? monteCarloTime)
        {
            _results.Rows.Clear();

            _results.Rows.Add("t, мс", randomsMathTime?.TotalMilliseconds, monteCarloTime?.TotalMilliseconds, GetPersentRatio(randomsMathTime?.TotalMilliseconds, monteCarloTime?.TotalMilliseconds));
            _results.Rows.Add("μ", (object)randomsMath?.Mean, monteCarlo?.Mean, GetPersentRatio((double?)randomsMath?.Mean, monteCarlo?.Mean));
            _results.Rows.Add("σ", randomsMath?.StandardDeviation, monteCarlo?.StandardDeviation, GetPersentRatio(randomsMath?.StandardDeviation, monteCarlo?.StandardDeviation));
            _results.Rows.Add("σ²", (object)randomsMath?.Variance, monteCarlo?.Variance, GetPersentRatio((double?)randomsMath?.Variance, monteCarlo?.Variance));

            double pPlus = ((double)numericProbability.Value + 1) / 2d;
            double pMinus = (1 - (double)numericProbability.Value) / 2d;
            double? pPlusQm = randomsMath?.Quantile(pPlus);
            double? pPlusQc = monteCarlo?.Quantile(pPlus);
            double? pMinusQm = randomsMath?.Quantile(pMinus);
            double? pMinusQc = monteCarlo?.Quantile(pMinus);

            double? pPlusMinusMeanQm = (pPlusQm - pMinusQm) / 2d;//pPlusQm - randomsMath?.Mean;
            double? pPlusMinusMeanQc = (pPlusQc - pMinusQc) / 2d;//pPlusQc - monteCarlo?.Mean

            double? sqewQm = randomsMath?.Skewness;
            double? sqewQc = monteCarlo?.Skewness;

            _results.Rows.Add("U⁺", pPlusQm, pPlusQc, GetPersentRatio(pPlusQm, pPlusQc));
            _results.Rows.Add("U⁻", pMinusQm, pMinusQc, GetPersentRatio(pMinusQm, pMinusQc));
            _results.Rows.Add("γ", sqewQm, sqewQc, GetPersentRatio(sqewQm, sqewQc));
            _results.Rows.Add("U±", pPlusMinusMeanQm, pPlusMinusMeanQc, GetPersentRatio(pPlusMinusMeanQm, pPlusMinusMeanQc));
        }

        private double? GetPersentRatio(double? v1, double? v2)
        {
            if (v1.HasValue && v2.HasValue)
            {
                double v1V = v1.Value;
                double v2V = v2.Value;
                return ((v1V - v2V) / v1V * 100d);
            }
            else
            {
                return null;
            }
        }

#endregion
        #region Table distributions
        private void dataGridDistributions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = dataGridDistributions[e.ColumnIndex, e.RowIndex];
                if (cell.Value is DistributionSettings settings)
                {
                    if (cell.Value is MultivariateBasedNormalDistributionSettings multivariateBasedNormal)
                    {
                        MultivariateDistributionSettingsForm distributionsForm = new MultivariateDistributionSettingsForm(multivariateBasedNormal);
                        if (distributionsForm.ShowDialog(this) == DialogResult.OK)
                        {
                            var newData = distributionsForm.GetDistributionSettings();

                            multivariateBasedNormal.Coefficients = newData.Coefficients;
                            multivariateBasedNormal.MultivariateNormalDistributionSettings = newData.MultivariateNormalDistributionSettings;
                        }
                    }
                    else
                    {
                        CommonDistributionSettingsForm settingsForm = new CommonDistributionSettingsForm(settings);
                        settingsForm.ShowDialog(this);
                    }


                    dataGridDistributions.InvalidateRow(e.RowIndex);
                }
            }
        }
        private void dataGridDistributions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex >= 0)
            {
                dataGridDistributions.BeginEdit(true);

                if (dataGridDistributions.EditingControl is ComboBox combo)
                {
                    combo.DroppedDown = true;
                }
            }
        }
        private void btnAddDistr_Click(object sender, EventArgs e)
        {
            _argumentsSource.Add(new DistributionFunctionArgument(string.Empty, new NormalDistributionSettings()));
            _argumentsSource.MoveLast();
        }

        private void btnRemoveDistr_Click(object sender, EventArgs e)
        {
            if (_argumentsSource.Current != null)
            {
                _argumentsSource.RemoveCurrent();
            }
        }
#endregion

        private void btnOptimizations_Click(object sender, EventArgs e)
        {
            OptimizationsForm optimizations = new OptimizationsForm();
            optimizations.ShowDialog(this);
        }

        private void btnAddMultivariate_Click(object sender, EventArgs e)
        {
            MultivariateDistributionSettingsForm multivariateDistribution = new MultivariateDistributionSettingsForm(false, true);

            if (multivariateDistribution.ShowDialog(this) == DialogResult.OK)
            {
                var data = multivariateDistribution.GetFunctionArgument();
                _argumentsMultivariateSource.Add(data);
                _argumentsMultivariateSource.MoveLast();
            }
        }

        private void btnRemoveMultivariate_Click(object sender, EventArgs e)
        {
            if (_argumentsMultivariateSource.Current != null)
            {
                _argumentsMultivariateSource.RemoveCurrent();
            }
        }

        private void dataGridMultivariateDistributions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridMultivariateDistributions.Rows[e.RowIndex].DataBoundItem is MultivariateDistributionFunctionArgument current)
                {
                    MultivariateDistributionSettingsForm multivariateDistribution = new MultivariateDistributionSettingsForm(current);
                    if (multivariateDistribution.ShowDialog(this) == DialogResult.OK)
                    {
                        var edited = multivariateDistribution.GetFunctionArgument();
                        current.Arguments = edited.Arguments;
                        current.MultivariateDistributionSettings = edited.MultivariateDistributionSettings;
                        dataGridMultivariateDistributions.InvalidateRow(e.RowIndex);
                    }
                }
            }
        }
    }
}
