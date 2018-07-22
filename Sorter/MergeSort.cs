using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorter
{
    class MergeSort : SortAlgorithm
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
            Split((int[])list.Clone(), 0, 100, list);

            barrier.RemoveParticipant();
            end.SignalAndWait();
            Thread.Sleep(100);
        }

        private void Split(int[] B, int i, int j, int[] A)
        {
            if (j - i < 2)
            {
                return;
            }
            int m = (j + i) / 2;
            Split(A, i, m, B);
            Split(A, m, j, B);
            Merge(B, i, m, j, A);
        }

        private void Merge(int[] A, int i, int m, int j, int[] B)
        {
            int k = i;
            int l = m;

            for (int n = i; n < j; n++)
            {
                if (k < m && (l >= j || A[k] <= A[l]))
                {
                    B[n] = A[k];
                    k++;
                } else
                {
                    B[n] = A[l];
                    l++;
                }
                barrier.SignalAndWait();
                Thread.Sleep(10);
            }
        }


        public override string ToString()
        {
            return "Merge Sort";
        }
    }
}
