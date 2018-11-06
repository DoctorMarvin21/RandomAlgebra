using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Distribuitons
{
    public partial class OptimizationsForm : Form
    {
        public OptimizationsForm()
        {
            InitializeComponent();

            Text = Multilanguage.GetText("FormOptimizationsName");
            checkContiniousConvolution.Text = Multilanguage.GetText("FormOptimizationsCheckContinuous");
            checkFFTConvolution.Text = Multilanguage.GetText("FormOptimizationsCheckFFT");
            btnOk.Text = Multilanguage.GetText("ButtonOkName");
            btnCancel.Text = Multilanguage.GetText("ButtonCancelName");


            checkContiniousConvolution.Checked = Optimizations.UseContiniousConvolution;
            checkFFTConvolution.Checked = Optimizations.UseFFTConvolution;

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Optimizations.UseContiniousConvolution = checkContiniousConvolution.Checked;
            Optimizations.UseFFTConvolution = checkFFTConvolution.Checked;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
