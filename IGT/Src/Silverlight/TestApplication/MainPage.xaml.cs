using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TouchToolkit.GestureProcessor.ReturnTypes;
using TouchToolkit.GestureProcessor.Feedbacks.TouchFeedbacks;
using System.Windows.Media.Imaging;
using TouchToolkit.GestureProcessor.Feedbacks.GestureFeedbacks;
using System.Threading;
using System.Text;
using TouchToolkit.Framework;
using TouchToolkit.Framework.TouchInputProviders;
using System.Reflection;
using TouchToolkit.GestureProcessor.Objects;

namespace TestApplication
{
    public partial class MainPage : UserControl
    {
        private double DEFAULT_TOP_POSITION = 250;
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;

        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            TouchInfo info = new TouchInfo() { ActionType = TouchAction2.Down, GroupId = 0, Position = new Point(1, 1), TouchDeviceId = 14 };
            info.Tags.Add("size", "20");

            FrameInfo fi = new FrameInfo() { TimeStamp = 123121, WaitTime = 10 };
            fi.Touches.Add(info);

            List<FrameInfo> frames = new List<FrameInfo>();
            frames.Add(fi);

            string content = TouchToolkit.Framework.Utility.SerializationHelper.Serialize(frames);
            

            // Initialize Gesture Framework
            TouchInputProvider inputProvider = new SilverlightTouchInputProvider();
            GestureFramework.Initialize(inputProvider, LayoutRoot, Assembly.GetExecutingAssembly());
            //GestureFramework.ShowDebugPanel(GestureFramework.DebugPanels.GestureRecorder);

            // Add touch feedbacks
            GestureFramework.AddTouchFeedback(typeof(BubblesPath));

            // Add gesture feedbacks
            GestureFramework.AddGesturFeedback("lasso", typeof(HighlightSelectedArea));

            // Load UI
            LoadImages(false);
        }

        private void LoadImages(bool randomPosition)
        {
            // Load images from embedded resource
            var bitmaps = GetImages();
            int count = 0;
            foreach (var bitmap in bitmaps)
            {
                Image img = new Image();

                // Load the image in UI
                img.Source = bitmap;

                // TODO: For debugging
                img.Tag = count++;

                SetImageLocation(img, randomPosition);
                LayoutRoot.Children.Add(img);

                // Subscribe to gesture events for image
                GestureFramework.EventManager.AddEvent(img, "zoom", ZoomCallback);
                GestureFramework.EventManager.AddEvent(img, "pinch", PinchCallback);
                GestureFramework.EventManager.AddEvent(img, "drag", DragCallback);
                GestureFramework.EventManager.AddEvent(img, "rotate", RotateCallback);
            }

            // Subscribe to gesture events for the LayoutRoot
            GestureFramework.EventManager.AddEvent(LayoutRoot, "Lasso", LassoCallback);
            GestureFramework.EventManager.AddEvent(LayoutRoot, "left", LeftCallBack);
            GestureFramework.EventManager.AddEvent(LayoutRoot, "right", RightCallBack);
            GestureFramework.EventManager.AddEvent(LayoutRoot, "line", LineCallBack);
        }

        #region Setting image properties
        Random randomNumberGenerator = new Random();
        double lastImageLeftPost = 50;
        private void SetImageLocation(Image img, bool randomPosition)
        {
            double topPosition, leftPosition;
            if (randomPosition)
            {
                topPosition = randomNumberGenerator.Next(100, 700);
                leftPosition = randomNumberGenerator.Next(100, 1200);
            }
            else
            {
                topPosition = DEFAULT_TOP_POSITION;
                leftPosition = lastImageLeftPost;
            }

            img.SetValue(Canvas.TopProperty, topPosition);
            img.SetValue(Canvas.LeftProperty, leftPosition);
            img.Width = 120;
            img.Height = 90;

            lastImageLeftPost = leftPosition + 130;
        }

        /// <summary>
        /// Returns the images from assebly embedded resource
        /// </summary>
        /// <returns></returns>
        private List<BitmapImage> GetImages()
        {
            string[] imageNames = { "Hydrangeas.jpg", "Jellyfish.jpg", "Koala.jpg", "Lighthouse.jpg" };
            List<BitmapImage> images = new List<BitmapImage>();
            foreach (string imgName in imageNames)
            {
                BitmapImage img = TouchToolkit.Framework.Utility.ContentHelper.GetEmbeddedImage(this, imgName);
                images.Add(img);
            }

            return images;
        }
        #endregion

        #region Gesture Events
        private void LineCallBack(UIElement sender, GestureEventArgs e)
        {
            if (e.Values.Get<TouchPoints>().Count > 0)
            {

            }
        }
        private void RightCallBack(UIElement sender, GestureEventArgs e)
        {
            Thread t = new Thread(Scatter);
            t.Start();
        }
        private void LeftCallBack(UIElement sender, GestureEventArgs e)
        {
            Thread t = new Thread(Revert);
            t.Start();
        }

        private void ZoomCallback(UIElement sender, GestureEventArgs e)
        {
            var dis = e.Values.Get<DistanceChanged>();

            if (dis != null)
                Resize(sender as Image, dis.Delta);
        }

        private void RotateCallback(UIElement sender, GestureEventArgs e)
        {
            var slopeChanged = e.Values.Get<SlopeChanged>();
            if (slopeChanged != null)
            {
                var img = sender as Image;
                if (img != null)
                    Rotate(img, Math.Round(slopeChanged.Delta, 1));
            }
        }

        bool rotateInProgress = false;
        private void Rotate(Image img, double delta)
        {
            if (!rotateInProgress & delta != 0)
            {
                rotateInProgress = true;
                RotateTransform rt = img.RenderTransform as RotateTransform;

                if (rt == null)
                    rt = new RotateTransform();

                rt.Angle += delta;
                rt.CenterX = img.Width / 2;
                rt.CenterY = img.Height / 2;

                img.RenderTransform = rt;

                rotateInProgress = false;
            }
        }

        private void PinchCallback(UIElement sender, GestureEventArgs e)
        {
            var dis = e.Values.Get<DistanceChanged>();
            if (dis != null)
                Resize(sender as Image, dis.Delta);
        }

        private void DragCallback(UIElement sender, GestureEventArgs e)
        {
            var posChanged = e.Values.Get<PositionChanged>();
            if (posChanged != null)
            {
                MoveItem(sender, posChanged);
            }
        }

        private void LassoCallback(UIElement sender, GestureEventArgs e)
        {
            TouchPoints touchPoints = e.Values.Get<TouchPoints>();

            // Create a dummy polygon shape using the points of lasso
            // to run a hit test to find the selected elements
            Polygon polygon = CreatePolygon(touchPoints);
            polygon.Fill = new SolidColorBrush(Colors.White);

            polygon.Opacity = .01;
            polygon.Tag = "LASSO_TEST";
            LayoutRoot.Children.Add(polygon);

            Thread t = new Thread(new ParameterizedThreadStart(HighlightItems));


            t.Start(polygon);
        }

        private void MoveItem(UIElement sender, PositionChanged posChanged)
        {
            Image item = sender as Image;
            double x = (double)item.GetValue(Canvas.LeftProperty);
            double y = (double)item.GetValue(Canvas.TopProperty);

            item.SetValue(Canvas.LeftProperty, x + posChanged.X);
            item.SetValue(Canvas.TopProperty, y + posChanged.Y);
        }

        private void Resize(Image image, double delta)
        {
            if (image.Height + delta > 0)
                image.Height += delta;

            if (image.Width + delta > 0)
                image.Width += delta;
        }

        #endregion

        #region UI Helper Methods
        private void DrawLine(Point p1, Point p2)
        {
            System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();

            line.X1 = p1.X;
            line.X2 = p2.X;
            line.Y1 = p1.Y;
            line.Y2 = p2.Y;
            line.Stroke = new SolidColorBrush(Colors.Red);

            LayoutRoot.Children.Add(line);
        }

        Polygon CreatePolygon(TouchPoints points)
        {
            Polygon p = new Polygon();

            foreach (var point in points)
            {
                p.Points.Add(point.Position);
            }

            return p;
        }

        private void HighlightItems(object param)
        {
            Thread.Sleep(10);

            Action action = () =>
            {
                Polygon selectedArea = param as Polygon;

                //TODO: For test only, we need to find more efficient approach
                foreach (var item in LayoutRoot.Children)
                {
                    if (item is Image)
                    {
                        Image img = item as Image;
                        Point p1 = new Point((double)img.GetValue(Canvas.LeftProperty), (double)img.GetValue(Canvas.TopProperty));
                        Rect area = new Rect(p1, new Point(p1.X + img.Width, p1.Y + img.Height));

                        var elements = VisualTreeHelper.FindElementsInHostCoordinates(area, LayoutRoot);

                        var list = elements.ToList();

                        foreach (var e in elements)
                        {
                            if (e is Polygon)
                            {
                                if (e == selectedArea)
                                {
                                    img.Opacity = 0.5;
                                    break;
                                }
                            }
                        }
                    }
                }

                // Hit-test completed, remove the dummy polygon
                LayoutRoot.Children.Remove(selectedArea);
            };

            Dispatcher.BeginInvoke(action);
        }

        private void Scatter()
        {
            Thread.Sleep(105);
            Action action = () =>
            {
                foreach (var element in LayoutRoot.Children)
                {
                    Image img = element as Image;
                    lastImageLeftPost = 50;
                    if (img != null)
                    {
                        SetImageLocation(img, true);
                    }
                }
            };
            Dispatcher.BeginInvoke(action);
        }
        private void Revert()
        {
            Thread.Sleep(105);
            Action action = () =>
            {
                double x = 50;
                double y = DEFAULT_TOP_POSITION;
                foreach (var element in LayoutRoot.Children)
                {
                    Image img = element as Image;
                    if (img != null)
                    {
                        img.SetValue(Canvas.TopProperty, y);
                        img.SetValue(Canvas.LeftProperty, x);
                        x += 130;
                    }
                }
            };
            Dispatcher.BeginInvoke(action);
        }
        #endregion

    }
}
