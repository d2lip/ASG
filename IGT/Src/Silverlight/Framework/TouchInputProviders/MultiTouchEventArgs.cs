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

namespace TouchToolkit.Framework.TouchInputProviders
{
    public class MultiTouchEventArgs : EventArgs
    {
        public MultiTouchEventArgs(List<TouchPoint2> points)
        {
            touchPoints = points;
        }

        private List<TouchPoint2> touchPoints;
        public List<TouchPoint2> TouchPoints
        {
            get
            {
                return touchPoints;
            }
        }
    }
}
