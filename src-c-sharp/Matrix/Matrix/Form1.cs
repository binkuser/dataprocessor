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
        private string EncodeXML(string strInput)
        {
            byte[] bytes = (new UnicodeEncoding()).GetBytes(strInput);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] += Convert.ToByte(bytes.Length + i / 7);
            }
            string strRet = (new UnicodeEncoding()).GetString(bytes);
            return strRet;
        }
        private string DecodeXML(string strInput)
        {
            byte[] bytes = (new UnicodeEncoding()).GetBytes(strInput);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] -= Convert.ToByte(bytes.Length + i / 7);
            }
            string strRet = (new UnicodeEncoding()).GetString(bytes);
            return strRet;
        }

        private void Scri()
        {
            try
            {
                DataSet ds = new DataSet();
                StreamReader sr = new StreamReader("key.dat");
                string strXML = "";
                while (!sr.EndOfStream)
                {
                    string strLine = sr.ReadLine();
                    strXML += DecodeXML(strLine);
                }
                TextReader txt = new StringReader(strXML);
                ds.ReadXml(txt);
                sr.Dispose();
                DateTime StartDate = DateTime.Parse(ds.Tables[0].Rows[0]["start_date"].ToString())
                    , EndDate = DateTime.Parse(ds.Tables[0].Rows[0]["end_date"].ToString())
                    , LastDate = DateTime.Parse(ds.Tables[0].Rows[0]["last_date"].ToString());
                if (DateTime.Now.CompareTo(StartDate) < 0
                    || DateTime.Now.CompareTo(EndDate) > 0
                    || DateTime.Now.CompareTo(LastDate) < 0)
                {
                    throw new Exception("license expired");
                }
                ds.Tables[0].Rows[0]["last_date"] = DateTime.Now;
                strXML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> \r\n" + ds.GetXml();
                StreamWriter sw = File.CreateText("key.dat");
                StringReader str = new StringReader(strXML);
                while (true)
                {
                    string strLine = str.ReadLine();
                    if (strLine == null) break;
                    sw.WriteLine(EncodeXML(strLine));
                }
                str.Dispose();
                sw.Dispose();
            }

            catch (Exception ex)
            {
                Exception exx = new Exception("Invalid license key. " + ex.Message, ex);
                throw exx;
            }
        }

        public Form1()
        {
            m = new Matrix();
            InitializeComponent();
            lblMessage.Text = "";
           
            this.BackColor = Color.FromArgb(230, 230, 230); ;
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.GetType().Equals(typeof(Button)))
                {
                    Button btn = ((Button)ctrl);//.BackColor = Color.Wheat;
                    btn.BackColor = Color.FromArgb(230, 230, 230);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 200, 225);
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(128, 128, 128);
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
            try
            {
                Scri();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid license key");

                Application.ExitThread();
                Application.Exit();
            }
            DirectoryInfo dir = new DirectoryInfo("Data/");
            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles();
                string strDataFile = "";
                if (files.Length > 0)
                {
                    strDataFile = files[0].Name;
                }
                string str = dir.FullName;
                tbHexRead.Text = str;
                // files[0].FullName;//= System.IO.Path.Combine("../../Data/", Environment.CurrentDirectory);
                //tbHexSave.Text = Path.Combine(Path.GetDirectoryName(tbHexRead.Text),
                //    Path.GetFileNameWithoutExtension(tbHexRead.Text) + "-rad" + Path.GetExtension(tbHexRead.Text));
                //tbExeRead.Text = tbHexRead.Text;
                //tbExeSave.Text = str + "Result.ang";
                //tbDisRead.Text = tbExeSave.Text;
                //tbDisSave.Text = str + "Result1.ang";
            }
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            btnExecute.Enabled = File.Exists(tbHexRead.Text);
            tbHexRead_TextChanged(null, null);
        }
        #endregion

        #region functions
        private void Distr(string strPath, string strSavePath)
        {
            try
            {
                lblMessage.Text = "Working@Please wait... ";
                DateTime timeStart = DateTime.Now;
                m.Save_NX(strPath, strSavePath, Convert.ToInt32(tbInterval.Text));
                Thread.Sleep(Sleep);

                lblMessage.Text = "Done@Success";
                TimeSpan diff = DateTime.Now - timeStart;
                double sec = diff.TotalMilliseconds;
                lblMessage.Text += "\r\n\r\nTime cost:" + sec.ToString("N2") + "ms";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Failed@" + ex.Message;
                lblMessage.Text += "\r\n\r\n Stack trace:\r\n" + ex.StackTrace;
            }
        }
        private void _4To6(string strPathSource, string strPathSave)
        {
            try
            {
                lblMessage.Text = "Working@Please wait... ";
                DateTime timeStart = DateTime.Now;

                string strSavePath;
                m._4to6(strPathSource, strPathSave);
                Thread.Sleep(Sleep);
                lblMessage.Text = "Converting Success";
                TimeSpan diff = DateTime.Now - timeStart;
                double sec = diff.TotalMilliseconds;
                lblMessage.Text += "\r\n\r\nTime cost:" + sec.ToString("N2") + "ms";
                //tbExeRead.Text = tbHexSave.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Failed@" + ex.Message;
                lblMessage.Text += "\r\n\r\n Stack trace:\r\n" + ex.StackTrace;
            }
        }
        private void Excute(string strPath, string strSave)
        {
            string strTempSavePath = "";
            try
            {
                lblMessage.Text = "Working@Please wait... ";
                DateTime timeStart = DateTime.Now;
                double sec;
                string result = Exe(strPath, strSave);
                //   tbDisRead.Text = strSave;

                TimeSpan diff = DateTime.Now - timeStart;
                sec = diff.TotalMilliseconds;
                lblMessage.Text = "Done@Success";
                lblMessage.Text += "\r\n\r\nTime cost:" + sec.ToString("N2") + "ms";
                lblResults.Text = result;
                lblResults.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Failed@" + ex.Message;
                lblMessage.Text += "\r\n\r\n Stack trace:\r\n" + ex.StackTrace;
            }
        }
        private string Exe(string strPath, string strSave1Path)
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


        #endregion

        #region events

        //private void treeNewBee_DoubleClick(object sender, EventArgs e)
        //{
        //    if (File.Exists(treeNewBee.SelectedNode.Tag.ToString()))
        //        System.Diagnostics.Process.Start(treeNewBee.SelectedNode.Tag.ToString());
        //}
        //private void treePop(string Path)
        //{
        //    DirectoryInfo dir = new DirectoryInfo(Path);
        //    TreeNode node = new TreeNode(dir.Name);
        //    TreeNode nodeSelect = node;
        //    node.Tag = dir.FullName;
        //    node.Name = node.Tag.ToString();

        //    while (dir.Parent != null && dir.Parent.Exists)
        //    {
        //        dir = dir.Parent;

        //        TreeNode nodeParent = new TreeNode(dir.Name);
        //        nodeParent.Tag = dir.FullName;
        //        nodeParent.Name = node.Tag.ToString();
        //        nodeParent.Nodes.Add((TreeNode)node.Clone());
        //        foreach (DirectoryInfo dirChilds in dir.GetDirectories())
        //        {
        //            TreeNode nodeTemp = new TreeNode(dirChilds.Name);
        //            nodeTemp.Tag = dirChilds.FullName;
        //            nodeTemp.Name = nodeTemp.Tag.ToString();
        //            if (nodeParent.Nodes.Find(nodeTemp.Name, false).Length == 0)
        //                nodeParent.Nodes.Add(nodeTemp);
        //        }
        //        foreach (FileInfo fileChilds in dir.GetFiles())
        //        {
        //            TreeNode nodeTemp = new TreeNode(fileChilds.Name);
        //            nodeTemp.Tag = fileChilds.FullName;
        //            nodeTemp.Name = nodeTemp.Tag.ToString();
        //            if (nodeParent.Nodes.Find(nodeTemp.Name, false).Length == 0)
        //                nodeParent.Nodes.Add(nodeTemp);
        //        }
        //        node = nodeParent;
        //        node.ExpandAll();
        //    }

        //    treeNewBee.Nodes.Add(node);
        //    treeNewBee.SelectedNode = nodeSelect;
        //}
        private void btnDis_Click(object sender, EventArgs e)
        {
            string strFiles = ((FileInfo)lbFiles.SelectedItem).FullName
            , strSave = strFiles.ToLower().Replace(".ang", "-result.ang");
            Distr(strFiles, strSave); 
            tbHexRead_TextChanged(null, null);
            for (int i = 0; i < lbFiles.Items.Count; i++)
            {
                if (((FileInfo)lbFiles.Items[i]).FullName.Equals(strSave, StringComparison.CurrentCultureIgnoreCase))
                    lbFiles.SelectedItem = lbFiles.Items[i];
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strFiles = ((FileInfo)lbFiles.SelectedItem).FullName;
            lblMessage.Text = m.Read_NX(strFiles, "", Convert.ToInt32(tbInterval.Text));
        }
        private void btnExecute_Click(object sender, EventArgs e)
        {
            string strFiles = ((FileInfo)lbFiles.SelectedItem).FullName
              , strSave = strFiles.ToLower().Replace(".ang", "-result.ang");
            Excute(strFiles, strSave);
            tbHexRead_TextChanged(null, null);
            for (int i = 0; i < lbFiles.Items.Count; i++)
            {
                if (((FileInfo)lbFiles.Items[i]).FullName.Equals(strSave, StringComparison.CurrentCultureIgnoreCase))
                    lbFiles.SelectedItem = lbFiles.Items[i];
            }
        }
        private void btnConvert_Click(object sender, EventArgs e)
        {
            string strFiles = ((FileInfo)lbFiles.SelectedItem).FullName
                , strSave = strFiles.ToLower().Replace(".ang", "-rad.ang");
            _4To6(strFiles, strSave);
            tbHexRead_TextChanged(null, null);
            for (int i = 0; i < lbFiles.Items.Count; i++)
            {
                if (((FileInfo)lbFiles.Items[i]).FullName.Equals(strSave, StringComparison.CurrentCultureIgnoreCase))
                    lbFiles.SelectedItem = lbFiles.Items[i];
            }
        }

        private void tbHexRead_TextChanged(object sender, EventArgs e)
        {
            lbFiles.Items.Clear();
            btnConvert.Enabled = false;
            btnDis.Enabled = false;
            btnExecute.Enabled = false;
            btnPreview.Enabled = false;
            DirectoryInfo dir = new DirectoryInfo(tbHexRead.Text);
            foreach (FileInfo fileChilds in dir.GetFiles())
            {
                lbFiles.Items.Add(fileChilds);             
            }
        }

        private void btnHexBrowse_Click(object sender, EventArgs e)
        {
            //"../../Data/"
            folderBrowserDialog1.SelectedPath = tbHexRead.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbHexRead.Text = folderBrowserDialog1.SelectedPath;
            }
            //openFileDialog1.InitialDirectory = tbHexRead.Text;
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //    tbHexRead.Text = openFileDialog1.FileName;
        }
 
        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblHex.Text = ((FileInfo)lbFiles.SelectedItem).FullName; ;
            btnConvert.Enabled = true;
            btnDis.Enabled = true;
            btnExecute.Enabled = true;
            btnPreview.Enabled = true;
        }

        #endregion


    }
}
