using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorter
{
    class StoogeSort : SortAlgorithm
    {
        Thread[] threads = new Thread[100];
        Barrier barrier = new Barrier(participantCount: 0);
        Barrier end = new Barrier(participantCount: 100);

        public override int[][] Sort(int[][] lists)
        {
            barrier.AddParticipants(100);
            for (int i = 0; i < 100; i++)
            {
                int k = i;
                threads[k] = new Thread(() => SortList(lists[k]));
                threads[k].Start();
            }

            while (barrier.ParticipantCount > 0)
            {
                Program.gui.form.Invoke((MethodInvoker)(() => Program.gui.Draw(lists)));
            }
            Program.gui.form.Invoke((MethodInvoker)(() => Program.gui.Draw(lists)));

            return lists;
        }

        private void SortList(int[] list)
        {

            Stooge(list, 0, 99);


            barrier.RemoveParticipant();
            end.SignalAndWait();
            Thread.Sleep(100);
        }


        public void Stooge(int[] list, int i, int j)
        {
            if (list[i] > list[j])
            {
                int temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
            barrier.SignalAndWait();
            if (j - i + 1 > 2)
            {
                int t = (j - i + 1) / 3;
                Stooge(list, i, j - t);
                Stooge(list, i + t, j);
                Stooge(list, i, j - t);
            } 
        }


        public override string ToString()
        {
            return "Stooge Sort";
        }
    }

}
