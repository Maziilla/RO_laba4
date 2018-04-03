using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba1
{
    public partial class Form1 : Form
    {
        DataSet Learn,Test;

        public void add_animal(DataTable table, double a1, double a2, double a3, double a4, string a5)
        {
            table.Rows.Add(a5, a1, a2, a3, a4);
        }      
      
        public void add_col(DataTable table)
        {
            DataColumn col_bee =
               table.Columns.Add("name", typeof(string));
            table.Columns.Add("sepal length", typeof(double));
            table.Columns.Add("sepal width", typeof(double));
            table.Columns.Add("petal length", typeof(double));
            table.Columns.Add("petal width", typeof(double));
        }
        public double Evkl_dist(DataRow row, double[] row1)
        {
            return Math.Sqrt(Math.Pow((double)row.ItemArray[1] - row1[0], 2) + Math.Pow((double)row.ItemArray[2] - row1[1], 2) + Math.Pow((double)row.ItemArray[3] -
                row1[2], 2) + Math.Pow((double)row.ItemArray[4] - row1[3], 2));
        }
        public double Evkl_dist(DataRow row, DataRow row1)
        {
            return Math.Sqrt(Math.Pow((double)row.ItemArray[1] - (double)row1.ItemArray[1], 2) + Math.Pow((double)row.ItemArray[2] - (double)row1.ItemArray[2], 2) +
                Math.Pow((double)row.ItemArray[3] - (double)row1.ItemArray[3], 2) + Math.Pow((double)row.ItemArray[4] - (double)row1.ItemArray[4], 2));
        }
        public int Choise(DataRow Row)
        {
            double min_dist = Double.MaxValue, dist = 0;
            string Name = "";
            foreach (DataTable tabl in Learn.Tables)
            {
                foreach (DataRow row in tabl.Rows)
                {
                    dist = Evkl_dist(row,Row);
                    
                    if (dist < min_dist)
                    {
                        min_dist = dist;
                        Name = (string)row.ItemArray[0];
                    }

                }
            }

            if ((string)Row.ItemArray[0] == Name && (string)Row.ItemArray[0] == "Iris - versicolor")
                return 1;
            else if ((string)Row.ItemArray[0] == Name)
                return 2;
            else
                if ((string)Row.ItemArray[0] == "Iris-versicolor")
                return 3;//1 род
            else
                return 4;//2 род
            //l_name.Text = Name;
        }
        //table.Rows.Add(nUD_pol.Value, nUD_hight.Value, nUD_long.Value);
        public Form1()
        {
            InitializeComponent();
            cb_rast.SelectedIndex = 0;
            Test = new DataSet("Test");
            Learn = new DataSet("Learn");
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
           StreamReader textReader;
            string sLine = "";
            string[] split;
            double[] znach = new double[4];
            double Fn=0, Fp=0, Right=0,tn=0,tp=0;
            int typeEr;
            int j;
            double TPR;
            double FPR;
            Test.Tables.Clear();
            Learn.Tables.Clear();
            Learn.Tables.Add("1");
            Test.Tables.Add("1");
            add_col(Learn.Tables[0]);
            add_col(Test.Tables[0]);
            if (cb_rast.SelectedIndex == 0)
            //------------------------------------------------------------------------------------------------------
            {
                Test.Clear();
                Learn.Clear();
                Test.Tables.Clear();
                Learn.Tables.Clear();
                textReader = new StreamReader("J:\\Универ\\3 курс\\2 триместр\\РО\\Laba1\\Laba1\\dataset.txt");
                sLine = "";
                Learn.Tables.Add("1");
                Test.Tables.Add("1");
                add_col(Learn.Tables[0]);
                add_col(Test.Tables[0]);

                textReader.ReadLine();

                j = 0;
                while ((sLine = textReader.ReadLine()) != null)
                {
                    j++;
                    split = sLine.Split('\t');
                    for (int i = 0; i < znach.Length; i++)
                    {
                        znach[i] = Convert.ToDouble(split[i]);
                    }
                    if (j % numericUpDown1.Value == 0)
                        add_animal(Test.Tables[0], znach[0], znach[1], znach[2], znach[3], split[4]);
                    else
                        add_animal(Learn.Tables[0], znach[0], znach[1], znach[2], znach[3], split[4]);
                }
                

                for (double T = 0; T < 45; T += 0.1)
                {
                    Fn = Fp = Right = tn = tp = 0;
                    foreach (DataRow row in Test.Tables[0].Rows)
                    {
                        double min_dist = Double.MaxValue, dist = 0;
                        string Name = "";
                        foreach (DataTable tabl in Learn.Tables)
                        {
                            foreach (DataRow row1 in tabl.Rows)
                            {
                                dist = Evkl_dist(row1, row);

                                if (dist < min_dist)
                                {
                                    min_dist = dist;
                                    Name = (string)row1.ItemArray[0];
                                }

                            }
                        }

                        if(min_dist > T)
                        {
                            if ((string)row.ItemArray[0] == Name)
                                tn++;
                            else
                                Fp++;
                        }
                        else
                        {
                            if ((string)row.ItemArray[0] == Name)
                                tp++;
                            else
                                Fn++;
                        }
                        //typeEr = Choise(row);
                        //switch (typeEr)
                        //{
                        //    case 1: tn++; break;
                        //    case 2: tp++; break;
                        //    case 3: Fn++; break;
                        //    case 4: Fp++; break;
                        //}
                    }
                    //TPR = tp / (tp + Fp + 1e-15);
                    //FPR = tn / (tp + Fn + 1e-15);
                    //chart1.Series[0].Points.AddXY(1 - TPR, FPR);
                    TPR = tp / (tp + Fn + 1e-15);
                    FPR = tn / (tp + Fp + 1e-15);
                    chart1.Series[0].Points.AddXY(FPR, TPR);
                }
            }
            //------------------------------------------------------------------------------------------
            else
            {
                for (double T = 0; T < 45; T += 0.1)
                {  Fn = Fp = Right = tn = tp = 0;
                    sLine = "";
                Fn = Fp = Right = tn = tp = 0;
                for (int k = 0; k < numericUpDown1.Value; k++)
                {
                    Test.Tables[0].Rows.Clear();
                    Learn.Tables[0].Rows.Clear();
                    textReader = new StreamReader("J:\\Универ\\3 курс\\2 триместр\\РО\\Laba1\\Laba1\\dataset.txt");
                    textReader.ReadLine();

                    j = 0;
                    while ((sLine = textReader.ReadLine()) != null)
                    {
                        j++;
                        split = sLine.Split('\t');
                        for (int i = 0; i < znach.Length; i++)
                        {
                            znach[i] = Convert.ToDouble(split[i]);
                        }
                        if (j % numericUpDown1.Value == k)
                            add_animal(Test.Tables[0], znach[0], znach[1], znach[2], znach[3], split[4]);
                        else
                            add_animal(Learn.Tables[0], znach[0], znach[1], znach[2], znach[3], split[4]);
                    }

                    
                        foreach (DataRow row in Test.Tables[0].Rows)
                        {
                            double min_dist = Double.MaxValue, dist = 0;
                            string Name = "";
                            foreach (DataTable tabl in Learn.Tables)
                            {
                                foreach (DataRow row1 in tabl.Rows)
                                {
                                    dist = Evkl_dist(row1, row);

                                    if (dist < min_dist)
                                    {
                                        min_dist = dist;
                                        Name = (string)row1.ItemArray[0];
                                    }

                                }
                            }

                            if (min_dist > T)
                            {
                                if ((string)row.ItemArray[0] == Name)
                                    tn++;
                                else
                                    Fp++;
                            }
                            else
                            {
                                if ((string)row.ItemArray[0] == Name)
                                    tp++;
                                else
                                    Fn++;
                            }
                        }
                    }
                    tp /= (int)numericUpDown1.Value;
                    tn /= (int)numericUpDown1.Value;
                    Fn /= (int)numericUpDown1.Value;
                    Fp /= (int)numericUpDown1.Value;
                    TPR = tp / (tp + Fn + 1e-15);
                    FPR = tn / (tp + Fp + 1e-15);
                    chart1.Series[0].Points.AddXY(FPR, TPR);
                }
            }
            //------------------------------------------------------------------------------------------
            tb_FEr.Text = Fn.ToString();
            tb_SEr.Text = Fp.ToString();
            tb_Ri.Text = Right.ToString();
            //Choise();
        }


    }
}
