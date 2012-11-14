using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Shapes;
using TouchToolkit.GestureProcessor.ReturnTypes;
using System.Collections.Generic;
using System.Windows.Media;
using System;
namespace TouchToolkit.GestureProcessor.Feedbacks.GestureFeedbacks
{
    public class HighlightSelectedArea : IGestureFeedback
    {
        Canvas _feedbackCanvas;
        Timer _uiUpdateTimer;
        Dispatcher _dispatcher;
        int _age = 0;
        Polygon _polygon;
        const int MaxAge = 5;

        /// <summary>
        /// Renders selected area on the specified canvas using the dispatcher thread
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="feedbackCanvas"></param>
        /// <param name="values"></param>
        public void RenderUI(Dispatcher dispatcher, Canvas feedbackCanvas, List<IReturnType> values)
        {
            _dispatcher = dispatcher;
            _feedbackCanvas = feedbackCanvas;

            TouchPaths touchPaths= values.Get<TouchPaths>();

            if (touchPaths != null && touchPaths.Count > 0)
            {
                DrawLassoArea(touchPaths[0]);
                StartAnimation();
            }
        }

        private void DrawLassoArea(TouchPath touchPath)
        {
            _polygon = new Polygon();

            _polygon.Fill = new SolidColorBrush(Colors.LightGray);
            _polygon.Opacity = 0.7;
            _polygon.Tag = "SELECTED_AREA";

            foreach (var point in touchPath.Points)
            {
                _polygon.Points.Add(point);
            }

            _feedbackCanvas.Children.Add(_polygon);
        }

        private void StartAnimation()
        {
            // Start auto update 
            TimerCallback callback = new TimerCallback(UpdateUI);
            _uiUpdateTimer = new Timer(callback, null, 40, 50);

        }

        private void StopAnimation()
        {
            // Start auto update 
            _uiUpdateTimer.Dispose();
        }

        private void UpdateUI(object state)
        {
            Action action = () =>
            {
                if (_age >= MaxAge)
                {
                    // Its old enough to go away :)
                    _feedbackCanvas.Children.Remove(_polygon);

                    StopAnimation();
                }
                else
                {
                    // Update UI
                    if (_polygon.Opacity > .1)
                    {
                        _polygon.Opacity -= .1;
                    }
                    else
                    {
                        // Though code shouldn't come here, but incase of any miss
                        // configuration...
                        _age = MaxAge;
                        _feedbackCanvas.Children.Remove(_polygon);
                    }
                }
            };

            _dispatcher.BeginInvoke(action);
        }

    }
}
