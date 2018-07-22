using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorter
{
    class SelectionSort : SortAlgorithm
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
            for (int i = 0; i < 99; i++)
            {
                int min = i;
                for (int j = i + 1; j < 100; j++)
                {
                    if (list[j] < list[min])
                    {
                        min = j;
                    }
                }
                if (min != i)
                {
                    int temp = list[i];
                    list[i] = list[min];
                    list[min] = temp;
                }
                Thread.Sleep(10);
                barrier.SignalAndWait();
            }
            barrier.RemoveParticipant();
            end.SignalAndWait();
            Thread.Sleep(100);
        }


        public override string ToString()
        {
            return "Selection Sort";
        }
    }
}
