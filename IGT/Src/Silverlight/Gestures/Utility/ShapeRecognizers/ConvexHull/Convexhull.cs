using System.Windows;

namespace ShapeRecognizers.ConvexHull
{
    //TODO: Mention code source
    public class Convexhull
    {
        public static Point[] convexhull(Point[] pts)
        {
            // Sort points lexicographically by increasing (x, y)
            int N = pts.Length;
            Polysort.Quicksort(pts);
            Point left = pts[0], right = pts[N - 1];

            // Partition into lower hull and upper hull
            CDLL lower = new CDLL(left), upper = new CDLL(left);
            for (int i = 0; i < N; i++)
            {
                double det = Utility.Area(left, right, pts[i]);
                if (det > 0)
                    upper = upper.Append(new CDLL(pts[i]));
                else if (det < 0)
                    lower = lower.Prepend(new CDLL(pts[i]));
            }

            lower = lower.Prepend(new CDLL(right));
            upper = upper.Append(new CDLL(right)).Next;

            // Eliminate points not on the hull
            eliminate(lower);
            eliminate(upper);

            // Eliminate duplicate endpoints
            if (lower.Prev.val.Equals(upper.val))
                lower.Prev.Delete();
            if (upper.Prev.val.Equals(lower.val))
                upper.Prev.Delete();

            // Join the lower and upper hull
            Point[] res = new Point[lower.Size() + upper.Size()];
            lower.CopyInto(res, 0);
            upper.CopyInto(res, lower.Size());

            return res;
        }

        // Graham's scan
        private static void eliminate(CDLL start)
        {
            CDLL v = start, w = start.Prev;
            bool fwd = false;
            while (v.Next != start || !fwd)
            {
                if (v.Next == w)
                    fwd = true;
                if (Utility.Area(v.val, v.Next.val, v.Next.Next.val) < 0) // right turn
                    v = v.Next;
                else
                {                                       // left turn or straight
                    v.Next.Delete();
                    v = v.Prev;
                }
            }
        }
    }
}