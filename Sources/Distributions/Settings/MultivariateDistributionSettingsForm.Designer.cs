namespace Distributions
{
    partial class MultivariateDistributionSettingsForm
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
            this.groupMatrixParameters = new System.Windows.Forms.GroupBox();
            this.btnBuildTables = new System.Windows.Forms.Button();
            this.numericDimensions = new System.Windows.Forms.NumericUpDown();
            this.lbDimensions = new System.Windows.Forms.Label();
            this.comboDistributionType = new System.Windows.Forms.ComboBox();
            this.lbDistributionType = new System.Windows.Forms.Label();
            this.dataGridMatrix = new System.Windows.Forms.DataGridView();
            this.groupMatrix = new System.Windows.Forms.GroupBox();
            this.groupArguments = new System.Windows.Forms.GroupBox();
            this.dataGridArguments = new System.Windows.Forms.DataGridView();
            this.groupDistributionParameters = new System.Windows.Forms.GroupBox();
            this.numericDegreesOfFreedom = new System.Windows.Forms.NumericUpDown();
            this.lbDegreesOfFreedom = new System.Windows.Forms.Label();
            this.groupMeans = new System.Windows.Forms.GroupBox();
            this.dataGridMeans = new System.Windows.Forms.DataGridView();
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.groupCoefficients = new System.Windows.Forms.GroupBox();
            this.dataGridCoefficients = new System.Windows.Forms.DataGridView();
            this.panelParameters = new System.Windows.Forms.Panel();
            this.PanelControl = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupMatrixParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericDimensions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMatrix)).BeginInit();
            this.groupMatrix.SuspendLayout();
            this.groupArguments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridArguments)).BeginInit();
            this.groupDistributionParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericDegreesOfFreedom)).BeginInit();
            this.groupMeans.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMeans)).BeginInit();
            this.tableMain.SuspendLayout();
            this.groupCoefficients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCoefficients)).BeginInit();
            this.panelParameters.SuspendLayout();
            this.PanelControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupMatrixParameters
            // 
            this.groupMatrixParameters.Controls.Add(this.btnBuildTables);
            this.groupMatrixParameters.Controls.Add(this.numericDimensions);
            this.groupMatrixParameters.Controls.Add(this.lbDimensions);
            this.groupMatrixParameters.Location = new System.Drawing.Point(3, 3);
            this.groupMatrixParameters.Name = "groupMatrixParameters";
            this.groupMatrixParameters.Size = new System.Drawing.Size(157, 102);
            this.groupMatrixParameters.TabIndex = 0;
            this.groupMatrixParameters.TabStop = false;
            this.groupMatrixParameters.Text = "groupMatrixParameters";
            // 
            // btnBuildTables
            // 
            this.btnBuildTables.Location = new System.Drawing.Point(6, 70);
            this.btnBuildTables.Name = "btnBuildTables";
            this.btnBuildTables.Size = new System.Drawing.Size(143, 23);
            this.btnBuildTables.TabIndex = 1;
            this.btnBuildTables.Text = "btnBuildTables";
            this.btnBuildTables.UseVisualStyleBackColor = true;
            this.btnBuildTables.Click += new System.EventHandler(this.btnBuildTables_Click);
            // 
            // numericDimensions
            // 
            this.numericDimensions.Location = new System.Drawing.Point(9, 32);
            this.numericDimensions.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericDimensions.Name = "numericDimensions";
            this.numericDimensions.Size = new System.Drawing.Size(140, 20);
            this.numericDimensions.TabIndex = 0;
            this.numericDimensions.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // lbDimensions
            // 
            this.lbDimensions.AutoSize = true;
            this.lbDimensions.Location = new System.Drawing.Point(6, 16);
            this.lbDimensions.Name = "lbDimensions";
            this.lbDimensions.Size = new System.Drawing.Size(69, 13);
            this.lbDimensions.TabIndex = 0;
            this.lbDimensions.Text = "lbDimensions";
            // 
            // comboDistributionType
            // 
            this.comboDistributionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDistributionType.FormattingEnabled = true;
            this.comboDistributionType.Location = new System.Drawing.Point(7, 33);
            this.comboDistributionType.Name = "comboDistributionType";
            this.comboDistributionType.Size = new System.Drawing.Size(150, 21);
            this.comboDistributionType.TabIndex = 0;
            this.comboDistributionType.SelectedIndexChanged += new System.EventHandler(this.comboDistributionType_SelectedIndexChanged);
            // 
            // lbDistributionType
            // 
            this.lbDistributionType.AutoSize = true;
            this.lbDistributionType.Location = new System.Drawing.Point(7, 16);
            this.lbDistributionType.Name = "lbDistributionType";
            this.lbDistributionType.Size = new System.Drawing.Size(91, 13);
            this.lbDistributionType.TabIndex = 0;
            this.lbDistributionType.Text = "lbDistributionType";
            // 
            // dataGridMatrix
            // 
            this.dataGridMatrix.AllowUserToAddRows = false;
            this.dataGridMatrix.AllowUserToDeleteRows = false;
            this.dataGridMatrix.AllowUserToResizeRows = false;
            this.dataGridMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridMatrix.Location = new System.Drawing.Point(3, 16);
            this.dataGridMatrix.Name = "dataGridMatrix";
            this.dataGridMatrix.Size = new System.Drawing.Size(493, 112);
            this.dataGridMatrix.TabIndex = 0;
            // 
            // groupMatrix
            // 
            this.groupMatrix.Controls.Add(this.dataGridMatrix);
            this.groupMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupMatrix.Location = new System.Drawing.Point(3, 271);
            this.groupMatrix.Name = "groupMatrix";
            this.groupMatrix.Size = new System.Drawing.Size(499, 131);
            this.groupMatrix.TabIndex = 0;
            this.groupMatrix.TabStop = false;
            this.groupMatrix.Text = "groupMatrix";
            // 
            // groupArguments
            // 
            this.groupArguments.Controls.Add(this.dataGridArguments);
            this.groupArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupArguments.Location = new System.Drawing.Point(3, 195);
            this.groupArguments.Name = "groupArguments";
            this.groupArguments.Size = new System.Drawing.Size(499, 70);
            this.groupArguments.TabIndex = 0;
            this.groupArguments.TabStop = false;
            this.groupArguments.Text = "groupArguments";
            // 
            // dataGridArguments
            // 
            this.dataGridArguments.AllowUserToAddRows = false;
            this.dataGridArguments.AllowUserToDeleteRows = false;
            this.dataGridArguments.AllowUserToResizeRows = false;
            this.dataGridArguments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridArguments.Location = new System.Drawing.Point(3, 16);
            this.dataGridArguments.Name = "dataGridArguments";
            this.dataGridArguments.Size = new System.Drawing.Size(493, 51);
            this.dataGridArguments.TabIndex = 0;
            // 
            // groupDistributionParameters
            // 
            this.groupDistributionParameters.Controls.Add(this.numericDegreesOfFreedom);
            this.groupDistributionParameters.Controls.Add(this.lbDegreesOfFreedom);
            this.groupDistributionParameters.Controls.Add(this.lbDistributionType);
            this.groupDistributionParameters.Controls.Add(this.comboDistributionType);
            this.groupDistributionParameters.Location = new System.Drawing.Point(166, 3);
            this.groupDistributionParameters.Name = "groupDistributionParameters";
            this.groupDistributionParameters.Size = new System.Drawing.Size(166, 102);
            this.groupDistributionParameters.TabIndex = 1;
            this.groupDistributionParameters.TabStop = false;
            this.groupDistributionParameters.Text = "groupDistributionParameters";
            // 
            // numericDegreesOfFreedom
            // 
            this.numericDegreesOfFreedom.DecimalPlaces = 4;
            this.numericDegreesOfFreedom.Enabled = false;
            this.numericDegreesOfFreedom.Location = new System.Drawing.Point(7, 73);
            this.numericDegreesOfFreedom.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericDegreesOfFreedom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericDegreesOfFreedom.Name = "numericDegreesOfFreedom";
            this.numericDegreesOfFreedom.Size = new System.Drawing.Size(150, 20);
            this.numericDegreesOfFreedom.TabIndex = 2;
            this.numericDegreesOfFreedom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbDegreesOfFreedom
            // 
            this.lbDegreesOfFreedom.AutoSize = true;
            this.lbDegreesOfFreedom.Location = new System.Drawing.Point(6, 57);
            this.lbDegreesOfFreedom.Name = "lbDegreesOfFreedom";
            this.lbDegreesOfFreedom.Size = new System.Drawing.Size(107, 13);
            this.lbDegreesOfFreedom.TabIndex = 0;
            this.lbDegreesOfFreedom.Text = "lbDegreesOfFreedom";
            // 
            // groupMeans
            // 
            this.groupMeans.Controls.Add(this.dataGridMeans);
            this.groupMeans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupMeans.Location = new System.Drawing.Point(3, 408);
            this.groupMeans.Name = "groupMeans";
            this.groupMeans.Size = new System.Drawing.Size(499, 70);
            this.groupMeans.TabIndex = 0;
            this.groupMeans.TabStop = false;
            this.groupMeans.Text = "groupMeans";
            // 
            // dataGridMeans
            // 
            this.dataGridMeans.AllowUserToAddRows = false;
            this.dataGridMeans.AllowUserToDeleteRows = false;
            this.dataGridMeans.AllowUserToResizeRows = false;
            this.dataGridMeans.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMeans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridMeans.Location = new System.Drawing.Point(3, 16);
            this.dataGridMeans.Name = "dataGridMeans";
            this.dataGridMeans.Size = new System.Drawing.Size(493, 51);
            this.dataGridMeans.TabIndex = 0;
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 1;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.groupCoefficients, 0, 1);
            this.tableMain.Controls.Add(this.panelParameters, 0, 0);
            this.tableMain.Controls.Add(this.groupMeans, 0, 4);
            this.tableMain.Controls.Add(this.groupArguments, 0, 2);
            this.tableMain.Controls.Add(this.groupMatrix, 0, 3);
            this.tableMain.Controls.Add(this.PanelControl, 0, 5);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 6;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableMain.Size = new System.Drawing.Size(505, 516);
            this.tableMain.TabIndex = 0;
            // 
            // groupCoefficients
            // 
            this.groupCoefficients.Controls.Add(this.dataGridCoefficients);
            this.groupCoefficients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupCoefficients.Location = new System.Drawing.Point(3, 119);
            this.groupCoefficients.Name = "groupCoefficients";
            this.groupCoefficients.Size = new System.Drawing.Size(499, 70);
            this.groupCoefficients.TabIndex = 0;
            this.groupCoefficients.TabStop = false;
            this.groupCoefficients.Text = "groupCoefficients";
            // 
            // dataGridCoefficients
            // 
            this.dataGridCoefficients.AllowUserToAddRows = false;
            this.dataGridCoefficients.AllowUserToDeleteRows = false;
            this.dataGridCoefficients.AllowUserToResizeRows = false;
            this.dataGridCoefficients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCoefficients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridCoefficients.Location = new System.Drawing.Point(3, 16);
            this.dataGridCoefficients.Name = "dataGridCoefficients";
            this.dataGridCoefficients.Size = new System.Drawing.Size(493, 51);
            this.dataGridCoefficients.TabIndex = 0;
            // 
            // panelParameters
            // 
            this.panelParameters.Controls.Add(this.groupMatrixParameters);
            this.panelParameters.Controls.Add(this.groupDistributionParameters);
            this.panelParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParameters.Location = new System.Drawing.Point(3, 3);
            this.panelParameters.Name = "panelParameters";
            this.panelParameters.Size = new System.Drawing.Size(499, 110);
            this.panelParameters.TabIndex = 0;
            // 
            // PanelControl
            // 
            this.PanelControl.Controls.Add(this.btnCancel);
            this.PanelControl.Controls.Add(this.btnOk);
            this.PanelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelControl.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.PanelControl.Location = new System.Drawing.Point(3, 484);
            this.PanelControl.Name = "PanelControl";
            this.PanelControl.Size = new System.Drawing.Size(499, 29);
            this.PanelControl.TabIndex = 4;
            this.PanelControl.WrapContents = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(421, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(340, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // MultivariateDistributionSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 516);
            this.Controls.Add(this.tableMain);
            this.Name = "MultivariateDistributionSettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MultivariateDistributionSettingsForm";
            this.groupMatrixParameters.ResumeLayout(false);
            this.groupMatrixParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericDimensions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMatrix)).EndInit();
            this.groupMatrix.ResumeLayout(false);
            this.groupArguments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridArguments)).EndInit();
            this.groupDistributionParameters.ResumeLayout(false);
            this.groupDistributionParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericDegreesOfFreedom)).EndInit();
            this.groupMeans.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMeans)).EndInit();
            this.tableMain.ResumeLayout(false);
            this.groupCoefficients.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCoefficients)).EndInit();
            this.panelParameters.ResumeLayout(false);
            this.PanelControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupMatrixParameters;
        private System.Windows.Forms.Button btnBuildTables;
        private System.Windows.Forms.NumericUpDown numericDimensions;
        private System.Windows.Forms.Label lbDimensions;
        private System.Windows.Forms.ComboBox comboDistributionType;
        private System.Windows.Forms.Label lbDistributionType;
        private System.Windows.Forms.DataGridView dataGridMatrix;
        private System.Windows.Forms.GroupBox groupMatrix;
        private System.Windows.Forms.GroupBox groupArguments;
        private System.Windows.Forms.DataGridView dataGridArguments;
        private System.Windows.Forms.GroupBox groupDistributionParameters;
        private System.Windows.Forms.GroupBox groupMeans;
        private System.Windows.Forms.DataGridView dataGridMeans;
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.Panel panelParameters;
        private System.Windows.Forms.FlowLayoutPanel PanelControl;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.NumericUpDown numericDegreesOfFreedom;
        private System.Windows.Forms.Label lbDegreesOfFreedom;
        private System.Windows.Forms.GroupBox groupCoefficients;
        private System.Windows.Forms.DataGridView dataGridCoefficients;
    }
}