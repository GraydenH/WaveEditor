using System;
using System.Windows.Forms;

namespace WaveEditor {

    public partial class Form1 {
    
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.sampleChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.fequencyChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.openItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.cutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.magItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.triangleItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hammingItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.highPassItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowPassItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bandPassItem = new System.Windows.Forms.ToolStripMenuItem();
            this.QuantizationMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SixteenBitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ThirtyTwoBitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sampleRateMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.rate1Item = new System.Windows.Forms.ToolStripMenuItem();
            this.rate2Item = new System.Windows.Forms.ToolStripMenuItem();
            this.rate3Item = new System.Windows.Forms.ToolStripMenuItem();
            this.DFTButton = new System.Windows.Forms.Button();
            this.filterButton = new System.Windows.Forms.Button();
            this.stopRecordButton = new System.Windows.Forms.Button();
            this.recordGroup = new System.Windows.Forms.GroupBox();
            this.startRecordButton = new System.Windows.Forms.Button();
            this.DFTGroup = new System.Windows.Forms.GroupBox();
            this.DFTWorker = new System.ComponentModel.BackgroundWorker();
            this.playGroup = new System.Windows.Forms.GroupBox();
            this.stopPlayButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sampleChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fequencyChart)).BeginInit();
            this.menu.SuspendLayout();
            this.recordGroup.SuspendLayout();
            this.DFTGroup.SuspendLayout();
            this.playGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.sampleChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.ScaleView.Zoomable = false;
            chartArea1.AxisY.ScaleView.Zoomable = false;
            chartArea1.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Cross;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.Name = "ChartArea1";
            this.sampleChart.ChartAreas.Add(chartArea1);
            this.sampleChart.Cursor = System.Windows.Forms.Cursors.VSplit;
            legend1.Name = "Legend1";
            this.sampleChart.Legends.Add(legend1);
            this.sampleChart.Location = new System.Drawing.Point(0, 29);
            this.sampleChart.Name = "chart1";
            this.sampleChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Samples";
            this.sampleChart.Series.Add(series1);
            this.sampleChart.Size = new System.Drawing.Size(677, 225);
            this.sampleChart.TabIndex = 0;
            this.sampleChart.Text = "chart1";
            this.sampleChart.SelectionRangeChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CursorEventArgs>(this.SamplesSelected);
            // 
            // chart2
            // 
            this.fequencyChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.AxisX.ScaleView.Zoomable = false;
            chartArea2.AxisY.ScaleView.Zoomable = false;
            chartArea2.CursorX.IsUserEnabled = true;
            chartArea2.CursorX.IsUserSelectionEnabled = true;
            chartArea2.Name = "ChartArea1";
            this.fequencyChart.ChartAreas.Add(chartArea2);
            this.fequencyChart.Cursor = System.Windows.Forms.Cursors.VSplit;
            legend2.Name = "Legend1";
            this.fequencyChart.Legends.Add(legend2);
            this.fequencyChart.Location = new System.Drawing.Point(0, 260);
            this.fequencyChart.Name = "chart2";
            this.fequencyChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Frequency Bins";
            this.fequencyChart.Series.Add(series2);
            this.fequencyChart.Size = new System.Drawing.Size(677, 238);
            this.fequencyChart.TabIndex = 1;
            this.fequencyChart.Text = "chart2";
            this.fequencyChart.SelectionRangeChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CursorEventArgs>(this.FilterSelected);
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.editMenu,
            this.windowMenu,
            this.filterMenu,
            this.QuantizationMenu,
            this.sampleRateMenu});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(778, 24);
            this.menu.TabIndex = 2;
            this.menu.Text = "Menu";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openItem,
            this.saveItem});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "File";
            // 
            // openItem
            // 
            this.openItem.Name = "openItem";
            this.openItem.Size = new System.Drawing.Size(152, 22);
            this.openItem.Text = "Open";
            this.openItem.Click += new System.EventHandler(this.Open);
            // 
            // saveItem
            // 
            this.saveItem.Name = "saveItem";
            this.saveItem.Size = new System.Drawing.Size(152, 22);
            this.saveItem.Text = "Save";
            this.saveItem.Click += new System.EventHandler(this.Save);
            // 
            // editMenu
            // 
            this.editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutItem,
            this.copyItem,
            this.pasteItem,
            this.magItem});
            this.editMenu.Name = "editMenu";
            this.editMenu.Size = new System.Drawing.Size(39, 20);
            this.editMenu.Text = "Edit";
            // 
            // cutItem
            // 
            this.cutItem.Name = "cutItem";
            this.cutItem.Size = new System.Drawing.Size(118, 22);
            this.cutItem.Text = "Cut";
            this.cutItem.Click += new System.EventHandler(this.Cut);
            // 
            // copyItem
            // 
            this.copyItem.Name = "copyItem";
            this.copyItem.Size = new System.Drawing.Size(118, 22);
            this.copyItem.Text = "Copy";
            this.copyItem.Click += new System.EventHandler(this.Copy);
            // 
            // pasteItem
            // 
            this.pasteItem.Name = "pasteItem";
            this.pasteItem.Size = new System.Drawing.Size(118, 22);
            this.pasteItem.Text = "Paste";
            this.pasteItem.Click += new System.EventHandler(this.Paste);
            // 
            // magItem
            // 
            this.magItem.Name = "magItem";
            this.magItem.Size = new System.Drawing.Size(118, 22);
            this.magItem.Text = "Magnify";
            this.magItem.Click += new System.EventHandler(this.Magnify);
            // 
            // windowMenu
            // 
            this.windowMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.triangleItem,
            this.hammingItem});
            this.windowMenu.Name = "windowMenu";
            this.windowMenu.Size = new System.Drawing.Size(63, 20);
            this.windowMenu.Text = "Window";
            // 
            // triangleItem
            // 
            this.triangleItem.Name = "triangleItem";
            this.triangleItem.Size = new System.Drawing.Size(173, 22);
            this.triangleItem.Text = "Triangle window";
            this.triangleItem.Click += new System.EventHandler(this.SetWindow);
            // 
            // hammingItem
            // 
            this.hammingItem.Name = "hammingItem";
            this.hammingItem.Size = new System.Drawing.Size(173, 22);
            this.hammingItem.Text = "Hamming window";
            this.hammingItem.Click += new System.EventHandler(this.SetWindow);
            // 
            // filterMenu
            // 
            this.filterMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.highPassItem,
            this.lowPassItem,
            this.bandPassItem});
            this.filterMenu.Name = "filterMenu";
            this.filterMenu.Size = new System.Drawing.Size(45, 20);
            this.filterMenu.Text = "Filter";
            // 
            // highPassItem
            // 
            this.highPassItem.Name = "highPassItem";
            this.highPassItem.Size = new System.Drawing.Size(127, 22);
            this.highPassItem.Text = "High pass";
            this.highPassItem.Click += new System.EventHandler(this.SetFilter);
            // 
            // lowPassItem
            // 
            this.lowPassItem.Name = "lowPassItem";
            this.lowPassItem.Size = new System.Drawing.Size(127, 22);
            this.lowPassItem.Text = "Low pass";
            this.lowPassItem.Click += new System.EventHandler(this.SetFilter);
            // 
            // bandPassItem
            // 
            this.bandPassItem.Name = "bandPassItem";
            this.bandPassItem.Size = new System.Drawing.Size(127, 22);
            this.bandPassItem.Text = "Band pass";
            this.bandPassItem.Click += new System.EventHandler(this.SetFilter);
            // 
            // QuantizationMenu
            // 
            this.QuantizationMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SixteenBitItem,
            this.ThirtyTwoBitItem});
            this.QuantizationMenu.Name = "QuantizationMenu";
            this.QuantizationMenu.Size = new System.Drawing.Size(87, 20);
            this.QuantizationMenu.Text = "Quantization";
            // 
            // SixteenBitItem
            // 
            this.SixteenBitItem.Name = "SixteenBitItem";
            this.SixteenBitItem.Size = new System.Drawing.Size(103, 22);
            this.SixteenBitItem.Text = "16 Bit";
            this.SixteenBitItem.Click += new System.EventHandler(this.SetBitDepth);
            // 
            // ThirtyTwoBitItem
            // 
            this.ThirtyTwoBitItem.Name = "ThirtyTwoBitItem";
            this.ThirtyTwoBitItem.Size = new System.Drawing.Size(103, 22);
            this.ThirtyTwoBitItem.Text = "32 Bit";
            this.ThirtyTwoBitItem.Click += new System.EventHandler(this.SetBitDepth);
            // 
            // sampleRateMenu
            // 
            this.sampleRateMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rate1Item,
            this.rate2Item,
            this.rate3Item});
            this.sampleRateMenu.Name = "sampleRateMenu";
            this.sampleRateMenu.Size = new System.Drawing.Size(84, 20);
            this.sampleRateMenu.Text = "Sample Rate";
            // 
            // rate1Item
            // 
            this.rate1Item.Name = "rate1Item";
            this.rate1Item.Size = new System.Drawing.Size(118, 22);
            this.rate1Item.Text = "11025Hz";
            this.rate1Item.Click += new System.EventHandler(this.SetSamplingRate);
            // 
            // rate2Item
            // 
            this.rate2Item.Name = "rate2Item";
            this.rate2Item.Size = new System.Drawing.Size(118, 22);
            this.rate2Item.Text = "22050Hz";
            this.rate2Item.Click += new System.EventHandler(this.SetSamplingRate);
            // 
            // rate3Item
            // 
            this.rate3Item.Name = "rate3Item";
            this.rate3Item.Size = new System.Drawing.Size(118, 22);
            this.rate3Item.Text = "44100Hz";
            this.rate3Item.Click += new System.EventHandler(this.SetSamplingRate);
            // 
            // DFTButton
            // 
            this.DFTButton.Location = new System.Drawing.Point(6, 19);
            this.DFTButton.Name = "DFTButton";
            this.DFTButton.Size = new System.Drawing.Size(75, 23);
            this.DFTButton.TabIndex = 3;
            this.DFTButton.Text = "Start";
            this.DFTButton.UseVisualStyleBackColor = true;
            this.DFTButton.Click += new System.EventHandler(this.StartDFT);
            // 
            // filterButton
            // 
            this.filterButton.Location = new System.Drawing.Point(6, 48);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(75, 23);
            this.filterButton.TabIndex = 13;
            this.filterButton.Text = "Filter";
            this.filterButton.UseVisualStyleBackColor = true;
            this.filterButton.Click += new System.EventHandler(this.Filter);
            // 
            // stopRecordButton
            // 
            this.stopRecordButton.Image = global::WaveEditor.Properties.Resources.icon_vo_stop2;
            this.stopRecordButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.stopRecordButton.Location = new System.Drawing.Point(6, 48);
            this.stopRecordButton.Name = "stopRecordButton";
            this.stopRecordButton.Size = new System.Drawing.Size(75, 23);
            this.stopRecordButton.TabIndex = 15;
            this.stopRecordButton.Text = "Stop Recording";
            this.stopRecordButton.UseVisualStyleBackColor = true;
            this.stopRecordButton.Click += new System.EventHandler(this.StopRecord);
            // 
            // recordGroup
            // 
            this.recordGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recordGroup.Controls.Add(this.stopRecordButton);
            this.recordGroup.Controls.Add(this.startRecordButton);
            this.recordGroup.Location = new System.Drawing.Point(686, 150);
            this.recordGroup.Name = "recordGroup";
            this.recordGroup.Size = new System.Drawing.Size(86, 77);
            this.recordGroup.TabIndex = 16;
            this.recordGroup.TabStop = false;
            this.recordGroup.Text = "Record";
            // 
            // startRecordButton
            // 
            this.startRecordButton.Image = global::WaveEditor.Properties.Resources.red_circle;
            this.startRecordButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.startRecordButton.Location = new System.Drawing.Point(6, 19);
            this.startRecordButton.Name = "startRecordButton";
            this.startRecordButton.Size = new System.Drawing.Size(75, 23);
            this.startRecordButton.TabIndex = 14;
            this.startRecordButton.Text = "Start";
            this.startRecordButton.UseVisualStyleBackColor = true;
            this.startRecordButton.Click += new System.EventHandler(this.StartRecord);
            // 
            // DFTGroup
            // 
            this.DFTGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DFTGroup.Controls.Add(this.DFTButton);
            this.DFTGroup.Controls.Add(this.filterButton);
            this.DFTGroup.Location = new System.Drawing.Point(686, 233);
            this.DFTGroup.Name = "DFTGroup";
            this.DFTGroup.Size = new System.Drawing.Size(86, 77);
            this.DFTGroup.TabIndex = 17;
            this.DFTGroup.TabStop = false;
            this.DFTGroup.Text = "DFT";
            // 
            // DFTWorker
            // 
            this.DFTWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DFTWork);
            this.DFTWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DFTCompleted);
            // 
            // playGroup
            // 
            this.playGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.playGroup.Controls.Add(this.stopPlayButton);
            this.playGroup.Controls.Add(this.pauseButton);
            this.playGroup.Controls.Add(this.playButton);
            this.playGroup.Location = new System.Drawing.Point(686, 37);
            this.playGroup.Name = "playGroup";
            this.playGroup.Size = new System.Drawing.Size(85, 107);
            this.playGroup.TabIndex = 18;
            this.playGroup.TabStop = false;
            this.playGroup.Text = "Play";
            // 
            // stopPlayButton
            // 
            this.stopPlayButton.Image = global::WaveEditor.Properties.Resources.icon_vo_stop2;
            this.stopPlayButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.stopPlayButton.Location = new System.Drawing.Point(6, 77);
            this.stopPlayButton.Name = "stopPlayButton";
            this.stopPlayButton.Size = new System.Drawing.Size(75, 23);
            this.stopPlayButton.TabIndex = 14;
            this.stopPlayButton.Text = "Stop";
            this.stopPlayButton.UseVisualStyleBackColor = true;
            this.stopPlayButton.Click += new System.EventHandler(this.StopPlay);
            // 
            // pauseButton
            // 
            this.pauseButton.Image = global::WaveEditor.Properties.Resources.pauselines;
            this.pauseButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pauseButton.Location = new System.Drawing.Point(6, 48);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(75, 23);
            this.pauseButton.TabIndex = 13;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.PausePlay);
            // 
            // playButton
            // 
            this.playButton.Image = global::WaveEditor.Properties.Resources.playtriangle;
            this.playButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.playButton.Location = new System.Drawing.Point(6, 19);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(75, 23);
            this.playButton.TabIndex = 12;
            this.playButton.Text = "Start";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.StartPlay);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 511);
            this.Controls.Add(this.playGroup);
            this.Controls.Add(this.DFTGroup);
            this.Controls.Add(this.recordGroup);
            this.Controls.Add(this.fequencyChart);
            this.Controls.Add(this.sampleChart);
            this.Controls.Add(this.menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Wave Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CloseForm);
            this.Load += new System.EventHandler(this.LoadForm);
            ((System.ComponentModel.ISupportInitialize)(this.sampleChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fequencyChart)).EndInit();
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.recordGroup.ResumeLayout(false);
            this.DFTGroup.ResumeLayout(false);
            this.playGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart sampleChart;
        public void addData(byte[] data) {
            if (data == null) return;
            for (int x = 0; x < data.Length; x++) {
                sampleChart.Series["Series1"].Points.AddY(data[x]);
            }
            var chartArea = sampleChart.ChartAreas["ChartArea1"];
            chartArea.CursorX.AutoScroll = true;
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Maximum = 999;
            sampleChart.Invalidate();
            sampleChart.Update();

        }

        private System.Windows.Forms.DataVisualization.Charting.Chart fequencyChart;
        private MenuStrip menu;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem openItem;
        private Button DFTButton;
        private ToolStripMenuItem saveItem;
        private ToolStripMenuItem editMenu;
        private ToolStripMenuItem cutItem;
        private ToolStripMenuItem copyItem;
        private ToolStripMenuItem pasteItem;
        private Button playButton;
        private ToolStripMenuItem windowMenu;
        private ToolStripMenuItem triangleItem;
        private ToolStripMenuItem hammingItem;
        private ToolStripMenuItem filterMenu;
        private ToolStripMenuItem highPassItem;
        private ToolStripMenuItem lowPassItem;
        private ToolStripMenuItem bandPassItem;
        private Button filterButton;
        private Button startRecordButton;
        private Button stopRecordButton;
        private GroupBox recordGroup;
        private GroupBox DFTGroup;
        private ToolStripMenuItem magItem;
        private System.ComponentModel.BackgroundWorker DFTWorker;
        private GroupBox playGroup;
        private Button stopPlayButton;
        private Button pauseButton;
        private ToolStripMenuItem QuantizationMenu;
        private ToolStripMenuItem SixteenBitItem;
        private ToolStripMenuItem ThirtyTwoBitItem;
        private ToolStripMenuItem sampleRateMenu;
        private ToolStripMenuItem rate1Item;
        private ToolStripMenuItem rate2Item;
        private ToolStripMenuItem rate3Item;
    }
}

