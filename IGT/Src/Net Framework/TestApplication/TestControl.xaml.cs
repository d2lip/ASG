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
using TouchToolkit.Framework.TouchInputProviders;
using TouchToolkit.Framework;
using TouchToolkit.GestureProcessor.Feedbacks.TouchFeedbacks;
using TouchToolkit.GestureProcessor.ReturnTypes;
using System.Threading;
using System.Reflection;
using TouchToolkit.GestureProcessor.Gesture_Definitions;
using TouchToolkit.GestureProcessor.Feedbacks.GestureFeedbacks;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.Utility;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;
using System.Windows.Ink;
using System.Diagnostics;
using System.Windows.Threading;

namespace TestApplication
{
    /// <summary>
    /// Interaction logic for TestControl.xaml
    /// </summary>
    public partial class TestControl : UserControl
    {
        private TouchInputProvider provider;
        private TouchPoint2 _touchPoint;
        private ValidSetOfTouchPoints points;
        private string gdl;
        private DispatcherTimer timerRecording;
        
        



        private bool recording = false;

        public TestControl(TouchInputProvider provider)
        {
            
            InitializeComponent();
            
            this.provider = provider;
            
            this.Loaded += new RoutedEventHandler(TestControl_Loaded);
            //AntiUnificator.board = board;
            
        }

        Polyline _polyLine = new Polyline() { Stroke = new SolidColorBrush(Colors.Red), StrokeThickness = 1 };

        void TestControl_Loaded(object sender, RoutedEventArgs e)
        {
          
            GestureFramework.Initialize(provider, LayoutRoot, Assembly.GetExecutingAssembly());
          //  GestureFramework.ShowDebugPanel(GestureFramework.DebugPanels.GestureRecorder);  
           
    //        GestureFramework.ShowDebugPanel(GestureFramework.DebugPanels.GestureRecorder);            
          //  GestureFramework.AddTouchFeedback(typeof(BubblesPath));

           // GestureFramework.EventManager.AddEvent(LayoutRoot,Gestures.Recognizer, ActorCallback);
            //GestureFramework.EventManager.AddEvent(LayoutRoot, "Actor", ActorCallback_step1, 0);
            //GestureFramework.EventManager.AddEvent(LayoutRoot, "Actor", ActorCallback_step2, 1);

           // GestureFramework.EventManager.AddEvent(LayoutRoot, "multi_finger_selection", SelectionCallback);
            //LayoutRoot.Children.Add(_polyLine);

            //_polyLine.Points.Add(new Point(100, 100));
            //_polyLine.Points.Add(new Point(200, 150));
            //_polyLine.Points.Add(new Point(100, 200));
            //_polyLine.Points.Add(new Point(100, 100));
      
            SetImages(false);
            this.Height = 1200;
            this.Width = 1200;


            

        }

        private void SelectionCallback(UIElement sender, GestureEventArgs e)
        {
            _polyLine.Points.Clear();
            var points = e.Values.Get<TouchPoints>();
            foreach (var point in points)
            {
                _polyLine.Points.Add(point.Position);
            }

            _polyLine.Points.Add(points[0].Position);



            LassoCallback(sender, e);
        }

        private void ActorCallback(UIElement sender, GestureEventArgs e)
        {
           // log.Text = "Actor gesture detected" + Environment.NewLine + log.Text;
        }

        private void ActorCallback_step1(UIElement sender, GestureEventArgs e)
        {
           // log.Text = "Actor gesture step 1" + Environment.NewLine + log.Text;
        }

        private void ActorCallback_step2(UIElement sender, GestureEventArgs e)
        {
           // log.Text = "Actor gesture step 2" + Environment.NewLine + log.Text;
        }

        //Sets the events of the images
        private void SetImages(bool randomPosition)
        {
            
           /* foreach (var bitmap in LayoutRoot.Children)
            {
                if (bitmap is Image)
                {
                    GestureFramework.EventManager.AddEvent(bitmap as Image, "Zoom", ZoomCallback);
                    GestureFramework.EventManager.AddEvent(bitmap as Image, "Pinch", PinchCallback);
                    GestureFramework.EventManager.AddEvent(bitmap as Image, "Drag", DragCallback);
                    GestureFramework.EventManager.AddEvent(bitmap as Image, "Rotate", RotateCallback);
                }
            }

            GestureFramework.EventManager.AddEvent(LayoutRoot, Gestures.Lasso, LassoCallback);
            GestureFramework.AddGesturFeedback(Gestures.Lasso, typeof(HighlightSelectedArea));

            //Uncomment here to add lasso functionality
            GestureFramework.EventManager.AddEvent(LayoutRoot, "Lasso", LassoCallback);
            * 
            * */
        }

        #region CallBacks

        private void DragCallback(UIElement sender, GestureEventArgs e)
        {
            var posChanged = e.Values.Get<PositionChanged>();
            if (posChanged != null)
            {
                MoveItem(sender, posChanged);
            }
        }



        private void ZoomCallback(UIElement sender, GestureEventArgs e)
        {
            var dis = e.Values.Get<DistanceChanged>();

            if (dis != null)
                Resize(sender as Image, dis.Delta);
        }

        private void PinchCallback(UIElement sender, GestureEventArgs e)
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
            if (sender == null)
                return;

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
            Thread.Sleep(5);

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

        private void rectangle1_TouchDown(object sender, TouchEventArgs e)
        {

            if (recording)
           { 
                if (points == null)
                    points = new ValidSetOfTouchPoints();                
                
                TouchPoint p = e.GetTouchPoint(LayoutRoot);
                TouchPoint2 p2 = provider.AddNewTouchPoint(p.ToTouchInfo(), LayoutRoot);
                points.Add(p2);

                Debug.WriteLine("point added, Device: "+p2.TouchDeviceId);
            }
        }

        private void rectangle1_TouchMove(object sender, TouchEventArgs e)
        {
            if (recording)
            {                    
                TouchPoint p = e.GetTouchPoint(LayoutRoot);

                
                foreach (TouchPoint2 p2 in points)
                {
                   if (p2.TouchDeviceId == p.TouchDevice.Id)
                       p2.Update(p.ToTouchInfo());                
                }                
              //  _touchPoint.Update(p.ToTouchInfo());
            }
        }

        private void startRecording() {
         //   board.Strokes.Clear();
            button1.Content = "Recording...(3)";
            timerRecording = new DispatcherTimer();
            timerRecording.Interval = TimeSpan.FromSeconds(2);            
            timerRecording.Tick += timer_Tick;
            timerRecording.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            button1.Content = "Done!";
            recording = !recording;
            timerRecording.Stop();            
            doneRecording();
        }
 
        private void doneRecording() {
            AntiUnificator.BreakIntoSteps = (breakGesture.IsChecked == true);
            
            gdl = AntiUnificator.GDLToPrimitive(points);            
            if ((gdl != null)&&(gdl != ""))
            {
                points = null;           
                _touchPoint = null;
                TreeViewItem newChild = new TreeViewItem();
                newChild.Header = "name: sample";
                newChild.Items.Add(gdl);
                gestures.Items.Add(newChild);
            }
            button1.Content = "Record gesture";
        } 

        private void rectangle1_TouchUp(object sender, TouchEventArgs e)
        {
            if (recording)
            {
              //  TouchPoint p = e.GetTouchPoint(LayoutRoot);
               // _touchPoint.Update(p.ToTouchInfo());

                TouchPoint p = e.GetTouchPoint(LayoutRoot);
                Debug.WriteLine("got here called by: " + p.TouchDevice.Id);
                foreach (TouchPoint2 p2 in points)
                {
                    if (p2.TouchDeviceId == p.TouchDevice.Id)
                    {
                        p2.Update(p.ToTouchInfo());
                        Debug.WriteLine("UP for: " + p2.TouchDeviceId);
                    }
                }
                
             /*    if (points == null){
                     points = new ValidSetOfTouchPoints();
                 }*/
               // points.Add(_touchPoint);
              //  _touchPoint = null;
            }
        }

      

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (recording)
            {
                doneRecording();                 
            }
            else {
                startRecording();
            }
            recording = !recording;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
          double accuracyValue = Convert.ToDouble(accuracy.Text);
          AntiUnificator.setMatchingAccuracy(accuracyValue);
          AntiUnificator.checkRules();

          rules.Text = AntiUnificator.GetSolutionGDL();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void gestures_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                int idx = gestures.Items.IndexOf(gestures.SelectedItem);

                if (AntiUnificator.removeSample(idx))
                {
                    gestures.Items.Remove(gestures.SelectedItem);
                }
            
            }
        }


        









       
    }
}
