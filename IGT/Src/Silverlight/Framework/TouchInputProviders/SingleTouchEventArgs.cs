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
    public class SingleTouchEventArgs : EventArgs
    {
        public SingleTouchEventArgs(TouchPoint2 point)
        {
            touchPoint = point;
        }

        private TouchPoint2 touchPoint;
        public TouchPoint2 TouchPoint
        {
            get
            {
                return touchPoint;
            }
        }
    }
}
