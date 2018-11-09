namespace Distributions
{
    partial class OptimizationsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkContiniousConvolution = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.checkFFTConvolution = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkContiniousConvolution
            // 
            this.checkContiniousConvolution.AutoSize = true;
            this.checkContiniousConvolution.Location = new System.Drawing.Point(12, 12);
            this.checkContiniousConvolution.Name = "checkContiniousConvolution";
            this.checkContiniousConvolution.Size = new System.Drawing.Size(161, 17);
            this.checkContiniousConvolution.TabIndex = 0;
            this.checkContiniousConvolution.Text = "checkContiniousConvolution";
            this.checkContiniousConvolution.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(180, 58);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(99, 58);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // checkFFTConvolution
            // 
            this.checkFFTConvolution.AutoSize = true;
            this.checkFFTConvolution.Location = new System.Drawing.Point(12, 35);
            this.checkFFTConvolution.Name = "checkFFTConvolution";
            this.checkFFTConvolution.Size = new System.Drawing.Size(131, 17);
            this.checkFFTConvolution.TabIndex = 1;
            this.checkFFTConvolution.Text = "checkFFTConvolution";
            this.checkFFTConvolution.UseVisualStyleBackColor = true;
            // 
            // OptimizationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 89);
            this.Controls.Add(this.checkFFTConvolution);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.checkContiniousConvolution);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptimizationsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OptimizationsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkContiniousConvolution;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox checkFFTConvolution;
    }
}