using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.Utility.TouchHelpers;

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public class TouchPathsCalculator : IReturnTypeCalculator
    {
        public IReturnType Calculate(ValidSetOfTouchPoints set)
        {
            TouchPaths paths = new TouchPaths();

            foreach (var item in set)
            {
                paths.Add(item.Stroke.StylusPoints.ToTouchPath(item.TouchDeviceId));
            }

            return paths;
        }
    }
}
