using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Collections.Generic;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.Utility.TouchHelpers;

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public class TouchPointsCalculator : IReturnTypeCalculator
    {
        public IReturnType Calculate(ValidSetOfTouchPoints set)
        {
            TouchPoints points = new TouchPoints();
            foreach (var touchPoint in set)
            {
                points.Add(new TouchInfo() { ActionType = touchPoint.Action.ToTouchAction(), Position = touchPoint.Position, TouchDeviceId = touchPoint.TouchDeviceId });
            }

            return points;
        }
    }
}
