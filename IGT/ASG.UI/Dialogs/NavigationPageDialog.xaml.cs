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
using ASG.UI;

namespace ASG.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for NavigationPageDialog.xaml
    /// </summary>
    public partial class NavigationPageDialog : UserControl
    {

        private SessionManager sessionManager = null;
        private PrototypeElement instance = null;
        private ASGPage SelectedPage;
        private string gesture = null;
        private Label behaviorLabel;
        public NavigationPageDialog(PrototypeElement _instance,  SessionManager _sessionManager, string _gesture, Label _behaviorLabel)
        {

            InitializeComponent();
            gesture = _gesture;
            const string gestureContent = "Use {0} to navigate to page:";
            Title.Content = String.Format(gestureContent, gesture);
            
            sessionManager = _sessionManager;
            instance = _instance;
            PagesListBox.SetBinding(SurfaceListBox.ItemsSourceProperty, new Binding() { Source = _sessionManager.CurrentProject.PageDictionary.ValueObservableCollection });
            behaviorLabel = _behaviorLabel;
        }


         

        private void PageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string errorMsg = null;
            SelectedPage = PagesListBox.SelectedItem as ASGPage;
            if (SelectedPage == null || SelectedPage == sessionManager.CurrentPage)
            {
                errorMsg = "target page";
            }
            else
            {
                long pageId = SelectedPage.UniqueId;
                if (instance.GestureTargetPageMap.ContainsKey(gesture))
                {
                    instance.GestureTargetPageMap.Remove(gesture);
                }

                instance.GestureTargetPageMap.Add(gesture, pageId);
                const string behaviorLabelFormatBase = "Navigate, Page {0}";
                behaviorLabel.Content = String.Format(behaviorLabelFormatBase, sessionManager.CurrentProject.PageDictionary[instance.GestureTargetPageMap[gesture]].PageNumber);
                ((ScatterView)this.Parent).Items.Remove(this);
            }

        }



        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ((ScatterView)this.Parent).Items.Remove(this);
        }


    }


    
    }

