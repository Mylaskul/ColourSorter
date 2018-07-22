using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorter
{
    static class Program
    {
        public static GUI gui;

        public static int Main(string[] args)
        {
            gui = new GUI();
            gui.form.ShowDialog();

            return 0;
        }


        public static void Sort(int[][] lists, SortAlgorithm alg)
        {
            lists = alg.Sort(lists);
            //gui.form.Invoke((MethodInvoker)(() => gui.Draw(lists)));
        }

    }

}
