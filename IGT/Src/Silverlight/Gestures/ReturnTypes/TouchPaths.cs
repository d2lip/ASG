using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public class TouchPaths : List<TouchPath>, IReturnType
    {

    }

    public class TouchPath
    {
        public int TouchDeviceId = 0;
        public List<Point> Points = new List<Point>();
    }
}
