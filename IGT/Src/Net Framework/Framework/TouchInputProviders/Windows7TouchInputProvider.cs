using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.GestureProcessor.Objects;
using System.Windows.Controls;
using System.Windows.Input;
using TouchToolkit.Framework.Utility;
using System.Windows;
using TouchToolkit.Framework.TouchInputProviders;
using TouchToolkit.Framework;

namespace TouchToolkit.Framework.TouchInputProviders
{
    public class Windows7TouchInputProvider : TouchInputProvider
    {
        public override event TouchInputProvider.FrameChangeEventHandler FrameChanged;
        public override event TouchInputProvider.SingleTouchChangeEventHandler SingleTouchChanged;
        public override event TouchInputProvider.MultiTouchChangeEventHandler MultiTouchChanged;

        public override void Init()
        {
            Touch.FrameReported += Touch_FrameReported;  
            GestureFramework.LayoutRoot.TouchDown += layoutRoot_TouchDown;
        }


        public void layoutRoot_TouchDown(object sender, TouchEventArgs e)
        {
            TouchPoint p = e.GetTouchPoint(GestureFramework.LayoutRoot);
            TouchInfo info = p.ToTouchInfo();
            UIElement source = e.OriginalSource as UIElement;

            base.AddNewTouchPoint(info, source);
        }

        int lastTimeStamp = 0;
        private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
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
}
