using System.Windows;

namespace ShapeRecognizers.ConvexHull
{
    //TODO: Mention code source
    class Polysort
    {
        private static void swap(int[] arr, int s, int t)
        {
            int tmp = arr[s]; arr[s] = arr[t]; arr[t] = tmp;
        }

        private static void swap(Point[] arr, int s, int t)
        {
            Point tmp = arr[s]; arr[s] = arr[t]; arr[t] = tmp;
        }

        // Typed OO-style quicksort a la Hoare/Wirth

        private static void qsort(int[] arr, int a, int b)
        {
            // sort arr[a..b]
            if (a < b)
            {
                int i = a, j = b;
                int x = arr[(i + j) / 2];
                do
                {
                    while (arr[i] < (x)) i++;
                    while (x < (arr[j])) j--;
                    if (i <= j)
                    {
                        swap(arr, i, j);
                        i++; j--;
                    }
                } while (i <= j);
                qsort(arr, a, j);
                qsort(arr, i, b);
            }
        }

        //To sort Point Array
        private static void qsort(Point[] arr, int a, int b)
        {
            // sort arr[a..b]
            if (a < b)
            {
                int i = a, j = b;
                Point x = arr[(i + j) / 2];
                do
                {
                    while (arr[i].Less(x)) i++;
                    while (x.Less(arr[j])) j--;
                    if (i <= j)
                    {
                        swap(arr, i, j);
                        i++; j--;
                    }
                } while (i <= j);
                qsort(arr, a, j);
                qsort(arr, i, b);
            }
        }

        public static void Quicksort(int[] arr)
        {
            qsort(arr, 0, arr.Length - 1);
        }

        // for point
        public static void Quicksort(Point[] arr)
        {
            qsort(arr, 0, arr.Length - 1);
        }
    }
}