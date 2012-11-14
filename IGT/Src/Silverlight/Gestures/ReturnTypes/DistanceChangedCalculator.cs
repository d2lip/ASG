using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TouchToolkit.GestureProcessor.Exceptions;
using TouchToolkit.GestureProcessor.Utility;
using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public class DistanceChangedCalculator : IReturnTypeCalculator
    {

        public IReturnType Calculate(ValidSetOfTouchPoints set)
        {
            if (set.Count != 2)
                throw new InvalidDataSetException(string.Format("Distance can only be calculated between two points. The parameter contains {0} touch point(s)!", set.Count));
            
            Point p1 = set[0].Position;
            Point p2 = set[1].Position;

            DistanceChanged value = new DistanceChanged();
            value.Distance = TrigonometricCalculationHelper.GetDistanceBetweenPoints(p1, p2);

            if (set[0].Stroke.StylusPoints.Count > 1 && set[1].Stroke.StylusPoints.Count > 1)
            {
                p1 = set[0].Stroke.StylusPoints[set[0].Stroke.StylusPoints.Count - 2].ToPoint();
                p2 = set[1].Stroke.StylusPoints[set[1].Stroke.StylusPoints.Count - 2].ToPoint();
                double prevDistance = TrigonometricCalculationHelper.GetDistanceBetweenPoints(p1, p2);

                value.Delta = value.Distance - prevDistance;
            }

            return value as IReturnType;
        }
    }
}
