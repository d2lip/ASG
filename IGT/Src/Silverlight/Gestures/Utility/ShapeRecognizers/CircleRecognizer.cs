using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.GestureProcessor.Objects;
using System.Windows.Input;
using TouchToolkit.GestureProcessor.Utility;

namespace TouchToolkit.Framework.ShapeRecognizers
{
    //Attempts to find the circle of best fit given a set of points using a (modified) Least Squares approach after a polar transform
    class CircleRecognizer
    {
        public double Radius; //An estimation of the radius of the circle of best fit
        public double X_Center; //An estimation of the center
        public double Y_Center;
        public double R;

        public CircleRecognizer(TouchPoint2 points)
        {            
            //Estimate the center (ie. the average: x,y coords)
            double xavg = 0;
            double yavg = 0;
            foreach (var p in points.Stroke.StylusPoints)
            {
                xavg += p.X;
                yavg += p.Y;
            }
            xavg = xavg / points.Stroke.StylusPoints.Count;
            yavg = yavg / points.Stroke.StylusPoints.Count;
            Y_Center = yavg;
            X_Center = xavg;
            StylusPoint center = new StylusPoint(X_Center, Y_Center);

            //Estimate the radius (ie. the avg distance from center)
            Radius = 0;
            foreach (var p in points.Stroke.StylusPoints)
            {
                Radius += TrigonometricCalculationHelper.GetDistanceBetweenPoints(center, p);
            }
            Radius = Radius / points.Stroke.StylusPoints.Count;

            //Calculate the average distance from the estimated circle

            double avgDist = 0;
            foreach (var p in points.Stroke.StylusPoints)
            {
                avgDist += TrigonometricCalculationHelper.GetDistanceBetweenPoints(p, center) - Radius;
            }
            avgDist = avgDist / points.Stroke.StylusPoints.Count;

            //Calculate the 'correlation'

            double residual = 0;
            double total = 0;
            foreach (var p in points.Stroke.StylusPoints)
            {
                double dist = TrigonometricCalculationHelper.GetDistanceBetweenPoints(p,center);
                residual += Math.Pow(dist - Radius, 2);
                total += Math.Pow(dist - avgDist,2);
            }

            R = 1 - (residual / total);
        }
    }
}
