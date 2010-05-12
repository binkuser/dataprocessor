using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace Matrix
{
    class Matrix
    {
        StreamWriter sw;
        #region public
        public void _4to6(string strPaths, string strSavePath)
        {
            string strDataFileName = strPaths;//System.IO.Path.Combine(strPaths, "2-2AC1-1.ang");
            //string strOrignfileName = Path.GetFileNameWithoutExtension(strPaths);
            //strSavePath = Path.Combine(Path.GetDirectoryName(strPaths), Path.GetFileNameWithoutExtension(strPaths) + "-rad" + Path.GetExtension(strPaths));
            StreamReader sr = new StreamReader(strDataFileName);
            if (File.Exists(strSavePath))
            {
                if (DialogResult.OK != MessageBox.Show(strSavePath + " already exists.\r\n Overwrite?", "waring", MessageBoxButtons.OKCancel))
                    throw new FileLoadException("User canceled.");
            }
            sw = new StreamWriter(strSavePath);
            float fPi = Convert.ToSingle(Math.PI / 180.0);
            while (!sr.EndOfStream)
            {
                float fPosX, fPosY;
                float[] A = new float[3];

                float IQ;

                string strLine = sr.ReadLine();//read a line
                if (strLine.IndexOf("#") >= 0)
                {
                    sw.WriteLine(strLine);
                    continue;//is a comment, skip.
                }
                else
                {
                    float[] Pars =
                        ExtractPars(strLine);//get the parameters we need.

                    A[0] = Pars[4] * fPi;
                    A[1] = Pars[5] * fPi;
                    A[2] = Pars[6] * fPi;
                    fPosX = Pars[2];
                    fPosY = Pars[3];
                    IQ = Pars[8];
                    sw.WriteLine(
                   A[0].ToString().PadLeft(10)
                   + A[1].ToString().PadLeft(10)
                   + A[2].ToString().PadLeft(10)
                   + fPosX.ToString().PadLeft(13)
                   + fPosY.ToString().PadLeft(13)
                   + IQ.ToString().PadLeft(10)
                        );
                }
            }
            sr.Dispose();
            sw.Dispose();
        }
        
        public void Read_File(string strPaths, string strSavePath, out float IQMax, out float IQMin, out int DNum, out int Nx)
        {
            string strDataFileName = strPaths;//System.IO.Path.Combine(strPaths, "2-2AC1-1.ang");

            if (File.Exists(strSavePath))
            {
                if (DialogResult.OK != MessageBox.Show(strSavePath + " already exists.\r\nOverwrite?", "waring", MessageBoxButtons.OKCancel))
                    throw new FileLoadException("User canceled.");
            }
            StreamReader sr = new StreamReader(strDataFileName);
            sw = new StreamWriter(strSavePath);
            float initialY = -1;
            int iRow = -1
                , iCol = 0;
            Boolean isEndOfPile = false;
            DataTable lineOld = new DataTable()
                , lineNew;
            lineOld.Columns.Add(new DataColumn("strLine", typeof(System.String)));
            lineOld.Columns.Add(new DataColumn("x", typeof(System.Single)));
            lineOld.Columns.Add(new DataColumn("matrixA", typeof(System.Single[,])));
            lineOld.Columns.Add(new DataColumn("boolG", typeof(System.Boolean)));
            lineNew = lineOld.Clone();
            IQMax = 0;
            IQMin = 0;
            DNum = 0; 
            Nx = 0;
            while (!sr.EndOfStream)
            {
                float fPosX, fPosY;
                float[] A = new float[3];

                float IQ;

                string strLine = sr.ReadLine();//read a line
                if (strLine.IndexOf("#") >= 0) continue;//is a comment, skip.


                DNum++;
                float[] Pars =
                    ExtractPars(strLine);//get the parameters we need.

                A[0] = Pars[0];
                A[1] = Pars[1];
                A[2] = Pars[2];
                fPosX = Pars[3];
                fPosY = Pars[4];
                IQ = Pars[5];
                if (A[0] == 0.77721f)
                {
 
                }
                if (IQ > IQMax) IQMax = IQ;
                if (IQMin == 0 || IQ < IQMin) IQMin = IQ;

                if (initialY != fPosY)//is the End Of a Row? same Y.
                {
                    if (iCol > Nx) Nx = iCol;
                    initialY = fPosY;
                    SaveRow(lineOld);//save old
                    //if (lineNew.Rows.Count > iCol + 1)
                    //    lineNew.Rows[iCol + 1].Delete();
                    lineOld = lineNew.Copy();//give up Old row. 
                    lineNew.Clear();
                    iRow++; iCol = 0;//next Row start.
                }
                else
                {
                    iCol++;//next column.
                }

                if (lineNew.Rows.Count == iCol)
                    lineNew.Rows.Add(lineNew.NewRow());
                float[,] mA = getMatrixA(A);
                lineNew.Rows[iCol].ItemArray = new object[] { strLine, fPosX, mA, true };//save node;
                float[,] mB = getMatrixB(mA);

                if (iRow == 0 && iCol == 5)
                {
 
                }

                bool G,GOrgin, GCurrent = false;
                float[,] mC;
                if (iCol > 0)
                {
                    //a compare
                    //compare to privous column.
                    mA = (float[,])lineNew.Rows[iCol - 1]["matrixA"];
                    GOrgin = (bool)lineNew.Rows[iCol - 1]["boolG"];
                    mC = getMatrixC(mA, mB);
                    G = getG(mC);
                    GOrgin = GOrgin || G;
                    GCurrent = GCurrent || G;
                    //G = compute(lineNew.Rows[iCol - 1], mB);
                    lineNew.Rows[iCol - 1]["boolG"] = GOrgin;
                    ///asg 3.27
                }

                int nextCol = iCol;
                if (lineOld.Rows.Count > iCol)
                {
                    //a compare
                    //compare to one has the same column index: 
                    mA = (float[,])lineOld.Rows[iCol]["matrixA"];
                    GOrgin = (bool)lineOld.Rows[iCol]["boolG"];
                    mC = getMatrixC(mA, mB);
                    G = getG(mC);
                    GOrgin = GOrgin || G;
                    GCurrent = GCurrent || G;
                    lineOld.Rows[iCol]["boolG"] = GOrgin;

                    float oldPosX = (float)lineOld.Rows[iCol]["X"];
                    if (oldPosX < fPosX)//if same col has a smaller X.
                    {
                        nextCol++;//then it must has the next col.
                    }
                    else if (oldPosX > fPosX)//if same col has a lager x
                    {
                        nextCol--;//compare to previous one.
                    }
                    else// same col same x, square mode.
                    {
                        nextCol = -1;//the end.
                    }
                }
                else //there is not a column in the above this node. maybe: it's the end of an odd line, or it's the first line.
                {
                    nextCol--;//compare to previous col.
                }

                if (nextCol >= 0 && lineOld.Rows.Count > nextCol)
                {
                    //a compare
                    mA = (float[,])lineOld.Rows[nextCol]["matrixA"];
                    GOrgin = (bool)lineOld.Rows[nextCol]["boolG"];
                    mC = getMatrixC(mA, mB);
                    G = getG(mC);
                    GOrgin = GOrgin || G;
                    GCurrent = GCurrent || G;
                    lineOld.Rows[nextCol]["boolG"] = GOrgin;
                    //
                }
                lineNew.Rows[iCol]["boolG"] = GCurrent;
            }
            SaveRow(lineOld);
            SaveRow(lineNew);
            sr.Dispose();
            sw.Dispose();
        }

        public void Read_NIQ(string strPaths, string strSavePaths,float IQMax, float IQMin)
        {
            float IQdiff= (IQMax - IQMin );
            string strDataFileName = strPaths;// System.IO.Path.Combine(Path.GetPathRoot(strPaths), "Result.ang");//System.IO.Path.Combine(strPaths, "2-2AC1-1.ang");
            if (File.Exists(strSavePaths))
            {
                if (DialogResult.OK != MessageBox.Show(strSavePaths + " already exists.\r\nOverwrite?", "waring", MessageBoxButtons.OKCancel))
                    throw new FileLoadException("User canceled.");
            }
            sw = new StreamWriter(strSavePaths);
            StreamReader sr;
            //write G>0
            sr = new StreamReader(strDataFileName);
            while (!sr.EndOfStream)
            {
                float[] A = new float[3];
                float IQ;
                bool G;

                string strLine = sr.ReadLine();//read a line
                if (strLine.IndexOf("#") >= 0) continue;//is a comment, skip.


                float[] Pars =
                    ExtractPars(strLine);//get the parameters we need.

                A[0] = Pars[0];
                A[1] = Pars[1];
                A[2] = Pars[2];
                IQ = Pars[5];
                G = Pars[Pars.Length - 1] > 0;
                if (((A[0] + A[1] + A[2]) > 0)
                    && !G)
                {
                    float N_IQ = 100 * (IQ - IQMin) / IQdiff;
                    sw.WriteLine(strLine + " " + N_IQ.ToString("0.000000"));
                }
            }
            sr.Dispose();
            sr = new StreamReader(strDataFileName);
            while (!sr.EndOfStream)
            {
                float[] A = new float[3];
                float IQ;
                bool G;

                string strLine = sr.ReadLine();//read a line
                if (strLine.IndexOf("#") >= 0) continue;//is a comment, skip.


                float[] Pars =
                    ExtractPars(strLine);//get the parameters we need.

                A[0] = Pars[0];
                A[1] = Pars[1];
                A[2] = Pars[2];
                IQ = Pars[5];
                G = Pars[Pars.Length - 1] > 0;
                if (((A[0] + A[1] + A[2]) > 0)
                    && G)
                {
                    float N_IQ = 100 * (IQ - IQMin) / IQdiff;
                    sw.WriteLine(strLine + " " + N_IQ.ToString("0.000000"));
                }
            }
            sr.Dispose();
            //write xyz=000
            sr = new StreamReader(strDataFileName);
            while (!sr.EndOfStream)
            {
                float[] A = new float[3];
                float IQ;
                bool G;

                string strLine = sr.ReadLine();//read a line
                if (strLine.IndexOf("#") >= 0) continue;//is a comment, skip.


                float[] Pars =
                    ExtractPars(strLine);//get the parameters we need.

                A[0] = Pars[0];
                A[1] = Pars[1];
                A[2] = Pars[2];
                IQ = Pars[5];
                if ((A[0] + A[1] + A[2]) == 0)
                {
                    float N_IQ = 100 * (IQ - IQMin) / IQdiff;
                    sw.WriteLine(strLine + " " + N_IQ.ToString("0.000000"));
                }
            }
            sr.Dispose();

            sw.Dispose();
        }

        public string Read_NX(string strPaths, string strSavePaths, float x)
        {
            int DNum = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("N-X", typeof(System.Single)));
            dt.Columns.Add(new DataColumn("Y-Freq", typeof(System.Single)));
            dt.Columns.Add(new DataColumn("N-num", typeof(System.Int32)));
            for (int i =1; i <= 24; i++)
            {
                DataRow dr = dt.NewRow();
                dr["N-X"] = i * x - 2.5;
                dr["N-num"] = 0;
                dt.Rows.Add(dr);
            }
            string strDataFileName = strPaths;
            
            StreamReader sr = new StreamReader(strDataFileName);
            while (!sr.EndOfStream)
            {
                string strLine = sr.ReadLine();//read a line
                if (strLine.IndexOf("#") >= 0) continue;//is a comment, skip.
                float[] Pars =
                  ExtractPars(strLine);//get the parameters we need.
                DNum++;
                float N_IQ = Pars[Pars.Length - 1];

                //int i = Convert.ToInt32(Math.Floor((N_IQ + 2.5) / x));
                //dt.Rows[i]["N-num"] = (int)dt.Rows[i]["N-num"] + 1;
                for (int i = 0; i < 24; i++)
                {
                    if ((float)dt.Rows[i]["N-X"] > N_IQ)
                    {
                        dt.Rows[i]["N-num"] = (int)dt.Rows[i]["N-num"] + 1;
                        break;
                    }
                }
            }
            sr.Dispose();
            string strRet = @"Selected Interval =		" + x.ToString("0") + "\r\nN-X	Y-Freq 	N-num\r\n";
            for (int i = 0; i < 24; i++)
            {
                dt.Rows[i]["Y-Freq"] = (int)dt.Rows[i]["N-num"] / Convert.ToSingle(DNum);
                strRet += ((float)dt.Rows[i]["N-X"]).ToString("0.0").PadLeft(5).PadRight(10)
                    + dt.Rows[i]["Y-Freq"].ToString().PadRight(15)
                    + dt.Rows[i]["N-num"].ToString().PadLeft(5)
                    + "\r\n";
            }
            return strRet;
        }

        public void Save_NX(string strPaths, string strSavePaths, float x)
        {
            if (File.Exists(strSavePaths))
            {
                if (DialogResult.OK != MessageBox.Show(strSavePaths + " already exists.\r\nOverwrite?", "waring", MessageBoxButtons.OKCancel))
                    throw new FileLoadException("User canceled.");
                File.Delete(strSavePaths);
            }
            sw = new StreamWriter(strSavePaths);
            string str = Read_NX(strPaths, strSavePaths, x);
            str += Read_NX(strPaths, strSavePaths, x - 1);
            str += Read_NX(strPaths, strSavePaths, x - 2);
            str += Read_NX(strPaths, strSavePaths, x + 1);
            str += Read_NX(strPaths, strSavePaths, x + 2);
            sw.Write(str);
            sw.Dispose();
        }
        #endregion

        #region protected
        /// <summary>
        /// Get the matrix by X,Y,Z
        /// </summary>
        /// <param name="A">Array of x,y,z</param>
        /// <returns></returns>
        protected float[,] getMatrixA(float[] A)
        {
            float[,] ret = new float[3, 3];
            float SinX = Convert.ToSingle(Math.Sin(A[0])), CosX = Convert.ToSingle(Math.Cos(A[0]));
            float SinY = Convert.ToSingle(Math.Sin(A[1])), CosY = Convert.ToSingle(Math.Cos(A[1]));
            float SinZ = Convert.ToSingle(Math.Sin(A[2])), CosZ = Convert.ToSingle(Math.Cos(A[2]));

            
            ret[0, 0] = CosX * CosZ - SinX * SinZ * CosY;//ret[0, 0] = SinX * CosZ - SinX * SinZ * CosY;///asg 3.27
            ret[0, 1] = SinX * CosZ + CosX * SinZ * CosY;
            ret[0, 2] = SinZ * SinY;

            ret[1, 0] = -1 * CosX * SinZ - SinX * CosZ * CosY;
            ret[1, 1] = -1 * SinX * SinZ + CosX * CosZ * CosY;//ret[1, 1] = -1 * SinX * SinY + CosX * CosZ * CosY;///asg 3.27
            ret[1, 2] = CosZ * SinY;

            ret[2, 0] = SinX * SinY;
            ret[2, 1] = -1 * CosX * SinY;
            ret[2, 2] = CosY;
            return ret;
        }

        protected float[,] getMatrixB(float[,] matrixA)
        {
            float m
                = matrixA[0, 0] * (matrixA[1, 1] * matrixA[2, 2] - matrixA[2, 1] * matrixA[1, 2])
                + matrixA[0, 1] * (matrixA[1, 2] * matrixA[2, 0] - matrixA[1, 0] * matrixA[2, 2])
                + matrixA[0, 2] * (matrixA[1, 0] * matrixA[2, 1] - matrixA[1, 1] * matrixA[2, 0]);
            float[,] ret = new float[3, 3];
            ret[0, 0] = (matrixA[1, 1] * matrixA[2, 2] - matrixA[1, 2] * matrixA[2, 1]) / m;
            ret[0, 1] = (matrixA[2, 1] * matrixA[0, 2] - matrixA[0, 1] * matrixA[2, 2]) / m;
            ret[0, 2] = (matrixA[0, 1] * matrixA[1, 2] - matrixA[0, 2] * matrixA[1, 1]) / m;
            ret[1, 0] = (matrixA[1, 2] * matrixA[2, 0] - matrixA[1, 0] * matrixA[2, 2]) / m;
            ret[1, 1] = (matrixA[2, 2] * matrixA[0, 0] - matrixA[0, 2] * matrixA[2, 0]) / m;
            ret[1, 2] = (matrixA[0, 2] * matrixA[1, 0] - matrixA[0, 0] * matrixA[1, 2]) / m;
            ret[2, 0] = (matrixA[1, 0] * matrixA[2, 1] - matrixA[1, 1] * matrixA[2, 0]) / m;
            ret[2, 1] = (matrixA[2, 0] * matrixA[0, 1] - matrixA[0, 0] * matrixA[2, 1]) / m;
            ret[2, 2] = (matrixA[0, 0] * matrixA[1, 1] - matrixA[0, 1] * matrixA[1, 0]) / m;
            return ret;
        }
        protected float[,] getMatrixC(float[,] matrixA, float[,] matrixB)
        {
            return maltiply(matrixA, matrixB);
        }
        protected float[,] maltiply(float[,] matrixA, float[,] matrixB)
        {
            float[,] ret = new float[3, 3];
            for (int iRow = 0; iRow < 3; iRow++)
            {
                for (int iCol = 0; iCol < 3; iCol++)
                {
                    float fValue = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        fValue += matrixA[iRow, i] * matrixB[i, iCol];
                    }
                    ret[iRow, iCol] = fValue;
                }
            }
            return ret;
        }

        protected float[,] Transpose(float[,] matrixC)
        {
            float[,] matrixA = new float[matrixC.GetLength(1), matrixC.GetLength(0)];
            for (int i = 0; i < matrixC.GetLength(0); i++)
            {
                for (int j = 0; j < matrixC.GetLength(1); j++)
                {
                    matrixA[j, i] = matrixC[i, j];
                }
            }
            return matrixA;
        }

        protected bool getG(float[,] matrixC)
        {// every F's value is above 0.785 == 1.minimum F is above 0.785
            bool ret = true;
            //asg 3.27
            /*
            float[,] matrixA = Transpose(matrixC);
            ret = ret & getF(matrixA[0, 0] + matrixA[1, 1] + matrixA[2, 2]);//1
            ret = ret & getF(matrixA[0, 0] - matrixA[1, 1] - matrixA[2, 2]);//2
            ret = ret & getF(matrixA[0, 1] + matrixA[1, 2] + matrixA[2, 0]);//3
            ret = ret & getF(-matrixA[0, 1] - matrixA[1, 2] + matrixA[2, 0]);//4
            //ret = ret & getF(-matrixA[0, 2] - matrixA[1, 0] - matrixA[2, 0]);//??
            ret = ret & getF(-matrixA[0, 2] - matrixA[1, 1] - matrixA[2, 0]);//5asg 
            ret = ret & getF(matrixA[0, 0] + matrixA[1, 2] - matrixA[2, 1]);//6
            ret = ret & getF(-matrixA[0, 2] - matrixA[1, 0] + matrixA[2, 1]);//7
            ret = ret & getF(matrixA[0, 1] + matrixA[1, 0] - matrixA[2, 2]);//8
            ret = ret & getF(-matrixA[0, 2] + matrixA[1, 1] + matrixA[2, 0]);//9
            ret = ret & getF(-matrixA[0, 1] + matrixA[1, 0] + matrixA[2, 2]);//10
            ret = ret & getF(-matrixA[0, 1] + matrixA[1, 2] - matrixA[2, 0]);//11
            ret = ret & getF(-matrixA[0, 0] + matrixA[1, 2] + matrixA[2, 1]);//12
            //ret = ret & getF(-matrixA[0, 0] - matrixA[1, 1] - matrixA[2, 2]);//??
            ret = ret & getF(-matrixA[0, 0] + matrixA[1, 1] - matrixA[2, 2]);//13asg
            ret = ret & getF(-matrixA[0, 0] - matrixA[1, 1] + matrixA[2, 2]);//14
            ret = ret & getF(matrixA[0, 1] - matrixA[1, 2] - matrixA[2, 0]);//15
            //ret = ret & getF(-matrixA[0, 2] - matrixA[1, 0] - matrixA[2, 0]);//??
            ret = ret & getF(matrixA[0, 2] - matrixA[1, 1] + matrixA[2, 0]);//16asg
            ret = ret & getF(matrixA[0, 2] + matrixA[1, 1] - matrixA[2, 0]);//17
            ret = ret & getF(matrixA[0, 1] - matrixA[1, 0] + matrixA[2, 2]);//18
            ret = ret & getF(-matrixA[0, 2] + matrixA[1, 0] - matrixA[2, 1]);//19
            ret = ret & getF(-matrixA[0, 1] - matrixA[1, 0] - matrixA[2, 2]);//20
            ret = ret & getF(matrixA[0, 0] - matrixA[1, 2] + matrixA[2, 1]);//21
            ret = ret & getF(matrixA[0, 2] + matrixA[1, 0] + matrixA[2, 1]);//22
            ret = ret & getF(matrixA[0, 2] - matrixA[1, 0] - matrixA[2, 1]);//23
            ret = ret & getF(-matrixA[0, 0] - matrixA[1, 2] - matrixA[2, 1]);//24
         */
            /*
   
            */       
            Object[] MatrixDs = new Object[24];

            MatrixDs[01] = new float[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            MatrixDs[02] = new float[3, 3] { { 1, 0, 0 }, { 0, -1, 0 }, { 0, 0, -1 } };
            MatrixDs[03] = new float[3, 3] { { 0, 1, 0 }, { 0, 0, 1 }, { 1, 0, 0 } };
            MatrixDs[04] = new float[3, 3] { { 0, -1, 0 }, { 0, 0, -1 }, { 1, 0, 0 } };
            MatrixDs[05] = new float[3, 3] { { 0, 0, -1 }, { 0, -1, 0 }, { -1, 0, 0 } };
            MatrixDs[06] = new float[3, 3] { { 1, 0, 0 }, { 0, 0, 1 }, { 0, -1, 0 } };
            MatrixDs[07] = new float[3, 3] { { 0, 0, -1 }, { -1, 0, 0 }, { 0, 1, 0 } };
            MatrixDs[08] = new float[3, 3] { { 0, 1, 0 }, { 1, 0, 0 }, { 0, 0, -1 } };
            MatrixDs[09] = new float[3, 3] { { 0, 0, -1 }, { 0, 1, 0 }, { 1, 0, 0 } };
            MatrixDs[10] = new float[3, 3] { { 0, -1, 0 }, { 1, 0, 0 }, { 0, 0, 1 } };
            MatrixDs[11] = new float[3, 3] { { 0, -1, 0 }, { 0, 0, 1 }, { -1, 0, 0 } };
            MatrixDs[12] = new float[3, 3] { { -1, 0, 0 }, { 0, 0, 1 }, { 0, 1, 0 } };
            MatrixDs[13] = new float[3, 3] { { -1, 0, 0 }, { 0, 1, 0 }, { 0, 0, -1 } };
            MatrixDs[14] = new float[3, 3] { { -1, 0, 0 }, { 0, -1, 0 }, { 0, 0, 1 } };
            MatrixDs[15] = new float[3, 3] { { 0, 1, 0 }, { 0, 0, -1 }, { -1, 0, 0 } };
            MatrixDs[16] = new float[3, 3] { { 0, 0, 1 }, { 0, -1, 0 }, { 1, 0, 0 } };
            MatrixDs[17] = new float[3, 3] { { 0, 0, 1 }, { 0, 1, 0 }, { -1, 0, 0 } };
            MatrixDs[18] = new float[3, 3] { { 0, 1, 0 }, { -1, 0, 0 }, { 0, 0, 1 } };
            MatrixDs[19] = new float[3, 3] { { 0, 0, -1 }, { 1, 0, 0 }, { 0, -1, 0 } };
            MatrixDs[20] = new float[3, 3] { { 0, -1, 0 }, { -1, 0, 0 }, { 0, 0, -1 } };
            MatrixDs[21] = new float[3, 3] { { 1, 0, 0 }, { 0, 0, -1 }, { 0, 1, 0 } };
            MatrixDs[22] = new float[3, 3] { { 0, 0, 1 }, { 1, 0, 0 }, { 0, 1, 0 } };
            MatrixDs[23] = new float[3, 3] { { 0, 0, 1 }, { -1, 0, 0 }, { 0, -1, 0 } };
            MatrixDs[00] = new float[3, 3] { { -1, 0, 0 }, { 0, 0, -1 }, { 0, -1, 0 } };

            foreach (object objD in MatrixDs)
            {
                float[,] MatrixD = (float[,])objD;
                float[,] MatrixE = maltiply(matrixC, MatrixD);
                ret = ret && getF(MatrixE[0, 0] + MatrixE[1, 1] + MatrixE[2, 2]);
            }



            if (!ret)
            {

            }
            else
            {

            }
            return ret;
        }

        protected bool getF(double Hn)
        {
            double fThis;
            fThis = Math.Acos((Math.Abs(Hn) - 1) / 2);
            bool ret;
            if (fThis == double.NaN)
                ret = true;
            else
                ret = fThis > 0.26179938779914941;
            if (ret)
            {

            }
            else
            {
 
            }
            return ret;
        }
        #endregion

        #region private
        /// <summary>
        /// Write the Result Row to file.
        /// </summary>
        /// <param name="htOld">data to write</param>
        private void SaveRow(DataTable htOld)
        {
            //throw new NotImplementedException();
            for (int i = 0; i < htOld.Rows.Count; i++)
            {
                sw.WriteLine((string)htOld.Rows[i]["strLine"] + " " + (((bool)htOld.Rows[i]["boolG"]) ? "1" : "0"));
            }
        }

        /// <summary>
        /// Get data by string
        /// </summary>
        /// <param name="strLine">Line string</param>
        /// <returns></returns>
        private float[] ExtractPars(string strLine)
        {
            string[] strPars = strLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            float[] ret = new float[strPars.Length];
            for (int i = 0; i < strPars.Length; i++)
            {
                ret[i] = Convert.ToSingle(strPars[i]);
            }
            return ret;
        }
        #endregion

    }



    //class Node_Line
    //{
    //    float[,] matrixA;
    //}
}
