using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Matrix
{
    public partial class Form1 : Form
    {
        #region properties
        Matrix m;
        int Sleep
        {
            get
            {
                FileInfo f = new FileInfo("config.ini");
                int delay;
                if (f.Exists)
                {
                    StreamReader sr = new StreamReader(f.FullName);
                    string strDelay = sr.ReadToEnd();
                    sr.Dispose();
                    if (int.TryParse(strDelay, out delay))
                    {
                        Random r = new Random();
                        return delay + r.Next(delay);
                    }
                }
                return 0;
            }
        }
        #endregion

        #region constractors & on load
        public Form1()
        {
            m = new Matrix();
            InitializeComponent();
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = "";


            this.BackColor = Color.FromArgb(200, 230, 230); ;
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.GetType().Equals(typeof(Button)))
                {
                    Button btn = ((Button)ctrl);//.BackColor = Color.Wheat;
                    btn.BackColor = Color.FromArgb(196, 230, 230);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 255, 255);
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(64, 128, 128);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.ForeColor = Color.Black;
                }
                if (ctrl.GetType().Equals(typeof(TextBox)))
                {
                    TextBox txt = ((TextBox)ctrl);//.BackColor = Color.Wheat;
                    txt.BackColor = Color.White;
                    txt.BorderStyle = BorderStyle.FixedSingle;

                }

            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo("../../Data/");
            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles();
                string strDataFile = "";
                if (files.Length>0)
                {
                    strDataFile = files[0].Name;
                }
                string str = dir.FullName;
                tbHexRead.Text = files[0].FullName;//= System.IO.Path.Combine("../../Data/", Environment.CurrentDirectory);
                tbHexSave.Text = Path.Combine(Path.GetDirectoryName(tbHexRead.Text),
                    Path.GetFileNameWithoutExtension(tbHexRead.Text) + "-rad" + Path.GetExtension(tbHexRead.Text));
                tbExeRead.Text = tbHexRead.Text;
                tbExeSave.Text = str + "Result.ang";
                tbDisRead.Text = tbExeSave.Text;
                tbDisSave.Text = str + "Result1.ang";
            }
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            btnExecute.Enabled = File.Exists(tbHexRead.Text);
        }
        #endregion

        #region functions
        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = "Working@Please wait... ";
                DateTime timeStart = DateTime.Now;

                string strSavePath;
                m._4to6(tbHexRead.Text, tbHexSave.Text);
                Thread.Sleep(Sleep);


                lblMessage.Text = "Converting Success";
                TimeSpan diff = DateTime.Now - timeStart;
                double sec = diff.TotalMilliseconds;
                lblMessage.Text += "\r\n\r\nTime cost:" + sec.ToString("N2") + "ms";
                treePop(tbHexRead.Text);
                tbExeRead.Text = tbHexSave.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Failed@" + ex.Message;
                lblMessage.Text += "\r\n\r\n Stack trace:\r\n" + ex.StackTrace;
            }
        }
        private void btnExecute_Click(object sender, EventArgs e)
        {
            string strTempSavePath = "";
            try
            {
                lblMessage.Text = "Working@Please wait... ";
                DateTime timeStart = DateTime.Now;
                string strPath = tbExeRead.Text;
                double sec;
                string result = Exe(strPath, tbExeSave.Text);
                treePop(tbExeSave.Text);
                tbDisRead.Text = tbExeSave.Text;

                TimeSpan diff = DateTime.Now - timeStart;
                sec = diff.TotalMilliseconds;
                lblMessage.Text = "Done@Success";
                lblMessage.Text += "\r\n\r\nTime cost:" + sec.ToString("N2") + "ms";
                lblResults.Text = result;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Failed@" + ex.Message;
                lblMessage.Text += "\r\n\r\n Stack trace:\r\n" + ex.StackTrace;
            }

        }
        private string Exe(string strPath,string strSave1Path)
        {
            string strTempSavePath = Path.Combine(Path.GetTempPath(), "temp.ang");
            //Path.Combine(tbPath.Text, "temp.ang");
            float IQMax;
            float IQMin;
            int DNum;
            int Nx;
            try
            {
                m.Read_File(strPath, strTempSavePath, out IQMax, out IQMin, out DNum, out Nx);
                m.Read_NIQ(strTempSavePath, strSave1Path, IQMax, IQMin);
                Thread.Sleep(Sleep);
                return string.Format("DataSize={0} ; X axis size: Nx={1};\r\n"
                                    + "IQmax={2} ; IQmin={3}", new object[] { DNum, Nx, IQMax, IQMin });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (File.Exists(strTempSavePath))
                    File.Delete(strTempSavePath);
            }
        }

        private void btnDis_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = "Working@Please wait... ";
                DateTime timeStart = DateTime.Now;
                string strSavePath = tbDisSave.Text;
                m.Save_NX(tbDisRead.Text, tbDisSave.Text, Convert.ToInt32(tbInterval.Text));
                Thread.Sleep(Sleep);

                lblMessage.Text = "Done@Success";
                TimeSpan diff = DateTime.Now - timeStart;
                double sec = diff.TotalMilliseconds;
                lblMessage.Text += "\r\n\r\nTime cost:" + sec.ToString("N2") + "ms";
                treePop(tbDisSave.Text);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Failed@" + ex.Message;
                lblMessage.Text += "\r\n\r\n Stack trace:\r\n" + ex.StackTrace;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            lblMessage.Text = m.Read_NX(tbDisRead.Text, "", Convert.ToInt32(tbInterval.Text));
        }

        #endregion

        #region events

        private void treeNewBee_DoubleClick(object sender, EventArgs e)
        {
            if (File.Exists(treeNewBee.SelectedNode.Tag.ToString()))
                System.Diagnostics.Process.Start(treeNewBee.SelectedNode.Tag.ToString());
        }
        private void treePop(string Path)
        {
            DirectoryInfo dir = new DirectoryInfo(Path);
            TreeNode node = new TreeNode(dir.Name);
            TreeNode nodeSelect = node;
            node.Tag = dir.FullName;
            node.Name = node.Tag.ToString();

            while (dir.Parent != null && dir.Parent.Exists)
            {
                dir = dir.Parent;

                TreeNode nodeParent = new TreeNode(dir.Name);
                nodeParent.Tag = dir.FullName;
                nodeParent.Name = node.Tag.ToString();
                nodeParent.Nodes.Add((TreeNode)node.Clone());
                foreach (DirectoryInfo dirChilds in dir.GetDirectories())
                {
                    TreeNode nodeTemp = new TreeNode(dirChilds.Name);
                    nodeTemp.Tag = dirChilds.FullName;
                    nodeTemp.Name = nodeTemp.Tag.ToString();
                    if (nodeParent.Nodes.Find(nodeTemp.Name, false).Length == 0)
                        nodeParent.Nodes.Add(nodeTemp);
                }
                foreach (FileInfo fileChilds in dir.GetFiles())
                {
                    TreeNode nodeTemp = new TreeNode(fileChilds.Name);
                    nodeTemp.Tag = fileChilds.FullName;
                    nodeTemp.Name = nodeTemp.Tag.ToString();
                    if (nodeParent.Nodes.Find(nodeTemp.Name, false).Length == 0)
                        nodeParent.Nodes.Add(nodeTemp);
                }
                node = nodeParent;
                node.ExpandAll();
            }

            treeNewBee.Nodes.Add(node);
            treeNewBee.SelectedNode = nodeSelect;
        }

        private void tbHexRead_TextChanged(object sender, EventArgs e)
        {
            treeNewBee.Nodes.Clear();
            treePop(tbHexRead.Text);
            btnConvert.Enabled = File.Exists(tbHexRead.Text);
            //DirectoryInfo dir = new DirectoryInfo(tbPath.Text);
            //if (dir.Exists)
            //    foreach (FileInfo file in dir.GetFiles())
            //    {
            //        treeNewBee.Nodes.Add(file.Name);
            //    }
            //if (treeNewBee.Nodes.Count > 0)
            //    treeNewBee.SelectedNode = treeNewBee.Nodes[0];
        }
        private void tbExeRead_TextChanged(object sender, EventArgs e)
        {
            btnExecute.Enabled = File.Exists(tbExeRead.Text);
        }
        private void tbDisRead_TextChanged(object sender, EventArgs e)
        {
            btnDis.Enabled = File.Exists(tbDisRead.Text);
            btnPreview.Enabled = File.Exists(tbDisRead.Text);
        }

        private void btnExeSaveBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = tbExeSave.Text;
            saveFileDialog1.FileName = tbExeSave.Text;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                tbExeSave.Text = saveFileDialog1.FileName;
        }
        private void btnDisSaveBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = tbDisSave.Text;
            saveFileDialog1.FileName = tbDisSave.Text;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                tbDisSave.Text = saveFileDialog1.FileName;
        }
        private void btnHexSaveBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = tbHexSave.Text;
            saveFileDialog1.FileName = tbHexSave.Text;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                tbHexSave.Text = saveFileDialog1.FileName;

        }

        private void btnHexBrowse_Click(object sender, EventArgs e)
        {
            //"../../Data/"
            //folderBrowserDialog1.SelectedPath = tbPath.Text;
            //if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            //    tbPath.Text = folderBrowserDialog1.SelectedPath;
            openFileDialog1.InitialDirectory = tbHexRead.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tbHexRead.Text = openFileDialog1.FileName;
        }
        private void btnExeBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = tbExeRead.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tbExeRead.Text = openFileDialog1.FileName;
            btnExecute.Enabled = File.Exists(tbExeRead.Text);
        }
        private void btnDisBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = tbDisRead.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tbDisRead.Text = openFileDialog1.FileName;
        }

        private void btnHexView_Click(object sender, EventArgs e)
        {
            if (File.Exists(tbHexRead.Text))
                System.Diagnostics.Process.Start(tbHexRead.Text.ToString());
        }
        private void btnExeView_Click(object sender, EventArgs e)
        {
            if (File.Exists(tbExeRead.Text))
                System.Diagnostics.Process.Start(tbExeRead.Text.ToString());
        }
        private void btnDisView_Click(object sender, EventArgs e)
        {
            if (File.Exists(tbDisRead.Text))
                System.Diagnostics.Process.Start(tbDisRead.Text.ToString());
        }




        #endregion
    }
}
