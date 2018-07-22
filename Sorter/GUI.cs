using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorter
{
    class GUI
    {
        public Button start;
        public Button sort;
        public ComboBox algorithm;
        public Form form;
        public PictureBox box;
        public Bitmap canvas;

        public int[][] lists;
        public Dictionary<int, Color> colors;

        public GUI()
        {

            form = new Form
            {
                Text = "Sorter"
            };
            form.SetDesktopBounds(800, 600, 900, 639);

            box = new PictureBox
            {
                Bounds = new Rectangle(100, 100, 400, 400),
                BackColor = Color.LightGray
            };
            form.Controls.Add(box);

            canvas = new Bitmap(400, 400);

            start = new Button()
            {
                Bounds = new Rectangle(620, 20, 120, 50),
                BackColor = Color.LightGray,
                Text = "Start"
            };
            start.Click += new EventHandler(Click_Start);
            form.Controls.Add(start);

            sort = new Button()
            {
                Bounds = new Rectangle(740, 20, 120, 50),
                BackColor = Color.LightGray,
                Text = "Sort"
            };
            sort.Click += new EventHandler(Click_Sort);
            form.Controls.Add(sort);

            algorithm = new ComboBox
            {
                Bounds = new Rectangle(620, 260, 120, 50),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            algorithm.Items.Add(new BubbleSort());
            algorithm.Items.Add(new InsertionSort());
            algorithm.Items.Add(new SelectionSort());
            algorithm.Items.Add(new StoogeSort());
            algorithm.Items.Add(new MergeSort());
            algorithm.SelectedIndex = 0;
            form.Controls.Add(algorithm);

        }

        private void Click_Start(object sender, EventArgs e)
        {
            lists = new int[100][];
            colors = new Dictionary<int, Color>();
            Random rng = new Random();

            for (int i = 0; i < 100; i++)
            {
                int[] row = Enumerable.Range(0, 100).OrderBy(x => rng.Next()).ToArray();
                lists[i] = row;
                Color c = HSL2RGB(i / 100f, 0.5, 0.5);
                colors.Add(i, c);
            }

            Graphics g = Graphics.FromImage(canvas);
            SolidBrush brush = new SolidBrush(Color.White);

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    brush.Color = colors[lists[i][j]];
                    g.FillRectangle(brush, j * 4, i * 4, 4, 4);
                }
            }

            box.Image = canvas;
        }

        private void Click_Sort(object sender, EventArgs e)
        {

            SortAlgorithm alg = (SortAlgorithm)algorithm.SelectedItem;
            Task.Factory.StartNew(() => Program.Sort(lists, alg));

        }


        public void Draw(int[][] lists)
        {
            Graphics g = Graphics.FromImage(canvas);
            SolidBrush brush = new SolidBrush(Color.White);

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    brush.Color = colors[lists[i][j]];
                    g.FillRectangle(brush , j * 4, i * 4, 4, 4);
                }
            }

            box.Refresh();
        }

        public void Draw(int[][] lists, int n)
        {
            Graphics g = Graphics.FromImage(canvas);
            SolidBrush brush = new SolidBrush(Color.White);

            for (int j = 0; j < 100; j++)
            {
                Color c = HSL2RGB(lists[n][j] / 100f, 0.5, 0.5);
                brush.Color = c;
                g.FillRectangle(brush, j * 4, n * 4, 4, 4);
            }

            box.Refresh();
        }


        public static Color HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            return Color.FromArgb(Convert.ToByte(r * 255.0f), Convert.ToByte(g * 255.0f), Convert.ToByte(b * 255.0f));
        }

    }
}
