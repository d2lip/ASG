using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TouchToolkit.Framework.Exceptions
{
    public class UnknowGestureException : FrameworkException
    {
        public UnknowGestureException(string message)
            : base(message)
        { }
    }
}
