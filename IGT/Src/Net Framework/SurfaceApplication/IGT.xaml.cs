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
using TouchToolkit.Framework;
using TouchToolkit.GestureProcessor.ReturnTypes;
using System.Threading;
using TouchToolkit.Framework.TouchInputProviders;
using TouchToolkit.GestureProcessor.Feedbacks.TouchFeedbacks;
using TouchToolkit.GestureProcessor.Gesture_Definitions;
using System.Reflection;
using System.Diagnostics;
using TouchToolkit.GestureProcessor.Objects;
using System.Windows.Threading;
using TouchToolkit.Framework.Utility;
using AntiUnification;
using Microsoft.Surface.Core;
using Microsoft.Surface;

using System.Windows.Ink;
using Microsoft.Surface.Presentation.Controls;
using System.IO;
using System.Drawing;
using TouchToolkit.GestureProcessor.Utility;
using SurfaceApplication.Providers;

namespace SurfaceApplication
{
    /// <summary>
    /// Interaction logic for IGT.xaml
    /// </summary>
    public partial class IGT : UserControl
    {


        private ValidSetOfTouchPoints points;

        private DispatcherTimer timerRecording;
        private DispatcherTimer timerMessage;
        private bool recording = false;
        private SurfaceTwoTouchInputProvider provider;
        public IGTOptions options;
        public IGTRules rules;
        private double timerCount = 0;
        private int messageCount = 0;
        private ScatterView Container;

        private void loadIGT(SurfaceTwoTouchInputProvider _provider)
        {
            InitializeComponent();
            this.provider = _provider;
            this.Loaded += new RoutedEventHandler(IGT_Loaded);
            options = new IGTOptions();
            rules = new IGTRules(this);
        }

        public IGT(Window handler, Canvas mainCanvas, ScatterView container)
        {
            provider = new SurfaceTwoTouchInputProvider(handler);
            GestureFramework.Initialize(provider, this.LayoutRoot, Assembly.GetExecutingAssembly());
            GestureFramework.EventManager.MultiTouchChanged += new TouchInputProvider.MultiTouchChangeEventHandler(EventManager_MultiTouchChanged);
            loadIGT(provider);
            Container = container;
        }



        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            addUserControl(options, options.MinHeight, options.MinWidth);


        }

        private void RulesButton_Click(object sender, RoutedEventArgs e)
        {
            rules.accuracy = Convert.ToDouble(options.accuracy.Text);
            addUserControl(rules, this.Height / 1.2, this.Width / 1.5);


        }


        private void addUserControl(UserControl control, double height, double width)
        {
            ScatterViewItem svi = Container.ItemContainerGenerator.ContainerFromItem(control) as ScatterViewItem;
            if (svi == null)
            {
                Container.Items.Add(control);
                svi = Container.ItemContainerGenerator.ContainerFromItem(control) as ScatterViewItem;
                svi.BorderThickness = new Thickness(10, 10, 10, 10);
                svi.Orientation = 0;
                svi.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
                svi.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
                svi.Height = height;
                svi.Width = width;
                svi.Visibility = System.Windows.Visibility.Hidden;
                svi.Center = new System.Windows.Point(svi.Width / 2, svi.Height / 2); // CanvasItem.ActualCenter;
            }


            if (svi.Visibility == System.Windows.Visibility.Visible)
                Container.Items.Remove(control);
            else
            {
                svi.Visibility = System.Windows.Visibility.Visible;
            }


        }

        public IGT(Window handler)
        {
            provider = new SurfaceTwoTouchInputProvider(handler);
            GestureFramework.Initialize(provider, this.LayoutRoot, Assembly.GetExecutingAssembly());
            GestureFramework.EventManager.MultiTouchChanged += new TouchInputProvider.MultiTouchChangeEventHandler(EventManager_MultiTouchChanged);
            loadIGT(provider);
            LayoutRoot.Children.Add(options);
            LayoutRoot.Children.Add(rules);
        }


        public void ShowMessageOnCanvas(String msg, int time)
        {
            messageCount = time;
            messageLabel.Visibility = Visibility.Visible;
            messageLabel.Text = msg;
            if (timerMessage != null)
                timerMessage.Stop();

            timerMessage = new DispatcherTimer();
            timerMessage.Interval = TimeSpan.FromSeconds(1);
            timerMessage.Tick += Message_Tick;
            timerMessage.Start();

        }


        void IGT_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutGrid.Height = this.ActualHeight;
            LayoutGrid.Width = this.ActualWidth;
            //    GestureFramework.Initialize(provider, LayoutRoot, Assembly.GetExecutingAssembly());
            // GestureFramework.AddTouchFeedback(typeof(ShowSelection));
            //     GestureFramework.EventManager.MultiTouchChanged += new TouchInputProvider.MultiTouchChangeEventHandler(EventManager_MultiTouchChanged);
            String msg = "Click 'Record gesture'" + Environment.NewLine +
                            "and perform the gesture you want the tool to learn" + Environment.NewLine +
                            "here on the canvas";
            gesture.Visibility = Visibility.Visible;
            testArea.Visibility = Visibility.Hidden;
            ShowMessageOnCanvas(msg, 3);
        }

        void EventManager_MultiTouchChanged(object sender, MultiTouchEventArgs e)
        {
            int x = 0;
        }





        private void Message_Tick(object sender, EventArgs e)
        {
            messageCount--;
            if (messageCount == 0)
            {
                messageLabel.Visibility = Visibility.Hidden;
                timerMessage.Stop();
            }

        }

        // IGT
        private void startRecording()
        {
            gesture.Strokes.Clear();
            messageLabel.Visibility = Visibility.Hidden;
            timerMessage.Stop();
            //   board.Strokes.Clear();
            timerCount = 3;
            RecordText.Text = "Recording...(" + timerCount + " segs)";
            RecordImage.Visibility = Visibility.Hidden;
            timerRecording = new DispatcherTimer();
            timerRecording.Interval = TimeSpan.FromSeconds(0.5);
            timerRecording.Tick += timer_Tick;
            timerRecording.Start();

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (RecordImage.Visibility == Visibility.Hidden)
                RecordImage.Visibility = Visibility.Visible;
            else
                RecordImage.Visibility = Visibility.Hidden;

            timerCount = timerCount - .5;
            if (timerCount == 0)
            {
                recording = !recording;
                timerRecording.Stop();
                doneRecording();
            }
            else if (timerCount % 1 == 0)
            {
                RecordText.Text = "RECORDING...(" + timerCount + " segs)";
            }

        }

        private void doneRecording()
        {
            PointTranslator.BreakIntoSteps = (options.breakGesture.IsChecked == true);
            double noise = Convert.ToDouble(options.noise.Text);
            if (noise < 40)
                PointTranslator.NoiseReduction = PointTranslator.NoiseReductionType.Low;
            else if (noise < 70)
                PointTranslator.NoiseReduction = PointTranslator.NoiseReductionType.Medium;
            else
                PointTranslator.NoiseReduction = PointTranslator.NoiseReductionType.High;

            AntiUnificator.GestureName = GestureName.Text.ToLower();
            List<String> gdl = AntiUnificator.TouchPointsToGDL(points);
            if ((gdl != null))
            {
                rules.addNewRule(gdl, gesture);

                gesture.Strokes.Clear();
                points = null;

                provider.ActiveTouchPoints = new Dictionary<int, TouchPoint2>();
            }

            RecordText.Text = "RECORD";
            RecordImage.Visibility = Visibility.Visible;

        }


        private void Record_Click(object sender, RoutedEventArgs e)
        {
            if (recording)
            {
                doneRecording();
            }
            else
            {
                startRecording();
            }
            recording = !recording;
        }



        private void gesture_ContactDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (recording)
            {
                if (points == null)
                    points = new ValidSetOfTouchPoints();
                TouchPoint2 touchpoint = provider.UpdateActiveTouchPoints(TouchAction2.Down, e);
                points.Add(touchpoint);
            }
            else
                e.Handled = true;
        }

        private void gesture_ContactLeave(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (recording)
            {
                foreach (TouchPoint2 p2 in points)
                {
                    if ((p2.TouchDeviceId == e.TouchDevice.Id) && (p2.Action != TouchAction.Up))
                    {
                        Console.WriteLine("p2:" + p2.TouchDeviceId + "  e:" + e.TouchDevice.Id);
                        TouchInfo info = provider.GetInfoFromContactEvent(TouchAction2.Up, e);
                        p2.Update(info);

                    }
                }
                if (points == null)
                {
                    points = new ValidSetOfTouchPoints();
                }


            }
            else
                e.Handled = true;
        }

        private void gesture_ContactChanged(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (recording)
            {

                foreach (TouchPoint2 p2 in points)
                {
                    if ((p2.TouchDeviceId == e.TouchDevice.Id) && (p2.Action != TouchAction.Up))
                    {

                        TouchInfo info = provider.GetInfoFromContactEvent(TouchAction2.Move, e);
                        p2.Update(info);
                    }
                }

            }
            else
                e.Handled = true;


        }



        private void TryButton_Click(object sender, RoutedEventArgs e)
        {
            if (testArea.Visibility == Visibility.Visible)
            {
                try
                {
                    testArea.Visibility = Visibility.Hidden;
                    gesture.Visibility = Visibility.Visible;
                    ScatterView Container = (this.Parent as ScatterView);
                    var svi = Container.ItemContainerGenerator.ContainerFromItem(this) as ScatterViewItem;
                    GestureFramework.EventManager.RemoveEvent(svi, GestureName.Text.ToLower());
                    TryButton.Content = "Try gesture";
                }
                catch (Exception ex)
                {
                    //  ShowMessageOnCanvas(ex.Message, 2);
                }
                return;

            }

            String msg = "";
            if (rules.rules.Items.Count == 0)
            {
                msg = "No rule was created to be tested" + Environment.NewLine +
                            "to create a rule, record samples" + Environment.NewLine +
                            "and then click the 'Generate Rule' button";
                ShowMessageOnCanvas(msg, 5);
            }
            else
            {
                TryButton.Content = "Done!";

                msg = "To test the created gesture" + Environment.NewLine +
                            "perform it on the testArea bellow";
                GestureLanguageProcessor.ReLoadGestureDefinitions();
                try
                {
                    testArea.Visibility = Visibility.Visible;
                    gesture.Visibility = Visibility.Hidden;
                    GestureFramework.EventManager.AddEvent(testArea, GestureName.Text.ToLower(), GestureRecognizedCallback);
                    ShowMessageOnCanvas(msg, 1);
                }
                catch (Exception ex)
                {
                    ShowMessageOnCanvas(ex.Message, 2);
                }
            }


        }



        void GestureRecognizedCallback(UIElement sender, GestureEventArgs e)
        {
            String msg = "Gesture" + Environment.NewLine +
                      "recognized!";
            ShowMessageOnCanvas(msg, 3);
            GestureFramework.EventManager.RemoveEvent(testArea, GestureName.Text.ToLower());
            testArea.Visibility = Visibility.Hidden;
            gesture.Visibility = Visibility.Visible;
            TryButton.Content = "Try gesture";
        }



        private void CompileButton_Click(object sender, RoutedEventArgs e)
        {
            string msg = "";

            if (GestureName.Text.Count() > 1)
            {
                int value = 0;
                if (int.TryParse(GestureName.Text[0] + "", out value))
                {
                    msg = "Name cannot start with a number";

                    return;
                }
            }


            if (GestureName.Text == "type name...")
            {
                msg = "Please type the name of the" + Environment.NewLine + " new gesture";

            }
            else if (rules.rules.Items.Count > 0)
            {

                //ShowMessageOnCanvas("Compiling " + GestureName.Text + Environment.NewLine + "Please wait...", 4);

                messageLabel.Visibility = Visibility.Visible;
                messageLabel.Text = "Compiling " + GestureName.Text + Environment.NewLine + "Please wait...";
                msg = DynamicParser.Parser("name:" + GestureName.Text.ToLower() + Environment.NewLine + rules.rules.Items[0].ToString(), GestureName.Text.ToLower());

            }
            else
            {
                msg = "No gesture defined" + Environment.NewLine
                      + "to export";

            }

            ShowMessageOnCanvas(msg, 4);
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            provider = null;
            GestureFramework.EventManager.MultiTouchChanged -= new TouchInputProvider.MultiTouchChangeEventHandler(EventManager_MultiTouchChanged);
            Container.Items.Remove(options);
            Container.Items.Remove(rules);
            ((ScatterView)this.Parent).Items.Remove(this);

        }

        private void testArea_TouchEnter(object sender, System.Windows.Input.TouchEventArgs e)
        {
            (sender as FrameworkElement).CaptureTouch(e.TouchDevice);
            e.Handled = true;
        }

        private void testArea_TouchLeave(object sender, System.Windows.Input.TouchEventArgs e)
        {
            (sender as FrameworkElement).ReleaseTouchCapture(e.TouchDevice);
            e.Handled = true;
        }

  





    }
}
