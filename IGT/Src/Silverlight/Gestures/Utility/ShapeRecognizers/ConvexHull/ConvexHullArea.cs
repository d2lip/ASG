using System;
using System.Windows;

namespace ShapeRecognizers.ConvexHull
{
    //TODO: Mention code source
    public class ConvexHullArea
    {
        public ConvexHullArea()
        {
        }

        public static Point[] GetConvexHullPoints(Point[] pts)
        {
            Point[] tmp = new Point[pts.Length];
            Array.Copy(pts, tmp, pts.Length);
            return Convexhull.convexhull(tmp);
        }

        public static double CalculateArea(Point[] pts)
        {
            return GetArea(GetConvexHullPoints(pts));
        }

        #region private
        // The centroid of a point set
        private static Point centroid(Point[] pts)
        {
            int N = pts.Length;
            double sumx = 0, sumy = 0;
            for (int i = 0; i < N; i++)
            {
                sumx += pts[i].X;
                sumy += pts[i].Y;
            }
            return new Point(sumx / N, sumy / N);
        }

        // The area of a polygon (represented by an array of ordered vertices)
        public static double GetArea(Point[] pts)
        {
            int N = pts.Length;
            Point centr = centroid(pts);
            double area2 = 0;
            for (int i = 0; i < N; i++)
                area2 += Utility.Area(centr, pts[i], pts[(i + 1) % N]);
            return Math.Abs(area2 / 2);
        }

        private static void print(Point[] pts)
        {
            int N = pts.Length;
            for (int i = 0; i < N; i++)
                Console.WriteLine(pts[i]);
        }

        #endregion
    }
}