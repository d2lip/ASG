using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public class TouchActionsCalculator : IReturnTypeCalculator
    {
        public IReturnType Calculate(Objects.ValidSetOfTouchPoints set)
        {
            TouchActions actions = new TouchActions();

            foreach (var item in set)
            {
                TouchActionInfo info = new TouchActionInfo()
                {
                    Action = item.Action,
                    TouchDeviceId = item.TouchDeviceId
                };
                actions.Add(info);
            }

            return actions;
        }
    }
}
