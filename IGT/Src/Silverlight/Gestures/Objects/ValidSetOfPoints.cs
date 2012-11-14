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

namespace TouchToolkit.GestureProcessor.Objects
{
    public class ValidSetOfTouchPoints : List<TouchPoint2>
    {
        public string Tag { get; set; }

        public ValidSetOfTouchPoints()
            : base()
        { }

        public ValidSetOfTouchPoints(int capacity)
            : base(capacity)
        {
        }

        public ValidSetOfTouchPoints(List<TouchPoint2> points)
        {
            this.AddRange(points);
        }

        public ValidSetOfTouchPoints(TouchPoint2[] array)
        {
            this.Capacity = array.Length;
            foreach (var item in array)
            {
                this.Add(item);
            }
        }
    }
}
