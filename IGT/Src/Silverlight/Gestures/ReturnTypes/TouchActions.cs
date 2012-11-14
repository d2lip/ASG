using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public class TouchActions : List<TouchActionInfo>, IReturnType
    {

    }

    public class TouchActionInfo
    {
        public int TouchDeviceId{get;set;}
        public TouchAction Action { get; set; }
    }
}
