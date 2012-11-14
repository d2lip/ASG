using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.Framework.TouchInputProviders;
using SBSDKComWrapperLib;
using System.Windows;
using TouchToolkit.Framework;
using TouchToolkit.Framework.GestureEvents;
using TouchToolkit.GestureProcessor.Objects;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using TouchToolkit.Framework.Utility;
using System.Threading;

namespace SMART_Board_Application.Providers
{
    public class SMARTBoardTouchInputProvider : TouchInputProvider
    {

        public override event TouchInputProvider.FrameChangeEventHandler FrameChanged;
        public override event TouchInputProvider.SingleTouchChangeEventHandler SingleTouchChanged;
        public override event TouchInputProvider.MultiTouchChangeEventHandler MultiTouchChanged;

        private Window mainWindow;

        private const int _frameRate = 30; //every 30 msecs
        private Thread _backgroundThread;
        private ThreadStart _backgrounndThreadStart;

        private ISBSDKBaseClass2 Sbsdk;
        private _ISBSDKBaseClass2Events_Event SbsdkEvents;
        [DllImport("user32.dll")]

        public static extern int RegisterWindowMessageA([MarshalAs(UnmanagedType.LPStr)] string lpString);

        private int SBSDKMessageID = RegisterWindowMessageA("SBSDK_NEW_MESSAGE");


        private Dictionary<int, TouchPoint2> _activeTouchPoints = new Dictionary<int, TouchPoint2>();
        private Dictionary<int, TouchInfo> _activeTouchInfos = new Dictionary<int, TouchInfo>();

        public SMARTBoardTouchInputProvider(Window window)
        {
            mainWindow = window;
            Sbsdk = new SBSDKBaseClass2();
            SbsdkEvents = (_ISBSDKBaseClass2Events_Event)Sbsdk;

            Init();
        }

        public override void Init()
        {

            SbsdkEvents.OnXYMove += new SBSDKComWrapperLib._ISBSDKBaseClass2Events_OnXYMoveEventHandler(this.OnXYMove);
            SbsdkEvents.OnXYDown += new SBSDKComWrapperLib._ISBSDKBaseClass2Events_OnXYDownEventHandler(this.OnXYDown);
            SbsdkEvents.OnXYUp += new SBSDKComWrapperLib._ISBSDKBaseClass2Events_OnXYUpEventHandler(this.OnXYUp);

            var handle = new WindowInteropHelper(mainWindow).Handle;
            var int_handle = handle.ToInt32();
            Sbsdk.SBSDKAttachWithMsgWnd(int_handle, false, int_handle);
            Sbsdk.SBSDKSetSendMouseEvents(handle.ToInt32(), _SBCSDK_MOUSE_EVENT_FLAG.SBCME_NEVER, -1);
            HwndSource.FromHwnd(handle).AddHook(new_message);

            _backgrounndThreadStart = new ThreadStart(RaiseEvents);
            _backgroundThread = new Thread(_backgrounndThreadStart);
            //_backgroundThread.Start();
        }

        private void OnXYMove(int x, int y, int z, int iPointerID)
        {

            UpdateActiveTouchPoints(TouchAction2.Move, new Point(x, y), iPointerID);
        }


        private void OnXYDown(int x, int y, int z, int iPointerID)
        {
            UpdateActiveTouchPoints(TouchAction2.Down, new Point(x, y), iPointerID);
        }

        private void OnXYUp(int x, int y, int z, int iPointerID)
        {
            UpdateActiveTouchPoints(TouchAction2.Up, new Point(x, y), iPointerID);
        }

        IntPtr new_message(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam, ref bool Handled)
        {
            if (Msg == SBSDKMessageID && Sbsdk != null) Sbsdk.SBSDKProcessData();
            return IntPtr.Zero;
        }

        private void RaiseEvents()
        {
            if (_activeTouchPoints.Count > 0)
            {
                Action act = delegate
                {
                    if (SingleTouchChanged != null)
                    {
                        for (int i = 0; i < _activeTouchPoints.Count; i++)
                        {
                            var touchPoint = _activeTouchPoints.Values.ToArray()[i];
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
            //Thread.Sleep(_frameRate);
            
        }

        TouchInfo _lastTouchInfo = null;
        public void UpdateActiveTouchPoints(TouchAction2 action, Point position, int iPointerID)
        {
            TouchInfo info = new TouchInfo();
            info.ActionType = action;
            info.Position = position;
            info.TouchDeviceId = iPointerID;

            TouchPoint2 touchPoint = null;


            //If it is contact down, we want to add the point, otherwise we want to update that particular point
            if (action == TouchAction2.Down)
            {
                //VisualTreeHelper.HitTest(mainWindow, null, new HitTestResultCallback(HitTestCallBack), new GeometryHitTestParameters(m_egHitArea));    

                //add the new touch point to the base
                touchPoint = base.AddNewTouchPoint(info, null);
                touchPoint.UpdateSource();
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

            _lastTouchInfo = info;

            RaiseEvents();
        }
    }
}
