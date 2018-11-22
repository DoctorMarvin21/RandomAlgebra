using RandomAlgebra;
using RandomAlgebra.DistributionsEvaluation;
using RandomAlgebra.Distributions;
using RandomAlgebra.Distributions.Settings;
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

namespace Distributions
{
    public partial class DistributionForm : Form
    {
        DistributionsPair _distributionsPair = new DistributionsPair();

        List<DistributionFunctionArgument> _argumentsUnivariate = new List<DistributionFunctionArgument>();
        BindingSource _argumentsUnivariateSource = new BindingSource();

        List<MultivariateDistributionFunctionArgument> _argumentsMultivariate = new List<MultivariateDistributionFunctionArgument>();
        BindingSource _argumentsMultivariateSource = new BindingSource();

        List<string> _warnings = new List<string>();
        BindingSource _warningsSource = new BindingSource();

        public DistributionForm()
        {
            InitializeComponent();

            SetLanguage();

            txtFunction.Text = "A+B*3+1";
            _argumentsUnivariate.Add(new DistributionFunctionArgument("A", new NormalDistributionSettings()));
            _argumentsUnivariate.Add(new DistributionFunctionArgument("B", new UniformDistributionSettings()));

            CreateColumnsUnivariateDistributions();
            CreateColumnsMuiltivriateDistributions();
            CreateColumnsResults();

            _argumentsUnivariateSource.DataSource = _argumentsUnivariate;
            dataGridUnivariateDistributions.DataSource = _argumentsUnivariateSource;

            _argumentsMultivariateSource.DataSource = _argumentsMultivariate;
            dataGridMultivariateDistributions.DataSource = _argumentsMultivariateSource;

            Charts.PrepareGraph(zedDistrPDF, zedDistrCDF);

            _warningsSource.DataSource = _warnings;
            listWarnings.DataSource = _warningsSource;

            CalculationProgress.Warning += CalculationProgress_Warning;

            //dataGridResults.AutoGenerateColumns = true;
            //dataGridResults.Columns.Clear();
            //dataGridResults.DataSource = Test.TestData();
        }

        private void CalculationProgress_Warning(object sender, WarningEventArgs e)
        {
            _warningsSource.Add(e.Message);
            _warningsSource.MoveLast();
        }

        private void SetLanguage()
        {
            Text = Languages.GetText("Distributions");

            btnEvaluate.Text = Languages.GetText("ButtonEvaluate");
            btnOptimizations.Text = Languages.GetText("Optimizations");
            btnExport.Text = Languages.GetText("Export");

            checkEvaluateRandomsAlgebra.Text = Languages.GetText("RandomsAlgebra");
            checkEvaluateMonteCarlo.Text = Languages.GetText("MonteCarlo");
            groupCommonParameters.Text = Languages.GetText("GroupCommonParameters");

            groupDistributions.Text = Languages.GetText("GroupDistributions");
            groupMultivariate.Text = Languages.GetText("GroupMultivariate");
            groupResults.Text = Languages.GetText("GroupResults");
            groupWarnings.Text = Languages.GetText("GroupWarnings");

            lbModel.Text = Languages.GetText("LabelModel");
            lbSamplesCount.Text = Languages.GetText("LabelSamplesCount");
            lbExperimentsCount.Text = Languages.GetText("LabelExperimentsCount");
            lbPocketsCount.Text = Languages.GetText("LabelPocketsCount");
            lbProbabilityP.Text = Languages.GetText("LabelProbablility");
            lbChartPoints.Text = Languages.GetText("LabelChartPoints");


            btnAddUnivariateDistribution.Text = Languages.GetText("ButtonAdd");
            btnRemoveUnivariateDistribution.Text = Languages.GetText("ButtonRemove");
            btnAddMultivariateDistribution.Text = Languages.GetText("ButtonAdd");
            btnRemoveMultivariateDistibution.Text = Languages.GetText("ButtonRemove");
        }

        #region Univariate distributions
        public void CreateColumnsUnivariateDistributions()
        {
            dataGridUnivariateDistributions.AutoGenerateColumns = false;

            var argumentColumn = new DataGridViewTextBoxColumn();
            argumentColumn.Width = 50;
            argumentColumn.HeaderText = Languages.GetText("Argument");
            argumentColumn.DataPropertyName = nameof(DistributionFunctionArgument.Argument);
            dataGridUnivariateDistributions.Columns.Add(argumentColumn);

            var distributionTypeColumn = new DataGridViewComboBoxColumn();
            distributionTypeColumn.HeaderText = Languages.GetText("DistributionType");
            distributionTypeColumn.DataPropertyName = nameof(DistributionFunctionArgument.SettingsType);
            distributionTypeColumn.DataSource = DisplayNameAndSettingType.DisplayNames;
            distributionTypeColumn.DisplayMember = nameof(DisplayNameAndSettingType.Name);
            distributionTypeColumn.ValueMember = nameof(DisplayNameAndSettingType.SettingsType);
            dataGridUnivariateDistributions.Columns.Add(distributionTypeColumn);

            var distributionSettingsColumn = new DataGridViewTextBoxColumn();
            distributionSettingsColumn.ReadOnly = true;
            distributionSettingsColumn.HeaderText = Languages.GetText("DistributionSettings");
            distributionSettingsColumn.DataPropertyName = nameof(DistributionFunctionArgument.DistributionSettings);
            dataGridUnivariateDistributions.Columns.Add(distributionSettingsColumn);
        }

        private void dataGridDistributions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = dataGridUnivariateDistributions[e.ColumnIndex, e.RowIndex];
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


                    dataGridUnivariateDistributions.InvalidateRow(e.RowIndex);
                }
            }
        }

        private void dataGridDistributions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex >= 0)
            {
                dataGridUnivariateDistributions.BeginEdit(true);

                if (dataGridUnivariateDistributions.EditingControl is ComboBox combo)
                {
                    combo.DroppedDown = true;
                }
            }
        }

        private void btnAddUnivariateDistribution_Click(object sender, EventArgs e)
        {
            _argumentsUnivariateSource.Add(new DistributionFunctionArgument(string.Empty, new NormalDistributionSettings()));
            _argumentsUnivariateSource.MoveLast();
        }

        private void btnRemoveUnivariateDistribution_Click(object sender, EventArgs e)
        {
            if (_argumentsUnivariateSource.Current != null)
            {
                _argumentsUnivariateSource.RemoveCurrent();
            }
        }
        #endregion

        #region Multivariate distributuions
        public void CreateColumnsMuiltivriateDistributions()
        {
            dataGridMultivariateDistributions.AutoGenerateColumns = false;

            var joinedArgumentsColumn = new DataGridViewTextBoxColumn();
            joinedArgumentsColumn.Width = 100;
            joinedArgumentsColumn.ReadOnly = true;
            joinedArgumentsColumn.HeaderText = Languages.GetText("Arguments");
            joinedArgumentsColumn.DataPropertyName = nameof(MultivariateDistributionFunctionArgument.JoinedArguments);
            dataGridMultivariateDistributions.Columns.Add(joinedArgumentsColumn);

            var multivariateDistributionTypeColumn = new DataGridViewTextBoxColumn();
            multivariateDistributionTypeColumn.Width = 100;
            multivariateDistributionTypeColumn.ReadOnly = true;
            multivariateDistributionTypeColumn.HeaderText = Languages.GetText("DistributionType");
            multivariateDistributionTypeColumn.DataPropertyName = nameof(MultivariateDistributionFunctionArgument.DistributionName);
            dataGridMultivariateDistributions.Columns.Add(multivariateDistributionTypeColumn);
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

        private void btnAddMultivariateDistribution_Click(object sender, EventArgs e)
        {
            MultivariateDistributionSettingsForm multivariateDistribution = new MultivariateDistributionSettingsForm(false, true);

            if (multivariateDistribution.ShowDialog(this) == DialogResult.OK)
            {
                var data = multivariateDistribution.GetFunctionArgument();
                _argumentsMultivariateSource.Add(data);
                _argumentsMultivariateSource.MoveLast();
            }
        }

        private void btnRemoveMultivariateDistribution_Click(object sender, EventArgs e)
        {
            if (_argumentsMultivariateSource.Current != null)
            {
                _argumentsMultivariateSource.RemoveCurrent();
            }
        }
        #endregion

        #region Results table
        public void CreateColumnsResults()
        {
            dataGridResults.AutoGenerateColumns = false;

            var parameterName = new DataGridViewTextBoxColumn();
            parameterName.Width = 50;
            parameterName.ReadOnly = true;
            parameterName.HeaderText = Languages.GetText("ColumnParameterName");
            parameterName.DataPropertyName = nameof(DistributionParameters.Name);
            dataGridResults.Columns.Add(parameterName);

            var randomsAlgebra = new DataGridViewTextBoxColumn();
            randomsAlgebra.Width = 70;
            randomsAlgebra.ReadOnly = true;
            randomsAlgebra.HeaderText = Languages.GetText("RandomsAlgebra");
            randomsAlgebra.DataPropertyName = nameof(DistributionParameters.RandomsAlgebra);
            dataGridResults.Columns.Add(randomsAlgebra);

            var monteCarlo = new DataGridViewTextBoxColumn();
            monteCarlo.Width = 70;
            monteCarlo.ReadOnly = true;
            monteCarlo.HeaderText = Languages.GetText("MonteCarlo");
            monteCarlo.DataPropertyName = nameof(DistributionParameters.MonteCarlo);
            dataGridResults.Columns.Add(monteCarlo);

            var persentRation = new DataGridViewTextBoxColumn();
            persentRation.Width = 70;
            persentRation.ReadOnly = true;
            persentRation.HeaderText = Languages.GetText("ColumnPersentRatio");
            persentRation.DataPropertyName = nameof(DistributionParameters.PersentRatio);
            dataGridResults.Columns.Add(persentRation);
        }
        #endregion

        #region Process
        private void btnEvaluate_Click(object sender, EventArgs e)
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

        private void Process()
        {
#if !DEBUG
            try
#endif
            {
                _warningsSource.Clear();

                string expression = txtFunction.Text;
                bool evaluateRandomsAlgebra = checkEvaluateRandomsAlgebra.Checked;
                bool evaluateMonteCarlo = checkEvaluateMonteCarlo.Checked;

                int samples = (int)numericSamplesCount.Value;
                int experiments = (int)numericExperimentsCount.Value;
                int pockets = (int)numericPocketsCount.Value;

                var univariate = DistributionFunctionArgument.CreateDictionary(_argumentsUnivariate);


                var multivariate = MultivariateDistributionFunctionArgument.CreateDictionary(_argumentsMultivariate);
                if (multivariate.Count == 0)
                    multivariate = null;

                Stopwatch sw = Stopwatch.StartNew();

                if (evaluateRandomsAlgebra)
                {
                    sw.Restart();
                    _distributionsPair.RandomsAlgebra = DistributionManager.RandomsAlgebraDistribution(expression, univariate, multivariate, samples);

                    if (_distributionsPair.RandomsAlgebra is ContinuousDistribution continuous)
                    {
                        //_distributionsPair.RandomsAlgebra = continuous.Discretize();
                    }


                    sw.Stop();

                    _distributionsPair.RandomsAlgebraTime = sw.Elapsed;
                }
                else
                {
                    _distributionsPair.RandomsAlgebra = null;
                    _distributionsPair.RandomsAlgebraTime = null;
                }

                if (evaluateMonteCarlo)
                {
                    sw.Restart();
                    _distributionsPair.MonteCarlo = DistributionManager.MonteCarloDistribution(expression, univariate, multivariate, experiments, pockets);
                    sw.Stop();

                    _distributionsPair.MonteCarloTime = sw.Elapsed;
                }
                else
                {
                    _distributionsPair.MonteCarlo = null;
                    _distributionsPair.MonteCarloTime = null;
                }

                FillResults();
                FillCharts();
            }
#if !DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Languages.GetText("Exception"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
        }

        private void FillResults()
        {
            dataGridResults.DataSource = DistributionManager.GetParameters(_distributionsPair, (double)numericProbability.Value);
        }

        private void FillCharts()
        {
            Charts.PrepareGraph(zedDistrPDF, zedDistrCDF);
            Charts.AddCharts(zedDistrPDF, zedDistrCDF, _distributionsPair, (int)numericChartPoints.Value);
        }

        private void numericProbability_ValueChanged(object sender, EventArgs e)
        {
            FillResults();
        }

        private void numericChartPoints_ValueChanged(object sender, EventArgs e)
        {
            FillCharts();
        }

        #endregion

        #region Parameters
        private void checkEvaluateRandomsAlgebra_CheckedChanged(object sender, EventArgs e)
        {
            bool eval = checkEvaluateRandomsAlgebra.Checked;
            numericSamplesCount.Enabled = eval;
        }

        private void checkEvaluateMonteCarlo_CheckedChanged(object sender, EventArgs e)
        {
            bool eval = checkEvaluateMonteCarlo.Checked;
            numericExperimentsCount.Enabled = eval;
            numericPocketsCount.Enabled = eval;
        }


        private void btnOptimizations_Click(object sender, EventArgs e)
        {
            OptimizationsForm optimizations = new OptimizationsForm();
            optimizations.ShowDialog(this);
        }

        #endregion

        #region Import/Export
        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Text|*.txt";

            if (saveFile.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string path = saveFile.FileName;
                    string toSave = ImportExport.ExportToText(_distributionsPair);
                    System.IO.File.WriteAllText(path, toSave);
                    System.Diagnostics.Process.Start(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Languages.GetText("Exception"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion
    }
}
