using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Distribuitons
{
    public partial class MultivariateDistributionSettingsForm : Form
    {
        DataTable _samples = new DataTable();
        BindingSource _samplesSource = new BindingSource();
        DataTable _arguments = new DataTable();
        BindingSource _argumentsSource = new BindingSource();

        DataTable _means = new DataTable();
        BindingSource _meansSource = new BindingSource();

        DataTable _coeff = new DataTable();
        BindingSource _coeffSource = new BindingSource();

        public MultivariateDistributionSettingsForm(bool showCoeff, bool showDistributionParameters)
        {
            InitializeComponent();

            _samplesSource.DataSource = _samples;
            _argumentsSource.DataSource = _arguments;
            _meansSource.DataSource = _means;
            _coeffSource.DataSource = _coeff;

            comboDistributionType.DataSource = new string[] { "Нормальное", "Стьюдента" };


            groupDistributionParameters.Visible = showDistributionParameters;
            ChangeRowVisibility(1, showCoeff);

        }

        public MultivariateDistributionSettingsForm(MultivariateDistributionFunctionArgument argument) : this(false, true)
        {
            checkCovariationMatrix.Checked = true;

            int dimension = argument.MultivariateDistributionSettings.Dimension;
            numericDimensions.Value = dimension;

            BuildTables();

            var arguments = argument.Arguments;
            var means = argument.MultivariateDistributionSettings.Means;
            var covMatrix = argument.MultivariateDistributionSettings.CovarianceMatrix;

            for (int i = 0; i < dimension; i++)
            {
                _arguments.Rows[0][i] = arguments[i];
                _means.Rows[0][i] = means[i];

                for (int j = 0; j < dimension; j++)
                {
                    _samples.Rows[j][i] = covMatrix[i, j];
                }
            }

            if (argument.MultivariateDistributionSettings is MultivariateNormalDistributionSettings)
            {
                comboDistributionType.SelectedIndex = 0;
            }
            else if (argument.MultivariateDistributionSettings is MultivariateTDistributionSettings multivariateT)
            {
                numericDegreesOfFreedom.Value = (decimal)multivariateT.DegreesOfFreedom;
                comboDistributionType.SelectedIndex = 1;
            }
        }

        public MultivariateDistributionSettingsForm(MultivariateBasedNormalDistributionSettings argument) : this(true, false)
        {
            ChangeRowVisibility(2, false);

            checkCovariationMatrix.Checked = true;
            comboDistributionType.SelectedIndex = 0;

            int dimension = argument.MultivariateNormalDistributionSettings.Dimension;
            numericDimensions.Value = dimension;

            BuildTables();

            var coeff = argument.Coefficients;
            var means = argument.MultivariateNormalDistributionSettings.Means;
            var covMatrix = argument.MultivariateNormalDistributionSettings.CovarianceMatrix;

            for (int i = 0; i < dimension; i++)
            {
                _coeff.Rows[0][i] = coeff[i];
                _means.Rows[0][i] = means[i];

                for (int j = 0; j < dimension; j++)
                {
                    _samples.Rows[j][i] = covMatrix[i, j];
                }
            }
        }

        private void btnBuildTables_Click(object sender, EventArgs e)
        {
            BuildTables();
        }

        private void BuildTables()
        {
            bool cov = checkCovariationMatrix.Checked;
            _arguments.Rows.Clear();
            _arguments.Columns.Clear();

            _samples.Rows.Clear();
            _samples.Columns.Clear();

            _means.Rows.Clear();
            _means.Columns.Clear();

            _coeff.Rows.Clear();
            _coeff.Columns.Clear();

            int dimesion = (int)numericDimensions.Value;

            for (int i = 0; i < dimesion; i++)
            {
                _samples.Columns.Add(new DataColumn($"Расп. {i + 1}", typeof(double)) { AllowDBNull = false });
                _arguments.Columns.Add(new DataColumn($"Расп. {i + 1}", typeof(string)) { AllowDBNull = false });
                _means.Columns.Add(new DataColumn($"Расп. {i + 1}", typeof(double)) { AllowDBNull = false });
                _coeff.Columns.Add(new DataColumn($"Расп. {i + 1}", typeof(double)) { AllowDBNull = false });
            }

            object[] args = new object[dimesion];
            for (int i = 0; i < dimesion; i++)
            {
                args[i] = Encoding.ASCII.GetString(new byte[] { (byte)(i + 0x41) });
            }

            _arguments.Rows.Add(args);

            if (cov)
            {
                for (int i = 0; i < dimesion; i++)
                {
                    object[] row = new object[dimesion];
                    for (int j = 0; j < dimesion; j++)
                    {
                        row[j] = j == i ? 1d : 0d;
                    }
                    _samples.Rows.Add(row);
                }
            }

            object[] means = new object[dimesion];
            for (int i = 0; i < dimesion; i++)
            {
                means[i] = 0d;
            }
            _means.Rows.Add(means);


            object[] coeff = new object[dimesion];
            for (int i = 0; i < dimesion; i++)
            {
                coeff[i] = 1d;
            }
            _coeff.Rows.Add(coeff);

            dataGridSamples.AllowUserToAddRows = !cov;
            dataGridSamples.AllowUserToDeleteRows = !cov;

            dataGridSamples.DataSource = _samplesSource;
            dataGridArguments.DataSource = _argumentsSource;
            dataGridMeans.DataSource = _meansSource;
            dataGridCoeff.DataSource = _coeffSource;

            ChangeRowVisibility(4, cov);

            for (int i = 0; i < dimesion; i++)
            {
                dataGridSamples.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridArguments.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridMeans.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridCoeff.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public MultivariateDistributionFunctionArgument GetFunctionArgument()
        {
            if (_arguments.Rows.Count > 0)
            {
                string[] args = _arguments.Rows[0].ItemArray.Cast<string>().ToArray();

                var settings = GetSettings();

                return new MultivariateDistributionFunctionArgument(args, settings);
            }
            else
            {
                throw new Exception("Аргументы не сгенерированы");
            }
        }

        public MultivariateDistributionSettings GetSettings()
        {
            if (_samples.Rows.Count > 0)
            {
                double degrees = (double)numericDegreesOfFreedom.Value;
                bool cov = checkCovariationMatrix.Checked;
                double[,] input = new double[_samples.Columns.Count, _samples.Rows.Count];

                for (int i = 0; i < _samples.Columns.Count; i++)
                {
                    for (int j = 0; j < _samples.Rows.Count; j++)
                    {
                        input[i, j] = (double)_samples.Rows[j][i];
                    }
                }

                double[] means = _means.Rows[0].ItemArray.Cast<double>().ToArray();

                if (comboDistributionType.SelectedIndex == 0)
                {
                    if (cov)
                    {
                        return new MultivariateNormalDistributionSettings(means, input);
                    }
                    else
                    {
                        return new MultivariateNormalDistributionSettings(input);
                    }
                }
                else if (comboDistributionType.SelectedIndex == 1)
                {
                    if (cov)
                    {
                        return new MultivariateTDistributionSettings(means, input, degrees);
                    }
                    else
                    {
                        return new MultivariateTDistributionSettings(input, degrees);
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                throw new Exception("Данные не заданы");
            }
        }

        public MultivariateBasedNormalDistributionSettings GetDistributionSettings()
        {
            if (_coeff.Rows.Count > 0)
            {
                double[] coeff = _coeff.Rows[0].ItemArray.Cast<double>().ToArray();

                var settings = GetSettings();

                if (!(settings is MultivariateNormalDistributionSettings))
                {
                    settings = new MultivariateNormalDistributionSettings(settings.Means, settings.CovarianceMatrix);
                }

                return new MultivariateBasedNormalDistributionSettings(coeff, (MultivariateNormalDistributionSettings)settings);
            }
            else
            {
                throw new Exception("Коэфициенты не сгенерированы");
            }
        }

        private void ChangeRowVisibility(int row, bool visible)
        {
            if (visible)
            {
                tableMain.RowStyles[row].SizeType = SizeType.AutoSize;

            }
            else
            {
                tableMain.RowStyles[row].SizeType = SizeType.Absolute;
                tableMain.RowStyles[row].Height = 0;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                var data = GetFunctionArgument();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void comboDistributionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericDegreesOfFreedom.Enabled = comboDistributionType.SelectedIndex == 1;
        }
    }
}
