using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Text;

using mil.AnotoPen;
using mil.conoto;

using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.Utility;


namespace TouchToolkit.Framework.TouchInputProviders
{
    public class AnotoInputProvider : TouchInputProvider
    {
        public override event FrameChangeEventHandler FrameChanged;

        public override event MultiTouchChangeEventHandler MultiTouchChanged;

        public override event SingleTouchChangeEventHandler SingleTouchChanged;

        private AnotoStreamingServer anotoServer;
        private static ConotoElementManager manager;
        private static ConotoConvert conoto;

        long lastTimeStamp = 0;

        public AnotoInputProvider()
        {
        }
        public override void Init()
        {
            // Initialize configuration
            manager = new ConotoElementManager();

            // Load config file created by MultiPatternConfigurator (or any other conoto config tool)
            // In this example the configuration file is stored in the folder "config".
            // This file maps the A4 sized Anoto-Pattern-Page "70.0.0.1" to a 1024x768 sized screen area.
            manager.LoadConfigFile("config/PatternConfig.pcf.xml");

            // Initialize event converter (needs the element manager that holds the configuration)
            conoto = new ConotoConvert(manager);

            // Initialize AnotoPen with the license key
            mil.AnotoPen.AnotoPen.Initialize("71ad1f27386ec4f5d91e15eca16f46c4");

            // Initialize the real-time streaming receiver of AnotoPen
            anotoServer = new AnotoStreamingServer();
            // Add event handler to receive pen up/down/move events.
            anotoServer.OnStroke += new AnotoStreamingServer.AnotoEventHandler(anotoServer_OnStroke);
            // Start receiver
            anotoServer.Start();
        }

        protected override void Dispose(bool value)
        {
            base.Dispose(value);

            // Stop receiver and dispose AnotoPen assembly. 
            // Otherwise the receiver thread will not be killed and the application process would
            // be still running after exit.
            anotoServer.Stop();
            mil.AnotoPen.AnotoPen.Dispose();
        }

        private void anotoServer_OnStroke(object sender, AnotoPenEvent args)
        {
            // Initialize some variables onto the conoto converter will write to.
            string function; // function string of the area hit by the pen (as configured in configuration)
            long functionid; // function id of the area hit by the pen (as configured in configuration)
            double x, y; // converted x/y coordinates
            TouchInfo info = new TouchInfo();
            TouchAction2 actionType = TouchAction2.Move;

            // handle the type of event we got and call other handler functions
            switch (args.Type)
            {
                case AnotoPenEventType.StrokeStart: // = pen down
                    actionType = TouchAction2.Down;
                    break;
                case AnotoPenEventType.StrokeDrag: // = pen move/drag (there is no hover due the AnotoPen does not support it)
                    actionType = TouchAction2.Move;
                    break;
                case AnotoPenEventType.StrokeEnd: // = pen up
                    actionType = TouchAction2.Up;
                    break;
            }

            // convert AnotoPen coordiantes to the configured ones (in this case, screen coordinates)
            conoto.Convert((ulong)args.PageId, args.X, args.Y,
                           out function, out functionid, out x, out y);

            info.ActionType = actionType;
            info.Position = new Point(x, y);
            info.TouchDeviceId = (int)args.PenId;

            RemoveInactiveTouchPoints();
            UpdateActiveTouchPoint(info);
            foreach (var point in ActiveTouchPoints.Values)
            {
                if (actionType == TouchAction2.Down)
                {
                    point.UpdateSource();
                }
            }

            //Call the delegates
            if (SingleTouchChanged != null)
            {
                foreach (var point in ActiveTouchPoints.Values)
                {
                    SingleTouchChanged(this, new SingleTouchEventArgs(point));
                }
            }

            if (MultiTouchChanged != null)
            {
                MultiTouchChanged(this, new MultiTouchEventArgs(ActiveTouchPoints.Values.ToList<TouchPoint2>()));
            }

            if (FrameChanged != null)
            {
                FrameInfo finfo = new FrameInfo();
                List<TouchPoint2> points = ActiveTouchPoints.Values.ToList<TouchPoint2>();
                List<TouchInfo> infos = new List<TouchInfo>();
                foreach (var point in points)
                {
                    TouchInfo inf = new TouchInfo();
                    inf.ActionType = point.Action.ToTouchActions();
                    inf.Position = point.Position;
                    inf.TouchDeviceId = point.TouchDeviceId;
                    infos.Add(inf);
                }

                finfo.Touches = infos;
                finfo.TimeStamp = args.Timestamp;
                finfo.WaitTime = (int)args.Timestamp - (int)lastTimeStamp;
                FrameChanged(this, finfo);
            }
            lastTimeStamp = (int)args.Timestamp;
        }
    }
}
