using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        int my, mx, k;
        Graphics g;
        long jp, jg, cg;
        double pc, pm, th, xa, xb, ya, yb, pk;
        ListViewItem lv;
        double [] pop = new double[1];
        double[] pop1 = new double[1];
        double[] pop2 = new double[1];
        double[] pop3 = new double[1];
        double[] pop4 = new double[1];
        double[] pop5 = new double[1];
        Random r = new Random();
        int[][] clus = new int[1][];
        
        public Form1()
        {
            InitializeComponent();
                            
        }

        int ex(double x) {
            return (int)Math.Floor((xa + x) * 350 / (xa - xb));
        }

        int ey(double y) {
            return (int)Math.Floor((ya + y) * 350 / (ya - yb));
        }

        double fitkrom(double x, double y)
        {
            return Math.Round(19+x*Math.Sin(x*Math.PI)+(10-y)*Math.Sin(y*Math.PI),k);
        }
                
        double N() { 
            double r1,r2;
            r1 = r.NextDouble();
            r2 = r.NextDouble();
            return Math.Round(Math.Sqrt(-2*Math.Log(r1))*Math.Sin(2*Math.PI*r2),k);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            k =5;
            pb1.Minimum = 0;
            jp = Convert.ToInt32(jpop.Text);
            jg = Convert.ToInt32(jgen.Text);
            pb1.Maximum = (int)jg +1;
            pb1.Value = 0;
            Array.Resize(ref pop,(int)jp);
            Array.Resize(ref pop1,(int)jp);
            Array.Resize(ref pop2, (int)jp);
            Array.Resize(ref pop3, (int)jp);
            Array.Resize(ref pop4, (int)jp*7);
            Array.Resize(ref pop5, (int)jp*7);
            
            double[] fit = new double[jp];
            double[] fit1= new double[jp*7];
            xa = Double.Parse (x1.Text);
            xb = Convert.ToDouble(x2.Text);
            ya = Convert.ToDouble(y1.Text);
            yb = Convert.ToDouble(y2.Text);
            cg = -1;
            
            //populasi awal
            
            for (int j = 0; j < jp;j++ ){
                //for (int i = 0; i < mx + my; i++) 
                pop[j] = Math.Round(r.Next(Convert.ToInt32(xb * Math.Pow(10, k)), Convert.ToInt32(xa * Math.Pow(10, k)))/
                    Math.Pow(10,k),k);
                pop1[j] = Math.Round(r.Next(Convert.ToInt32(yb * Math.Pow(10, k)), Convert.ToInt32(ya * Math.Pow(10, k)))/
                    Math.Pow(10,k),k);
                pop2[j] = Math.Round(r.NextDouble(),k);
                pop3[j] = Math.Round(r.NextDouble(),k);
            }
            
            for (int i = 0; i < jp; i++){
                fit[i] = fitkrom(pop[i],pop1[i]);
            }
   
            cg++;
            Array.Sort(fit);
            while ((cg<=jg) && (pk<=th) ){
                for (int i = 0; i < jp;i++)
                {
                    int z = 0;
                    for (int j = 0; j < 7; j++)
                    {
                        pop4[i * 7 + j] = pop[i] + pop2[i] * N();
                        pop5[i * 7 + j] = pop1[i] + pop3[i] * N();
                        fit1[i * 7 + j] = fitkrom(pop4[i * 7 + j], pop5[i * 7 + j]);
                        if (fit1[i * 7 + j] > fit[i])
                        {
                            z++;    
                        } 

                    }
                    if (z >= 2)
                    {
                        pop2[i] = Math.Round(1.1 * pop2[i], k); ;
                        pop3[i] = Math.Round(1.1 * pop3[i], k);
                    }
                    else {
                        pop2[i] = Math.Round(0.9 * pop2[i], k);
                        pop3[i] = Math.Round(0.9 * pop3[i], k);
                    }
                }
                Array.Sort(fit1);

                for (int i = 0; i < jp*7;i++)
                {
                   int j=Array.IndexOf(fit1,fitkrom(pop4[i],pop5[i])); 
                   if (j>jp*6-1) {
                       pop[jp * 7 - j-1] = pop4[i];
                       pop1[jp * 7 - j-1] = pop5[i];
                   }
                }
                cg++;
                pb1.Value++;
            }
            
            listView1.Columns.Add("Pop", 50, HorizontalAlignment.Left);
            listView1.Columns.Add("X1", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("X2", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("sigma1", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("sigma2", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Fitness", 90, HorizontalAlignment.Left);
            iterasi.Text = Convert.ToString(cg-1);
            for (int i = 1; i <= jp; i++)
            {
                lv = new ListViewItem(Convert.ToString(i)); 
                lv.SubItems.Add(Convert.ToString(pop[i-1]));
                lv.SubItems.Add(Convert.ToString(pop1[i-1]));
                lv.SubItems.Add(Convert.ToString(pop2[i - 1]));
                lv.SubItems.Add(Convert.ToString(pop3[i - 1]));
                lv.SubItems.Add(Convert.ToString(fit[i-1]));
                listView1.Items.Add(lv);
            }
            for (int i = 0; i < jp; i++)
            {
                //g.FillEllipse(new SolidBrush(Color.Blue), ex(Math.Round(Encodex(bintodes(pop[i].Substring(0, mx))), k)),
                  //  ey(Math.Round(Encodey(bintodes(pop[i].Substring(mx, my))), k)), 7, 7);
            }
            pb1.Value = pb1.Maximum;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            iterasi.Text = "0";
           
        }

        private void pnl_Draw_Paint(object sender, PaintEventArgs e)
        {
           
            Pen p = new Pen(Color.Black,1);
          
        }
    }
}
