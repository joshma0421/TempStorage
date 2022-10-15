using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _clrs = Get256Color();
        }

        public void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        Color[] Get256Color()
        {
            Color[] clrs = new Color[256];
            double h = 0.0;
            double s = 0.8;
            double v = 0.8;
            double hs = 360 / 248;
            for (int i = 0; i < 256; i++)
            {
                if (i < 8)
                {
                    clrs[i] = Color.FromArgb(i * 35 + 5, i * 35 + 5, i * 35 + 5);
                }
                else
                {
                    clrs[i] = ColorFromHSV(h, s, v);
                    h += hs;
                }                
            }
            return clrs;
        }

        void SetPalette(Bitmap bmp, Color[] clrs)
        {
            ColorPalette palette = bmp.Palette;
            for(int i = 0; i < palette.Entries.Length; i++)
            {
                palette.Entries[i] = clrs[i];
            }
            bmp.Palette = palette;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pictureBox1.BackColor = _clrs[trackBar1.Value];
            this.Text = trackBar1.Value.ToString() + ", " + pictureBox1.BackColor.ToString();
        }

        unsafe private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(256, 1, PixelFormat.Format8bppIndexed);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, 256, 1), ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte* ptr = (byte*)(data.Scan0.ToPointer());
            for (int i = 0; i < 256; i++)
                ptr[i] = (byte)i;
            bmp.UnlockBits(data);
            SetPalette(bmp, _clrs);
            //pictureBox2.Image = bmp;
            bmp.Save("D:\\test.bmp");
        }

        Color[] _clrs;
    }
}
