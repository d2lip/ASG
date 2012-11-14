using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Threading;
using TouchToolkit.GestureProcessor.Objects;
using System.Diagnostics;

namespace TouchToolkit.GestureProcessor.Feedbacks.TouchFeedbacks
{
    public class BubblesPath : ITouchFeedback
    {
        Timer _uiUpdateTimer;
        Dispatcher _dispatcher;

        List<ProxyObject> _proxyObjects = new List<ProxyObject>();
        Panel _rootPanel;

        public void Init(Panel rootPanel, Dispatcher dispatcher)
        {
            // Save display play
            _rootPanel = rootPanel;
            _dispatcher = dispatcher;

            // Start auto update 
            TimerCallback callback = new TimerCallback(UpdateUI);
            _uiUpdateTimer = new Timer(callback, null, 100, 100);

        }

        public void FrameChanged(FrameInfo frameInfo)
        {
            CreateProxyObjects(frameInfo.Touches);
        }

        private void CreateProxyObjects(List<TouchInfo> touchInfos)
        {
            Action action = () =>
            {
                foreach (var touchInfo in touchInfos)
                {
                    //Create proxies when touch points move. 
                    //No point of creating at touch down the 
                    //space will be covered by users fingers

                    if (touchInfo.ActionType == TouchAction2.Move)
                    {
                        CreateProxyObject(touchInfo);
                    }
                }
            };
            _dispatcher.BeginInvoke(action);
        }

        private void CreateProxyObject(TouchInfo touchInfo)
        {
            ProxyObject po = new ProxyObject(touchInfo.Position.X, touchInfo.Position.Y, touchInfo.GroupId);
            _rootPanel.Children.Add(po);
            _proxyObjects.Add(po);
        }


        private void UpdateUI(object state)
        {
            Action action = () =>
                {
                    List<ProxyObject> itemsToRemove = new List<ProxyObject>();
                    foreach (var po in _proxyObjects)
                    {
                        if (po.Age > 4)
                        {
                            // Its old enough to die :)
                            itemsToRemove.Add(po);
                        }
                        else
                        {
                            po.UpdateUI();
                        }
                    }

                    // Remove aged items
                    foreach (var po in itemsToRemove)
                    {
                        _rootPanel.Children.Remove(po);
                        _proxyObjects.Remove(po);
                    }
                };

            _dispatcher.BeginInvoke(action);
        }

        class ProxyObject : Grid
        {
            public int TouchId { get; set; }

            public int Age { get; private set; }
            int sizeDecayRate = 2;
            float opacityDecayRate = 0.1f;

            public ProxyObject(double x, double y, int groupId)
            {
                Age = 0;

                // Set postion
                this.SetValue(Canvas.TopProperty, y);
                this.SetValue(Canvas.LeftProperty, x);

                // Set default size & opacity
                this.Height = 20;
                this.Width = 20;

                // 1. Create the circle shape that works as proxy of touch

                Ellipse e = new Ellipse();

                // TODO: Temporary implementation for demo
                if (groupId == 0)
                    e.Fill = new SolidColorBrush(Colors.LightGray);
                else if (groupId == 1)
                    e.Fill = new SolidColorBrush(Colors.Blue);
                else if (groupId == 2)
                    e.Fill = new SolidColorBrush(Colors.Green);
                else
                    e.Fill = new SolidColorBrush(Colors.Red);

                e.Opacity = 0.6;

                this.Children.Add(e);
            }

            public void UpdateUI()
            {
                // Update UI
                if (this.Height < sizeDecayRate || Width < sizeDecayRate || Opacity < opacityDecayRate)
                {
                    // Sorry, can not age more
                }
                else
                {
                    this.Age++;
                    this.Height -= sizeDecayRate;
                    this.Width -= sizeDecayRate;
                    this.Opacity -= opacityDecayRate;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
