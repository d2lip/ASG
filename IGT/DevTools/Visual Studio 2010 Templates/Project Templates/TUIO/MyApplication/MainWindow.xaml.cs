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
using TouchToolkit.GestureProcessor.Feedbacks.TouchFeedbacks;
using System.Reflection;
using TouchToolkit.Framework;
using TouchToolkit.GestureProcessor.ReturnTypes;
using TouchToolkit.GestureProcessor.Gesture_Definitions;
using TouchToolkit.Framework.TouchInputProviders;

namespace MyApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize
            InitializeTouchToolkit();

            // Register gesture events
            foreach (var item in LayoutRoot.Children)
            {
                if (item is Rectangle)
                {
                    Rectangle rect = item as Rectangle;

                    // Adding drag, resize and rotate gestures to each rectangle inside the canvas
                    GestureFramework.EventManager.AddEvent(rect, Gestures.Drag, DragCallback);
                    GestureFramework.EventManager.AddEvent(rect, Gestures.Zoom, ResizeCallback);
                    GestureFramework.EventManager.AddEvent(rect, Gestures.Pinch, ResizeCallback);
                    GestureFramework.EventManager.AddEvent(rect, Gestures.Rotate, RotateCallback);
                }
            }

            // Access to raw touch data
            GestureFramework.EventManager.SingleTouchChanged += new TouchInputProvider.SingleTouchChangeEventHandler(EventManager_SingleTouchChanged);
            GestureFramework.EventManager.MultiTouchChanged += new TouchInputProvider.MultiTouchChangeEventHandler(EventManager_MultiTouchChanged);
        }

        void EventManager_SingleTouchChanged(object sender, SingleTouchEventArgs e)
        {
            // Raw touch data
            // e.TouchPoint;
        }

        void EventManager_MultiTouchChanged(object sender, MultiTouchEventArgs e)
        {
            // Raw touch data
            // e.TouchPoints
        }

        #region Init
        private void InitializeTouchToolkit()
        {
            /* Initialize framework */
            var inputProvider = new TUIOProvider();
            GestureFramework.Initialize(inputProvider, LayoutRoot, Assembly.GetExecutingAssembly());

            /* Add common visual feedbacks */
            GestureFramework.AddTouchFeedback(typeof(BubblesPath));
        }
        #endregion

        #region Gesture Event Callbacks

        void DragCallback(UIElement sender, GestureEventArgs e)
        {
            // Note: e.Values property contains the return type(s) defined in the gesture definition 
            var positionChanged = e.Values.Get<PositionChanged>();

            if (positionChanged != null)
            {
                // Since we know that this callback is only used for the Rectangle 
                // type of objects we can safely cast it to Rectangle
                Rectangle rect = sender as Rectangle;

                double x = (double)rect.GetValue(Canvas.LeftProperty);
                double y = (double)rect.GetValue(Canvas.TopProperty);

                rect.SetValue(Canvas.LeftProperty, x + positionChanged.X);
                rect.SetValue(Canvas.TopProperty, y + positionChanged.Y);
            }
        }

        void ResizeCallback(UIElement sender, GestureEventArgs e)
        {
            // Note: e.Values property contains the return type(s) defined in the gesture definition 
            var distanceChanged = e.Values.Get<DistanceChanged>();

            if (distanceChanged != null)
            {
                // Since we know that this callback is only used for the Rectangle 
                // type of objects we can safely cast it to Rectangle
                Rectangle rect = sender as Rectangle;

                rect.Width += distanceChanged.Delta;
                rect.Height += distanceChanged.Delta;
            }
        }

        bool rotateInProgress = false;
        void RotateCallback(UIElement sender, GestureEventArgs e)
        {
            // Note: e.Values property contains the return type(s) defined in the gesture definition 
            var slopeChanged = e.Values.Get<SlopeChanged>();

            if (slopeChanged != null)
            {
                // Since we know that this callback is only used for the Rectangle 
                // type of objects we can safely cast it to Rectangle
                Rectangle rect = sender as Rectangle;

                if (!rotateInProgress & slopeChanged.Delta != 0)
                {
                    rotateInProgress = true;

                    // Get current state
                    RotateTransform rt = rect.RenderTransform as RotateTransform;

                    if (rt == null)
                        rt = new RotateTransform();

                    // Set new values
                    rt.Angle += slopeChanged.Delta;
                    rt.CenterX = rect.Width / 2;
                    rt.CenterY = rect.Height / 2;

                    // Update 
                    rect.RenderTransform = rt;

                    rotateInProgress = false;
                }
            }
        }
        #endregion
    }
}
