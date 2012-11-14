using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.Utility;
using TouchToolkit.Framework.TouchInputProviders;
using System.Windows.Media;

namespace TouchToolkit.Framework.TouchInputProviders
{
    public class SilverlightTouchInputProvider : TouchInputProvider
    {
        public override event FrameChangeEventHandler FrameChanged;
        public override event SingleTouchChangeEventHandler SingleTouchChanged;
        public override event MultiTouchChangeEventHandler MultiTouchChanged;

        public override void Init()
        {
            Initialize();
        }

        private void Initialize()
        {
            Touch.FrameReported += Touch_FrameReported;
        }

        int lastTimeStamp = 0;
        private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            /* If ui-control-specific gesture detection is enabled,
             * on touch down, update touch ID & Gesture mapping: Which gestures we need to detect for this touch
             * 
             * NOTE: While this is useful when application wants ui-control specific gestures but not so good in
             * terms of performance application uses root canvas level gestures
             */

            // Update stroke records of each active touch
            TouchPointCollection slPoints = e.GetTouchPoints(GestureFramework.LayoutRoot);
            List<TouchInfo> touchInfos = slPoints.ToTouchInfo();
            List<TouchPoint2> touchPoints = base.UpdateActiveTouchPoints(touchInfos);

            // Determine touch source for any new touch point
            foreach (var touchPoint in touchPoints)
            {
                if (touchPoint.Action == TouchAction.Down)
                {
                    touchPoint.UpdateSource();
                }
            }

            // Raw data, frame changed
            if (FrameChanged != null)
            {
                FrameInfo fi = new FrameInfo();
                fi.Touches = touchInfos;
                fi.TimeStamp = e.Timestamp;
                fi.WaitTime = (lastTimeStamp == 0 ? 0 : e.Timestamp - lastTimeStamp);
                FrameChanged(this, fi);
            }

            // Creating one callback for each touch point
            if (SingleTouchChanged != null)
            {
                foreach (TouchPoint2 p in touchPoints)
                {
                    SingleTouchChanged(this, new SingleTouchEventArgs(p));
                }
            }

            // Sending all points in one callback
            if (MultiTouchChanged != null)
            {
                MultiTouchChanged(this, new MultiTouchEventArgs(touchPoints));
            }

            lastTimeStamp = e.Timestamp;
        }

    }

    internal static class SilverlightTouchListerHelper
    {
        public static List<TouchInfo> ToTouchInfo(this TouchPointCollection self)
        {
            List<TouchInfo> list = new List<TouchInfo>(self.Count);
            foreach (var item in self)
            {
                var info = new TouchInfo()
                {
                    ActionType = item.Action.ToTouchActions(),
                    Position = item.Position,
                    TouchDeviceId = item.TouchDevice.Id
                };

                info.Tags.Add("Size", item.Size.ToString());

                list.Add(info);
            }

            return list;
        }
    }
}
