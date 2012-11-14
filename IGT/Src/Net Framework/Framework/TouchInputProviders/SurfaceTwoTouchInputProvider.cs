using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.Framework.Utility;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.TouchInputProviders;
using Microsoft.Surface;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;
using Microsoft.Surface.Core;
using ContactEventHandler = Microsoft.Surface.Core.TouchEventArgs;
using TouchToolkit.Framework;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TouchToolkit.GestureProcessor.Utility;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Input;

namespace SurfaceApplication.Providers
{
    public class SurfaceTwoTouchInputProvider : TouchInputProvider
    {
        public override event TouchInputProvider.FrameChangeEventHandler FrameChanged;
        public override event TouchInputProvider.SingleTouchChangeEventHandler SingleTouchChanged;
        public override event TouchInputProvider.MultiTouchChangeEventHandler MultiTouchChanged;

        private Window _window;
        public TouchTarget _contactTarget;

        private int i = 0;
        public Dictionary<DateTime, System.Drawing.Bitmap> snapshots = new Dictionary<DateTime, System.Drawing.Bitmap>();
        static private Microsoft.Surface.Core.ImageMetrics normalizedMetrics;

        static private byte[] normalizedImage;
        private long oldTimeStamp;
        private System.Windows.Size MinimumSize = new System.Windows.Size(1.5, 1.5);
     


        public SurfaceTwoTouchInputProvider(SurfaceWindow window)
        {
            _window = window;
           
            
        }

        public SurfaceTwoTouchInputProvider(Window window)
        {
            _window = window;

        }

        private Dictionary<int, TouchPoint2> _activeTouchPoints = new Dictionary<int, TouchPoint2>();
        private Dictionary<int, TouchInfo> _activeTouchInfos = new Dictionary<int, TouchInfo>();

        public override void Init()
        {
            // Add the necessary event handlers
            _window.TouchEnter += TouchDown;
            _window.PreviewTouchMove += TouchMove;
            _window.TouchLeave += TouchUp;


           // IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(_window).Handle;
          //  _contactTarget = new Microsoft.Surface.Core.TouchTarget(hwnd);

            Touch.FrameReported += _contactTarget_FrameReceived;
          //  _contactTarget.FrameReceived += _contactTarget_FrameReceived;
            
          //  EnableRawImage();

         //   _contactTarget.EnableInput();

        }

        private void EnableRawImage()
        {
            _contactTarget.EnableImage(Microsoft.Surface.Core.ImageType.Normalized);
        //    _contactTarget.FrameReceived += _contactTarget_FrameReceived;
        }

        private void DisableRawImage()
        {
            _contactTarget.DisableImage(Microsoft.Surface.Core.ImageType.Normalized);
        //    _contactTarget.FrameReceived -= _contactTarget_FrameReceived;
        }


        //  private void getImage(object sender, DoWorkEventArgs args)
        private void getImage(FrameReceivedEventArgs e)
        {
            //    FrameReceivedEventArgs e = args.Argument as FrameReceivedEventArgs;
            bool imageAvailable = false;
            int paddingLeft,
                  paddingRight;
            try
            {
                if (normalizedImage == null)
                {
                    imageAvailable = e.TryGetRawImage(Microsoft.Surface.Core.ImageType.Normalized,
                       0, 0,
                        Microsoft.Surface.Core.InteractiveSurface.PrimarySurfaceDevice.Width,
                        Microsoft.Surface.Core.InteractiveSurface.PrimarySurfaceDevice.Height,
                        out normalizedImage,
                        out normalizedMetrics,
                        out paddingLeft,
                        out paddingRight);
                }
                else
                {

                    imageAvailable = e.UpdateRawImage(Microsoft.Surface.Core.ImageType.Normalized,
                         normalizedImage,
                         0, 0,
                         Microsoft.Surface.Core.InteractiveSurface.PrimarySurfaceDevice.Width,
                         Microsoft.Surface.Core.InteractiveSurface.PrimarySurfaceDevice.Height);
                }

            }

            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            if (imageAvailable)
            {
                imageAvailable = false;

                GCHandle h = GCHandle.Alloc(normalizedImage, GCHandleType.Pinned);
                IntPtr ptr = h.AddrOfPinnedObject();
                System.Drawing.Bitmap imageBitmap = new System.Drawing.Bitmap(normalizedMetrics.Width,
                                      normalizedMetrics.Height,
                                      normalizedMetrics.Stride,
                                      System.Drawing.Imaging.PixelFormat.Format8bppIndexed,
                                      ptr);


                ImageHelper.BinarizeImage(imageBitmap);
                System.Drawing.Bitmap imgClone = new System.Drawing.Bitmap(imageBitmap);
                imgClone.Palette = imageBitmap.Palette;
                DateTime now = DateTime.Now;
                snapshots.Add(now, imgClone);

                //  imgClone.Save(".\\aft\\BEFORE" + DateTime.Now.ToString("hh-mm-ss-fff") + ".jpg");

            }

        }

        private bool IsThereAnyBlob(List<TouchPoint2> touchPoints)
        {
            foreach (TouchPoint2 p in touchPoints)
            {
                if (!p.isFinger && p.Tag == null)
                    return true;
            }

            return false;


        }

        private List<Bitmap> getImageAtTouchTime(TouchPoint2 point)
        {
            List<Bitmap> result = new List<Bitmap>();
            var images = (from imgs in snapshots
                          where
                              imgs.Key.Ticks >= point.StartTime.Ticks && imgs.Key.Ticks <= point.EndTime.Ticks
                          select imgs);

            foreach (KeyValuePair<DateTime, Bitmap> img in images.ToList())
            {
                result.Add(img.Value);
                snapshots.Remove(img.Key);
            }
            return result;
        }


        private List<Bitmap> AddSnapshots(TouchPoint2 point)
        {
            List<Bitmap> result = new List<Bitmap>();
            if (!point.isFinger && point.Tag == null)
            {
                List<Bitmap> copy = getImageAtTouchTime(point);
                if (copy == null)
                    return result;

                foreach (Bitmap img in copy)
                {
                    result.Add(img);
                }
            }

            return result;
        }



        private void _contactTarget_FrameReceived(object sender, TouchFrameEventArgs e)
        {
            if (_activeTouchInfos.Values.Count > 0)
            {
                List<TouchInfo> touchInfoList = _activeTouchInfos.Values.ToList<TouchInfo>();
                List<TouchPoint2> touchPoints = _activeTouchPoints.Values.ToList<TouchPoint2>();


            /*    long now = DateTime.Now.Ticks;
                long ticksDelta = Math.Abs(now - oldTimeStamp);
                //Update image every 100 milliseconds.
                if ((IsThereAnyBlob(touchPoints)) && (ticksDelta > 30 * 10000))
                {
                //    getImage(e);
                    oldTimeStamp = now;
                }*/


                // Raise "SingleTouchChanged" event if necessary
                if (SingleTouchChanged != null)
                {
                    foreach (var touchPoint in touchPoints)
                    {
                        SingleTouchChanged(this, new SingleTouchEventArgs(touchPoint));
                    }
                }

                // Raise "MultiTouchChanged" event if necessary
                if (MultiTouchChanged != null)
                {
                    MultiTouchChanged(this, new MultiTouchEventArgs(touchPoints));
                }

                // Raise "MultiTouchChanged" event if necessary
                if (FrameChanged != null)
                {
                    var frameInfo = new FrameInfo() { TimeStamp = DateTime.Now.Ticks, Touches = touchInfoList };
                    FrameChanged(this, frameInfo);

                  //  Debug.WriteLine(touchInfoList[0].ActionType.ToString());
                }

                // Clean up local cache
                foreach (var touchInfo in touchInfoList)
                {
                    if (touchInfo.ActionType == TouchAction2.Up)
                    {
                        _activeTouchInfos.Remove(touchInfo.TouchDeviceId);
                        _activeTouchPoints.Remove(touchInfo.TouchDeviceId);
                        
                    }
                }
            }
        }


        public TouchInfo GetInfoFromContactEvent(TouchAction2 action, System.Windows.Input.TouchEventArgs e)
        {
            //Get the  point position from the ContactEventArgs (can optionally use e.Contact.getCenterPosition here for more accuracy)
            System.Windows.Point position = e.GetTouchPoint(GestureFramework.LayoutRoot).Position; // new System.Windows.Point(e.TouchPoint.CenterX, e.TouchPoint.CenterY);

            //Create a new touchinfo which will be used later to add a touchpoint
            TouchInfo info = new TouchInfo();
            

            //IF not Using in surface there will be not size or tag, therefore all the points are fingers.
            System.Windows.Size size = e.TouchDevice.GetTouchPoint(GestureFramework.LayoutRoot).Size;
            if (size.Height <= MinimumSize.Height && size.Width <= MinimumSize.Width)
            {
                info.IsFinger = true;
                info.Tag = null;


            }
            else
            {
                info.IsFinger = e.TouchDevice.GetTouchPoint(GestureFramework.LayoutRoot).ToTouchInfo().IsFinger;

                if (e.GetTouchPoint(GestureFramework.LayoutRoot).ToTouchInfo().Tag != null)
                {

                   // info.Tag = e.GetTouchPoint(GestureFramework.LayoutRoot).ToTouchInfo().Tag;
                    /*
                    switch (e.TouchPoint.Tag.Type)
                    {
                        case Microsoft.Surface.Presentation.TagType.Byte:
                            info.Tag = e.Contact.Tag.Byte.ToString();
                            break;
                        case Microsoft.Surface.Presentation.TagType.Identity:
                            info.Tag = e.Contact.Tag.Identity.ToString();
                            break;
                        default:
                            info.Tag = null;
                            break;

                    }*/

                }
                else if (!info.IsFinger)
                {
                    //info.Bounds = new Rect((double)e.TouchPoint.CenterX, (double)e.TouchPoint.CenterY, (double)e.TouchPoint.Bounds.Width, (double)e.TouchPoint.Bounds.Height);
                    info.Bounds = e.GetTouchPoint(GestureFramework.LayoutRoot).ToTouchInfo().Bounds;
                }            
            }

            //Set the action type to the passed in action
            info.ActionType = action;

            //Set the position of the touchinfo to the previously found position from e
            info.Position = position;

            //Set the deviceid of the touchinfo to the id of the contact
            info.TouchDeviceId = e.GetTouchPoint(GestureFramework.LayoutRoot).TouchDevice.Id;
            
            return info;

        }



      
        public void TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            UpdateActiveTouchPoints(TouchAction2.Up, e);
        }

        public void TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            UpdateActiveTouchPoints(TouchAction2.Move, e);
        }

        public void TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {

            UpdateActiveTouchPoints(TouchAction2.Down, e);
        }

        public TouchPoint2 UpdateActiveTouchPoints(TouchAction2 action, System.Windows.Input.TouchEventArgs e)
        {
            TouchPoint2 touchPoint = null;
            TouchInfo info = GetInfoFromContactEvent(action, e);

            //If it is contact down, we want to add the point, otherwise we want to update that particular point
            if (action == TouchAction2.Down)
            {
                //add the new touch point to the base
                touchPoint = base.AddNewTouchPoint(info, e.TouchDevice.Captured as UIElement);

            }
            else
            {
                //add the new touch point to the base    
                touchPoint = base.UpdateActiveTouchPoint(info, e.TouchDevice.Captured as UIElement);
            }

           /* Image<Bgr, Byte> bitImg = ImageHelper.getBetterImage(AddSnapshots(touchPoint), touchPoint);

            if (bitImg != null)
            {
                touchPoint.Snapshot = ImageHelper.ExtractContourAndHull(bitImg);
            }
            */
            // Update local cache
            if (_activeTouchPoints.ContainsKey(touchPoint.TouchDeviceId))
            {
                //_activeTouchPoints[info.TouchDeviceId] = touchPoint;
                _activeTouchInfos[info.TouchDeviceId].Update(info);
                _activeTouchPoints[info.TouchDeviceId].Update(info);
                //  _activeTouchPoints[info.TouchDeviceId].Snapshot.AddRange(touchPoint.Snapshot);
            }
            else
            {
                _activeTouchPoints.Add(info.TouchDeviceId, touchPoint);
                _activeTouchInfos.Add(info.TouchDeviceId, info);
            }

            return touchPoint;
        }

   



    }
}
