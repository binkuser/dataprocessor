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
            this.btnExecute = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tbHexRead = new System.Windows.Forms.TextBox();
            this.btnHexBrowse = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.treeNewBee = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Folder = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbExeSave = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDisSaveBrowse = new System.Windows.Forms.Button();
            this.tbDisSave = new System.Windows.Forms.TextBox();
            this.lblResults = new System.Windows.Forms.Label();
            this.btnHexOpen = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbInterval = new System.Windows.Forms.TextBox();
            this.btnDis = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.tbExeRead = new System.Windows.Forms.TextBox();
            this.btnHexSaveBrowse = new System.Windows.Forms.Button();
            this.tbHexSave = new System.Windows.Forms.TextBox();
            this.btnDisView = new System.Windows.Forms.Button();
            this.btnDisBrowse = new System.Windows.Forms.Button();
            this.tbDisRead = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(100, 183);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(156, 28);
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // tbHexRead
            // 
            this.tbHexRead.Location = new System.Drawing.Point(99, 38);
            this.tbHexRead.Name = "tbHexRead";
            this.tbHexRead.Size = new System.Drawing.Size(460, 21);
            this.tbHexRead.TabIndex = 1;
            this.tbHexRead.TextChanged += new System.EventHandler(this.tbHexRead_TextChanged);
            // 
            // btnHexBrowse
            // 
            this.btnHexBrowse.Location = new System.Drawing.Point(554, 38);
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
            // treeNewBee
            // 
            this.treeNewBee.Location = new System.Drawing.Point(415, 0);
            this.treeNewBee.Name = "treeNewBee";
            this.treeNewBee.Size = new System.Drawing.Size(283, 247);
            this.treeNewBee.TabIndex = 5;
            this.treeNewBee.Visible = false;
            this.treeNewBee.DoubleClick += new System.EventHandler(this.treeNewBee_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 366);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Files:";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.lblMessage);
            this.panel1.Controls.Add(this.treeNewBee);
            this.panel1.Location = new System.Drawing.Point(14, 349);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(698, 247);
            this.panel1.TabIndex = 7;
            // 
            // Folder
            // 
            this.Folder.AutoSize = true;
            this.Folder.Location = new System.Drawing.Point(10, 42);
            this.Folder.Name = "Folder";
            this.Folder.Size = new System.Drawing.Size(35, 12);
            this.Folder.TabIndex = 8;
            this.Folder.Text = "Data:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "Save to:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(554, 156);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 21);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Browse";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnExeSaveBrowse_Click);
            // 
            // tbExeSave
            // 
            this.tbExeSave.Location = new System.Drawing.Point(99, 156);
            this.tbExeSave.Name = "tbExeSave";
            this.tbExeSave.Size = new System.Drawing.Size(460, 21);
            this.tbExeSave.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "Distribution:";
            // 
            // btnDisSaveBrowse
            // 
            this.btnDisSaveBrowse.Location = new System.Drawing.Point(553, 263);
            this.btnDisSaveBrowse.Name = "btnDisSaveBrowse";
            this.btnDisSaveBrowse.Size = new System.Drawing.Size(75, 21);
            this.btnDisSaveBrowse.TabIndex = 13;
            this.btnDisSaveBrowse.Text = "Browse";
            this.btnDisSaveBrowse.UseVisualStyleBackColor = true;
            this.btnDisSaveBrowse.Click += new System.EventHandler(this.btnDisSaveBrowse_Click);
            // 
            // tbDisSave
            // 
            this.tbDisSave.Location = new System.Drawing.Point(98, 263);
            this.tbDisSave.Name = "tbDisSave";
            this.tbDisSave.Size = new System.Drawing.Size(460, 21);
            this.tbDisSave.TabIndex = 12;
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(98, 184);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(0, 12);
            this.lblResults.TabIndex = 15;
            // 
            // btnHexOpen
            // 
            this.btnHexOpen.Location = new System.Drawing.Point(635, 38);
            this.btnHexOpen.Name = "btnHexOpen";
            this.btnHexOpen.Size = new System.Drawing.Size(75, 21);
            this.btnHexOpen.TabIndex = 16;
            this.btnHexOpen.Text = "View";
            this.btnHexOpen.UseVisualStyleBackColor = true;
            this.btnHexOpen.Click += new System.EventHandler(this.btnHexView_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(99, 92);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(156, 24);
            this.btnConvert.TabIndex = 19;
            this.btnConvert.Text = "Convert to Hexagonal";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(96, 294);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "Interval:";
            // 
            // tbInterval
            // 
            this.tbInterval.Location = new System.Drawing.Point(161, 291);
            this.tbInterval.MaxLength = 2;
            this.tbInterval.Name = "tbInterval";
            this.tbInterval.Size = new System.Drawing.Size(39, 21);
            this.tbInterval.TabIndex = 21;
            this.tbInterval.Text = "5";
            // 
            // btnDis
            // 
            this.btnDis.Location = new System.Drawing.Point(268, 315);
            this.btnDis.Name = "btnDis";
            this.btnDis.Size = new System.Drawing.Size(156, 28);
            this.btnDis.TabIndex = 22;
            this.btnDis.Text = "Save";
            this.btnDis.UseVisualStyleBackColor = true;
            this.btnDis.Click += new System.EventHandler(this.btnDis_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(98, 315);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(156, 28);
            this.btnPreview.TabIndex = 23;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(635, 129);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 21);
            this.btnView.TabIndex = 26;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnExeView_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(554, 129);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 21);
            this.btnRead.TabIndex = 25;
            this.btnRead.Text = "Browse";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnExeBrowse_Click);
            // 
            // tbExeRead
            // 
            this.tbExeRead.Location = new System.Drawing.Point(99, 129);
            this.tbExeRead.Name = "tbExeRead";
            this.tbExeRead.Size = new System.Drawing.Size(460, 21);
            this.tbExeRead.TabIndex = 24;
            this.tbExeRead.TextChanged += new System.EventHandler(this.tbExeRead_TextChanged);
            // 
            // btnHexSaveBrowse
            // 
            this.btnHexSaveBrowse.Location = new System.Drawing.Point(554, 65);
            this.btnHexSaveBrowse.Name = "btnHexSaveBrowse";
            this.btnHexSaveBrowse.Size = new System.Drawing.Size(75, 21);
            this.btnHexSaveBrowse.TabIndex = 28;
            this.btnHexSaveBrowse.Text = "Browse";
            this.btnHexSaveBrowse.UseVisualStyleBackColor = true;
            this.btnHexSaveBrowse.Click += new System.EventHandler(this.btnHexSaveBrowse_Click);
            // 
            // tbHexSave
            // 
            this.tbHexSave.Location = new System.Drawing.Point(99, 65);
            this.tbHexSave.Name = "tbHexSave";
            this.tbHexSave.Size = new System.Drawing.Size(460, 21);
            this.tbHexSave.TabIndex = 27;
            // 
            // btnDisView
            // 
            this.btnDisView.Location = new System.Drawing.Point(634, 236);
            this.btnDisView.Name = "btnDisView";
            this.btnDisView.Size = new System.Drawing.Size(75, 21);
            this.btnDisView.TabIndex = 32;
            this.btnDisView.Text = "View";
            this.btnDisView.UseVisualStyleBackColor = true;
            this.btnDisView.Click += new System.EventHandler(this.btnDisView_Click);
            // 
            // btnDisBrowse
            // 
            this.btnDisBrowse.Location = new System.Drawing.Point(553, 236);
            this.btnDisBrowse.Name = "btnDisBrowse";
            this.btnDisBrowse.Size = new System.Drawing.Size(75, 21);
            this.btnDisBrowse.TabIndex = 31;
            this.btnDisBrowse.Text = "Browse";
            this.btnDisBrowse.UseVisualStyleBackColor = true;
            this.btnDisBrowse.Click += new System.EventHandler(this.btnDisBrowse_Click);
            // 
            // tbDisRead
            // 
            this.tbDisRead.Location = new System.Drawing.Point(98, 236);
            this.tbDisRead.Name = "tbDisRead";
            this.tbDisRead.Size = new System.Drawing.Size(460, 21);
            this.tbDisRead.TabIndex = 30;
            this.tbDisRead.TextChanged += new System.EventHandler(this.tbDisRead_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 608);
            this.Controls.Add(this.btnDisView);
            this.Controls.Add(this.btnDisBrowse);
            this.Controls.Add(this.tbDisRead);
            this.Controls.Add(this.btnHexSaveBrowse);
            this.Controls.Add(this.tbHexSave);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.tbExeRead);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnDis);
            this.Controls.Add(this.tbInterval);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnHexOpen);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDisSaveBrowse);
            this.Controls.Add(this.tbDisSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbExeSave);
            this.Controls.Add(this.Folder);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnHexBrowse);
            this.Controls.Add(this.tbHexRead);
            this.Controls.Add(this.btnExecute);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox tbHexRead;
        private System.Windows.Forms.Button btnHexBrowse;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TreeView treeNewBee;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Folder;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbExeSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDisSaveBrowse;
        private System.Windows.Forms.TextBox tbDisSave;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.Button btnHexOpen;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbInterval;
        private System.Windows.Forms.Button btnDis;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.TextBox tbExeRead;
        private System.Windows.Forms.Button btnHexSaveBrowse;
        private System.Windows.Forms.TextBox tbHexSave;
        private System.Windows.Forms.Button btnDisView;
        private System.Windows.Forms.Button btnDisBrowse;
        private System.Windows.Forms.TextBox tbDisRead;
    }
}

