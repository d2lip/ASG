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
using System.Diagnostics;
using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public class PositionChangedCalculator : IReturnTypeCalculator
    {

        #region IReturnTypeCalculator Members

        public IReturnType Calculate(ValidSetOfTouchPoints set)
        {
            if (set.Count < 1)
            {
                throw new InvalidDataSetException(string.Format("At least one touch point required to calculate position change. The parameter contains {0} touch point(s)!", set.Count));
            }
            else
            {
                PositionChanged val = new PositionChanged();

                if (set[0].Stroke.StylusPoints.Count > 1)
                {
                    Point p1 = set[0].Position;
                    Point p2 = set[0].Stroke.StylusPoints[set[0].Stroke.StylusPoints.Count - 2].ToPoint();

                    val.X = p1.X - p2.X;
                    val.Y = p1.Y - p2.Y;
                }

                return val;
            }
        }

        #endregion
    }
}
