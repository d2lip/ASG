using System;
using System.Collections.Generic;
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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using ActiveStoryTouch.DataModel;
using Microsoft.Surface.Presentation.Controls.Primitives;
using ASG.UI.Dialogs;
using TouchToolkit.Framework;

namespace ASG.UI
{
    /// <summary>
    /// Interaction logic for ASGEvents.xaml
    /// </summary>
    public partial class EventsGesture : UserControl
    {
        private Main asg;
        private PrototypeElement selectedElement;
        private NavigationPageDialog choose;
        private const string CUSTOM_GESTURE = "Custom gesture";
        
        
        public EventsGesture( Main _asg)
        {
            InitializeComponent();
            asg = _asg;
           


        }

        private void ObjectsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            selectedElement = e.NewValue as PrototypeElement;           
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ((ScatterView)this.Parent).Items.Remove(this);
        }

  

        private void IGT_Click(object sender, RoutedEventArgs e)
        {
            asg.igt = new SurfaceApplication.IGT(asg.handler, asg.mainCanvasRoot, asg.Container);


            var svi = asg.Container.ItemContainerGenerator.ContainerFromItem(asg.igt) as ScatterViewItem;

            if (svi == null)
            {
                asg.Container.Items.Add(asg.igt);
                var mainSvi =  asg.Container.ItemContainerGenerator.ContainerFromItem(this) as ScatterViewItem;
                svi = asg.Container.ItemContainerGenerator.ContainerFromItem(asg.igt) as ScatterViewItem;
                svi.Orientation = 0;
                svi.Width = asg.Container.ActualWidth*2.5;
                svi.Height = asg.Container.ActualHeight*2.5;
                asg.igt.Height = asg.Container.ActualHeight / 1.03;
                asg.igt.Width = asg.Container.ActualWidth / 1.5;
                svi.Center = new Point(asg.Width / 2, asg.Height / 2);
                svi.CanMove = false;
                svi.CanScale = false;
                svi.CanRotate = false;             
                mainSvi.Visibility = Visibility.Hidden;
                svi.ShowsActivationEffects = false;               
            }      
            svi.Visibility = Visibility.Visible;
        }        

        /*
       * USES THE FRAMEWORK TO LOOK INTO THE
       * CUSTOM FOLDER TO GET THE CUSTOM GESTURES
       */
        private void LookForNewGestures()
        {
            List<string> gestures = GestureLanguageProcessor.getAllAvailableGestureNames();
            foreach (string gesture in gestures)
            {
                if (!asg.ActiveSessionManager.CommonGestureManager.ContainsKey(gesture.ToLower()))
                {
                    Gesture g = new Gesture();
                    g.Description = CUSTOM_GESTURE;
                    g.DisplayName = gesture;
                    g.Identifier = gesture.ToLower();
                    asg.ActiveSessionManager.CommonGestureManager.Add(gesture.ToLower(), g);
                }
            }
        }


        /*
         * PREPARE EVENTS PANEL TO BE USED WITH A PROTOTYPE ELEMENT
         */


        public void SetupEventsPanel(PrototypeElement instance = null)
        {
            this.TriggersGrid.IsEnabled = (instance != null);

            this.TriggersGrid.Items.Clear();

            ScatterViewItem svi = asg.Container.ItemContainerGenerator.ContainerFromItem(this) as ScatterViewItem;

            if (svi == null)
                return;
          

            const string behaviorLabelFormatBase = "Navigate, Page {0}";

            LookForNewGestures();

            foreach (var item in asg.ActiveSessionManager.CommonGestureManager.GestureList)
            {
                //SurfaceListBoxItem listItem = new SurfaceListBoxItem();

                DockPanel panel = new DockPanel();

                Border borderLeft = new Border { BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(5), Background = Brushes.White };
                Label gestureLabel = new Label { Content = item.DisplayName, Width = svi.Width /4, FontSize=20 };
                borderLeft.Child = gestureLabel;
                panel.Children.Add(borderLeft);

                Border borderRight = new Border { BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(5), Background = Brushes.White };
                DockPanel rightDockPanel = new DockPanel { LastChildFill = true };
                borderRight.Child = rightDockPanel;

                String behaviorLabelText = String.Empty;
                if (instance != null)
                {
                    if (instance.GestureTargetPageMap.ContainsKey(item.Identifier) && asg.ActiveSessionManager.CurrentProject.PageDictionary.ContainsKey(instance.GestureTargetPageMap[item.Identifier]))
                        behaviorLabelText = String.Format(behaviorLabelFormatBase, asg.ActiveSessionManager.CurrentProject.PageDictionary[instance.GestureTargetPageMap[item.Identifier]].PageNumber);
                }
                Label behaviorLabel = new Label { Content = behaviorLabelText, Width = svi.Width/2.2, FontSize=20 };

          
                SurfaceButton setValueButton = new SurfaceButton
                {
                    Content = "+",
                    FontSize=25,
                    FontWeight = FontWeights.Bold,                    
                    Width = 40,
                    Height=40,
                    Foreground = new SolidColorBrush(Colors.White),
                    Background = new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#282828")),                   
                    BorderThickness = new Thickness(0, 0, 0, 0),

                    Tag = item

                };

                SurfaceButton removeValueButton = new SurfaceButton
                {
                    Content = "-",
                    FontSize = 25,
                    Height=40,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontWeight = FontWeights.Bold,

                    Width = 45,
                    Background = new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#282828")),                    
                    BorderThickness = new Thickness(0, 0, 0, 0),

                    Tag = item

                };

                setValueButton.Click += new RoutedEventHandler(delegate(object sender, RoutedEventArgs args)
                {
                    if (instance == null)
                        return;

                    if (asg.Container.Items.Contains(choose))
                    {
                        asg.Container.Items.Remove(choose);
                    }

                    String selectedGestureId = (setValueButton.Tag as Gesture).Identifier;
                    choose = new NavigationPageDialog(instance, asg.ActiveSessionManager, selectedGestureId, behaviorLabel);
                    asg.Container.Items.Add(choose);
                    ScatterViewItem newItem = asg.Container.ItemContainerGenerator.ContainerFromItem(choose) as ScatterViewItem;
                    newItem.Orientation = 0;
                    newItem.Height = asg.Height / 2;
                    newItem.Width = asg.Width / 1.3;
                   

                    newItem.Center = asg.CanvasItem.ActualCenter;



                });

                removeValueButton.Click += new RoutedEventHandler(delegate(object sender, RoutedEventArgs args)
                {

                    String selectedGestureId = (setValueButton.Tag as Gesture).Identifier;

                    if (instance.GestureTargetPageMap.ContainsKey(selectedGestureId))
                    {
                        instance.GestureTargetPageMap.Remove(selectedGestureId);
                        behaviorLabel.Content = "";
                    }
                });
                DockPanel.SetDock(setValueButton, Dock.Right);
                DockPanel.SetDock(removeValueButton, Dock.Right);
                rightDockPanel.Children.Add(removeValueButton);
                rightDockPanel.Children.Add(setValueButton);
                rightDockPanel.Children.Add(behaviorLabel);
                panel.Children.Add(borderRight);
                this.TriggersGrid.Items.Add(panel);
            }
        }


        
    }
}
