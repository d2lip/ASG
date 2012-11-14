using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ShapeRecognizers.ConvexHull;

namespace TouchToolkit.Framework.ShapeRecognizers
{
    public class EllipseRecognizer : IShapeRecognizer
    {
        Point[] _points = null;
        public EllipseRecognizer(Point[] points)
        {
            //TODO: Filter the points to improve performance
            _points = points;
        }

        public EllipseRecognizer() { }

        public bool IsMatch(Point[] points)
        {
            _points = points;
            return IsMatch();
        }

        public bool IsMatch()
        {
            if (!IsTwoEndMet())
                return false;

            if (GetLength() / GetLengthOfConvexHull() > 1.3)
                return false;

            if (GetEnclosingRecHeight() < 20 || GetEnclosingRecHeight() < 20)
                return false;

            float enclosingRecArea = GetEnclosingRecHeight() * GetEnclosingRecWidth();
            float convexHullArea = GetConvexHullArea();

            if (convexHullArea / enclosingRecArea > 0.7f && convexHullArea / enclosingRecArea <= 0.8f)
                return true;

            return false;
        }

        public float GetEnclosingRecHeight()
        {
            float h = GetDistance(new Point(GetMinX(), GetMinY()),
                                   new Point(GetMinX(), GetMaxY()));

            float w = GetDistance(new Point(GetMinX(), GetMinY()),
                                   new Point(GetMaxX(), GetMinY()));

            //return the higher value
            if (h > w)
                return h;
            else
                return w;
        }

        private bool IsTwoEndMet()
        {
            float twoEndDistance = GetDistance(_points[0], _points[_points.Length - 1]);
            float lengthOfStroke = GetLength();

            //TODO: Magic number?? 100
            if (twoEndDistance < 100)
                if (lengthOfStroke / twoEndDistance > 2f)
                    return true;
            return false;
        }

        private float GetLength()
        {
            return GetLength(_points);
        }

        private float GetLength(Point[] points)
        {
            float distance = 0.0f;
            float tmp = 0.0f;
            int n = points.Length - 1;
            for (int i = 0; i < n; i++)
            {
                tmp = GetDistance(points[i], points[i + 1]);
                distance += tmp;
            }
            return distance;
        }

        private float GetDistance(Point x, Point y)
        {
            return (float)Math.Sqrt(((x.X - y.X) * (x.X - y.X) + (x.Y - y.Y) * (x.Y - y.Y)));
        }

        public float GetLengthOfConvexHull()
        {
            return GetLength(GetConvexHullPoints());
        }

        public double GetMaxX()
        {
            double maxX = 0;
            foreach (Point x in _points)
            {
                if (maxX < x.X)
                    maxX = x.X;
            }
            return maxX;
        }

        public double GetMaxY()
        {
            double maxY = 0;

            foreach (Point y in _points)
            {
                if (maxY < y.Y)
                    maxY = y.Y;
            }
            return maxY;
        }

        public double GetMinX()
        {
            double minX = int.MaxValue;

            foreach (Point x in _points)
            {
                if (minX > x.X)
                    minX = x.X;
            }
            return minX;
        }

        public double GetMinY()
        {
            double minY = int.MaxValue;

            foreach (Point y in _points)
            {
                if (minY > y.Y)
                    minY = y.Y;
            }
            return minY;
        }

        private Point[] GetConvexHullPoints()
        {
            return ConvexHullArea.GetConvexHullPoints(_points);
        }

        public float GetEnclosingRecWidth()
        {
            float h = GetDistance(new Point(GetMinX(), GetMinY()),
                new Point(GetMinX(), GetMaxY()));

            float w = GetDistance(new Point(GetMinX(), GetMinY()),
                new Point(GetMaxX(), GetMinY()));

            //return the lower value
            if (h > w)
                return w;
            else
                return h;
        }

        public float GetConvexHullArea()
        {
            return (float)ConvexHullArea.CalculateArea(_points);
        }

    }
}
