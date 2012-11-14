using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Surface.Presentation.Controls;

using TouchToolkit.Framework;
using System.Reflection;
using SurfaceApplication.Providers;
using TouchToolkit.Framework.TouchInputProviders;
using ActiveStoryTouch.DataModel;
using System.Windows.Controls;

namespace ASG.UI
{
    /// <summary>
    /// Interaction logic for PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window
    {
        public long currentPageIndex { get; set; }
        private Main main;
        private SurfaceTwoTouchInputProvider provider;
        // private ScatterView container;

        public PreviewWindow(Main _main)
        {
            InitializeComponent();
            main = _main;
            provider = new SurfaceTwoTouchInputProvider(this);
            GestureFramework.Initialize(provider, this.previewRoot, Assembly.GetExecutingAssembly());
            GestureFramework.EventManager.MultiTouchChanged += new TouchInputProvider.MultiTouchChangeEventHandler(EventManager_MultiTouchChanged);
        }


        void EventManager_MultiTouchChanged(object sender, MultiTouchEventArgs e)
        {
            int x = 0;
        }



        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {

            foreach (ASGPage previewPage in main.ActiveSessionManager.CurrentProject.PageDictionary.Values)
            {

                foreach (var item in previewPage.PrototypeElementDictionary.Values)
                {
                    ScatterViewItem svi = new ScatterViewItem();
                    svi.Width = item.Width;
                    svi.Height = item.Height;
                    svi.Tag = item;
                    foreach (var gesture in item.GestureTargetPageMap)
                    {
                        DetachEvent(gesture, svi);
                    }
                }
            }
            provider = null;
            GestureFramework.EventManager.MultiTouchChanged -= new TouchInputProvider.MultiTouchChangeEventHandler(EventManager_MultiTouchChanged);
            Application.Current.Windows[1].Close();

        }


        private void preparePreview(ScatterView _Container)
        {

            this.Width = main.Width;
            this.Height = main.Height;

            this.CanvasItem.Width = this.Width;
            this.CanvasItem.Height = this.Height;
            this.CanvasItem.Center = new Point((this.Width / 2), (this.Height / 2));
            TopMenu.Center = new Point((this.Width - 100), 0);

        }

        public void previewPage()
        {
            main.ActiveSessionManager.SaveCurrentPage(main.PageInkCanvas.Strokes);

            preparePreview(main.Container);

            SurfaceInkCanvas preview = this.previewCanvas;
            preview.Strokes.Clear();
            ScatterView previewContainer = this.previewContainer;
            CanvasItem.IsTopmostOnActivation = false;


            preview.Strokes.Add(main.PageInkCanvas.Strokes);
            try
            {
                this.BackgroundImage.Source = new BitmapImage(new Uri(EnvironmentFolder.getImagesFolder() + main.ActiveSessionManager.CurrentPage.BackgroundImageSource));
                preview.Background = new SolidColorBrush(Colors.Transparent);
            }
            catch (Exception ex)
            {
                preview.Background = new SolidColorBrush(Colors.White);
            }

            // String content = String.Empty;
            preview.Height = main.PageInkCanvas.ActualHeight;
            preview.Width = main.PageInkCanvas.ActualWidth;
            this.currentPageIndex = main.ActiveSessionManager.CurrentPage.UniqueId;

            //it will add all the containers and just let visible the container of the current page
            foreach (ASGPage previewPage in main.ActiveSessionManager.CurrentProject.PageDictionary.Values)
            {

                foreach (var item in previewPage.PrototypeElementDictionary.Values)
                {
                    ScatterViewItem svi = new ScatterViewItem();

                    if (item.BackgroundImage != "")
                    {
                        BitmapImage src = new BitmapImage();
                        src.BeginInit();
                        src.UriSource = new Uri(EnvironmentFolder.getImagesFolder() + item.BackgroundImage, UriKind.Absolute);
                        src.EndInit();
                        svi.Content = new Image() { Source = src };
                    }
                    else
                        svi.Content = new Rectangle() { Fill = new SolidColorBrush(Colors.Transparent) };


                    svi.Width = item.Width;
                    svi.Height = item.Height;
                    svi.Tag = item;
                    svi.ContainerStaysActive = false;
                    svi.IsTopmostOnActivation = true;

                    foreach (var gesture in item.GestureTargetPageMap)
                    {
                        AttachEvent(gesture, svi);
                    }
                    previewContainer.Items.Add(svi);
                    svi.Orientation = item.Orientation;
                    svi.Center = item.Center;

                    svi.CanMove = false;
                    svi.CanRotate = false;
                    svi.CanScale = false;

                    if (svi.ApplyTemplate())
                    {
                        svi.Background = new SolidColorBrush(Colors.Transparent);
                        svi.BorderBrush = System.Windows.Media.Brushes.Transparent;
                        Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome ssc;
                        svi.ShowsActivationEffects = false;
                        ssc = svi.Template.FindName("shadow", svi) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
                        ssc.Visibility = Visibility.Hidden;
                    }

                    if (previewPage.UniqueId == main.ActiveSessionManager.CurrentPage.UniqueId)
                    {
                        svi.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        svi.Visibility = Visibility.Hidden;
                    }


                }
            }

        }

        private void Preview_NavigateToPage(long pageIndex)
        {
            SurfaceInkCanvas preview = this.previewCanvas;
            preview.Strokes.Clear();

            //Hidding current page elements
            ActiveStoryTouch.DataModel.ASGPage currentPage = main.ActiveSessionManager.CurrentProject.PageDictionary[this.currentPageIndex];
            foreach (var element in currentPage.PrototypeElementDictionary.Values)
            {
                ScatterViewItem svi = getItemByElement(element);
                svi.Visibility = Visibility.Hidden;
            }

            // showing new page elements
            ASGPage pageToLoad = main.ActiveSessionManager.CurrentProject.PageDictionary[pageIndex];
            preview.Strokes.Add(StrokeHelper.ConvertToStrokeCollection(pageToLoad.Strokes));
            try
            {
                this.BackgroundImage.Source = new BitmapImage(new Uri(pageToLoad.BackgroundImageSource));
                preview.Background = new SolidColorBrush(Colors.Transparent);

            }
            catch (Exception ex)
            {
                preview.Background = new SolidColorBrush(Colors.White);
            }


            foreach (var element in pageToLoad.PrototypeElementDictionary.Values)
            {
                ScatterViewItem svi = getItemByElement(element);
                svi.Visibility = Visibility.Visible;
            }
            this.currentPageIndex = pageToLoad.UniqueId;
        }


        private ScatterViewItem getItemByElement(PrototypeElement element)
        {
            ScatterView previewContainer = this.previewContainer;

            foreach (var item in previewContainer.Items)
            {
                if (item is ScatterViewItem)
                    if ((item as ScatterViewItem).Tag is PrototypeElement)
                        if (((item as ScatterViewItem).Tag as PrototypeElement).UniqueId == element.UniqueId)
                            return item as ScatterViewItem;
            }

            return null;
        }





        public void DetachEvent(KeyValuePair<string, long> pair, ScatterViewItem svi)
        {
            GestureFramework.EventManager.RemoveEvent(svi, pair.Key.ToLower());
        }

        private void AttachEvent(KeyValuePair<string, long> pair, ScatterViewItem svi)
        {
            if (svi.Content is Image)
            {
                GestureFramework.EventManager.AddEvent((svi.Content as Image), pair.Key.ToLower(), new GestureEventHandler(delegate(UIElement sender, GestureEventArgs e)
                {
                    Preview_NavigateToPage(pair.Value);
                })
                    );

                (svi.Content as Image).TouchEnter += GestureArea_TouchEnter;
                (svi.Content as Image).TouchLeave += GestureArea_TouchLeave;
            }
            if (svi.Content is Rectangle)
            {
                GestureFramework.EventManager.AddEvent((svi.Content as Rectangle), pair.Key.ToLower(), new GestureEventHandler(delegate(UIElement sender, GestureEventArgs e)
                {
                    Preview_NavigateToPage(pair.Value);
                })
                );

                (svi.Content as Rectangle).TouchEnter += GestureArea_TouchEnter;
                (svi.Content as Rectangle).TouchLeave += GestureArea_TouchLeave;
            
            
            }

     

        }

        private void GestureArea_TouchEnter(object sender, System.Windows.Input.TouchEventArgs e)
        {
            (sender as FrameworkElement).CaptureTouch(e.TouchDevice);
            e.Handled = true;
        }

        private void GestureArea_TouchLeave(object sender, System.Windows.Input.TouchEventArgs e)
        {
            (sender as FrameworkElement).ReleaseTouchCapture(e.TouchDevice);
            e.Handled = true;
        }




    }
}
