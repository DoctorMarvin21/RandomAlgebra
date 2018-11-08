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

namespace Distributions
{
    public partial class MultivariateDistributionSettingsForm : Form
    {
        DataTable _matrix = new DataTable();
        BindingSource _matrixSource = new BindingSource();
        DataTable _arguments = new DataTable();
        BindingSource _argumentsSource = new BindingSource();

        DataTable _means = new DataTable();
        BindingSource _meansSource = new BindingSource();

        DataTable _coeff = new DataTable();
        BindingSource _coeffSource = new BindingSource();

        public MultivariateDistributionSettingsForm(bool showCoeff, bool showDistributionParameters)
        {
            InitializeComponent();
            
            Text = Languages.GetText("MultivariateDistribution");
            btnOk.Text = Languages.GetText("ButtonOk");
            btnCancel.Text = Languages.GetText("ButtonCancel");

            groupMatrixParameters.Text = Languages.GetText("GroupMatrixParameters");
            groupDistributionParameters.Text = Languages.GetText("GroupDistributionParameters");


            _matrixSource.DataSource = _matrix;
            _argumentsSource.DataSource = _arguments;
            _meansSource.DataSource = _means;
            _coeffSource.DataSource = _coeff;

            comboDistributionType.DataSource = new string[] { Languages.GetText(nameof(MultivariateNormalDistributionSettings)), Languages.GetText(nameof(MultivariateTDistributionSettings)) };


            groupDistributionParameters.Visible = showDistributionParameters;
            ChangeRowVisibility(1, showCoeff);

        }

        public MultivariateDistributionSettingsForm(MultivariateDistributionFunctionArgument argument) : this(false, true)
        {
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
                    _matrix.Rows[j][i] = covMatrix[i, j];
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
                    _matrix.Rows[j][i] = covMatrix[i, j];
                }
            }
        }

        private void btnBuildTables_Click(object sender, EventArgs e)
        {
            BuildTables();
        }

        private void BuildTables()
        {
            _arguments.Rows.Clear();
            _arguments.Columns.Clear();

            _matrix.Rows.Clear();
            _matrix.Columns.Clear();

            _means.Rows.Clear();
            _means.Columns.Clear();

            _coeff.Rows.Clear();
            _coeff.Columns.Clear();

            int dimesion = (int)numericDimensions.Value;

            for (int i = 0; i < dimesion; i++)
            {
                _matrix.Columns.Add(new DataColumn($"{Languages.GetText("Distribution")} {i + 1}", typeof(double)) { AllowDBNull = false });
                _arguments.Columns.Add(new DataColumn($"{Languages.GetText("Distribution")} {i + 1}", typeof(string)) { AllowDBNull = false });
                _means.Columns.Add(new DataColumn($"{Languages.GetText("Distribution")} {i + 1}", typeof(double)) { AllowDBNull = false });
                _coeff.Columns.Add(new DataColumn($"{Languages.GetText("Distribution")} {i + 1}", typeof(double)) { AllowDBNull = false });
            }

            object[] args = new object[dimesion];
            for (int i = 0; i < dimesion; i++)
            {
                args[i] = Encoding.ASCII.GetString(new byte[] { (byte)(i + 0x41) });
            }

            _arguments.Rows.Add(args);

            for (int i = 0; i < dimesion; i++)
            {
                object[] row = new object[dimesion];
                for (int j = 0; j < dimesion; j++)
                {
                    row[j] = j == i ? 1d : 0d;
                }
                _matrix.Rows.Add(row);
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

            dataGridMatrix.DataSource = _matrixSource;
            dataGridArguments.DataSource = _argumentsSource;
            dataGridMeans.DataSource = _meansSource;
            dataGridCoeff.DataSource = _coeffSource;

            for (int i = 0; i < dimesion; i++)
            {
                dataGridMatrix.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
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
                throw new Exception(Languages.GetText("ExceptionArgumentsMissing"));
            }
        }

        public MultivariateDistributionSettings GetSettings()
        {
            if (_matrix.Rows.Count > 0)
            {
                double degrees = (double)numericDegreesOfFreedom.Value;

                double[,] input = new double[_matrix.Columns.Count, _matrix.Rows.Count];

                for (int i = 0; i < _matrix.Columns.Count; i++)
                {
                    for (int j = 0; j < _matrix.Rows.Count; j++)
                    {
                        input[i, j] = (double)_matrix.Rows[j][i];
                    }
                }

                double[] means = _means.Rows[0].ItemArray.Cast<double>().ToArray();

                if (comboDistributionType.SelectedIndex == 0)
                {
                    return new MultivariateNormalDistributionSettings(means, input);
                }
                else if (comboDistributionType.SelectedIndex == 1)
                {
                    return new MultivariateTDistributionSettings(means, input, degrees);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                throw new Exception(Languages.GetText("ExceptionMaxtrixMissing"));
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
                throw new Exception(Languages.GetText("ExceptionCoeffitientsMissing"));
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
                MessageBox.Show(ex.Message, Languages.GetText("Exception"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
