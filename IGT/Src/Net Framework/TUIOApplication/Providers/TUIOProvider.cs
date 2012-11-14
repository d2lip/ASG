using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
using System.Linq;
using System.Text;

using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.Utility;

using TUIO;

namespace TouchToolkit.Framework.TouchInputProviders
{
    class TUIOProvider : TouchInputProvider, TuioListener
    {
        private TuioClient _client;
        private int _port;
        private object _cursorSync = new object();
        private long lastTimeStamp = 0;

        public override event TouchInputProvider.FrameChangeEventHandler FrameChanged;

        public override event TouchInputProvider.SingleTouchChangeEventHandler SingleTouchChanged;

        public override event TouchInputProvider.MultiTouchChangeEventHandler MultiTouchChanged;

        public TUIOProvider(int port = 3333)
        {
            _port = port;
            this.Init();
        }

        public override void Init()
        {
            base.Init();

            _client = new TuioClient(_port);
            _client.addTuioListener(this);
            _client.connect();
        }

        protected override void Dispose(bool value)
        {
            base.Dispose(value);

            _client.disconnect();
        }

        public void addTuioCursor(TuioCursor c)
        {
            TouchInfo info = MakeInfo(c, TouchAction2.Down);
            //UIElement source = PerformHitTest(info.Position);
            RemoveInactiveTouchPoints();
            UpdateActiveTouchPoint(info);
            foreach (var p in ActiveTouchPoints.Values)
            {
                p.UpdateSource();
            }
            CallDelegates(c);
        }

        public void updateTuioCursor(TuioCursor c)
        {
            TouchInfo info = MakeInfo(c, TouchAction2.Move);
            RemoveInactiveTouchPoints();
            UpdateActiveTouchPoint(info);
            CallDelegates(c);
        }

        public void removeTuioCursor(TuioCursor c)
        {
            TouchInfo info = MakeInfo(c, TouchAction2.Up);
            RemoveInactiveTouchPoints();
            UpdateActiveTouchPoint(info);
            CallDelegates(c);
        }

        private void CallDelegates(TuioCursor c)
        {
            CallDelegates(c.getTuioTime());
        }

        private void CallDelegates(TuioTime ftime)
        {
            if (SingleTouchChanged != null)
            {
                foreach (var point in ActiveTouchPoints.Values)
                {
                    SingleTouchChanged(this, new SingleTouchEventArgs(point));
                }
            }

            if (MultiTouchChanged != null)
            {
                List<TouchPoint2> list = ActiveTouchPoints.Values.ToList<TouchPoint2>();
                MultiTouchChanged(this, new MultiTouchEventArgs(list));
            }

            if (FrameChanged != null)
            {
                List<TouchPoint2> points = ActiveTouchPoints.Values.ToList<TouchPoint2>();
                List<TouchInfo> infos = new List<TouchInfo>();
                foreach (var point in points)
                {
                    TouchInfo info = new TouchInfo();
                    info.ActionType = point.Action.ToTouchActions();
                    info.Position = point.Position;
                    info.TouchDeviceId = point.TouchDeviceId;
                    infos.Add(info);
                }
                FrameInfo finfo = new FrameInfo();

                finfo.Touches = infos;
                finfo.TimeStamp = ftime.getMicroseconds();
                finfo.WaitTime = (int)ftime.getMicroseconds() - (int)lastTimeStamp;
                FrameChanged(this, finfo);
            }
            lastTimeStamp = (int)ftime.getMicroseconds();
        }

        private TouchInfo MakeInfo(TuioCursor c, TouchAction2 action)
        {
            Tuple<double, double> screenDim = GetDimensions();
            double screen_width = screenDim.Item1;
            double screen_height = screenDim.Item2;

            double x = c.getX() * screen_width;
            double y = c.getY() * screen_height;
            TouchInfo info = new TouchInfo();
            info.ActionType = action;
            info.Position = new Point(x, y);
            info.TouchDeviceId = (int)c.getSessionID();

            return info;
        }

        private Tuple<double, double> GetDimensions()
        {
            double height = 0;
            double width = 0;
            Action action = new Action(
                    delegate()
                    {
                        height = GestureFramework.LayoutRoot.ActualHeight;
                        width = GestureFramework.LayoutRoot.ActualWidth;
                    }
                );

            if (!GestureFramework.LayoutRoot.Dispatcher.CheckAccess())
            {
                GestureFramework.LayoutRoot.Dispatcher.Invoke(
                  System.Windows.Threading.DispatcherPriority.Send,
                  action);
            }
            else
            {
                action();
            }
            return new Tuple<double, double>(width, height);
        }

        public void refresh(TuioTime ftime)
        {

        }

        private UIElement PerformHitTest(Point point)
        {
            UIElement source = GestureFramework.LayoutRoot;
            Action action = delegate()
            {
                if (GestureFramework.LayoutRoot.Parent == null)
                {
                    //TODO: Its a fake UI created by the automated UnitTest. The VisualTreeHelper won't work in this case, so find an alternet way

                    //Temporary workaround - point to root canvas
                    source = GestureFramework.LayoutRoot;
                }
                else
                {
                    var hitTestResult = VisualTreeHelper.HitTest(GestureFramework.LayoutRoot, point);

                    if (hitTestResult == null)
                        source = GestureFramework.LayoutRoot;
                    else
                        source = hitTestResult.VisualHit as UIElement;

                    return;
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
            return source;
        }
        #region NotUsed
        public void addTuioObject(TuioObject tobj)
        {
            throw new NotImplementedException();
        }

        public void updateTuioObject(TuioObject tobj)
        {
            throw new NotImplementedException();
        }

        public void removeTuioObject(TuioObject tobj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
