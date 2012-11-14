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

namespace TouchToolkit.GestureProcessor.Objects
{
    public class TouchFrame
    {
        public DateTime Time = DateTime.Now;
        public List<TouchPoint2> Touches = new List<TouchPoint2>();
    }
}
