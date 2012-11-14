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

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public class PositionCalculator : IReturnTypeCalculator
    {
        /// <summary>
        /// Returns top-left position
        /// </summary>
        /// <param name="relatedTouches"></param>
        /// <returns></returns>
        public IReturnType Calculate(ValidSetOfTouchPoints set)
        {
            if (set.Count > 0)
            {
                int len = set[0].Stroke.StylusPoints.Count;
                Position p = new Position();
                p.X = set[0].Stroke.StylusPoints[len - 1].X;
                p.Y = set[0].Stroke.StylusPoints[len - 1].Y;

                return p;
            }
            else
            {
                return null;
            }
        }

    }
}
