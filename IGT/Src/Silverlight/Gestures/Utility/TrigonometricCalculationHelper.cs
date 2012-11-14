using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TouchToolkit.GestureProcessor.Objects;



namespace TouchToolkit.GestureProcessor.Utility
{
    public class TrigonometricCalculationHelper
    {
        private static double accuracy = 0.5;

        public static bool isASinglePoint(TouchPoint2 points)
        {
          //  StylusPointCollection sPoints = removeRedundancy(points.Stroke.StylusPoints);
            double length = CalculatePathLength(points);
           // return sPoints.Count <= 4;
            return length < 10;
        }

        public static double CalculatePathLength(TouchPoint2 point)
        {
            // Paths generally contain a lot of points, we are skipping some points 
            // to improve performance. The 'step' variable decides how much we should skip

            int step = 3;
            double pathLength = 0f;
            StylusPointCollection filteredPoints = removeRedundancy(point.Stroke.StylusPoints);

            int len = filteredPoints.Count;
            if (len > step + 1)
            {
                // Initial point
                StylusPoint p1 = filteredPoints[0];

                for (int i = 1; i < len; i += step)
                {
                    StylusPoint p2 = filteredPoints[i - 1];

                    pathLength += TrigonometricCalculationHelper.GetDistanceBetweenPoints(p1, p2);

                    p1 = p2;
                }
            }

            return pathLength;
        }

        public static StylusPointCollection removeRedundancy(StylusPointCollection points)
        {         
            //Remove duplicate points
            StylusPointCollection workingList = new StylusPointCollection();
            
            for (int i = 1; i < points.Count; i++)
            {
                var point1 = points[i - 1];
                var point2 = points[i];
                
                if (!(point1.X == point2.X && point1.Y == point2.Y))
                {
                    if (! workingList.Contains(point1))    
                    workingList.Add(point1);
                }
            }
            return workingList;
        }




        public static double GetSlopeBetweenPoints(Point p1, Point p2)
        {
            return GetSlopeBetweenPoints(new StylusPoint(p1.X, p1.Y), new StylusPoint(p2.X, p2.Y));
        }

        public static double GetAngleBetweenPoints(StylusPoint p1, StylusPoint p2)
        {            
            double DeltaY = p1.Y - p2.Y;
            double DeltaX = p2.X - p1.X;
            return Math.Atan2(DeltaY, DeltaX);
        }


        // NOTE: The overload with 'StylusPoint' is used more frequently than the other one.
        public static double GetSlopeBetweenPoints(StylusPoint p1, StylusPoint p2)
        {
            double DeltaY = p2.Y - p1.Y;            
            double DeltaX = p2.X - p1.X;
            return Math.Atan2(DeltaY, DeltaX);
        }

        public static double GetDistanceBetweenPoints(Point p1, Point p2)
        {
            return GetDistanceBetweenPoints(new StylusPoint(p1.X, p1.Y), new StylusPoint(p2.X, p2.Y));
        }

        public static double GetDistanceBetweenPoints(StylusPoint p1, StylusPoint p2)
        {
            double xDiff = p1.X - p2.X;
            double yDiff = p1.Y - p2.Y;

            double distance = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

            return Math.Abs(distance);
        }


        public static bool isSquare(Rect box)
        {
            double higher = Math.Max(box.Size.Height,box.Size.Width);
            double lower = Math.Min(box.Size.Height, box.Size.Width);

            return (higher - lower <= accuracy * higher);               
        }

    }
}
