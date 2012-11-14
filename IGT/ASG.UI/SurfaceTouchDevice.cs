using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Input;
using System.Windows.Controls;

namespace Blake.NUI.WPF.Surface
{
    public class SurfaceTouchDevice : TouchDevice
    {
        #region Class Members

        private static Dictionary<int, SurfaceTouchDevice> deviceDictionary = new Dictionary<int, SurfaceTouchDevice>();

        public TouchDevice Contact { get; protected set; }

        #endregion

        #region Public Static Methods

        public static void RegisterEvents(FrameworkElement root)
        {
            root.TouchEnter += new EventHandler<TouchEventArgs>(ContactDown);
            root.PreviewTouchMove += new EventHandler<TouchEventArgs>(ContactChanged);
            root.TouchLeave += new EventHandler<TouchEventArgs>(ContactUp);
        }

        public static TouchDevice GetContact(TouchDevice device)
        {
            try
            {
                SurfaceTouchDevice surfaceDevice = device as SurfaceTouchDevice;

                if (surfaceDevice == null)
                    return null;

                return surfaceDevice.Contact;
            }
            //Ignore InvalidOperationException due to race condition on Surface hardware
            catch (InvalidOperationException)
            { }
            return null;
        }

        #endregion

        #region Private Static Methods

        private static void ContactDown(object sender, TouchEventArgs e)
        {
            try
            {
                if (deviceDictionary.Keys.Contains(e.TouchDevice.Id))
                {
                    return;
                }
                SurfaceTouchDevice device = new SurfaceTouchDevice(e.TouchDevice);
                deviceDictionary.Add(e.TouchDevice.Id, device);

                device.SetActiveSource(e.Device.ActiveSource);
                device.Activate();
                device.ReportDown();
            }
            //Ignore InvalidOperationException due to race condition on Surface hardware
            catch (InvalidOperationException)
            { }
        }

        private static void ContactChanged(object sender, TouchEventArgs e)
        {
            try
            {
                int id = e.TouchDevice.Id;
                if (!deviceDictionary.Keys.Contains(id))
                {
                    ContactDown(sender, e);
                }

                SurfaceTouchDevice device = deviceDictionary[id];
                if (device != null &&
                    device.IsActive)
                {
                    device.Contact = e.TouchDevice;
                    device.ReportMove();
                }
            }
            //Ignore InvalidOperationException due to race condition on Surface hardware
            catch (InvalidOperationException)
            { }
        }

        private static void ContactUp(object sender, TouchEventArgs e)
        {
            try
            {
                int id = e.TouchDevice.Id;
                if (!deviceDictionary.Keys.Contains(id))
                {
                    ContactDown(sender, e);
                }
                SurfaceTouchDevice device = deviceDictionary[id];

                if (device != null &&
                    device.IsActive)
                {
                    device.ReportUp();
                    device.Deactivate();

                    deviceDictionary.Remove(id);
                }
            }
            //Ignore InvalidOperationException due to race condition on Surface hardware
            catch (InvalidOperationException)
            { }
        }

        #endregion

        #region Constructors

        public SurfaceTouchDevice(TouchDevice contact) :
            base(contact.Id)
        {
            Contact = contact;
        }

        #endregion

        #region Overridden methods

        public override TouchPointCollection GetIntermediateTouchPoints(IInputElement relativeTo)
        {
            TouchPointCollection collection = new TouchPointCollection();
            UIElement element = relativeTo as UIElement;

            if (element == null)
                return collection;
            try
            {
                foreach (TouchPoint c in Contact.GetIntermediateTouchPoints(relativeTo))
                {
                    Point point = c.Position;// c.GetPosition(null);
                    if (relativeTo != null)
                    {
                        point = this.ActiveSource.RootVisual.TransformToDescendant((Visual)relativeTo).Transform(point);
                    }
                    collection.Add(new TouchPoint(this, point, c.Bounds, TouchAction.Move));
                }
            }
            //Ignore InvalidOperationException due to race condition on Surface hardware
            catch (InvalidOperationException)
            { }
            return collection;
        }

        public override TouchPoint GetTouchPoint(IInputElement relativeTo)
        {
            try
            {
                Point point = this.Contact.GetPosition(relativeTo);
                if (relativeTo != null)
                {
                    point = this.ActiveSource.RootVisual.TransformToDescendant((Visual)relativeTo).Transform(point);
                }

//#warning Provide an System.Windows.IInputElement for GetBounds
                Rect rect = this.Contact.GetBounds(relativeTo);

                return new TouchPoint(this, point, rect, TouchAction.Move);
            }
            //Ignore InvalidOperationException due to race condition on Surface hardware
            catch (InvalidOperationException)
            { }
            return null;
        }

        #endregion

    }
}
