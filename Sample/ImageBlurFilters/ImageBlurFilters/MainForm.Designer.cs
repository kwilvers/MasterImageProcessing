namespace ImageBlurFilter
{
    partial class MainForm
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
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.btnOpenOriginal = new System.Windows.Forms.Button();
            this.btnSaveNewImage = new System.Windows.Forms.Button();
            this.cmbBlurFilter = new System.Windows.Forms.ComboBox();
            this.lblBlurFilter = new System.Windows.Forms.Label();
            this.pnlBlur = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.picOriginal = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.pnlBlur.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).BeginInit();
            this.SuspendLayout();
            // 
            // picPreview
            // 
            this.picPreview.BackColor = System.Drawing.Color.Silver;
            this.picPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPreview.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPreview.Location = new System.Drawing.Point(3, 3);
            this.picPreview.Name = "picPreview";
            this.tableLayoutPanel1.SetRowSpan(this.picPreview, 2);
            this.picPreview.Size = new System.Drawing.Size(953, 386);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 13;
            this.picPreview.TabStop = false;
            // 
            // btnOpenOriginal
            // 
            this.btnOpenOriginal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenOriginal.Location = new System.Drawing.Point(962, 395);
            this.btnOpenOriginal.Name = "btnOpenOriginal";
            this.btnOpenOriginal.Size = new System.Drawing.Size(228, 46);
            this.btnOpenOriginal.TabIndex = 15;
            this.btnOpenOriginal.Text = "Load Image";
            this.btnOpenOriginal.UseVisualStyleBackColor = true;
            this.btnOpenOriginal.Click += new System.EventHandler(this.btnOpenOriginal_Click);
            // 
            // btnSaveNewImage
            // 
            this.btnSaveNewImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveNewImage.Location = new System.Drawing.Point(962, 591);
            this.btnSaveNewImage.Name = "btnSaveNewImage";
            this.btnSaveNewImage.Size = new System.Drawing.Size(228, 46);
            this.btnSaveNewImage.TabIndex = 16;
            this.btnSaveNewImage.Text = "Save Image";
            this.btnSaveNewImage.UseVisualStyleBackColor = true;
            this.btnSaveNewImage.Click += new System.EventHandler(this.btnSaveNewImage_Click);
            // 
            // cmbBlurFilter
            // 
            this.cmbBlurFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlurFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBlurFilter.FormattingEnabled = true;
            this.cmbBlurFilter.Location = new System.Drawing.Point(7, 36);
            this.cmbBlurFilter.Name = "cmbBlurFilter";
            this.cmbBlurFilter.Size = new System.Drawing.Size(213, 37);
            this.cmbBlurFilter.TabIndex = 20;
            this.cmbBlurFilter.SelectedIndexChanged += new System.EventHandler(this.FilterValueChangedEventHandler);
            // 
            // lblBlurFilter
            // 
            this.lblBlurFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlurFilter.Location = new System.Drawing.Point(2, 3);
            this.lblBlurFilter.Name = "lblBlurFilter";
            this.lblBlurFilter.Size = new System.Drawing.Size(217, 30);
            this.lblBlurFilter.TabIndex = 26;
            this.lblBlurFilter.Text = "Blur Filter";
            this.lblBlurFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlBlur
            // 
            this.pnlBlur.BackColor = System.Drawing.Color.Silver;
            this.pnlBlur.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBlur.Controls.Add(this.lblBlurFilter);
            this.pnlBlur.Controls.Add(this.cmbBlurFilter);
            this.pnlBlur.Location = new System.Drawing.Point(962, 3);
            this.pnlBlur.Name = "pnlBlur";
            this.pnlBlur.Size = new System.Drawing.Size(228, 76);
            this.pnlBlur.TabIndex = 34;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.pnlBlur, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.picPreview, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveNewImage, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenOriginal, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.picOriginal, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1193, 787);
            this.tableLayoutPanel1.TabIndex = 35;
            // 
            // picOriginal
            // 
            this.picOriginal.BackColor = System.Drawing.SystemColors.ControlDark;
            this.picOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picOriginal.Location = new System.Drawing.Point(3, 395);
            this.picOriginal.Name = "picOriginal";
            this.tableLayoutPanel1.SetRowSpan(this.picOriginal, 2);
            this.picOriginal.Size = new System.Drawing.Size(953, 389);
            this.picOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOriginal.TabIndex = 35;
            this.picOriginal.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1193, 787);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Blur Filter";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.pnlBlur.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Button btnOpenOriginal;
        private System.Windows.Forms.Button btnSaveNewImage;
        private System.Windows.Forms.ComboBox cmbBlurFilter;
        private System.Windows.Forms.Label lblBlurFilter;
        private System.Windows.Forms.Panel pnlBlur;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox picOriginal;
    }
}

