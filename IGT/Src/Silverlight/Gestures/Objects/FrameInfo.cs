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
    public class FrameInfo
    {
        private List<TouchInfo> _touches = new List<TouchInfo>();
        public List<TouchInfo> Touches
        {
            get
            {
                return _touches;
            }
            set
            {
                _touches = value;
            }
        }

        public long TimeStamp { get; set; }
        public int WaitTime { get; set; }
    }
}
