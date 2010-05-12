namespace Matrix
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnExecute = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tbHexRead = new System.Windows.Forms.TextBox();
            this.btnHexBrowse = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Folder = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lblResults = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbInterval = new System.Windows.Forms.TextBox();
            this.btnDis = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.lblHex = new System.Windows.Forms.Label();
            this.Formating = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.Formating.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(6, 26);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(156, 28);
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // tbHexRead
            // 
            this.tbHexRead.Location = new System.Drawing.Point(12, 33);
            this.tbHexRead.Name = "tbHexRead";
            this.tbHexRead.Size = new System.Drawing.Size(210, 21);
            this.tbHexRead.TabIndex = 1;
            this.tbHexRead.TextChanged += new System.EventHandler(this.tbHexRead_TextChanged);
            // 
            // btnHexBrowse
            // 
            this.btnHexBrowse.Location = new System.Drawing.Point(212, 33);
            this.btnHexBrowse.Name = "btnHexBrowse";
            this.btnHexBrowse.Size = new System.Drawing.Size(75, 21);
            this.btnHexBrowse.TabIndex = 2;
            this.btnHexBrowse.Text = "Browse";
            this.btnHexBrowse.UseVisualStyleBackColor = true;
            this.btnHexBrowse.Click += new System.EventHandler(this.btnHexBrowse_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(3, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(59, 12);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "I\'m here!";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.lblMessage);
            this.panel1.Location = new System.Drawing.Point(12, 381);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(610, 145);
            this.panel1.TabIndex = 7;
            // 
            // Folder
            // 
            this.Folder.AutoSize = true;
            this.Folder.Location = new System.Drawing.Point(12, 18);
            this.Folder.Name = "Folder";
            this.Folder.Size = new System.Drawing.Size(77, 12);
            this.Folder.TabIndex = 8;
            this.Folder.Text = "Data folder:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(6, 57);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(0, 12);
            this.lblResults.TabIndex = 15;
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(6, 20);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(156, 31);
            this.btnConvert.TabIndex = 19;
            this.btnConvert.Text = "Convert to Hexagonal";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "Interval:";
            // 
            // tbInterval
            // 
            this.tbInterval.Location = new System.Drawing.Point(120, 15);
            this.tbInterval.MaxLength = 2;
            this.tbInterval.Name = "tbInterval";
            this.tbInterval.Size = new System.Drawing.Size(39, 21);
            this.tbInterval.TabIndex = 21;
            this.tbInterval.Text = "5";
            // 
            // btnDis
            // 
            this.btnDis.Location = new System.Drawing.Point(6, 76);
            this.btnDis.Name = "btnDis";
            this.btnDis.Size = new System.Drawing.Size(156, 28);
            this.btnDis.TabIndex = 22;
            this.btnDis.Text = "Save";
            this.btnDis.UseVisualStyleBackColor = true;
            this.btnDis.Click += new System.EventHandler(this.btnDis_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(6, 42);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(156, 28);
            this.btnPreview.TabIndex = 23;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // lbFiles
            // 
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.ItemHeight = 12;
            this.lbFiles.Location = new System.Drawing.Point(12, 62);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(275, 304);
            this.lbFiles.TabIndex = 33;
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
            // 
            // lblHex
            // 
            this.lblHex.AutoSize = true;
            this.lblHex.Location = new System.Drawing.Point(316, 42);
            this.lblHex.Name = "lblHex";
            this.lblHex.Size = new System.Drawing.Size(0, 12);
            this.lblHex.TabIndex = 34;
            // 
            // Formating
            // 
            this.Formating.Controls.Add(this.btnConvert);
            this.Formating.Location = new System.Drawing.Point(315, 62);
            this.Formating.Name = "Formating";
            this.Formating.Size = new System.Drawing.Size(279, 88);
            this.Formating.TabIndex = 35;
            this.Formating.TabStop = false;
            this.Formating.Text = "Formating";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnExecute);
            this.groupBox1.Controls.Add(this.lblResults);
            this.groupBox1.Location = new System.Drawing.Point(315, 156);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 97);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Compute";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPreview);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbInterval);
            this.groupBox2.Controls.Add(this.btnDis);
            this.groupBox2.Location = new System.Drawing.Point(315, 259);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 111);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Distribution";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 608);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Formating);
            this.Controls.Add(this.lblHex);
            this.Controls.Add(this.lbFiles);
            this.Controls.Add(this.Folder);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnHexBrowse);
            this.Controls.Add(this.tbHexRead);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "xMatrix";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.Formating.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox tbHexRead;
        private System.Windows.Forms.Button btnHexBrowse;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Folder;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbInterval;
        private System.Windows.Forms.Button btnDis;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Label lblHex;
        private System.Windows.Forms.GroupBox Formating;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

