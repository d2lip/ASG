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
using System.Collections;
using TouchToolkit.GestureProcessor.Objects;
using System.Collections.Generic;

namespace TouchToolkit.Framework.Utility
{
    public class FrameInfoComparer : IComparer<FrameInfo>
    {
        public int Compare(FrameInfo x, FrameInfo y)
        {
            return x.TimeStamp.CompareTo(y.TimeStamp);
        }
    }
}
