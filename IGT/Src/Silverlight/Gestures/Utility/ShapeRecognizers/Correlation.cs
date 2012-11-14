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

using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.Framework.ShapeRecognizers
{
    public class Correlation
    {
        public bool VerticalLine { get; private set; }
        public double SlopeRad { get; private set; }
        public double Slope { get; private set; }
        public double Intercept { get; private set; }

        public double RSquared { get; private set; }

        private double xavg;
        private double yavg;
        public Correlation(TouchPoint2 points)
        {
            //Account for a degenerate amount of points
            if (points.Stroke.StylusPoints.Count <= 1)
            {
                RSquared = 0;
                Slope = 0;
                Intercept = 0;
                VerticalLine = false;
                SlopeRad = 0;
                return;
            }

            var pointlist = points.Stroke.StylusPoints;
            
            //Remove duplicate points
            var workingList = new StylusPointCollection();
            for (int i = 1; i < pointlist.Count; i++ )
            {
                var point1 = pointlist[i - 1];
                var point2 = pointlist[i];
                if (!(point1.X == point2.X && point1.Y == point2.Y))
                {
                    workingList.Add(point1);
                }
            }
            VerticalLine = false;

            xavg = 0;
            yavg = 0;

            foreach (var p in workingList)
            {
                xavg += p.X;
                yavg += p.Y;
            }
            xavg = xavg / workingList.Count;
            yavg = yavg / workingList.Count;

            double numerator = 0;
            double denominator = 0;
            foreach (var p in workingList)
            {
                numerator += (p.X - xavg) * (p.Y - yavg);
                denominator += Math.Pow(p.X - xavg,2);
            }

            SlopeRad = Math.Atan2(numerator, denominator);
            if (denominator != 0)
            {
                Slope = numerator / denominator;
                Intercept = yavg - Slope * xavg;
            }
            else
            {
                VerticalLine = true;
            }
            TouchPoint2 tp = points.GetEmptyCopy();
            if(workingList.Count > 0)
                tp.Stroke.StylusPoints = workingList;
            RSquared = CalculateRSquared(tp);
        }

        private double CalculateRSquared(TouchPoint2 points)
        {
            if (VerticalLine)
            {
                return 1;
            }
            //Sum of Squares
            double total = 0;
            double residual = 0;

            foreach (var p in points.Stroke.StylusPoints)
            {
                residual += Math.Pow(p.Y - F(p.X),2);
                total += Math.Pow(p.Y - yavg,2);
            }

            return 1 - (residual/total);
        }

        private double F(double x)
        {
            return Intercept + Slope * x;
        }
    }
}
