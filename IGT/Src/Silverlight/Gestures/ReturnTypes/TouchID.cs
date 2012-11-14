using System;
using System.Collections.Generic;

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public class TouchID : List<int>, IReturnType
    {
        public string ID { get; set; }
    }
}
