namespace Distribuitons
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
            this.btnFunction = new System.Windows.Forms.Button();
            this.txtFunction = new System.Windows.Forms.TextBox();
            this.lbFunction = new System.Windows.Forms.Label();
            this.tableLayoutSplitGraphics = new System.Windows.Forms.TableLayoutPanel();
            this.zedDistrCDF = new ZedGraph.ZedGraphControl();
            this.zedDistrPDF = new ZedGraph.ZedGraphControl();
            this.groupParameters = new System.Windows.Forms.GroupBox();
            this.groupCommonParameters = new System.Windows.Forms.GroupBox();
            this.numericGraphPoints = new System.Windows.Forms.NumericUpDown();
            this.lbGraphPoints = new System.Windows.Forms.Label();
            this.lbProbabilityP = new System.Windows.Forms.Label();
            this.numericProbability = new System.Windows.Forms.NumericUpDown();
            this.groupMonteCarloParameters = new System.Windows.Forms.GroupBox();
            this.checkUseMonteCarloMethod = new System.Windows.Forms.CheckBox();
            this.lbRandomsCount = new System.Windows.Forms.Label();
            this.numericRandomValues = new System.Windows.Forms.NumericUpDown();
            this.numericPockets = new System.Windows.Forms.NumericUpDown();
            this.lbPockets = new System.Windows.Forms.Label();
            this.groupRandomsMathParameters = new System.Windows.Forms.GroupBox();
            this.lbTolerance = new System.Windows.Forms.Label();
            this.numericTolerance = new System.Windows.Forms.NumericUpDown();
            this.checkUseRandomsMath = new System.Windows.Forms.CheckBox();
            this.lbSamplesCount = new System.Windows.Forms.Label();
            this.numericSamplesCount = new System.Windows.Forms.NumericUpDown();
            this.splitDistr = new System.Windows.Forms.SplitContainer();
            this.tableLayoutRight = new System.Windows.Forms.TableLayoutPanel();
            this.groupMultivariate = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridMultivariateDistributions = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRemoveMultivariate = new System.Windows.Forms.Button();
            this.btnAddMultivariate = new System.Windows.Forms.Button();
            this.groupResults = new System.Windows.Forms.GroupBox();
            this.dataGridResults = new System.Windows.Forms.DataGridView();
            this.groupDistributions = new System.Windows.Forms.GroupBox();
            this.tableLayoutDistributionsList = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridDistributions = new System.Windows.Forms.DataGridView();
            this.panelAddRemove = new System.Windows.Forms.Panel();
            this.btnRemoveDistr = new System.Windows.Forms.Button();
            this.btnAddDistr = new System.Windows.Forms.Button();
            this.tableLayoutLeft.SuspendLayout();
            this.panelFunction.SuspendLayout();
            this.tableLayoutSplitGraphics.SuspendLayout();
            this.groupParameters.SuspendLayout();
            this.groupCommonParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericGraphPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericProbability)).BeginInit();
            this.groupMonteCarloParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericRandomValues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPockets)).BeginInit();
            this.groupRandomsMathParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericTolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSamplesCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitDistr)).BeginInit();
            this.splitDistr.Panel1.SuspendLayout();
            this.splitDistr.Panel2.SuspendLayout();
            this.splitDistr.SuspendLayout();
            this.tableLayoutRight.SuspendLayout();
            this.groupMultivariate.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMultivariateDistributions)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResults)).BeginInit();
            this.groupDistributions.SuspendLayout();
            this.tableLayoutDistributionsList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDistributions)).BeginInit();
            this.panelAddRemove.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutLeft
            // 
            this.tableLayoutLeft.ColumnCount = 1;
            this.tableLayoutLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutLeft.Controls.Add(this.panelFunction, 0, 0);
            this.tableLayoutLeft.Controls.Add(this.tableLayoutSplitGraphics, 0, 1);
            this.tableLayoutLeft.Controls.Add(this.groupParameters, 0, 2);
            this.tableLayoutLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutLeft.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutLeft.Name = "tableLayoutLeft";
            this.tableLayoutLeft.RowCount = 3;
            this.tableLayoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutLeft.Size = new System.Drawing.Size(941, 580);
            this.tableLayoutLeft.TabIndex = 1;
            // 
            // panelFunction
            // 
            this.panelFunction.Controls.Add(this.btnOptimizations);
            this.panelFunction.Controls.Add(this.btnFunction);
            this.panelFunction.Controls.Add(this.txtFunction);
            this.panelFunction.Controls.Add(this.lbFunction);
            this.panelFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFunction.Location = new System.Drawing.Point(0, 0);
            this.panelFunction.Margin = new System.Windows.Forms.Padding(0);
            this.panelFunction.Name = "panelFunction";
            this.panelFunction.Size = new System.Drawing.Size(941, 29);
            this.panelFunction.TabIndex = 1;
            // 
            // btnOptimizations
            // 
            this.btnOptimizations.Location = new System.Drawing.Point(286, 3);
            this.btnOptimizations.Name = "btnOptimizations";
            this.btnOptimizations.Size = new System.Drawing.Size(91, 23);
            this.btnOptimizations.TabIndex = 4;
            this.btnOptimizations.Text = "Оптимизации";
            this.btnOptimizations.UseVisualStyleBackColor = true;
            this.btnOptimizations.Click += new System.EventHandler(this.btnOptimizations_Click);
            // 
            // btnFunction
            // 
            this.btnFunction.Location = new System.Drawing.Point(205, 3);
            this.btnFunction.Name = "btnFunction";
            this.btnFunction.Size = new System.Drawing.Size(75, 23);
            this.btnFunction.TabIndex = 3;
            this.btnFunction.Text = "Расчёт";
            this.btnFunction.UseVisualStyleBackColor = true;
            this.btnFunction.Click += new System.EventHandler(this.btnFunction_Click);
            // 
            // txtFunction
            // 
            this.txtFunction.Location = new System.Drawing.Point(64, 5);
            this.txtFunction.Name = "txtFunction";
            this.txtFunction.Size = new System.Drawing.Size(135, 20);
            this.txtFunction.TabIndex = 2;
            this.txtFunction.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFunction_KeyDown);
            // 
            // lbFunction
            // 
            this.lbFunction.AutoSize = true;
            this.lbFunction.Location = new System.Drawing.Point(3, 8);
            this.lbFunction.Name = "lbFunction";
            this.lbFunction.Size = new System.Drawing.Size(55, 13);
            this.lbFunction.TabIndex = 1;
            this.lbFunction.Text = "Формула";
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
            this.tableLayoutSplitGraphics.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 433F));
            this.tableLayoutSplitGraphics.Size = new System.Drawing.Size(935, 433);
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
            this.zedDistrCDF.Size = new System.Drawing.Size(462, 427);
            this.zedDistrCDF.TabIndex = 1;
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
            this.zedDistrPDF.Size = new System.Drawing.Size(461, 427);
            this.zedDistrPDF.TabIndex = 0;
            this.zedDistrPDF.UseExtendedPrintDialog = true;
            // 
            // groupParameters
            // 
            this.groupParameters.Controls.Add(this.groupCommonParameters);
            this.groupParameters.Controls.Add(this.groupMonteCarloParameters);
            this.groupParameters.Controls.Add(this.groupRandomsMathParameters);
            this.groupParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupParameters.Location = new System.Drawing.Point(3, 471);
            this.groupParameters.Name = "groupParameters";
            this.groupParameters.Size = new System.Drawing.Size(935, 106);
            this.groupParameters.TabIndex = 1;
            this.groupParameters.TabStop = false;
            this.groupParameters.Text = "Параметры";
            // 
            // groupCommonParameters
            // 
            this.groupCommonParameters.Controls.Add(this.numericGraphPoints);
            this.groupCommonParameters.Controls.Add(this.lbGraphPoints);
            this.groupCommonParameters.Controls.Add(this.lbProbabilityP);
            this.groupCommonParameters.Controls.Add(this.numericProbability);
            this.groupCommonParameters.Location = new System.Drawing.Point(561, 19);
            this.groupCommonParameters.Name = "groupCommonParameters";
            this.groupCommonParameters.Size = new System.Drawing.Size(252, 80);
            this.groupCommonParameters.TabIndex = 12;
            this.groupCommonParameters.TabStop = false;
            this.groupCommonParameters.Text = "Общие параметры";
            // 
            // numericGraphPoints
            // 
            this.numericGraphPoints.Location = new System.Drawing.Point(96, 50);
            this.numericGraphPoints.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericGraphPoints.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericGraphPoints.Name = "numericGraphPoints";
            this.numericGraphPoints.Size = new System.Drawing.Size(138, 20);
            this.numericGraphPoints.TabIndex = 9;
            this.numericGraphPoints.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lbGraphPoints
            // 
            this.lbGraphPoints.AutoSize = true;
            this.lbGraphPoints.Location = new System.Drawing.Point(6, 52);
            this.lbGraphPoints.Name = "lbGraphPoints";
            this.lbGraphPoints.Size = new System.Drawing.Size(83, 13);
            this.lbGraphPoints.TabIndex = 10;
            this.lbGraphPoints.Text = "Точек графика";
            // 
            // lbProbabilityP
            // 
            this.lbProbabilityP.AutoSize = true;
            this.lbProbabilityP.Location = new System.Drawing.Point(6, 18);
            this.lbProbabilityP.Name = "lbProbabilityP";
            this.lbProbabilityP.Size = new System.Drawing.Size(84, 13);
            this.lbProbabilityP.TabIndex = 8;
            this.lbProbabilityP.Text = "Вероятность, p";
            // 
            // numericProbability
            // 
            this.numericProbability.DecimalPlaces = 3;
            this.numericProbability.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericProbability.Location = new System.Drawing.Point(96, 16);
            this.numericProbability.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericProbability.Name = "numericProbability";
            this.numericProbability.Size = new System.Drawing.Size(138, 20);
            this.numericProbability.TabIndex = 9;
            this.numericProbability.Value = new decimal(new int[] {
            95,
            0,
            0,
            131072});
            // 
            // groupMonteCarloParameters
            // 
            this.groupMonteCarloParameters.Controls.Add(this.checkUseMonteCarloMethod);
            this.groupMonteCarloParameters.Controls.Add(this.lbRandomsCount);
            this.groupMonteCarloParameters.Controls.Add(this.numericRandomValues);
            this.groupMonteCarloParameters.Controls.Add(this.numericPockets);
            this.groupMonteCarloParameters.Controls.Add(this.lbPockets);
            this.groupMonteCarloParameters.Location = new System.Drawing.Point(283, 19);
            this.groupMonteCarloParameters.Name = "groupMonteCarloParameters";
            this.groupMonteCarloParameters.Size = new System.Drawing.Size(272, 80);
            this.groupMonteCarloParameters.TabIndex = 11;
            this.groupMonteCarloParameters.TabStop = false;
            this.groupMonteCarloParameters.Text = "Метод Монте-Карло";
            // 
            // checkUseMonteCarloMethod
            // 
            this.checkUseMonteCarloMethod.AutoSize = true;
            this.checkUseMonteCarloMethod.Location = new System.Drawing.Point(7, 14);
            this.checkUseMonteCarloMethod.Name = "checkUseMonteCarloMethod";
            this.checkUseMonteCarloMethod.Size = new System.Drawing.Size(119, 17);
            this.checkUseMonteCarloMethod.TabIndex = 7;
            this.checkUseMonteCarloMethod.Text = "Выполнять расчёт";
            this.checkUseMonteCarloMethod.UseVisualStyleBackColor = true;
            // 
            // lbRandomsCount
            // 
            this.lbRandomsCount.AutoSize = true;
            this.lbRandomsCount.Location = new System.Drawing.Point(6, 34);
            this.lbRandomsCount.Name = "lbRandomsCount";
            this.lbRandomsCount.Size = new System.Drawing.Size(105, 13);
            this.lbRandomsCount.TabIndex = 2;
            this.lbRandomsCount.Text = "Случайных величин";
            // 
            // numericRandomValues
            // 
            this.numericRandomValues.Location = new System.Drawing.Point(7, 50);
            this.numericRandomValues.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericRandomValues.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericRandomValues.Name = "numericRandomValues";
            this.numericRandomValues.Size = new System.Drawing.Size(119, 20);
            this.numericRandomValues.TabIndex = 3;
            this.numericRandomValues.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // numericPockets
            // 
            this.numericPockets.Location = new System.Drawing.Point(132, 50);
            this.numericPockets.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericPockets.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericPockets.Name = "numericPockets";
            this.numericPockets.Size = new System.Drawing.Size(119, 20);
            this.numericPockets.TabIndex = 5;
            this.numericPockets.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lbPockets
            // 
            this.lbPockets.AutoSize = true;
            this.lbPockets.Location = new System.Drawing.Point(129, 34);
            this.lbPockets.Name = "lbPockets";
            this.lbPockets.Size = new System.Drawing.Size(137, 13);
            this.lbPockets.TabIndex = 4;
            this.lbPockets.Text = "Карманов (для графиков)";
            // 
            // groupRandomsMathParameters
            // 
            this.groupRandomsMathParameters.Controls.Add(this.lbTolerance);
            this.groupRandomsMathParameters.Controls.Add(this.numericTolerance);
            this.groupRandomsMathParameters.Controls.Add(this.checkUseRandomsMath);
            this.groupRandomsMathParameters.Controls.Add(this.lbSamplesCount);
            this.groupRandomsMathParameters.Controls.Add(this.numericSamplesCount);
            this.groupRandomsMathParameters.Location = new System.Drawing.Point(9, 19);
            this.groupRandomsMathParameters.Name = "groupRandomsMathParameters";
            this.groupRandomsMathParameters.Size = new System.Drawing.Size(268, 80);
            this.groupRandomsMathParameters.TabIndex = 10;
            this.groupRandomsMathParameters.TabStop = false;
            this.groupRandomsMathParameters.Text = "Композиционный метод";
            // 
            // lbTolerance
            // 
            this.lbTolerance.AutoSize = true;
            this.lbTolerance.Location = new System.Drawing.Point(133, 34);
            this.lbTolerance.Name = "lbTolerance";
            this.lbTolerance.Size = new System.Drawing.Size(101, 13);
            this.lbTolerance.TabIndex = 8;
            this.lbTolerance.Text = "Чувствительность";
            // 
            // numericTolerance
            // 
            this.numericTolerance.DecimalPlaces = 9;
            this.numericTolerance.Location = new System.Drawing.Point(136, 50);
            this.numericTolerance.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericTolerance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            589824});
            this.numericTolerance.Name = "numericTolerance";
            this.numericTolerance.Size = new System.Drawing.Size(119, 20);
            this.numericTolerance.TabIndex = 7;
            this.numericTolerance.Value = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            // 
            // checkUseRandomsMath
            // 
            this.checkUseRandomsMath.AutoSize = true;
            this.checkUseRandomsMath.Checked = true;
            this.checkUseRandomsMath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUseRandomsMath.Location = new System.Drawing.Point(8, 14);
            this.checkUseRandomsMath.Name = "checkUseRandomsMath";
            this.checkUseRandomsMath.Size = new System.Drawing.Size(119, 17);
            this.checkUseRandomsMath.TabIndex = 6;
            this.checkUseRandomsMath.Text = "Выполнять расчёт";
            this.checkUseRandomsMath.UseVisualStyleBackColor = true;
            // 
            // lbSamplesCount
            // 
            this.lbSamplesCount.AutoSize = true;
            this.lbSamplesCount.Location = new System.Drawing.Point(8, 34);
            this.lbSamplesCount.Name = "lbSamplesCount";
            this.lbSamplesCount.Size = new System.Drawing.Size(87, 13);
            this.lbSamplesCount.TabIndex = 0;
            this.lbSamplesCount.Text = "Число отсчетов";
            // 
            // numericSamplesCount
            // 
            this.numericSamplesCount.Location = new System.Drawing.Point(11, 50);
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
            this.numericSamplesCount.Size = new System.Drawing.Size(119, 20);
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
            this.splitDistr.Panel2.Controls.Add(this.tableLayoutRight);
            this.splitDistr.Size = new System.Drawing.Size(1271, 580);
            this.splitDistr.SplitterDistance = 941;
            this.splitDistr.TabIndex = 2;
            // 
            // tableLayoutRight
            // 
            this.tableLayoutRight.ColumnCount = 1;
            this.tableLayoutRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutRight.Controls.Add(this.groupMultivariate, 0, 1);
            this.tableLayoutRight.Controls.Add(this.groupResults, 0, 2);
            this.tableLayoutRight.Controls.Add(this.groupDistributions, 0, 0);
            this.tableLayoutRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutRight.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutRight.Name = "tableLayoutRight";
            this.tableLayoutRight.RowCount = 3;
            this.tableLayoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutRight.Size = new System.Drawing.Size(326, 580);
            this.tableLayoutRight.TabIndex = 3;
            // 
            // groupMultivariate
            // 
            this.groupMultivariate.Controls.Add(this.tableLayoutPanel1);
            this.groupMultivariate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupMultivariate.Location = new System.Drawing.Point(3, 168);
            this.groupMultivariate.Name = "groupMultivariate";
            this.groupMultivariate.Size = new System.Drawing.Size(320, 159);
            this.groupMultivariate.TabIndex = 13;
            this.groupMultivariate.TabStop = false;
            this.groupMultivariate.Text = "Многомерные распределения (Монте-Карло)";
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(314, 140);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // dataGridMultivariateDistributions
            // 
            this.dataGridMultivariateDistributions.AllowUserToResizeRows = false;
            this.dataGridMultivariateDistributions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMultivariateDistributions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridMultivariateDistributions.Location = new System.Drawing.Point(3, 3);
            this.dataGridMultivariateDistributions.Name = "dataGridMultivariateDistributions";
            this.dataGridMultivariateDistributions.Size = new System.Drawing.Size(308, 102);
            this.dataGridMultivariateDistributions.TabIndex = 0;
            this.dataGridMultivariateDistributions.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridMultivariateDistributions_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRemoveMultivariate);
            this.panel1.Controls.Add(this.btnAddMultivariate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 108);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(314, 32);
            this.panel1.TabIndex = 1;
            // 
            // btnRemoveMultivariate
            // 
            this.btnRemoveMultivariate.Location = new System.Drawing.Point(81, 3);
            this.btnRemoveMultivariate.Name = "btnRemoveMultivariate";
            this.btnRemoveMultivariate.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveMultivariate.TabIndex = 1;
            this.btnRemoveMultivariate.Text = "Удалить";
            this.btnRemoveMultivariate.UseVisualStyleBackColor = true;
            this.btnRemoveMultivariate.Click += new System.EventHandler(this.btnRemoveMultivariate_Click);
            // 
            // btnAddMultivariate
            // 
            this.btnAddMultivariate.Location = new System.Drawing.Point(3, 3);
            this.btnAddMultivariate.Name = "btnAddMultivariate";
            this.btnAddMultivariate.Size = new System.Drawing.Size(75, 23);
            this.btnAddMultivariate.TabIndex = 0;
            this.btnAddMultivariate.Text = "Добавить";
            this.btnAddMultivariate.UseVisualStyleBackColor = true;
            this.btnAddMultivariate.Click += new System.EventHandler(this.btnAddMultivariate_Click);
            // 
            // groupResults
            // 
            this.groupResults.Controls.Add(this.dataGridResults);
            this.groupResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupResults.Location = new System.Drawing.Point(3, 333);
            this.groupResults.Name = "groupResults";
            this.groupResults.Size = new System.Drawing.Size(320, 244);
            this.groupResults.TabIndex = 2;
            this.groupResults.TabStop = false;
            this.groupResults.Text = "Результаты";
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
            this.dataGridResults.Size = new System.Drawing.Size(314, 225);
            this.dataGridResults.TabIndex = 0;
            // 
            // groupDistributions
            // 
            this.groupDistributions.Controls.Add(this.tableLayoutDistributionsList);
            this.groupDistributions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupDistributions.Location = new System.Drawing.Point(3, 3);
            this.groupDistributions.Name = "groupDistributions";
            this.groupDistributions.Size = new System.Drawing.Size(320, 159);
            this.groupDistributions.TabIndex = 0;
            this.groupDistributions.TabStop = false;
            this.groupDistributions.Text = "Распределения";
            // 
            // tableLayoutDistributionsList
            // 
            this.tableLayoutDistributionsList.ColumnCount = 1;
            this.tableLayoutDistributionsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutDistributionsList.Controls.Add(this.dataGridDistributions, 0, 0);
            this.tableLayoutDistributionsList.Controls.Add(this.panelAddRemove, 0, 1);
            this.tableLayoutDistributionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutDistributionsList.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutDistributionsList.Name = "tableLayoutDistributionsList";
            this.tableLayoutDistributionsList.RowCount = 2;
            this.tableLayoutDistributionsList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutDistributionsList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutDistributionsList.Size = new System.Drawing.Size(314, 140);
            this.tableLayoutDistributionsList.TabIndex = 1;
            // 
            // dataGridDistributions
            // 
            this.dataGridDistributions.AllowUserToResizeRows = false;
            this.dataGridDistributions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDistributions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridDistributions.Location = new System.Drawing.Point(3, 3);
            this.dataGridDistributions.Name = "dataGridDistributions";
            this.dataGridDistributions.Size = new System.Drawing.Size(308, 102);
            this.dataGridDistributions.TabIndex = 0;
            this.dataGridDistributions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridDistributions_CellClick);
            this.dataGridDistributions.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridDistributions_CellDoubleClick);
            // 
            // panelAddRemove
            // 
            this.panelAddRemove.Controls.Add(this.btnRemoveDistr);
            this.panelAddRemove.Controls.Add(this.btnAddDistr);
            this.panelAddRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAddRemove.Location = new System.Drawing.Point(0, 108);
            this.panelAddRemove.Margin = new System.Windows.Forms.Padding(0);
            this.panelAddRemove.Name = "panelAddRemove";
            this.panelAddRemove.Size = new System.Drawing.Size(314, 32);
            this.panelAddRemove.TabIndex = 1;
            // 
            // btnRemoveDistr
            // 
            this.btnRemoveDistr.Location = new System.Drawing.Point(81, 3);
            this.btnRemoveDistr.Name = "btnRemoveDistr";
            this.btnRemoveDistr.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveDistr.TabIndex = 1;
            this.btnRemoveDistr.Text = "Удалить";
            this.btnRemoveDistr.UseVisualStyleBackColor = true;
            this.btnRemoveDistr.Click += new System.EventHandler(this.btnRemoveDistr_Click);
            // 
            // btnAddDistr
            // 
            this.btnAddDistr.Location = new System.Drawing.Point(3, 3);
            this.btnAddDistr.Name = "btnAddDistr";
            this.btnAddDistr.Size = new System.Drawing.Size(75, 23);
            this.btnAddDistr.TabIndex = 0;
            this.btnAddDistr.Text = "Добавить";
            this.btnAddDistr.UseVisualStyleBackColor = true;
            this.btnAddDistr.Click += new System.EventHandler(this.btnAddDistr_Click);
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
            this.groupParameters.ResumeLayout(false);
            this.groupCommonParameters.ResumeLayout(false);
            this.groupCommonParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericGraphPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericProbability)).EndInit();
            this.groupMonteCarloParameters.ResumeLayout(false);
            this.groupMonteCarloParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericRandomValues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPockets)).EndInit();
            this.groupRandomsMathParameters.ResumeLayout(false);
            this.groupRandomsMathParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericTolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSamplesCount)).EndInit();
            this.splitDistr.Panel1.ResumeLayout(false);
            this.splitDistr.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDistr)).EndInit();
            this.splitDistr.ResumeLayout(false);
            this.tableLayoutRight.ResumeLayout(false);
            this.groupMultivariate.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMultivariateDistributions)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResults)).EndInit();
            this.groupDistributions.ResumeLayout(false);
            this.tableLayoutDistributionsList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDistributions)).EndInit();
            this.panelAddRemove.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutLeft;
        private System.Windows.Forms.Panel panelFunction;
        private System.Windows.Forms.TextBox txtFunction;
        private System.Windows.Forms.Label lbFunction;
        private System.Windows.Forms.SplitContainer splitDistr;
        private System.Windows.Forms.GroupBox groupDistributions;
        private System.Windows.Forms.DataGridView dataGridDistributions;
        private System.Windows.Forms.GroupBox groupParameters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutSplitGraphics;
        private ZedGraph.ZedGraphControl zedDistrPDF;
        private ZedGraph.ZedGraphControl zedDistrCDF;
        private System.Windows.Forms.Button btnFunction;
        private System.Windows.Forms.NumericUpDown numericSamplesCount;
        private System.Windows.Forms.Label lbSamplesCount;
        private System.Windows.Forms.NumericUpDown numericRandomValues;
        private System.Windows.Forms.Label lbRandomsCount;
        private System.Windows.Forms.NumericUpDown numericPockets;
        private System.Windows.Forms.Label lbPockets;
        private System.Windows.Forms.CheckBox checkUseMonteCarloMethod;
        private System.Windows.Forms.CheckBox checkUseRandomsMath;
        private System.Windows.Forms.NumericUpDown numericProbability;
        private System.Windows.Forms.Label lbProbabilityP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutDistributionsList;
        private System.Windows.Forms.Panel panelAddRemove;
        private System.Windows.Forms.Button btnRemoveDistr;
        private System.Windows.Forms.Button btnAddDistr;
        private System.Windows.Forms.GroupBox groupResults;
        private System.Windows.Forms.TableLayoutPanel tableLayoutRight;
        private System.Windows.Forms.GroupBox groupRandomsMathParameters;
        private System.Windows.Forms.GroupBox groupMonteCarloParameters;
        private System.Windows.Forms.GroupBox groupCommonParameters;
        private System.Windows.Forms.DataGridView dataGridResults;
        private System.Windows.Forms.Label lbTolerance;
        private System.Windows.Forms.NumericUpDown numericTolerance;
        private System.Windows.Forms.NumericUpDown numericGraphPoints;
        private System.Windows.Forms.Label lbGraphPoints;
        private System.Windows.Forms.Button btnOptimizations;
        private System.Windows.Forms.GroupBox groupMultivariate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridMultivariateDistributions;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRemoveMultivariate;
        private System.Windows.Forms.Button btnAddMultivariate;
    }
}

