using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.Framework.TouchInputProviders;
using libSMARTMultiTouch.Table;
using libSMARTMultiTouch.Input;
using System.Windows;
using System.Windows.Controls;
using TouchToolkit.Framework.GestureEvents;
using TouchToolkit.GestureProcessor.Objects;
using System.Windows.Shapes;
using System.Windows.Media;
using TouchToolkit.Framework;
using System.Threading;

namespace SMARTTabletop_Application.Providers
{
    public class SMARTTabletopTouchInputProvider : TouchInputProvider
    {
        public override event TouchInputProvider.FrameChangeEventHandler FrameChanged;

        public override event TouchInputProvider.SingleTouchChangeEventHandler SingleTouchChanged;

        public override event TouchInputProvider.MultiTouchChangeEventHandler MultiTouchChanged;

        private MainWindow _mainWindow;
        private const int _frameRate = 30; //every 30 msecs
        private Thread _backgroundThread;
        private ThreadStart _backgrounndThreadStart;

        private Dictionary<int, TouchPoint2> _activeTouchPoints = new Dictionary<int, TouchPoint2>();
        private Dictionary<int, TouchInfo> _activeTouchInfos = new Dictionary<int, TouchInfo>();

        public SMARTTabletopTouchInputProvider(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Init()
        {
            libSMARTMultiTouch.Input.TouchInputManager.AddTouchContactDownHandler(_mainWindow, new TouchContactEventHandler(TouchContactDown));
            libSMARTMultiTouch.Input.TouchInputManager.AddTouchContactUpHandler(_mainWindow, new TouchContactEventHandler(TouchContactUp));
            libSMARTMultiTouch.Input.TouchInputManager.AddTouchContactMoveHandler(_mainWindow, new TouchContactEventHandler(TouchMove));

            _backgrounndThreadStart = new ThreadStart(RaiseEvents);
            _backgroundThread = new Thread(_backgrounndThreadStart);
            _backgroundThread.Start();
        }

        private void TouchContactDown(object sender, TouchContactEventArgs e)
        {
            UpdateActiveTouchPoints(TouchAction2.Down, e);
        }

        private void TouchContactUp(object sender, TouchContactEventArgs e)
        {
            UpdateActiveTouchPoints(TouchAction2.Up, e);
        }

        private void TouchMove(object sender, TouchContactEventArgs e)
        {
            UpdateActiveTouchPoints(TouchAction2.Move, e);
        }

        private void RaiseEvents()
        {
            if (_activeTouchPoints.Count > 0)
            {
                Action act = delegate
                {
                    if (SingleTouchChanged != null)
                    {
                        foreach (var touchPoint in _activeTouchPoints.Values)
                        {
                            SingleTouchChanged(this, new SingleTouchEventArgs(touchPoint));
                        }
                    }

                    if (MultiTouchChanged != null)
                    {
                        MultiTouchChanged(this, new MultiTouchEventArgs(_activeTouchPoints.Values.ToList<TouchPoint2>()));
                    }

                    if (FrameChanged != null)
                    {
                        FrameChanged(this, new FrameInfo() { TimeStamp = DateTime.Now.Ticks, Touches = _activeTouchInfos.Values.ToList<TouchInfo>() });
                    }
                };

                GestureFramework.LayoutRoot.Dispatcher.Invoke(act, null);

                // Clear the local cache
                _activeTouchInfos.Clear();
                _activeTouchPoints.Clear();
            }

            // Wait for 30 msecs, then raise the events again
            Thread.Sleep(_frameRate);
            RaiseEvents();
        }

        public void UpdateActiveTouchPoints(TouchAction2 action, TouchContactEventArgs e)
        {
            Point position = e.TouchContact.GetPosition(GestureFramework.LayoutRoot);

            TouchInfo info = new TouchInfo();

            info.ActionType = action;

            info.Position = position;

            info.TouchDeviceId = e.TouchContact.ID;

            TouchPoint2 touchPoint = null;

            //If it is contact down, we want to add the point, otherwise we want to update that particular point
            if (action == TouchAction2.Down)
            {
                //add the new touch point to the base
                touchPoint = base.AddNewTouchPoint(info, e.OriginalSource as UIElement);
            }
            else
            {
                touchPoint = base.UpdateActiveTouchPoint(info);
            }

            // Update local cache
            if (_activeTouchPoints.ContainsKey(info.TouchDeviceId))
            {
                _activeTouchPoints[info.TouchDeviceId] = touchPoint;
                _activeTouchInfos[info.TouchDeviceId] = info;
            }
            else
            {
                _activeTouchPoints.Add(info.TouchDeviceId, touchPoint);
                _activeTouchInfos.Add(info.TouchDeviceId, info);
            }
        }

    }

}
