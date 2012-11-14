using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    class TouchIDCalculator : IReturnTypeCalculator
    {
        public IReturnType Calculate(Objects.ValidSetOfTouchPoints set)
        {
            TouchID id = new TouchID();
            foreach (var point in set)
            {
                if (!id.Contains(point.TouchDeviceId))
                {
                    id.Add(point.TouchDeviceId);
                }
            }
            return id;
        }
    }
}
