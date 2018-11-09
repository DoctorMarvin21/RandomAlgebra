using RandomsAlgebra;
using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Distributions
{
    public partial class CommonDistributionSettingsForm : Form
    {
        private DistributionSettings _settings = null;
        private Label[] _labels = null;
        private TextBox[] _textBoxes = null;
        private List<SettingsMembers> _members = new List<SettingsMembers>();

        public CommonDistributionSettingsForm(DistributionSettings settings)
        {
            InitializeComponent();

            Text = Languages.GetText("DistributionSettings");
            btnOk.Text = Languages.GetText("ButtonOk");
            btnCancel.Text = Languages.GetText("ButtonCancel");

            _labels = new Label[] { lbParameter0, lbParameter1, lbParameter2, lbParameter3, lbParameter4 };
            _textBoxes = new TextBox[] { txtParameter0, txtParameter1, txtParameter2, txtParameter3, txtParameter4 };

            _settings = settings;
            OnSettingsSet(settings);
        }

        private void OnSettingsSet(DistributionSettings distributionSettings)
        {
            var properties = distributionSettings.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanRead && x.CanWrite).ToArray();


            if (properties.Length > _labels.Length)
                throw new ArgumentOutOfRangeException(nameof(properties));

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];

                _members.Add(new SettingsMembers(_labels[i], _textBoxes[i], distributionSettings, property));
            }

        }

        protected virtual void OnOK()
        {

            foreach (var property in _members)
            {
                property.UpdateValue();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                OnOK();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Languages.GetText("Exception"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private class SettingsMembers
        {
            public SettingsMembers(Label nameHolder, TextBox valueHolder, DistributionSettings owner, PropertyInfo info)
            {
                var name = Languages.GetText(info.Name);
                nameHolder.Text = name;
                valueHolder.Text = info.GetValue(owner, null)?.ToString();

                nameHolder.Visible = true;
                valueHolder.Visible = true;

                NameHolder = nameHolder;
                ValueHolder = valueHolder;
                Owner = owner;
                Info = info;
                
            }

            public DistributionSettings Owner
            {
                get;
                private set;
            }

            public PropertyInfo Info
            {
                get;
                private set;
            }

            public Label NameHolder
            {
                get;
                private set;
            }

            public TextBox ValueHolder
            {
                get;
                private set;
            }

            public void UpdateValue()
            {
                try
                {
                    string text = ValueHolder.Text.Replace(',', '.');
                    var obj = Convert.ChangeType(text, Info.PropertyType, System.Globalization.CultureInfo.InvariantCulture);
                    Info.SetValue(Owner, obj, null);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        throw ex.InnerException;
                    else
                        throw ex;
                }
            }
        }
    }
}
