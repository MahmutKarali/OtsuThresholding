using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Otsu
{
    public partial class Form1 : Form
    {

        Bitmap bitmap, bitmap2 ;
        Image file;
        int[] histogram = new int[256];
        int[,] PureImage;
        public Form1()
        {
            InitializeComponent();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                file = Image.FromFile(openFileDialog1.FileName);
                bitmap = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = file;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int k = 0, value = 0;

            for (int v = 0; v < 256; v++)
            {
                k = 0;
                for (int i = 1; i < bitmap.Height - 1; i++)
                {
                    for (int j = 1; j < bitmap.Width - 1; j++)
                    {
                        Color c = bitmap.GetPixel(i, j);
                        value = c.R;
                        if (value >= 255) { value = 255; }
                        if (value <= 0) { value = 255; }

                        if (value == v)
                        {
                            k++;
                            histogram[v] = k;
                            this.chart1.Series["Histogram"].Points.AddXY(v, k);
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[] TresholdValue = new int[255];
            int temp = 0, temp2 = 0, temp3 = 0, temp4 = 0, temp5 = 0, temp6 = 0;
            int size = (bitmap.Height * bitmap.Width);

            int BWeight = 0, BMean = 0, BVariance = 0;
            int FWeight = 0, FMean = 0, FVariance = 0;

            int[] T = new int[256];
            int ClassVariance = 0;

            for (int i = 0; i < 256; i++)
            {
                //BWEİGHT HESABI
                temp = temp + histogram[i];
                BWeight = temp / size;

                //BMEAN HESABI
                temp2 = temp2 + (i * histogram[i]);
                if (temp == 0) { temp = 1; }//0'a bölme hatası için

                BMean = temp2 / temp;

                //BVARİANCE HESABI
                temp3 = temp3 + ((int)Math.Sqrt(i - BMean) * histogram[i]);
                BVariance = temp3 / temp;


                for (int j = i + 1; j < 256; j++)
                {
                    //FWEİGHT HESABI
                    temp4 = temp4 + histogram[j];
                    FWeight = temp4 / size;

                    //FMEAN HESABI
                    temp5 = temp5 + (j * histogram[j]);
                    if (temp4 == 0) temp4 = 1;
                    FMean = temp5 / temp4;

                    //FVARİANCE HESABI
                    temp6 = temp6 + ((int)Math.Sqrt(j - FMean) * histogram[j]);
                    FVariance = temp6 / temp4;
                }

                ClassVariance = (BWeight * BVariance + FWeight * FVariance);

                T[i] = ClassVariance;
            }

            int MinNumber = T[44], a = 0;

            for (int b = 1; b < 255; b++)
            {
                if (T[b] < MinNumber)
                {
                    MinNumber = T[b];
                    a = b;
                }
            }

            bitmap2 = new Bitmap(bitmap.Height, bitmap.Width);
            int Threshold = 0;

            for (int i = 0; i < bitmap2.Height; i++)
            {
                for (int j = 0; j < bitmap2.Width; j++)
                {
                    Color c1 = bitmap.GetPixel(i, j);
                    if (c1.B > a) { Threshold = 255; }
                    else { Threshold = 0; }
                    bitmap2.SetPixel(i, j, Color.FromArgb(Threshold, Threshold, Threshold));
                }
            }
            pictureBox2.Image = bitmap2;
        
        } 

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}