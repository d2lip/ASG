using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.Exceptions;

namespace TouchToolkit.Framework.Utility
{
    public static class TouchPointExtensionMethods
    {
        public static TouchPointCollection ToTouchPointCollection(this List<TouchInfo> self)
        {
            TouchPointCollection list = Activator.CreateInstance(typeof(TouchPointCollection)) as TouchPointCollection;
            foreach (TouchInfo touchInfo in self)
            {
                TouchPoint point = touchInfo.ToTouchPoint();
                list.Add(point);
            }

            return list;
        }

        public static List<TouchInfo> ToTouchInfo(this TouchPointCollection self)
        {
            List<TouchInfo> list = new List<TouchInfo>(self.Count);
            foreach (var item in self)
            {
                list.Add(item.ToTouchInfo());
            }

            return list;
        }

        public static TouchInfo ToTouchInfo(this TouchPoint self)
        {
            return new TouchInfo()
            {
                ActionType = self.Action.ToTouchActions(),
                Position = self.Position,
                TouchDeviceId = self.TouchDevice.Id
            };
        }

        public static TouchPoint ToTouchPoint(this TouchInfo self)
        {

#if SILVERLIGHT
            TouchPoint point = new TouchPoint();
            point.SetValue("Position", self.Position);
#else
            TouchAction action = self.ActionType.ToTouchAction();
            Rect rect = new Rect(self.Position, self.Position);

            TouchPoint point = new TouchPoint(null, self.Position, rect, action);
#endif
            return point;
        }

        /// <summary>
        /// Creates a copy of the list
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ValidSetOfTouchPoints Copy(this List<TouchPoint2> self)
        {
            ValidSetOfTouchPoints copy = new ValidSetOfTouchPoints(self.Count);

            foreach (var item in self)
            {
                copy.Add(item);
            }

            return copy;

        }

        /// <summary>
        /// Reduces the number of points in a stroke of touch without losing any cretical data
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Point[] ToFilteredPoints(this StylusPointCollection self)
        {
            //TODO: This is a place holder, Ideally, this function should filter and reduce the ammount of points to increase the performance
            Point[] filteredPoints = new Point[self.Count];
            int i = 0;
            foreach (var item in self)
            {
                filteredPoints[i++] = new Point(item.X, item.Y);
            }

            return filteredPoints;
        }

        /// <summary>
        /// Adds the items from the specified collection into existing collection
        /// </summary>
        /// <param name="self"></param>
        /// <param name="itemsToAdd"></param>
        public static void Add(this ValidSetOfPointsCollection self, ValidSetOfPointsCollection itemsToAdd)
        {
            foreach (ValidSetOfTouchPoints item in itemsToAdd)
            {
                self.Add(item);
            }
        }

        public static Point ToPoint(this StylusPoint point)
        {
            // TODO: move to more appropriate location [FYI, same extension method also exists in Gesture assembly]
            return new Point(point.X, point.Y);
        }

        /// <summary>
        /// Determines if the specified type implements certain interface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool IsTypeOf(this Type type, Type interfaceType)
        {
            // TODO: move to more appropriate location [FYI, same extension method also exists in Gesture assembly]
            bool result = false;
            Type[] interfaces = type.GetInterfaces();
            foreach (var i in interfaces)
            {
                if (i.IsAssignableFrom(interfaceType))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Removes matching items of specified list from existing list
        /// </summary>
        /// <param name="self"></param>
        /// <param name="sets"></param>
        public static void Remove(this ValidSetOfTouchPoints self, ValidSetOfPointsCollection sets)
        {
            foreach (var set in sets)
            {
                foreach (var point in set)
                {
                    self.Remove(point);
                }
            }
        }

        public static TouchAction2 ToTouchActions(this TouchAction action)
        {
            if (action == TouchAction.Down)
                return TouchAction2.Down;
            else if (action == TouchAction.Up)
                return TouchAction2.Up;
            else if (action == TouchAction.Move)
                return TouchAction2.Move;
            else
                throw new FrameworkException("Unknown touch action type");
        }

        public static TouchAction ToTouchAction(this TouchAction2 action)
        {
            if (action == TouchAction2.Down)
                return TouchAction.Down;
            else if (action == TouchAction2.Up)
                return TouchAction.Up;
            else if (action == TouchAction2.Move)
                return TouchAction.Move;
            else
                throw new FrameworkException("Unknown touch action type");
        }

        /// <summary>
        /// Determines the source ui-element of the specified touch using hit-test
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static UIElement UpdateSource(this TouchPoint2 self)
        {
            UIElement uiElement = null;
            Point point = self.Stroke.StylusPoints[0].ToPoint();

            // This code block is only for Silverlight platform. We do not need hitTesting to find the UIElement as
            // the touchDown event already provides this data

#if SILVERLIGHT
            var list = VisualTreeHelper.FindElementsInHostCoordinates(point, GestureFramework.LayoutRoot);
            var e1 = list.GetEnumerator();

            if (e1.MoveNext())
                uiElement = e1.Current;
#else
            Action action = delegate()
            {
                if (GestureFramework.LayoutRoot.Parent == null)
                {
                    //TODO: Its a fake UI created by the automated UnitTest. The VisualTreeHelper won't work in this case, so find an alternet way

                    //Temporary workaround - point to root canvas
                    uiElement = GestureFramework.LayoutRoot;
                }
                else
                {
                    var hitTestResult = VisualTreeHelper.HitTest(GestureFramework.LayoutRoot, point);

                    if (hitTestResult == null)
                        uiElement = GestureFramework.LayoutRoot;
                    else
                        uiElement = hitTestResult.VisualHit as UIElement;
                }
            };
            if (!GestureFramework.LayoutRoot.Dispatcher.CheckAccess())
            {
                GestureFramework.LayoutRoot.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, action);
            }
            else
            {
                action();
            }
#endif
            self.Source = uiElement;

            return uiElement;
        }

        /// <summary>
        /// Merges the frame lists from different gestureInfo into one list
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static GestureInfo Merge(this List<GestureInfo> self)
        {
            // touchIdPadding is used to ensure unique id for each touch point when multiple sets are merged
            int touchIdPadding = 0;

            DateTime startTime = DateTime.Now;
            DateTime timeline = DateTime.Now;
            GestureInfo mergedGestureInfo = new GestureInfo();

            int groupId = 0;
            foreach (var gesture in self)
            {
                touchIdPadding += 50;

                // 1. Update timestamp of each frame in all list
                timeline = startTime;
                foreach (var frame in gesture.Frames)
                {
                    timeline = timeline.AddMilliseconds(frame.WaitTime);
                    frame.TimeStamp = timeline.Ticks;

                    // 1.1 Update groupIds for individual touch
                    foreach (var touch in frame.Touches)
                    {
                        touch.GroupId = groupId;
                        touch.TouchDeviceId += touchIdPadding;
                    }
                }

                // 2. Combine lists into one
                mergedGestureInfo.Frames.AddRange(gesture.Frames);

                groupId++;
            }

            // 3. Sort the unified list 
            mergedGestureInfo.Frames.Sort(new FrameInfoComparer());

            return mergedGestureInfo; 
        }
    }
}
