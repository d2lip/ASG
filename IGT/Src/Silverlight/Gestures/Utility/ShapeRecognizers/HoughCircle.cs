using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.Utility;

namespace TouchToolkit.Framework.ShapeRecognizers
{
    /* Algorithm to help identify circles given a set of points.*/
    public class HoughCircle : IShapeRecognizer
    {
        private double deg_to_radians = Math.PI / 180;
        private double threshhold = 0.3;
        private bool IsCircle;
        private TouchPoint2 points;
        private int[] Accumulator;
        private int binmax;
        private int rmax;

        public HoughCircle(TouchPoint2 p)
        {
            points = p;

            IsCircle = false;
            FindRanges();
            binmax = 0;
            Accumulator = new int[rmax];
            for (int r = 0; r < rmax; r++)
            {
                Accumulator[r] = 0;
            }

            
            FindCircle();
        }

        public bool IsMatch()
        {
            return IsCircle;
        }

        //Find smallest/largest x,y values of our points
        private void FindRanges()
        {
            
            double xmin = points.Stroke.StylusPoints[0].X;
            double xmax = points.Stroke.StylusPoints[0].X;
            double ymin = points.Stroke.StylusPoints[0].Y;
            double ymax = points.Stroke.StylusPoints[0].Y;

            foreach (var point in points.Stroke.StylusPoints)
            {
                if (point.X < xmin)
                    xmin = (int)point.X;
                if (point.X > xmax)
                    xmax = (int)point.X;
                if (point.Y < ymin)
                    ymin = (int)point.Y;
                if (point.Y > ymax)
                    ymax = (int)point.Y;
            }
            rmax = (int)TrigonometricCalculationHelper.GetDistanceBetweenPoints(new Point(xmin, ymin), new Point(xmax, ymax));
        }

        /*Modified Hough Algorthm where the center of the circle is estimated by taking the average 
         x and y values of all points */
        private TouchPoint2 FindCircle()
        {
            int length = points.Stroke.StylusPoints.Count;

            /*First add up the x and y lengths of all points then divide by the total length to get the 
             average */
            double xdistance = 0;
            double ydistance = 0;
            double totdistance = 0;
            for (int i = 0; i < length - 1; i++)
            {
                var point1 = points.Stroke.StylusPoints[i];
                var point2 = points.Stroke.StylusPoints[i + 1];
                
                double distance = TrigonometricCalculationHelper.GetDistanceBetweenPoints(point1, point2);
                totdistance += distance;

                xdistance += point1.X * distance;
                ydistance += point1.Y * distance;
            }

            int avgX = (int) Math.Floor(xdistance / totdistance);
            int avgY = (int) Math.Floor(ydistance / totdistance);

            /*Using the estimated center, apply the Hough Transform with R as the only free parameter*/
            double radius = 0;
            foreach (var point in points.Stroke.StylusPoints)
            {
                double x = point.X;
                double y = point.Y;
                StylusPoint center = new StylusPoint(avgX, avgY);

                int rpoint = (int) Math.Floor(
                    Math.Abs(TrigonometricCalculationHelper.GetDistanceBetweenPoints(center, point)));
                    
                if (rpoint > 0 && rpoint < rmax -1)
                {
                    Accumulator[rpoint]++;
                    Accumulator[rpoint + 1]++;
                    Accumulator[rpoint - 1]++;
                }
            }

            for (int r = 0; r < rmax; r++)
            {
                //check bin size to see if it's max
                if (Accumulator[r] > binmax)
                {
                    binmax = Accumulator[r];
                    radius = r;
                }
            }
            IsCircle = binmax > length * threshhold && binmax > 10;

            if (IsCircle)
            {
                //Keep track of all points not on the circle
                Stack<int> removeStack = new Stack<int>();
                for (int i = 0; i < length; i++)
                {
                    var point = points.Stroke.StylusPoints[i];
                    StylusPoint center = new StylusPoint(avgX, avgY);

                    int rpoint = (int)Math.Floor(
                        Math.Abs(TrigonometricCalculationHelper.GetDistanceBetweenPoints(center, point)));
                    
                    if (Math.Abs(radius - rpoint) > 3)
                    {
                        removeStack.Push(i);
                    }
                }

                while (removeStack.Count > 0)
                {
                    points.Stroke.StylusPoints.RemoveAt(removeStack.Pop());
                }
            }
            else
            {
                return null;
            }
            return points;
        }

    }
}
