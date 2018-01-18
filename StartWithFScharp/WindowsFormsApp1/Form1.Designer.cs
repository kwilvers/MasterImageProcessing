namespace WindowsFormsApp1
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tbMaxRadius = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMinRadius = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.tbParam2 = new System.Windows.Forms.TrackBar();
            this.tbParam1 = new System.Windows.Forms.TrackBar();
            this.param2 = new System.Windows.Forms.Label();
            this.param1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.upDp = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMinDist = new System.Windows.Forms.TrackBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbMinThreshold = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbMaxThreshold = new System.Windows.Forms.TrackBar();
            this.cbByColor = new System.Windows.Forms.CheckBox();
            this.cbByArea = new System.Windows.Forms.CheckBox();
            this.cbByCircularity = new System.Windows.Forms.CheckBox();
            this.cbByConvexity = new System.Windows.Forms.CheckBox();
            this.cbByInertia = new System.Windows.Forms.CheckBox();
            this.tbMinArea = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbMaxArea = new System.Windows.Forms.TrackBar();
            this.tbMinConvexity = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbMaxConvexity = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParam2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParam1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinDist)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinConvexity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxConvexity)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(735, 627);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Size = new System.Drawing.Size(910, 627);
            this.splitContainer1.SplitterDistance = 171;
            this.splitContainer1.TabIndex = 2;
            // 
            // tbMaxRadius
            // 
            this.tbMaxRadius.LargeChange = 10;
            this.tbMaxRadius.Location = new System.Drawing.Point(9, 323);
            this.tbMaxRadius.Maximum = 200;
            this.tbMaxRadius.Name = "tbMaxRadius";
            this.tbMaxRadius.Size = new System.Drawing.Size(156, 45);
            this.tbMaxRadius.SmallChange = 10;
            this.tbMaxRadius.TabIndex = 11;
            this.tbMaxRadius.TickFrequency = 10;
            this.tbMaxRadius.Value = 50;
            this.tbMaxRadius.Scroll += new System.EventHandler(this.tb_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 307);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "maxRadius";
            this.toolTip1.SetToolTip(this.label4, "Maximum circle radius");
            // 
            // tbMinRadius
            // 
            this.tbMinRadius.Location = new System.Drawing.Point(8, 261);
            this.tbMinRadius.Maximum = 100;
            this.tbMinRadius.Name = "tbMinRadius";
            this.tbMinRadius.Size = new System.Drawing.Size(156, 45);
            this.tbMinRadius.TabIndex = 9;
            this.tbMinRadius.TickFrequency = 10;
            this.tbMinRadius.Value = 10;
            this.tbMinRadius.Scroll += new System.EventHandler(this.tb_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 245);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "minRadius";
            this.toolTip1.SetToolTip(this.label3, "Minimum circle radius");
            // 
            // tbParam2
            // 
            this.tbParam2.Location = new System.Drawing.Point(6, 194);
            this.tbParam2.Maximum = 200;
            this.tbParam2.Name = "tbParam2";
            this.tbParam2.Size = new System.Drawing.Size(156, 45);
            this.tbParam2.TabIndex = 7;
            this.tbParam2.TickFrequency = 10;
            this.tbParam2.Value = 10;
            this.tbParam2.Scroll += new System.EventHandler(this.tb_Scroll);
            // 
            // tbParam1
            // 
            this.tbParam1.Location = new System.Drawing.Point(6, 119);
            this.tbParam1.Maximum = 300;
            this.tbParam1.Name = "tbParam1";
            this.tbParam1.Size = new System.Drawing.Size(156, 45);
            this.tbParam1.TabIndex = 6;
            this.tbParam1.TickFrequency = 10;
            this.tbParam1.Value = 200;
            this.tbParam1.Scroll += new System.EventHandler(this.tb_Scroll);
            // 
            // param2
            // 
            this.param2.AutoSize = true;
            this.param2.Location = new System.Drawing.Point(6, 178);
            this.param2.Name = "param2";
            this.param2.Size = new System.Drawing.Size(42, 13);
            this.param2.TabIndex = 5;
            this.param2.Text = "param2";
            this.toolTip1.SetToolTip(this.param2, resources.GetString("param2.ToolTip"));
            // 
            // param1
            // 
            this.param1.AutoSize = true;
            this.param1.Location = new System.Drawing.Point(6, 103);
            this.param1.Name = "param1";
            this.param1.Size = new System.Drawing.Size(42, 13);
            this.param1.TabIndex = 4;
            this.param1.Text = "param1";
            this.toolTip1.SetToolTip(this.param1, "First method-specific parameter. In case of CV_HOUGH_GRADIENT , it is the higher " +
        "threshold of the two passed to the Canny edge detector (the lower one is twice s" +
        "maller).\r\n");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "minDist";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // upDp
            // 
            this.upDp.Location = new System.Drawing.Point(74, 11);
            this.upDp.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.upDp.Name = "upDp";
            this.upDp.Size = new System.Drawing.Size(88, 20);
            this.upDp.TabIndex = 2;
            this.upDp.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDp.ValueChanged += new System.EventHandler(this.tb_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dp";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // tbMinDist
            // 
            this.tbMinDist.Location = new System.Drawing.Point(6, 58);
            this.tbMinDist.Maximum = 200;
            this.tbMinDist.Name = "tbMinDist";
            this.tbMinDist.Size = new System.Drawing.Size(156, 45);
            this.tbMinDist.TabIndex = 0;
            this.tbMinDist.TickFrequency = 10;
            this.tbMinDist.Value = 20;
            this.tbMinDist.Scroll += new System.EventHandler(this.tb_Scroll);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(85, 572);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(171, 627);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbMaxRadius);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.tbMinDist);
            this.tabPage1.Controls.Add(this.tbMinRadius);
            this.tabPage1.Controls.Add(this.upDp);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbParam2);
            this.tabPage1.Controls.Add(this.param1);
            this.tabPage1.Controls.Add(this.tbParam1);
            this.tabPage1.Controls.Add(this.param2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(163, 601);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbMinConvexity);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.tbMaxConvexity);
            this.tabPage2.Controls.Add(this.tbMinArea);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.tbMaxArea);
            this.tabPage2.Controls.Add(this.cbByInertia);
            this.tabPage2.Controls.Add(this.cbByConvexity);
            this.tabPage2.Controls.Add(this.cbByCircularity);
            this.tabPage2.Controls.Add(this.cbByArea);
            this.tabPage2.Controls.Add(this.cbByColor);
            this.tabPage2.Controls.Add(this.tbMinThreshold);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.tbMaxThreshold);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(163, 601);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbMinThreshold
            // 
            this.tbMinThreshold.Location = new System.Drawing.Point(3, 43);
            this.tbMinThreshold.Maximum = 200;
            this.tbMinThreshold.Name = "tbMinThreshold";
            this.tbMinThreshold.Size = new System.Drawing.Size(156, 45);
            this.tbMinThreshold.TabIndex = 13;
            this.tbMinThreshold.TickFrequency = 10;
            this.tbMinThreshold.Value = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "minThreshold";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "maxThreshold";
            // 
            // tbMaxThreshold
            // 
            this.tbMaxThreshold.Location = new System.Drawing.Point(3, 104);
            this.tbMaxThreshold.Maximum = 300;
            this.tbMaxThreshold.Name = "tbMaxThreshold";
            this.tbMaxThreshold.Size = new System.Drawing.Size(156, 45);
            this.tbMaxThreshold.TabIndex = 16;
            this.tbMaxThreshold.TickFrequency = 10;
            this.tbMaxThreshold.Value = 200;
            // 
            // cbByColor
            // 
            this.cbByColor.AutoSize = true;
            this.cbByColor.Location = new System.Drawing.Point(3, 3);
            this.cbByColor.Name = "cbByColor";
            this.cbByColor.Size = new System.Drawing.Size(64, 17);
            this.cbByColor.TabIndex = 17;
            this.cbByColor.Text = "By color";
            this.cbByColor.UseVisualStyleBackColor = true;
            this.cbByColor.CheckedChanged += new System.EventHandler(this.button1_Click);
            // 
            // cbByArea
            // 
            this.cbByArea.AutoSize = true;
            this.cbByArea.Location = new System.Drawing.Point(3, 155);
            this.cbByArea.Name = "cbByArea";
            this.cbByArea.Size = new System.Drawing.Size(62, 17);
            this.cbByArea.TabIndex = 18;
            this.cbByArea.Text = "By area";
            this.cbByArea.UseVisualStyleBackColor = true;
            this.cbByArea.CheckedChanged += new System.EventHandler(this.button1_Click);
            // 
            // cbByCircularity
            // 
            this.cbByCircularity.AutoSize = true;
            this.cbByCircularity.Location = new System.Drawing.Point(-1, 450);
            this.cbByCircularity.Name = "cbByCircularity";
            this.cbByCircularity.Size = new System.Drawing.Size(85, 17);
            this.cbByCircularity.TabIndex = 19;
            this.cbByCircularity.Text = "By circularity";
            this.cbByCircularity.UseVisualStyleBackColor = true;
            this.cbByCircularity.CheckedChanged += new System.EventHandler(this.button1_Click);
            // 
            // cbByConvexity
            // 
            this.cbByConvexity.AutoSize = true;
            this.cbByConvexity.Location = new System.Drawing.Point(0, 305);
            this.cbByConvexity.Name = "cbByConvexity";
            this.cbByConvexity.Size = new System.Drawing.Size(86, 17);
            this.cbByConvexity.TabIndex = 20;
            this.cbByConvexity.Text = "By convexity";
            this.cbByConvexity.UseVisualStyleBackColor = true;
            this.cbByConvexity.CheckedChanged += new System.EventHandler(this.button1_Click);
            // 
            // cbByInertia
            // 
            this.cbByInertia.AutoSize = true;
            this.cbByInertia.Location = new System.Drawing.Point(0, 482);
            this.cbByInertia.Name = "cbByInertia";
            this.cbByInertia.Size = new System.Drawing.Size(69, 17);
            this.cbByInertia.TabIndex = 21;
            this.cbByInertia.Text = "By inertia";
            this.cbByInertia.UseVisualStyleBackColor = true;
            this.cbByInertia.CheckedChanged += new System.EventHandler(this.button1_Click);
            // 
            // tbMinArea
            // 
            this.tbMinArea.Location = new System.Drawing.Point(3, 193);
            this.tbMinArea.Maximum = 2000;
            this.tbMinArea.Name = "tbMinArea";
            this.tbMinArea.Size = new System.Drawing.Size(156, 45);
            this.tbMinArea.TabIndex = 22;
            this.tbMinArea.TickFrequency = 100;
            this.tbMinArea.Value = 1500;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "minArea";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 238);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "maxArea";
            // 
            // tbMaxArea
            // 
            this.tbMaxArea.Location = new System.Drawing.Point(3, 254);
            this.tbMaxArea.Maximum = 3000;
            this.tbMaxArea.Name = "tbMaxArea";
            this.tbMaxArea.Size = new System.Drawing.Size(156, 45);
            this.tbMaxArea.TabIndex = 25;
            this.tbMaxArea.TickFrequency = 200;
            // 
            // tbMinConvexity
            // 
            this.tbMinConvexity.Location = new System.Drawing.Point(0, 338);
            this.tbMinConvexity.Maximum = 200;
            this.tbMinConvexity.Name = "tbMinConvexity";
            this.tbMinConvexity.Size = new System.Drawing.Size(156, 45);
            this.tbMinConvexity.TabIndex = 26;
            this.tbMinConvexity.TickFrequency = 10;
            this.tbMinConvexity.Value = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(0, 322);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "minConvexity";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 383);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "maxConvexity";
            // 
            // tbMaxConvexity
            // 
            this.tbMaxConvexity.Location = new System.Drawing.Point(0, 399);
            this.tbMaxConvexity.Maximum = 300;
            this.tbMaxConvexity.Name = "tbMaxConvexity";
            this.tbMaxConvexity.Size = new System.Drawing.Size(156, 45);
            this.tbMaxConvexity.TabIndex = 29;
            this.tbMaxConvexity.TickFrequency = 10;
            this.tbMaxConvexity.Value = 200;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 627);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParam2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParam1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinDist)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinConvexity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxConvexity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenCvSharp.UserInterface.PictureBoxIpl pictureBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TrackBar tbMinDist;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown upDp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar tbMaxRadius;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar tbMinRadius;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar tbParam2;
        private System.Windows.Forms.TrackBar tbParam1;
        private System.Windows.Forms.Label param2;
        private System.Windows.Forms.Label param1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TrackBar tbMinThreshold;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar tbMaxThreshold;
        private System.Windows.Forms.TrackBar tbMinArea;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar tbMaxArea;
        private System.Windows.Forms.CheckBox cbByInertia;
        private System.Windows.Forms.CheckBox cbByConvexity;
        private System.Windows.Forms.CheckBox cbByCircularity;
        private System.Windows.Forms.CheckBox cbByArea;
        private System.Windows.Forms.CheckBox cbByColor;
        private System.Windows.Forms.TrackBar tbMinConvexity;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TrackBar tbMaxConvexity;
    }
}

