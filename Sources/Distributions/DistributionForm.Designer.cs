namespace Distributions
{
    partial class DistributionForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutLeft = new System.Windows.Forms.TableLayoutPanel();
            this.panelFunction = new System.Windows.Forms.Panel();
            this.btnOptimizations = new System.Windows.Forms.Button();
            this.btnEvaluate = new System.Windows.Forms.Button();
            this.txtFunction = new System.Windows.Forms.TextBox();
            this.lbModel = new System.Windows.Forms.Label();
            this.tableLayoutSplitGraphics = new System.Windows.Forms.TableLayoutPanel();
            this.zedDistrCDF = new ZedGraph.ZedGraphControl();
            this.zedDistrPDF = new ZedGraph.ZedGraphControl();
            this.panelParameters = new System.Windows.Forms.Panel();
            this.groupCommonParameters = new System.Windows.Forms.GroupBox();
            this.numericChartPoints = new System.Windows.Forms.NumericUpDown();
            this.lbChartPoints = new System.Windows.Forms.Label();
            this.lbProbabilityP = new System.Windows.Forms.Label();
            this.numericProbability = new System.Windows.Forms.NumericUpDown();
            this.groupMonteCarloParameters = new System.Windows.Forms.GroupBox();
            this.checkEvaluateMonteCarlo = new System.Windows.Forms.CheckBox();
            this.lbExperimentsCount = new System.Windows.Forms.Label();
            this.numericExperimentsCount = new System.Windows.Forms.NumericUpDown();
            this.numericPocketsCount = new System.Windows.Forms.NumericUpDown();
            this.lbPocketsCount = new System.Windows.Forms.Label();
            this.groupRandomsAlgebraParameters = new System.Windows.Forms.GroupBox();
            this.checkEvaluateRandomsAlgebra = new System.Windows.Forms.CheckBox();
            this.lbSamplesCount = new System.Windows.Forms.Label();
            this.numericSamplesCount = new System.Windows.Forms.NumericUpDown();
            this.splitDistr = new System.Windows.Forms.SplitContainer();
            this.splitRight = new System.Windows.Forms.SplitContainer();
            this.splitDistributions = new System.Windows.Forms.SplitContainer();
            this.groupDistributions = new System.Windows.Forms.GroupBox();
            this.tableLayoutDistributionsList = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridUnivariateDistributions = new System.Windows.Forms.DataGridView();
            this.panelAddRemove = new System.Windows.Forms.Panel();
            this.btnRemoveUnivariateDistribution = new System.Windows.Forms.Button();
            this.btnAddUnivariateDistribution = new System.Windows.Forms.Button();
            this.groupMultivariate = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridMultivariateDistributions = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRemoveMultivariateDistibution = new System.Windows.Forms.Button();
            this.btnAddMultivariateDistribution = new System.Windows.Forms.Button();
            this.groupResults = new System.Windows.Forms.GroupBox();
            this.dataGridResults = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            this.tableLayoutLeft.SuspendLayout();
            this.panelFunction.SuspendLayout();
            this.tableLayoutSplitGraphics.SuspendLayout();
            this.panelParameters.SuspendLayout();
            this.groupCommonParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericChartPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericProbability)).BeginInit();
            this.groupMonteCarloParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericExperimentsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPocketsCount)).BeginInit();
            this.groupRandomsAlgebraParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSamplesCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitDistr)).BeginInit();
            this.splitDistr.Panel1.SuspendLayout();
            this.splitDistr.Panel2.SuspendLayout();
            this.splitDistr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).BeginInit();
            this.splitRight.Panel1.SuspendLayout();
            this.splitRight.Panel2.SuspendLayout();
            this.splitRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitDistributions)).BeginInit();
            this.splitDistributions.Panel1.SuspendLayout();
            this.splitDistributions.Panel2.SuspendLayout();
            this.splitDistributions.SuspendLayout();
            this.groupDistributions.SuspendLayout();
            this.tableLayoutDistributionsList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUnivariateDistributions)).BeginInit();
            this.panelAddRemove.SuspendLayout();
            this.groupMultivariate.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMultivariateDistributions)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResults)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutLeft
            // 
            this.tableLayoutLeft.ColumnCount = 1;
            this.tableLayoutLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutLeft.Controls.Add(this.panelFunction, 0, 0);
            this.tableLayoutLeft.Controls.Add(this.tableLayoutSplitGraphics, 0, 1);
            this.tableLayoutLeft.Controls.Add(this.panelParameters, 0, 2);
            this.tableLayoutLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutLeft.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutLeft.Name = "tableLayoutLeft";
            this.tableLayoutLeft.RowCount = 3;
            this.tableLayoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutLeft.Size = new System.Drawing.Size(941, 580);
            this.tableLayoutLeft.TabIndex = 1;
            // 
            // panelFunction
            // 
            this.panelFunction.Controls.Add(this.btnExport);
            this.panelFunction.Controls.Add(this.btnOptimizations);
            this.panelFunction.Controls.Add(this.btnEvaluate);
            this.panelFunction.Controls.Add(this.txtFunction);
            this.panelFunction.Controls.Add(this.lbModel);
            this.panelFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFunction.Location = new System.Drawing.Point(0, 0);
            this.panelFunction.Margin = new System.Windows.Forms.Padding(0);
            this.panelFunction.Name = "panelFunction";
            this.panelFunction.Padding = new System.Windows.Forms.Padding(3);
            this.panelFunction.Size = new System.Drawing.Size(941, 29);
            this.panelFunction.TabIndex = 1;
            // 
            // btnOptimizations
            // 
            this.btnOptimizations.Location = new System.Drawing.Point(275, 3);
            this.btnOptimizations.Name = "btnOptimizations";
            this.btnOptimizations.Size = new System.Drawing.Size(91, 23);
            this.btnOptimizations.TabIndex = 3;
            this.btnOptimizations.Text = "btnOptimizations";
            this.btnOptimizations.UseVisualStyleBackColor = true;
            this.btnOptimizations.Click += new System.EventHandler(this.btnOptimizations_Click);
            // 
            // btnEvaluate
            // 
            this.btnEvaluate.Location = new System.Drawing.Point(194, 3);
            this.btnEvaluate.Name = "btnEvaluate";
            this.btnEvaluate.Size = new System.Drawing.Size(75, 23);
            this.btnEvaluate.TabIndex = 2;
            this.btnEvaluate.Text = "btnEvaluate";
            this.btnEvaluate.UseVisualStyleBackColor = true;
            this.btnEvaluate.Click += new System.EventHandler(this.btnEvaluate_Click);
            // 
            // txtFunction
            // 
            this.txtFunction.Location = new System.Drawing.Point(53, 4);
            this.txtFunction.Name = "txtFunction";
            this.txtFunction.Size = new System.Drawing.Size(135, 20);
            this.txtFunction.TabIndex = 1;
            this.txtFunction.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFunction_KeyDown);
            // 
            // lbModel
            // 
            this.lbModel.AutoSize = true;
            this.lbModel.Location = new System.Drawing.Point(6, 8);
            this.lbModel.Name = "lbModel";
            this.lbModel.Size = new System.Drawing.Size(44, 13);
            this.lbModel.TabIndex = 0;
            this.lbModel.Text = "lbModel";
            // 
            // tableLayoutSplitGraphics
            // 
            this.tableLayoutSplitGraphics.ColumnCount = 2;
            this.tableLayoutSplitGraphics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutSplitGraphics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutSplitGraphics.Controls.Add(this.zedDistrCDF, 0, 0);
            this.tableLayoutSplitGraphics.Controls.Add(this.zedDistrPDF, 0, 0);
            this.tableLayoutSplitGraphics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutSplitGraphics.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutSplitGraphics.Name = "tableLayoutSplitGraphics";
            this.tableLayoutSplitGraphics.RowCount = 1;
            this.tableLayoutSplitGraphics.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutSplitGraphics.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 465F));
            this.tableLayoutSplitGraphics.Size = new System.Drawing.Size(935, 465);
            this.tableLayoutSplitGraphics.TabIndex = 2;
            // 
            // zedDistrCDF
            // 
            this.zedDistrCDF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedDistrCDF.Location = new System.Drawing.Point(470, 3);
            this.zedDistrCDF.Name = "zedDistrCDF";
            this.zedDistrCDF.ScrollGrace = 0D;
            this.zedDistrCDF.ScrollMaxX = 0D;
            this.zedDistrCDF.ScrollMaxY = 0D;
            this.zedDistrCDF.ScrollMaxY2 = 0D;
            this.zedDistrCDF.ScrollMinX = 0D;
            this.zedDistrCDF.ScrollMinY = 0D;
            this.zedDistrCDF.ScrollMinY2 = 0D;
            this.zedDistrCDF.Size = new System.Drawing.Size(462, 459);
            this.zedDistrCDF.TabIndex = 0;
            this.zedDistrCDF.UseExtendedPrintDialog = true;
            // 
            // zedDistrPDF
            // 
            this.zedDistrPDF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedDistrPDF.Location = new System.Drawing.Point(3, 3);
            this.zedDistrPDF.Name = "zedDistrPDF";
            this.zedDistrPDF.ScrollGrace = 0D;
            this.zedDistrPDF.ScrollMaxX = 0D;
            this.zedDistrPDF.ScrollMaxY = 0D;
            this.zedDistrPDF.ScrollMaxY2 = 0D;
            this.zedDistrPDF.ScrollMinX = 0D;
            this.zedDistrPDF.ScrollMinY = 0D;
            this.zedDistrPDF.ScrollMinY2 = 0D;
            this.zedDistrPDF.Size = new System.Drawing.Size(461, 459);
            this.zedDistrPDF.TabIndex = 0;
            this.zedDistrPDF.UseExtendedPrintDialog = true;
            // 
            // panelParameters
            // 
            this.panelParameters.Controls.Add(this.groupCommonParameters);
            this.panelParameters.Controls.Add(this.groupMonteCarloParameters);
            this.panelParameters.Controls.Add(this.groupRandomsAlgebraParameters);
            this.panelParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParameters.Location = new System.Drawing.Point(3, 503);
            this.panelParameters.Name = "panelParameters";
            this.panelParameters.Size = new System.Drawing.Size(935, 74);
            this.panelParameters.TabIndex = 3;
            // 
            // groupCommonParameters
            // 
            this.groupCommonParameters.Controls.Add(this.numericChartPoints);
            this.groupCommonParameters.Controls.Add(this.lbChartPoints);
            this.groupCommonParameters.Controls.Add(this.lbProbabilityP);
            this.groupCommonParameters.Controls.Add(this.numericProbability);
            this.groupCommonParameters.Location = new System.Drawing.Point(382, 3);
            this.groupCommonParameters.Name = "groupCommonParameters";
            this.groupCommonParameters.Size = new System.Drawing.Size(225, 64);
            this.groupCommonParameters.TabIndex = 2;
            this.groupCommonParameters.TabStop = false;
            this.groupCommonParameters.Text = "groupCommonParameters";
            // 
            // numericChartPoints
            // 
            this.numericChartPoints.Location = new System.Drawing.Point(115, 36);
            this.numericChartPoints.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericChartPoints.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericChartPoints.Name = "numericChartPoints";
            this.numericChartPoints.Size = new System.Drawing.Size(100, 20);
            this.numericChartPoints.TabIndex = 2;
            this.numericChartPoints.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericChartPoints.ValueChanged += new System.EventHandler(this.numericChartPoints_ValueChanged);
            // 
            // lbChartPoints
            // 
            this.lbChartPoints.AutoSize = true;
            this.lbChartPoints.Location = new System.Drawing.Point(112, 20);
            this.lbChartPoints.Name = "lbChartPoints";
            this.lbChartPoints.Size = new System.Drawing.Size(69, 13);
            this.lbChartPoints.TabIndex = 0;
            this.lbChartPoints.Text = "lbChartPoints";
            // 
            // lbProbabilityP
            // 
            this.lbProbabilityP.AutoSize = true;
            this.lbProbabilityP.Location = new System.Drawing.Point(6, 20);
            this.lbProbabilityP.Name = "lbProbabilityP";
            this.lbProbabilityP.Size = new System.Drawing.Size(70, 13);
            this.lbProbabilityP.TabIndex = 0;
            this.lbProbabilityP.Text = "lbProbabilityP";
            // 
            // numericProbability
            // 
            this.numericProbability.DecimalPlaces = 3;
            this.numericProbability.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericProbability.Location = new System.Drawing.Point(9, 36);
            this.numericProbability.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericProbability.Name = "numericProbability";
            this.numericProbability.Size = new System.Drawing.Size(100, 20);
            this.numericProbability.TabIndex = 1;
            this.numericProbability.Value = new decimal(new int[] {
            95,
            0,
            0,
            131072});
            this.numericProbability.ValueChanged += new System.EventHandler(this.numericProbability_ValueChanged);
            // 
            // groupMonteCarloParameters
            // 
            this.groupMonteCarloParameters.Controls.Add(this.checkEvaluateMonteCarlo);
            this.groupMonteCarloParameters.Controls.Add(this.lbExperimentsCount);
            this.groupMonteCarloParameters.Controls.Add(this.numericExperimentsCount);
            this.groupMonteCarloParameters.Controls.Add(this.numericPocketsCount);
            this.groupMonteCarloParameters.Controls.Add(this.lbPocketsCount);
            this.groupMonteCarloParameters.Location = new System.Drawing.Point(153, 3);
            this.groupMonteCarloParameters.Name = "groupMonteCarloParameters";
            this.groupMonteCarloParameters.Size = new System.Drawing.Size(223, 64);
            this.groupMonteCarloParameters.TabIndex = 1;
            this.groupMonteCarloParameters.TabStop = false;
            // 
            // checkEvaluateMonteCarlo
            // 
            this.checkEvaluateMonteCarlo.AutoSize = true;
            this.checkEvaluateMonteCarlo.Location = new System.Drawing.Point(6, 0);
            this.checkEvaluateMonteCarlo.Name = "checkEvaluateMonteCarlo";
            this.checkEvaluateMonteCarlo.Size = new System.Drawing.Size(152, 17);
            this.checkEvaluateMonteCarlo.TabIndex = 0;
            this.checkEvaluateMonteCarlo.Text = "checkEvaluateMonteCarlo";
            this.checkEvaluateMonteCarlo.UseVisualStyleBackColor = true;
            this.checkEvaluateMonteCarlo.CheckedChanged += new System.EventHandler(this.checkEvaluateMonteCarlo_CheckedChanged);
            // 
            // lbExperimentsCount
            // 
            this.lbExperimentsCount.AutoSize = true;
            this.lbExperimentsCount.Location = new System.Drawing.Point(6, 20);
            this.lbExperimentsCount.Name = "lbExperimentsCount";
            this.lbExperimentsCount.Size = new System.Drawing.Size(100, 13);
            this.lbExperimentsCount.TabIndex = 0;
            this.lbExperimentsCount.Text = "lbExperimentsCount";
            // 
            // numericExperimentsCount
            // 
            this.numericExperimentsCount.Enabled = false;
            this.numericExperimentsCount.Location = new System.Drawing.Point(7, 36);
            this.numericExperimentsCount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericExperimentsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericExperimentsCount.Name = "numericExperimentsCount";
            this.numericExperimentsCount.Size = new System.Drawing.Size(100, 20);
            this.numericExperimentsCount.TabIndex = 1;
            this.numericExperimentsCount.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // numericPocketsCount
            // 
            this.numericPocketsCount.Enabled = false;
            this.numericPocketsCount.Location = new System.Drawing.Point(113, 36);
            this.numericPocketsCount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericPocketsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericPocketsCount.Name = "numericPocketsCount";
            this.numericPocketsCount.Size = new System.Drawing.Size(100, 20);
            this.numericPocketsCount.TabIndex = 2;
            this.numericPocketsCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lbPocketsCount
            // 
            this.lbPocketsCount.AutoSize = true;
            this.lbPocketsCount.Location = new System.Drawing.Point(112, 20);
            this.lbPocketsCount.Name = "lbPocketsCount";
            this.lbPocketsCount.Size = new System.Drawing.Size(82, 13);
            this.lbPocketsCount.TabIndex = 0;
            this.lbPocketsCount.Text = "lbPocketsCount";
            // 
            // groupRandomsAlgebraParameters
            // 
            this.groupRandomsAlgebraParameters.Controls.Add(this.checkEvaluateRandomsAlgebra);
            this.groupRandomsAlgebraParameters.Controls.Add(this.lbSamplesCount);
            this.groupRandomsAlgebraParameters.Controls.Add(this.numericSamplesCount);
            this.groupRandomsAlgebraParameters.Location = new System.Drawing.Point(9, 3);
            this.groupRandomsAlgebraParameters.Name = "groupRandomsAlgebraParameters";
            this.groupRandomsAlgebraParameters.Size = new System.Drawing.Size(138, 64);
            this.groupRandomsAlgebraParameters.TabIndex = 0;
            this.groupRandomsAlgebraParameters.TabStop = false;
            // 
            // checkEvaluateRandomsAlgebra
            // 
            this.checkEvaluateRandomsAlgebra.AutoSize = true;
            this.checkEvaluateRandomsAlgebra.Checked = true;
            this.checkEvaluateRandomsAlgebra.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEvaluateRandomsAlgebra.Location = new System.Drawing.Point(6, 0);
            this.checkEvaluateRandomsAlgebra.Name = "checkEvaluateRandomsAlgebra";
            this.checkEvaluateRandomsAlgebra.Size = new System.Drawing.Size(179, 17);
            this.checkEvaluateRandomsAlgebra.TabIndex = 0;
            this.checkEvaluateRandomsAlgebra.Text = "checkEvaluateRandomsAlgebra";
            this.checkEvaluateRandomsAlgebra.UseVisualStyleBackColor = true;
            this.checkEvaluateRandomsAlgebra.CheckedChanged += new System.EventHandler(this.checkEvaluateRandomsAlgebra_CheckedChanged);
            // 
            // lbSamplesCount
            // 
            this.lbSamplesCount.AutoSize = true;
            this.lbSamplesCount.Location = new System.Drawing.Point(6, 20);
            this.lbSamplesCount.Name = "lbSamplesCount";
            this.lbSamplesCount.Size = new System.Drawing.Size(83, 13);
            this.lbSamplesCount.TabIndex = 0;
            this.lbSamplesCount.Text = "lbSamplesCount";
            // 
            // numericSamplesCount
            // 
            this.numericSamplesCount.Location = new System.Drawing.Point(9, 36);
            this.numericSamplesCount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericSamplesCount.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericSamplesCount.Name = "numericSamplesCount";
            this.numericSamplesCount.Size = new System.Drawing.Size(120, 20);
            this.numericSamplesCount.TabIndex = 1;
            this.numericSamplesCount.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // splitDistr
            // 
            this.splitDistr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitDistr.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitDistr.Location = new System.Drawing.Point(0, 0);
            this.splitDistr.Name = "splitDistr";
            // 
            // splitDistr.Panel1
            // 
            this.splitDistr.Panel1.Controls.Add(this.tableLayoutLeft);
            // 
            // splitDistr.Panel2
            // 
            this.splitDistr.Panel2.Controls.Add(this.splitRight);
            this.splitDistr.Size = new System.Drawing.Size(1271, 580);
            this.splitDistr.SplitterDistance = 941;
            this.splitDistr.TabIndex = 2;
            // 
            // splitRight
            // 
            this.splitRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRight.Location = new System.Drawing.Point(0, 0);
            this.splitRight.Name = "splitRight";
            this.splitRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitRight.Panel1
            // 
            this.splitRight.Panel1.Controls.Add(this.splitDistributions);
            // 
            // splitRight.Panel2
            // 
            this.splitRight.Panel2.Controls.Add(this.groupResults);
            this.splitRight.Size = new System.Drawing.Size(326, 580);
            this.splitRight.SplitterDistance = 357;
            this.splitRight.TabIndex = 0;
            // 
            // splitDistributions
            // 
            this.splitDistributions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitDistributions.Location = new System.Drawing.Point(0, 0);
            this.splitDistributions.Name = "splitDistributions";
            this.splitDistributions.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitDistributions.Panel1
            // 
            this.splitDistributions.Panel1.Controls.Add(this.groupDistributions);
            // 
            // splitDistributions.Panel2
            // 
            this.splitDistributions.Panel2.Controls.Add(this.groupMultivariate);
            this.splitDistributions.Size = new System.Drawing.Size(326, 357);
            this.splitDistributions.SplitterDistance = 178;
            this.splitDistributions.TabIndex = 0;
            // 
            // groupDistributions
            // 
            this.groupDistributions.Controls.Add(this.tableLayoutDistributionsList);
            this.groupDistributions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupDistributions.Location = new System.Drawing.Point(0, 0);
            this.groupDistributions.Name = "groupDistributions";
            this.groupDistributions.Size = new System.Drawing.Size(326, 178);
            this.groupDistributions.TabIndex = 0;
            this.groupDistributions.TabStop = false;
            this.groupDistributions.Text = "groupDistributions";
            // 
            // tableLayoutDistributionsList
            // 
            this.tableLayoutDistributionsList.ColumnCount = 1;
            this.tableLayoutDistributionsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutDistributionsList.Controls.Add(this.dataGridUnivariateDistributions, 0, 0);
            this.tableLayoutDistributionsList.Controls.Add(this.panelAddRemove, 0, 1);
            this.tableLayoutDistributionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutDistributionsList.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutDistributionsList.Name = "tableLayoutDistributionsList";
            this.tableLayoutDistributionsList.RowCount = 2;
            this.tableLayoutDistributionsList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutDistributionsList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutDistributionsList.Size = new System.Drawing.Size(320, 159);
            this.tableLayoutDistributionsList.TabIndex = 1;
            // 
            // dataGridUnivariateDistributions
            // 
            this.dataGridUnivariateDistributions.AllowUserToResizeRows = false;
            this.dataGridUnivariateDistributions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridUnivariateDistributions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridUnivariateDistributions.Location = new System.Drawing.Point(3, 3);
            this.dataGridUnivariateDistributions.Name = "dataGridUnivariateDistributions";
            this.dataGridUnivariateDistributions.Size = new System.Drawing.Size(314, 121);
            this.dataGridUnivariateDistributions.TabIndex = 0;
            this.dataGridUnivariateDistributions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridDistributions_CellClick);
            this.dataGridUnivariateDistributions.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridDistributions_CellDoubleClick);
            // 
            // panelAddRemove
            // 
            this.panelAddRemove.Controls.Add(this.btnRemoveUnivariateDistribution);
            this.panelAddRemove.Controls.Add(this.btnAddUnivariateDistribution);
            this.panelAddRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAddRemove.Location = new System.Drawing.Point(0, 127);
            this.panelAddRemove.Margin = new System.Windows.Forms.Padding(0);
            this.panelAddRemove.Name = "panelAddRemove";
            this.panelAddRemove.Size = new System.Drawing.Size(320, 32);
            this.panelAddRemove.TabIndex = 1;
            // 
            // btnRemoveUnivariateDistribution
            // 
            this.btnRemoveUnivariateDistribution.Location = new System.Drawing.Point(81, 3);
            this.btnRemoveUnivariateDistribution.Name = "btnRemoveUnivariateDistribution";
            this.btnRemoveUnivariateDistribution.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveUnivariateDistribution.TabIndex = 1;
            this.btnRemoveUnivariateDistribution.Text = "btnRemoveUnivariateDistribution";
            this.btnRemoveUnivariateDistribution.UseVisualStyleBackColor = true;
            this.btnRemoveUnivariateDistribution.Click += new System.EventHandler(this.btnRemoveUnivariateDistribution_Click);
            // 
            // btnAddUnivariateDistribution
            // 
            this.btnAddUnivariateDistribution.Location = new System.Drawing.Point(3, 3);
            this.btnAddUnivariateDistribution.Name = "btnAddUnivariateDistribution";
            this.btnAddUnivariateDistribution.Size = new System.Drawing.Size(75, 23);
            this.btnAddUnivariateDistribution.TabIndex = 0;
            this.btnAddUnivariateDistribution.Text = "btnAddUnivariateDistribution";
            this.btnAddUnivariateDistribution.UseVisualStyleBackColor = true;
            this.btnAddUnivariateDistribution.Click += new System.EventHandler(this.btnAddUnivariateDistribution_Click);
            // 
            // groupMultivariate
            // 
            this.groupMultivariate.Controls.Add(this.tableLayoutPanel1);
            this.groupMultivariate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupMultivariate.Location = new System.Drawing.Point(0, 0);
            this.groupMultivariate.Name = "groupMultivariate";
            this.groupMultivariate.Size = new System.Drawing.Size(326, 175);
            this.groupMultivariate.TabIndex = 0;
            this.groupMultivariate.TabStop = false;
            this.groupMultivariate.Text = "groupMultivariate";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridMultivariateDistributions, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(320, 156);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // dataGridMultivariateDistributions
            // 
            this.dataGridMultivariateDistributions.AllowUserToResizeRows = false;
            this.dataGridMultivariateDistributions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMultivariateDistributions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridMultivariateDistributions.Location = new System.Drawing.Point(3, 3);
            this.dataGridMultivariateDistributions.Name = "dataGridMultivariateDistributions";
            this.dataGridMultivariateDistributions.Size = new System.Drawing.Size(314, 118);
            this.dataGridMultivariateDistributions.TabIndex = 0;
            this.dataGridMultivariateDistributions.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridMultivariateDistributions_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRemoveMultivariateDistibution);
            this.panel1.Controls.Add(this.btnAddMultivariateDistribution);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 124);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(320, 32);
            this.panel1.TabIndex = 1;
            // 
            // btnRemoveMultivariateDistibution
            // 
            this.btnRemoveMultivariateDistibution.Location = new System.Drawing.Point(81, 3);
            this.btnRemoveMultivariateDistibution.Name = "btnRemoveMultivariateDistibution";
            this.btnRemoveMultivariateDistibution.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveMultivariateDistibution.TabIndex = 1;
            this.btnRemoveMultivariateDistibution.Text = "btnRemoveMultivariateDistibution";
            this.btnRemoveMultivariateDistibution.UseVisualStyleBackColor = true;
            this.btnRemoveMultivariateDistibution.Click += new System.EventHandler(this.btnRemoveMultivariateDistribution_Click);
            // 
            // btnAddMultivariateDistribution
            // 
            this.btnAddMultivariateDistribution.Location = new System.Drawing.Point(3, 3);
            this.btnAddMultivariateDistribution.Name = "btnAddMultivariateDistribution";
            this.btnAddMultivariateDistribution.Size = new System.Drawing.Size(75, 23);
            this.btnAddMultivariateDistribution.TabIndex = 0;
            this.btnAddMultivariateDistribution.Text = "btnAddMultivariateDistribution";
            this.btnAddMultivariateDistribution.UseVisualStyleBackColor = true;
            this.btnAddMultivariateDistribution.Click += new System.EventHandler(this.btnAddMultivariateDistribution_Click);
            // 
            // groupResults
            // 
            this.groupResults.Controls.Add(this.dataGridResults);
            this.groupResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupResults.Location = new System.Drawing.Point(0, 0);
            this.groupResults.Name = "groupResults";
            this.groupResults.Size = new System.Drawing.Size(326, 219);
            this.groupResults.TabIndex = 0;
            this.groupResults.TabStop = false;
            this.groupResults.Text = "groupResults";
            // 
            // dataGridResults
            // 
            this.dataGridResults.AllowUserToAddRows = false;
            this.dataGridResults.AllowUserToDeleteRows = false;
            this.dataGridResults.AllowUserToResizeRows = false;
            this.dataGridResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridResults.Location = new System.Drawing.Point(3, 16);
            this.dataGridResults.Name = "dataGridResults";
            this.dataGridResults.ReadOnly = true;
            this.dataGridResults.Size = new System.Drawing.Size(320, 200);
            this.dataGridResults.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExport.Location = new System.Drawing.Point(863, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // DistributionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1271, 580);
            this.Controls.Add(this.splitDistr);
            this.Name = "DistributionForm";
            this.ShowIcon = false;
            this.Text = "Распределение";
            this.tableLayoutLeft.ResumeLayout(false);
            this.panelFunction.ResumeLayout(false);
            this.panelFunction.PerformLayout();
            this.tableLayoutSplitGraphics.ResumeLayout(false);
            this.panelParameters.ResumeLayout(false);
            this.groupCommonParameters.ResumeLayout(false);
            this.groupCommonParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericChartPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericProbability)).EndInit();
            this.groupMonteCarloParameters.ResumeLayout(false);
            this.groupMonteCarloParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericExperimentsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPocketsCount)).EndInit();
            this.groupRandomsAlgebraParameters.ResumeLayout(false);
            this.groupRandomsAlgebraParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSamplesCount)).EndInit();
            this.splitDistr.Panel1.ResumeLayout(false);
            this.splitDistr.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDistr)).EndInit();
            this.splitDistr.ResumeLayout(false);
            this.splitRight.Panel1.ResumeLayout(false);
            this.splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).EndInit();
            this.splitRight.ResumeLayout(false);
            this.splitDistributions.Panel1.ResumeLayout(false);
            this.splitDistributions.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDistributions)).EndInit();
            this.splitDistributions.ResumeLayout(false);
            this.groupDistributions.ResumeLayout(false);
            this.tableLayoutDistributionsList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUnivariateDistributions)).EndInit();
            this.panelAddRemove.ResumeLayout(false);
            this.groupMultivariate.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMultivariateDistributions)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutLeft;
        private System.Windows.Forms.Panel panelFunction;
        private System.Windows.Forms.TextBox txtFunction;
        private System.Windows.Forms.Label lbModel;
        private System.Windows.Forms.SplitContainer splitDistr;
        private System.Windows.Forms.TableLayoutPanel tableLayoutSplitGraphics;
        private ZedGraph.ZedGraphControl zedDistrPDF;
        private ZedGraph.ZedGraphControl zedDistrCDF;
        private System.Windows.Forms.Button btnEvaluate;
        private System.Windows.Forms.Button btnOptimizations;
        private System.Windows.Forms.Panel panelParameters;
        private System.Windows.Forms.GroupBox groupCommonParameters;
        private System.Windows.Forms.NumericUpDown numericChartPoints;
        private System.Windows.Forms.Label lbChartPoints;
        private System.Windows.Forms.Label lbProbabilityP;
        private System.Windows.Forms.NumericUpDown numericProbability;
        private System.Windows.Forms.GroupBox groupMonteCarloParameters;
        private System.Windows.Forms.CheckBox checkEvaluateMonteCarlo;
        private System.Windows.Forms.Label lbExperimentsCount;
        private System.Windows.Forms.NumericUpDown numericExperimentsCount;
        private System.Windows.Forms.NumericUpDown numericPocketsCount;
        private System.Windows.Forms.Label lbPocketsCount;
        private System.Windows.Forms.GroupBox groupRandomsAlgebraParameters;
        private System.Windows.Forms.CheckBox checkEvaluateRandomsAlgebra;
        private System.Windows.Forms.Label lbSamplesCount;
        private System.Windows.Forms.NumericUpDown numericSamplesCount;
        private System.Windows.Forms.SplitContainer splitRight;
        private System.Windows.Forms.SplitContainer splitDistributions;
        private System.Windows.Forms.GroupBox groupDistributions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutDistributionsList;
        private System.Windows.Forms.DataGridView dataGridUnivariateDistributions;
        private System.Windows.Forms.Panel panelAddRemove;
        private System.Windows.Forms.Button btnRemoveUnivariateDistribution;
        private System.Windows.Forms.Button btnAddUnivariateDistribution;
        private System.Windows.Forms.GroupBox groupMultivariate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridMultivariateDistributions;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRemoveMultivariateDistibution;
        private System.Windows.Forms.Button btnAddMultivariateDistribution;
        private System.Windows.Forms.GroupBox groupResults;
        private System.Windows.Forms.DataGridView dataGridResults;
        private System.Windows.Forms.Button btnExport;
    }
}

