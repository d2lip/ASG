using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Reflection;

using TouchToolkit.GestureProcessor.Feedbacks.TouchFeedbacks;
using TouchToolkit.GestureProcessor.ReturnTypes;
using TouchToolkit.Framework;
using TouchToolkit.Framework.TouchInputProviders;
using TouchToolkit.GestureProcessor.Gesture_Definitions;
using System.Diagnostics;

namespace TUIOApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<UIElement, int> _objectRegistration = new Dictionary<UIElement, int>();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(0);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var provider = new TUIOProvider();
            //var provider = new Windows7TouchInputProvider();

            GestureFramework.Initialize(provider, LayoutRoot, Assembly.GetExecutingAssembly());
            //GestureFramework.ShowDebugPanel(GestureFramework.DebugPanels.GestureRecorder);
            GestureFramework.AddTouchFeedback(typeof(BubblesPath));
            SetImages(false);
        }
        //Sets the events of the images
        private void SetImages(bool randomPosition)
        {
            foreach (var bitmap in LayoutRoot.Children)
            {
                GestureFramework.EventManager.AddEvent(bitmap as Image, Gestures.Zoom, ZoomCallback);
                GestureFramework.EventManager.AddEvent(bitmap as Image, Gestures.Pinch, PinchCallback);
                GestureFramework.EventManager.AddEvent(bitmap as Image, "Drag", DragCallback);
                GestureFramework.EventManager.AddEvent(bitmap as Image, Gestures.Rotate, RotateCallback);
            }

            //GestureFramework.EventManager.AddEvent(LayoutRoot, Gestures.Lasso, LassoCallback);
            //GestureFramework.EventManager.AddEvent(LayoutRoot, "Box", BoxCallback);
        }

        #region CallBacks

        private void BoxCallback(UIElement sender, GestureEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Box");
        }

        private void DragCallback(UIElement sender, GestureEventArgs e)
        {
            var posChanged = e.Values.Get<PositionChanged>();

            var touchActions = e.Values.Get<TouchActions>();
            foreach (var action in touchActions)
            {
                Debug.WriteLine("Touch action: " + action.Action);
            }

            if (posChanged != null && sender != null)
            {
                ThreadStart start = delegate()
                {
                    sender.Dispatcher.Invoke(DispatcherPriority.Send,
                                      new Action<UIElement, PositionChanged>(MoveItem), sender, posChanged);
                };
                start.Invoke();
            }

        }

        private void ZoomCallback(UIElement sender, GestureEventArgs e)
        {
            var dis = e.Values.Get<DistanceChanged>();

            if (dis != null)
            {
                ThreadStart start = delegate()
                {
                    sender.Dispatcher.Invoke(DispatcherPriority.Send,
                                      new Action<Image, double>(Resize), sender as Image, dis.Delta);
                };
                start.Invoke();
            }

        }

        private void PinchCallback(UIElement sender, GestureEventArgs e)
        {
            var dis = e.Values.Get<DistanceChanged>();
            if (dis != null)
            {
                ThreadStart start = delegate()
                {
                    sender.Dispatcher.Invoke(DispatcherPriority.Send,
                                      new Action<Image, double>(Resize), sender as Image, dis.Delta);
                };
                start.Invoke();
            }
        }

        private void RotateCallback(UIElement sender, GestureEventArgs e)
        {
            var slopeChanged = e.Values.Get<SlopeChanged>();
            if (slopeChanged != null)
            {
                var img = sender as Image;
                if (img != null)
                {
                    ThreadStart start = delegate()
                    {
                        sender.Dispatcher.Invoke(DispatcherPriority.Send,
                                          new Action<Image, double>(Rotate), sender as Image, Math.Round(slopeChanged.Delta, 1));
                    };
                    start.Invoke();
                }
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
        #endregion

        #region Helper Functions

        private bool ValidateID(int id, UIElement obj)
        {
            if (_objectRegistration.ContainsKey(obj))
            {
                return _objectRegistration[obj] == id;
            }
            else
            {
                _objectRegistration.Add(obj, id);
                return true;
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

        private void Resize(Image image, double delta)
        {
            if (image.Height + delta > 0)
                image.Height += delta;

            if (image.Width + delta > 0)
                image.Width += delta;
        }

        private void MoveItem(UIElement sender, PositionChanged posChanged)
        {
            Image item = sender as Image;
            double x = (double)item.GetValue(Canvas.LeftProperty);
            double y = (double)item.GetValue(Canvas.TopProperty);

            item.SetValue(Canvas.LeftProperty, x + posChanged.X);
            item.SetValue(Canvas.TopProperty, y + posChanged.Y);
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

        private List<Polygon> hitlist = new List<Polygon>();
        private void HighlightItems(object param)
        {
            Thread.Sleep(15);

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
                        RectangleGeometry area2 = new RectangleGeometry(area);

                        hitlist.Clear();

                        VisualTreeHelper.HitTest(selectedArea, null, HitTestCallBack, new GeometryHitTestParameters(area2));

                        foreach (var e in hitlist)
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

        private HitTestResultBehavior HitTestCallBack(HitTestResult result)
        {
            IntersectionDetail intersectionDetail =
             (result as GeometryHitTestResult).IntersectionDetail;

            Polygon resulting = result.VisualHit as Polygon;

            if (resulting != null && intersectionDetail == IntersectionDetail.Intersects)
            {
                hitlist.Add(resulting);
            }
            else if (resulting != null && intersectionDetail == IntersectionDetail.FullyContains)
            {
                hitlist.Add(resulting);
            }
            else if (resulting != null && intersectionDetail == IntersectionDetail.FullyInside)
            {
                hitlist.Add(resulting);
            }
            return HitTestResultBehavior.Continue;
        }
        #endregion
    }
}
