using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using TouchToolkit.GestureProcessor.Utility.TouchHelpers;
using System.Windows.Controls;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.Utility;
using TouchToolkit.Framework.TouchInputProviders;
using TouchToolkit.Framework.Exceptions;
using System.Windows.Media;

namespace TouchToolkit.Framework.TouchInputProviders
{
    public abstract class TouchInputProvider : IDisposable
    {
        // Allows application to subscribe to raw touch data
        public delegate void FrameChangeEventHandler(object sender, FrameInfo frameInfo);//(object sender, TouchFrameEventArgs e);
        public abstract event FrameChangeEventHandler FrameChanged;

        // Allows application to subscribe to every single touch changed event. For example, in case of multi-touch gesture
        // this event will be fired multiple times (one event for each touch point)
        public delegate void SingleTouchChangeEventHandler(object sender, SingleTouchEventArgs e);
        public abstract event SingleTouchChangeEventHandler SingleTouchChanged;

        // Allows application to subscribe a multi-touch scenario with a single event. Touches that started at the same time
        // will be grouped together. 
        // TODO: In future, we should also support grouping by user if the hardware supports user detection
        public delegate void MultiTouchChangeEventHandler(object sender, MultiTouchEventArgs e);
        public abstract event MultiTouchChangeEventHandler MultiTouchChanged;

        public List<TouchAction2> TouchStates = new List<TouchAction2>();
        public Dictionary<int, TouchPoint2> ActiveTouchPoints = new Dictionary<int, TouchPoint2>();
        private List<int> inactiveTouchPoints = new List<int>();

        public virtual void Init()
        { }


        //TODO: may need to re-think for more efficient way
        public int GetTouchStateCount(TouchAction actionType)
        {
            int count = (from t in TouchStates
                         where t == actionType.ToTouchActions()
                         select t).Count();

            return count;
        }

        /// <summary>
        /// Returns touch objects that represent current set of touchInfo
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public List<TouchPoint2> UpdateActiveTouchPoints(List<TouchInfo> touchInfos)
        {
            List<TouchPoint2> updateTouchPoints = new List<TouchPoint2>();

            UpdateTouchStates(touchInfos);

            // Update Active touch points
            foreach (TouchInfo info in touchInfos)
            {
                updateTouchPoints.Add(UpdateActiveTouchPoint(info));
            }

            RemoveInactiveTouchPoints();

            return updateTouchPoints;
        }

        public TouchPoint2 AddNewTouchPoint(TouchInfo info, UIElement source)
        {
            TouchPoint2 newTouchPoint = null;
            if (!ActiveTouchPoints.ContainsKey(info.TouchDeviceId))
            {
                newTouchPoint = new TouchPoint2(info, source);
                ActiveTouchPoints.Add(info.TouchDeviceId, newTouchPoint);
            }
            else
            {
                newTouchPoint = ActiveTouchPoints[info.TouchDeviceId];
                newTouchPoint.Source = source;
            }

            return newTouchPoint;
        }

        public TouchPoint2 UpdateActiveTouchPoint(TouchInfo info, UIElement source = null)
        {
            TouchPoint2 tPoint = null;

            // Update touch details (i.e. touch path)
            if (ActiveTouchPoints.ContainsKey(info.TouchDeviceId))
            {
                tPoint = ActiveTouchPoints[info.TouchDeviceId];
                tPoint.Update(info);
            }
            else
            {
                tPoint = AddNewTouchPoint(info, source);
            }

            // Touches that are going to be inactive in next frame
            if (info.ActionType == TouchAction.Up.ToTouchActions())
            {
                inactiveTouchPoints.Add(info.TouchDeviceId);
                ActiveTouchPoints.Remove(info.TouchDeviceId);
            }

            return tPoint;
        }

        private void UpdateTouchStates(List<TouchInfo> points)
        {
            TouchStates.Clear();
            foreach (var point in points)
                TouchStates.Add(point.ActionType);
        }

        protected void RemoveInactiveTouchPoints()
        {
            // Removing inactive touch points
            foreach (int touchId in inactiveTouchPoints)
            {
                // TODO: We should track history only if any active rule needs it
                var touchPoint = ActiveTouchPoints[touchId];
                TouchHistoryTracker.Add(touchPoint);

                ActiveTouchPoints.Remove(touchId);
            }

            // All done, clear the pending-removal queue
            inactiveTouchPoints.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// A call to Dispose(false) should only clean up native resources. A call to Dispose(true) should clean up both managed and native resources.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void Dispose(bool value)
        {
            if (value)
            {
                // Clean up both managed and native resources
                CleanUpManagedResources();
                CleanUpNativeResources();
            }
            else
            {
                // Clean up native resources
                CleanUpNativeResources();
            }

        }

        private void CleanUpNativeResources()
        {
            // Clean up native resources
        }

        private void CleanUpManagedResources()
        {
            // Clean up managed resources
        }
    }
}
