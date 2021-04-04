using RandomAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Distributions
{
    public partial class OptimizationsForm : Form
    {
        public OptimizationsForm()
        {
            InitializeComponent();

            Text = Languages.GetText("Optimizations");
            checkContinuousConvolution.Text = Languages.GetText("CheckContinuous");
            checkFFTConvolution.Text = Languages.GetText("CheckFFT");
            btnOk.Text = Languages.GetText("ButtonOk");
            btnCancel.Text = Languages.GetText("ButtonCancel");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
