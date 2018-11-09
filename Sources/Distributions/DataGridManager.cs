using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Distributions
{
    public class DataGridManager
    {
        static List<DataGridManager> _managers = new List<DataGridManager>();
        bool _enhancedMultiline = true;

        NumberFormatInfo _nfi = new NumberFormatInfo { NumberDecimalSeparator = ".", CurrencyDecimalSeparator = ".", PercentDecimalSeparator = "." };

        private DataGridManager(DataGridView datagrid)
        {
            datagrid.DataError += Datagrid_DataError;
            datagrid.KeyDown += Datagrid_KeyDown;
            DataGrid = datagrid;
        }

        private void Datagrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                foreach (DataGridViewCell cell in DataGrid.SelectedCells)
                {
                    if (!cell.ReadOnly && cell is DataGridViewTextBoxCell textBoxCell)
                    {
                        if (Nullable.GetUnderlyingType(textBoxCell.ValueType) != null)
                        {
                            cell.Value = null;

                            e.SuppressKeyPress = true;
                            e.Handled = true;
                        }
                        else if (textBoxCell.ValueType == typeof(string))
                        {
                            cell.Value = "";

                            e.SuppressKeyPress = true;
                            e.Handled = true;
                        }

                    }
                }
            }
            else if (_enhancedMultiline && e.Modifiers == Keys.Control && e.KeyCode == Keys.C)
            {

                var selectedCells = DataGrid.SelectedCells.Cast<DataGridViewCell>();

                if (selectedCells.Count() > 0)
                {

                    var minCellRow = selectedCells.Min(x => x.RowIndex);
                    var minCellColumn = selectedCells.Min(x => x.ColumnIndex);

                    var maxCellRow = selectedCells.Max(x => x.RowIndex);
                    var maxCellColumn = selectedCells.Max(x => x.ColumnIndex);

                    string[,] values = new string[maxCellColumn - minCellColumn + 1, maxCellRow - minCellRow + 1];

                    string resulted = string.Empty;
                    for (int j = minCellRow; j <= maxCellRow; j++)
                    {
                        for (int i = minCellColumn; i <= maxCellColumn; i++)
                        {

                            string value = string.Empty;
                            var cell = DataGrid[i, j];

                            if (selectedCells.Contains(cell))
                                value = (cell.Value ?? "").ToString();

                            value = value.Replace('\r', '\n');

                            resulted += value;

                            if (i == maxCellColumn)
                                resulted += "\r\n";
                            else
                                resulted += '\t';
                        }
                    }
                    Clipboard.SetText(resulted, TextDataFormat.UnicodeText);
                }

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (!DataGrid.ReadOnly && e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
            {
                try
                {
                    string text = Clipboard.GetText(TextDataFormat.UnicodeText);

                    if (!string.IsNullOrWhiteSpace(text) && DataGrid.SelectedCells.Count > 0)
                    {

                        string[][] dataTable = TextToTextTable(text);

                        var minRow = DataGrid.SelectedCells.Cast<DataGridViewCell>().Min(x => x.RowIndex);
                        if (minRow < 0)
                            minRow = 0;
                        var minColumn = DataGrid.SelectedCells.Cast<DataGridViewCell>().Min(x => x.ColumnIndex);
                        if (minColumn < 0)
                            minColumn = 0;

                        bool notCheckSelection = DataGrid.SelectedCells.Count == 1;

                        for (int i = minRow; i < DataGrid.RowCount && i - minRow < dataTable.Length; i++)
                        {
                            string[] row = dataTable[i - minRow];

                            for (int j = minColumn; j < DataGrid.ColumnCount && j - minColumn < row.Length; j++)
                            {
                                var cell = DataGrid[j, i];

                                if (!cell.ReadOnly && (notCheckSelection || cell.Selected) && cell is DataGridViewTextBoxCell tbCell)
                                {
                                    string newValue = row[j - minColumn];
                                    object result = null;
                                    try
                                    {
                                        Type type = tbCell.ValueType;
                                        Type nullableType = Nullable.GetUnderlyingType(type);
                                        if (nullableType != null)
                                            type = nullableType;

                                        result = Convert.ChangeType(newValue, type);
                                    }
                                    catch { }

                                    if (result != null)
                                    {
                                        if (_enhancedMultiline && result is string resultString)
                                            result = resultString.Replace("\n\n", "\r\n");

                                        tbCell.Value = result;
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private string[][] TextToTextTable(string text)
        {
            string[] rows = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            string[][] table = new string[rows.Length][];


            for (int i = 0; i < rows.Length; i++)
            {
                table[i] = rows[i].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            }

            return table;
        }

        private void Datagrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Context.HasFlag(DataGridViewDataErrorContexts.Parsing))
            {
                bool parsed = false;
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    var cell = DataGrid[e.ColumnIndex, e.RowIndex];
                    if (cell.EditedFormattedValue is string edited && IsFractional(cell.ValueType))
                    {
                        edited = edited.Replace(",", ".");
                        try
                        {
                            cell.Value = Convert.ChangeType(edited, cell.ValueType, _nfi);
                            parsed = true;
                        }
                        catch
                        {
                        }
                    }
                }
                if (!parsed)
                    MessageBox.Show(e.Exception.Message, Languages.GetText("Exception"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = false;
            }
            else
            {
                MessageBox.Show(e.Exception.Message, Languages.GetText("Exception"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private bool IsFractional(Type type)
        {
            var tc = Type.GetTypeCode(type);
            switch (tc)
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default: return false;
            }
        }

        private DataGridView DataGrid
        {
            get;
        }


        public static void StartManage(DataGridView datagrid)
        {
            _managers.Add(new DataGridManager(datagrid));
        }
    }
}
