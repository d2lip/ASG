using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ActiveStoryTouch.DataModel;
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
using System.Windows.Controls;

namespace ASG.UI
{
    /// <summary>
    /// Interaction logic for ASGThumbnails.xaml
    /// </summary>
    public partial class Thumbnails : UserControl
    {
        private Main main;
        

        public Thumbnails(Main _main)
        {

            main = _main;         
            InitializeComponent();
           
        }
        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
          //  main.previewPage();
        }

        private void SurfaceUserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue) == true)
            {
                main.ActiveSessionManager.SaveCurrentPage(main.PageInkCanvas.Strokes);
                
                PagesListBox.SetBinding(SurfaceListBox.ItemsSourceProperty,
                  new Binding()
                  {
                      Source = main.ActiveSessionManager.CurrentProject.PageDictionary.ValueObservableCollection
                  }
           );
                PagesListBox.GetBindingExpression(SurfaceListBox.ItemsSourceProperty).UpdateTarget();
            }
            
        }

        private void PagesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        
            if (PagesListBox.SelectedItem == null || PagesListBox.SelectedIndex < 0)
                return;
            if (PagesListBox.SelectedItem as ASGPage == main.ActiveSessionManager.CurrentPage)
                return;
            main.ActiveSessionManager.SaveCurrentPage(main.PageInkCanvas.Strokes);
            main.setContainerItemsVisibility(main.ActiveSessionManager.CurrentPage, Visibility.Hidden);
            ASGPage pageToLoad = PagesListBox.SelectedItem as ASGPage;
            main.ActiveSessionManager.LoadPage(pageToLoad);
            main.SetupAndRenderPage(pageToLoad);
            main.ShowMessageOnCanvas("Page " + pageToLoad.UniqueId + "  loaded", Main.MESSAGE_TIME_PERIOD);
        }

        private void PagesListBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ((ScatterView)this.Parent).Items.Remove(this);
        }

    }
}
