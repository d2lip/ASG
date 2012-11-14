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
    public class SlopeChangedCalculator : IReturnTypeCalculator
    {
        public IReturnType Calculate(ValidSetOfTouchPoints set)
        {
            SlopeChanged sc = new SlopeChanged();

            if (set.Count != 2)
                throw new InvalidDataSetException("Slope can only be calculated for two touch points!");

            // Calculate current slope
            sc.NewSlope = TrigonometricCalculationHelper.GetSlopeBetweenPoints(set[0].Position, set[1].Position) * 180 / 3.14;

            // Check if enough history data is available
            if (set[0].Stroke.StylusPoints.Count > 1 && set[1].Stroke.StylusPoints.Count > 1)
            {
                // Calculate slope for last position
                double prevSlope = TrigonometricCalculationHelper.GetSlopeBetweenPoints(
                    set[0].Stroke.StylusPoints[set[0].Stroke.StylusPoints.Count - 2],
                    set[1].Stroke.StylusPoints[set[1].Stroke.StylusPoints.Count - 2]) * 180 / 3.14;

                sc.Delta = sc.NewSlope - prevSlope;
            }

            return sc;
        }
    }
}
