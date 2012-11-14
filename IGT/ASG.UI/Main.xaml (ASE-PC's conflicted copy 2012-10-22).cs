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

using System.Windows.Ink;

using System.ComponentModel;
using ActiveStoryTouch.DataModel.Helpers;

using ActiveStoryTouch.DataModel;

using System.IO;
using System.Runtime.Serialization;
using Microsoft.Win32;
using Microsoft.Surface.Presentation.Controls.Primitives;
using ASG.UI.Dialogs;
using TouchToolkit.Framework.TouchInputProviders;
using TouchToolkit.Framework;
using System.Reflection;
using SurfaceApplication;
using System.Linq;
using System.Windows.Threading;
using ASG.UI.Enums;

using Microsoft.Surface.Presentation.Input;
using SurfaceApplication.Providers;




namespace ASG.UI
{
    /// <summary>
    /// Interaction logic for ASG.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        // DRAWING
        private DrawingCanvasHelper canvasHelper;
        private Stroke selectedStrokes;
        private StrokeCollection lastStrokes;

        private Rect originalSelection;

        // DIALOGS
        private LoadDialog load;
        private SaveDialog save;
        private EventsGesture events;        
        private Thumbnails thumbnails;
        public IGT igt;
        private PreviewWindow preview;

        private int messageCount;
        private double defaultOrientation = 0;
        private DispatcherTimer timerMessage;
        public const int MESSAGE_TIME_PERIOD = 2;

        public SessionManager ActiveSessionManager { get; set; }
        public Window handler;
        public Canvas mainCanvasRoot;

         

        public Main(Window _handler,Canvas root)
        {
            InitializeComponent();
            handler = _handler;
          
            CreateASG(root);
        }



        private void CreateASG(Canvas root)
        {   
            ActiveSessionManager = new SessionManager(PageInkCanvas, root);
        
            canvasHelper = new DrawingCanvasHelper(ActiveSessionManager, PageInkCanvas, Container, CurrentColor,ColorWheel);
            this.mainCanvasRoot = root;
            events = new EventsGesture(this);           
            thumbnails = new Thumbnails(this);   
            
            CanvasItem.Height = root.Height - 50;
            CanvasItem.Width = root.Width - 50;

            TextContainer.Center = new Point((double)root.Width / 2, (double)root.Height / 2); 
            CanvasItem.Center = new Point((double)root.Width / 2, (double)root.Height / 2); 
           
            TrashContainer.Center = new Point((double)root.Width - 30, (double)root.Height / 2);
            TrashContainer.Height = root.Height;
            TrashContainer.Width = 50;

            LeftMenu.Width = 50;
            LeftMenu.Center = new Point(10, (double)root.Height / 2.2);
            LeftMenu.Height = root.Height/1.5;
            
            TopMenu.Width = root.Width - 50 ;
            TopMenu.Center = new Point((double)root.Width / 2  , 25 );
            TopMenu.Height = 50;

            BottomMenu.Width = root.Width;
            BottomMenu.Height = 50;
            BottomMenu.Center = new Point((double)root.Width / 2 + 50, root.Height - 25);
            
            PageInkCanvas.Height = root.Height;
            PageInkCanvas.Width = root.Width;

            RemovingItemsShadow();


            TextContainer.Visibility = Visibility.Hidden;
            timerMessage = new DispatcherTimer();


            PageInkCanvas.ReleaseAllCaptures();
            canvasHelper.loadPenSettings();
            SetFontSize(root);
        }

        private void RemovingItemsShadow()
        {
            Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome ssc;

            TextContainer.ShowsActivationEffects = false;
            TextContainer.ApplyTemplate();
            ssc = TextContainer.Template.FindName("shadow", TextContainer) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
            ssc.Visibility = Visibility.Hidden;
            TextContainer.ApplyTemplate();

            TopMenu.ShowsActivationEffects = false;
            TopMenu.ApplyTemplate();
            ssc = TextContainer.Template.FindName("shadow", TopMenu) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
            ssc.Visibility = Visibility.Hidden;
            TopMenu.ApplyTemplate();

            LeftMenu.ShowsActivationEffects = false;
            LeftMenu.ApplyTemplate();
            ssc = TextContainer.Template.FindName("shadow", LeftMenu) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
            ssc.Visibility = Visibility.Hidden;
            LeftMenu.ApplyTemplate();


            BottomMenu.ShowsActivationEffects = false;
            BottomMenu.ApplyTemplate();
            ssc = TextContainer.Template.FindName("shadow", BottomMenu) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
            ssc.Visibility = Visibility.Hidden;
            BottomMenu.ApplyTemplate();


            TrashContainer.ShowsActivationEffects = false;
            TrashContainer.ApplyTemplate();
            ssc = TextContainer.Template.FindName("shadow", TrashContainer) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
            ssc.Visibility = Visibility.Hidden;
            TrashContainer.ApplyTemplate();

        }

        private void SetFontSize(Canvas root)
        {
            Double _FontSize;
            
                // based on screen size
            if (root.Width > 1300)
                    _FontSize = 45;
            if (root.Width > 1000)
                    _FontSize = 25;
                else
                    _FontSize = 30;
            
            this.Resources["MyFontSize"] = _FontSize;
        }



       

        private void GoToNextPage()
        {

            long id = ActiveSessionManager.CurrentPage.UniqueId;
            id++;
            while (id <= ActiveSessionManager.CurrentProject.LastAssignedUniqueID)
            {

                if (ActiveSessionManager.CurrentProject.PageDictionary.ContainsKey(id))
                {
                    ASGPage nextPage = ActiveSessionManager.CurrentProject.PageDictionary[id];
                    ActiveSessionManager.SaveCurrentPage(PageInkCanvas.Strokes);
                    setContainerItemsVisibility(ActiveSessionManager.CurrentPage, Visibility.Hidden);

                    ActiveSessionManager.LoadPage(nextPage);
                    SetupAndRenderPage(nextPage);
                    ShowMessageOnCanvas("Page " + nextPage.PageNumber + "  loaded", Main.MESSAGE_TIME_PERIOD);
                 
                    return;
                }
                id++;
            }
            ShowMessageOnCanvas("Last page", Main.MESSAGE_TIME_PERIOD);
            
        }



        private void GoToPreviousPage()
        {
            long id = ActiveSessionManager.CurrentPage.UniqueId;
            id--;
            while (id > 0)
            {
                if (ActiveSessionManager.CurrentProject.PageDictionary.ContainsKey(id))
                {
                    ASGPage nextPage = ActiveSessionManager.CurrentProject.PageDictionary[id];
                    ActiveSessionManager.SaveCurrentPage(PageInkCanvas.Strokes);
                    setContainerItemsVisibility(ActiveSessionManager.CurrentPage, Visibility.Hidden);

                    ActiveSessionManager.LoadPage(nextPage);
                    SetupAndRenderPage(nextPage);
                    ShowMessageOnCanvas("Page " + nextPage.PageNumber + "  loaded", Main.MESSAGE_TIME_PERIOD);
               
                    return;
                }
                id--;
            }
            ShowMessageOnCanvas("First page", Main.MESSAGE_TIME_PERIOD);
        }

        /*
         * DETECTS IF THE DESGINER IS TRYING TO DELETE AN AREA (CAN BE A GESTURE AREA OR SELECTED STROKES)
         * IF THE TOUCH IS WITHIN THE TRASH AREA, DELETES IF NOT, UPDATES THE EVENT SCREEN
         *          
         */ 


        public void SetupAndRenderPage(ASGPage pageToLoad)
        {
            events.SetupEventsPanel();            
            RenderPage(pageToLoad);
            canvasHelper.loadPenSettings();
        }


        public void setContainerItemsVisibility(ASGPage page, Visibility visibility)
        {
            IEnumerable<ScatterViewItem> list = from object item in Container.Items
                                                where (item is ScatterViewItem)
                                                         && ((item as ScatterViewItem).Tag is PrototypeElement)
                                                         && (page.PrototypeElementDictionary.Values.Contains((item as ScatterViewItem).Tag as PrototypeElement))
                                                select item as ScatterViewItem;
            List<ScatterViewItem> ItemsList = list.ToList();
            foreach (ScatterViewItem svi in ItemsList)
            {
                svi.Visibility = visibility;
            }
        }

        public void ClearCanvas()
        {
            IEnumerable<ScatterViewItem> list = from object item in Container.Items
                                                where (item is ScatterViewItem)
                                                         && ((item as ScatterViewItem).Tag is PrototypeElement)                                                        
                                                select item as ScatterViewItem;
            List<ScatterViewItem> ItemsList = list.ToList();
            foreach (ScatterViewItem svi in ItemsList)
            {
              Container.Items.Remove(svi);
            }
        }




        public void loadPageBackground(ASGPage pageToLoad)
        { 
        try
            {                 
                BackgroundImage.Source = new BitmapImage(new Uri(pageToLoad.BackgroundImageSource));
                PageInkCanvas.Background = new SolidColorBrush(Colors.Transparent);             
            }
            catch(Exception ex)
            {
                PageInkCanvas.Background = new SolidColorBrush(Colors.White); 
            }          
        
        }

        private void OnColorWheelTouchDown(object sender, TouchEventArgs args)
        {
            canvasHelper.HandleInputDown(sender, args, false);
        }
        private void OnColorWheelMouseDown(object sender, MouseButtonEventArgs args)
        {
            canvasHelper.HandleInputDown(sender, args, false);
        }



        /// <summary>
        /// Handles the MouseUp event for the current color indicator or color wheel.
        /// </summary>
        /// <param name="sender">The current color indicator or color wheel.</param>
        /// <param name="args">The arguments for the event.</param>
        private void OnColorWheelMouseUp(object sender, MouseButtonEventArgs args)
        {
            // If the mouse was already captured to the sender, release it
            IInputElement element = sender as IInputElement;
            if (args.Device.GetCaptured() == element)
            {
                element.ReleaseMouseCapture();
            }
        }



        private void RenderPage(ASGPage pageToLoad)
        {
            loadPageBackground(pageToLoad);
            PageInkCanvas.Strokes.Clear();
            PageInkCanvas.Strokes.Add(StrokeHelper.ConvertToStrokeCollection(pageToLoad.Strokes));
            PageNumber.Text = ActiveSessionManager.CurrentPage.PageNumber + "/" + ActiveSessionManager.CurrentProject.PageDictionary.Count;
            setContainerItemsVisibility(pageToLoad, Visibility.Visible);
        }



        private void CreateGestureArea()
        {
            Rect b = selectedStrokes.GetBounds();
            PrototypeElement element = new PrototypeElement { Orientation = defaultOrientation, Width = b.Width, Height = b.Height, Center = new Point(b.Location.X + b.Width / 2, b.Location.Y + b.Height / 2) };
            addThisElementToCanvas(ActiveSessionManager.CurrentPage, element, element.Orientation);
            ScatterViewItem svi = Container.ItemContainerGenerator.ContainerFromItem(events) as ScatterViewItem;
            if (svi == null)
            {
                Container.Items.Add(events);
                svi = Container.ItemContainerGenerator.ContainerFromItem(events) as ScatterViewItem;
                defaultOrientation = 0;
                svi.BorderThickness = new Thickness(10, 10, 10, 10);
                svi.Orientation = 0;
                svi.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
                svi.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
                svi.Height = this.Height / 1.5;
                svi.Width = this.Width / 2;
                svi.Visibility = System.Windows.Visibility.Hidden;
                // CanvasItem.ActualCenter;
            }
            svi.Center = new Point(this.Width / 2, this.Height / 2);
            svi.IsTopmostOnActivation = true;
            svi.Visibility = System.Windows.Visibility.Visible;
            events.SetupEventsPanel(element);
        
        }

        private void PageInkCanvas_TouchUp(object sender, TouchEventArgs e)
        {
            if ((selectedStrokes != null) && (selectedStrokes.StylusPoints.Count > 20) && (ActiveSessionManager.ActivePenMode == PenMode.Selection))
            {
                CreateSelection();

            }
            else if ((ActiveSessionManager.ActivePenMode == PenMode.GestureArea) && (selectedStrokes != null) && (selectedStrokes.StylusPoints.Count > 20))
            {
                CreateGestureArea();
            }

            if (selectedStrokes != null)
            {
                PageInkCanvas.Strokes.Remove(selectedStrokes);
                DisableGesture();
                selectedStrokes = null;
            }
            e.Handled = true;
        }

        private void PageInkCanvas_TouchDown(object sender, TouchEventArgs e)
        {
            canvasHelper.loadPenSettings(e);
            e.Handled = true;
        }






        private void Thumbnails_Click(object sender, RoutedEventArgs e)
        {
           
            ScatterViewItem svi = Container.ItemContainerGenerator.ContainerFromItem(thumbnails) as ScatterViewItem;             
            if (svi == null)
            {
                thumbnails = new Thumbnails(this);
                Container.Items.Add(thumbnails);
                svi = Container.ItemContainerGenerator.ContainerFromItem(thumbnails) as ScatterViewItem;          
              
                svi.Orientation = 0;
                svi.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
                svi.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
                svi.Height = this.Height / 2;
                svi.Width = this.Width/1.3;
                svi.CanMove = false;
                svi.CanRotate = false;
                svi.CanScale = false;
                
                svi.Visibility = System.Windows.Visibility.Hidden;
                svi.Center =  CanvasItem.ActualCenter;
            }
            if (svi.Visibility == System.Windows.Visibility.Visible)
                Container.Items.Remove(thumbnails);
            else
            {

                ActiveSessionManager.SaveCurrentPage(PageInkCanvas.Strokes);
                svi.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void GestureListButton_Click(object sender, RoutedEventArgs e)
        {
          
            ScatterViewItem svi = Container.ItemContainerGenerator.ContainerFromItem(events) as ScatterViewItem;
            if (svi == null)
            {
                Container.Items.Add(events);
                svi = Container.ItemContainerGenerator.ContainerFromItem(events) as ScatterViewItem;
                double o = 0;
                defaultOrientation = o;
                svi.BorderThickness = new Thickness(10, 10, 10, 10);
                svi.Orientation = o;
                svi.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
                svi.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
                svi.Height = this.Height / 2;
                svi.Width = this.Width / 2;
                svi.Visibility = System.Windows.Visibility.Hidden;
                svi.Center = new Point(svi.Width / 2, svi.Height / 2); // CanvasItem.ActualCenter;
            }
            events.SetupEventsPanel();

            if (svi.Visibility == System.Windows.Visibility.Visible)
                Container.Items.Remove(events);
            else
            {
                svi.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void NewPage_Click(object sender, RoutedEventArgs e)
        {
            ActiveSessionManager.SaveCurrentPage(PageInkCanvas.Strokes);
            setContainerItemsVisibility(ActiveSessionManager.CurrentPage, Visibility.Hidden);
            ActiveSessionManager.AddNewPageToProject();
            SetupAndRenderPage(ActiveSessionManager.CurrentPage);
            PageNumber.Text = ActiveSessionManager.CurrentPage.PageNumber + "/" + ActiveSessionManager.CurrentProject.PageDictionary.Count;
            ShowMessageOnCanvas("New page loaded", MESSAGE_TIME_PERIOD);
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            preview = new PreviewWindow(this);      
            preview.Show();
            preview.previewPage();
        }

        private void DisableSelect()
        {
            Select.IsChecked = false;
            foreach (Stroke s in InkSelector.Strokes)
                s.DrawingAttributes.IsHighlighter = false;

            ActiveSessionManager.ActivePenMode = PenMode.Draw;
            SelectorContainer.Visibility = Visibility.Hidden;
          

            if (selectedStrokes != null)
                PageInkCanvas.Strokes.Remove(selectedStrokes);

            canvasHelper.loadPenSettings();
        }

        private void DisableGesture()
        {
            Gesture.IsChecked = false;
            ActiveSessionManager.ActivePenMode = PenMode.Draw;
            //startGestureArea = false;

            canvasHelper.loadPenSettings();

            if (selectedStrokes != null)
                PageInkCanvas.Strokes.Remove(selectedStrokes);

        }


        private void DisableEraser()
        {
            Eraser.IsChecked = false;
            ActiveSessionManager.ActivePenMode = PenMode.Draw;
            canvasHelper.loadPenSettings();
        }



        private void OnStrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs args)
        {


            if (lastStrokes == null)
                lastStrokes = new StrokeCollection();
            lastStrokes.Add(args.Stroke);

            if (ActiveSessionManager.ActivePenMode == PenMode.Selection
                || ActiveSessionManager.ActivePenMode == PenMode.GestureArea)
                selectedStrokes = args.Stroke;            
        }

        private void Eraser_Click(object sender, RoutedEventArgs e)
        {
            if (Eraser.IsChecked == true)
            {
                DisableGesture();
                DisableSelect();
                ActiveSessionManager.ActivePenMode = PenMode.StrokeEraser;
                e.Handled = true;
                canvasHelper.loadPenSettings();
            }
            else
            {
                DisableEraser();            
                e.Handled = true;     
            }

        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {

            if (Select.IsChecked == true)
            {
                if (Gesture.IsChecked == true) DisableGesture();
                if (Eraser.IsChecked == true) DisableEraser();

                ActiveSessionManager.ActivePenMode = PenMode.Selection;
                e.Handled = true;
                canvasHelper.loadPenSettings();
            }
            else 
            {
                DisableSelect();
                e.Handled = true;            
            }
        }
        private void Gesture_Click(object sender, RoutedEventArgs e)
        {
            if (Gesture.IsChecked == true)
            {
                if (Select.IsChecked == true) DisableSelect();
                if (Eraser.IsChecked == true) DisableEraser();
                ActiveSessionManager.ActivePenMode = PenMode.GestureArea;
               // startGestureArea = true;
                e.Handled = true;
                canvasHelper.loadPenSettings();
            }
            else
            {
                e.Handled = true;
                DisableGesture();
            }
            e.Handled = false;

        }

        private void SaveProject_Click(object sender, RoutedEventArgs e)
        {

            if (save == null)
            {
                save = new SaveDialog(ActiveSessionManager, this);
            }


            ScatterViewItem newItem = Container.ItemContainerGenerator.ContainerFromItem(save) as ScatterViewItem;
            if (newItem == null)
            {
                Container.Items.Add(save);
                newItem = Container.ItemContainerGenerator.ContainerFromItem(save) as ScatterViewItem;
                newItem.Orientation = 0;
                newItem.Height = this.Height / 3;
                newItem.Width = this.Width / 3;
                newItem.Center = CanvasItem.ActualCenter;


            }

        }

        private void LoadProject_Click(object sender, RoutedEventArgs e)
        {

            if (load == null)
            {
                List<string> filter = new List<string>(new string[] { "*.asgproj" });
                load = new LoadDialog(ActiveSessionManager, this, filter, LoadDialog.LoadDialogType.Project);
            }


            ScatterViewItem newItem = Container.ItemContainerGenerator.ContainerFromItem(load) as ScatterViewItem;
            if (newItem == null)
            {
                Container.Items.Add(load);
                newItem = Container.ItemContainerGenerator.ContainerFromItem(load) as ScatterViewItem;
                newItem.Orientation = 0;
                newItem.Height = this.Height / 1.5;
                newItem.Width = this.Width / 3;
                newItem.Center = CanvasItem.ActualCenter;
            }


        }


        private void CreateSelection()
        {
            Rect b = selectedStrokes.GetBounds() ;
            bool strokeSelected = false;            

            PageInkCanvas.Strokes.Remove(selectedStrokes);
            selectedStrokes = null;
            InkSelector.Strokes.Clear();
            StrokeCollection strokecollection = new StrokeCollection();
            for (int i = 0; i < PageInkCanvas.Strokes.Count; i++) 
            {
                Stroke s = PageInkCanvas.Strokes[i];
                if (b.Contains(s.GetBounds()))
                {
                    strokecollection.Add(s);
                    strokeSelected = true;                    
                }
            }
            if (strokeSelected)
            {
                foreach (Stroke s in strokecollection)
                {
                    s.DrawingAttributes.IsHighlighter = true;
                    InkSelector.Strokes.Add(s);
                    PageInkCanvas.Strokes.Remove(s);
                }

                createContainerforStrokes(b);
                double x = -b.Location.X;
                double y = -b.Location.Y;
                Matrix inkTransform = new Matrix();
                inkTransform.Translate(x, y);
                InkSelector.Strokes.Transform(inkTransform, true);
            } 
        }

        private void createContainerforStrokes(Rect b)
        {
            if (Container.Items.IndexOf(SelectorContainer) > -1)
                Container.Items.Remove(SelectorContainer);

            InkSelector.Width = b.Width;
     
            InkSelector.ClipToBounds = true;
            Selector.Height = PageInkCanvas.Width;
            Selector.Width = PageInkCanvas.Width;
            SelectorContainer.Width = b.Width;
            SelectorContainer.Height = b.Height;
            SelectorContainer.CanScale = true;
      
            Container.Items.Add(SelectorContainer);
            SelectorContainer.Center = new Point(b.Location.X + b.Width / 2, b.Location.Y + b.Height / 2);
            SelectorContainer.Visibility = Visibility.Visible;

        }



        private void SelectorContainer_ManipulationCompleted(object sender, ContainerManipulationCompletedEventArgs e)
        {        
             Point center = (sender as ScatterViewItem).ActualCenter;

             if (canvasHelper.PointInsideContainer(center, TrashContainer))
             {
                 SelectorContainer.Visibility = Visibility.Hidden;
                 foreach (Stroke s in InkSelector.Strokes)
                 {   
                         PageInkCanvas.Strokes.Remove(s);
                         if (lastStrokes != null )
                             lastStrokes.Remove(s);                    
                 }
             }

             else
             {
                 double x = SelectorContainer.ActualCenter.X - SelectorContainer.Width / 2;
                 double y = SelectorContainer.ActualCenter.Y - SelectorContainer.Height / 2;
                 if ((x != 0 && y != 0) && (ActiveSessionManager.ActivePenMode == PenMode.Selection))
                 {
                  //   startSelection = false;
                     Matrix inkTransform = new Matrix();
                     Rect b = InkSelector.Strokes.GetBounds();
                     double scaleX = SelectorContainer.Width / originalSelection.Width;
                     double scaleY = SelectorContainer.Height / originalSelection.Height;
                     inkTransform.Scale(scaleX, scaleY);
                     inkTransform.Translate(x, y);
                     
                     // Transform strokes
                     InkSelector.Strokes.Transform(inkTransform, false);
                     foreach (Stroke s in InkSelector.Strokes)
                     {
                         s.DrawingAttributes.IsHighlighter = false;
                         PageInkCanvas.Strokes.Add(s);
                     }
                     SelectorContainer.Visibility = Visibility.Hidden;                     
                 }
             }
             DisableSelect();
             e.Handled = true;
        }

        private void SelectorContainer_TouchEnter(object sender, TouchEventArgs e)
        {
            if (originalSelection == null)
                originalSelection = new Rect();
            originalSelection.Width = SelectorContainer.Width;
            originalSelection.Height = SelectorContainer.Height;
            originalSelection.Location = SelectorContainer.ActualCenter;
            e.Handled = true;
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (lastStrokes != null && lastStrokes.Count > 0)
            {
                PageInkCanvas.Strokes.Remove(lastStrokes[lastStrokes.Count - 1]);
                lastStrokes.Remove(lastStrokes[lastStrokes.Count - 1]);
            }
            e.Handled = true;
        }

        private void OnCurrentColorTouchDown(object sender, TouchEventArgs args)
        {
            canvasHelper.HandleInputDown(sender, args, true);
        }

        private void OnCurrentColorMouseDown(object sender, MouseButtonEventArgs args)
        {
            canvasHelper.HandleInputDown(sender, args, true);
        }


        // COPY PAGE
        private void CopyPage_Click(object sender, RoutedEventArgs e)
        {
            ASGPage pageToCopy = ActiveSessionManager.CurrentPage;
            ActiveSessionManager.SaveCurrentPage(PageInkCanvas.Strokes);
            setContainerItemsVisibility(pageToCopy, Visibility.Hidden);
            ActiveSessionManager.AddNewPageToProject();
            ActiveSessionManager.CurrentPage.BackgroundImageSource = pageToCopy.BackgroundImageSource;
            
            SetupAndRenderPage(ActiveSessionManager.CurrentPage);
            PageInkCanvas.Strokes.Clear();
            PageInkCanvas.Strokes.Add(StrokeHelper.ConvertToStrokeCollection(pageToCopy.Strokes));


            foreach (PrototypeElement item in pageToCopy.PrototypeElementDictionary.Values)
            {
               addThisElementToCanvas(ActiveSessionManager.CurrentPage, item, item.Orientation);
            }
            PageNumber.Text = ActiveSessionManager.CurrentPage.PageNumber + "/" + ActiveSessionManager.CurrentProject.PageDictionary.Count;
            ShowMessageOnCanvas("Page successfully copied", MESSAGE_TIME_PERIOD);
        }


        public void ShowMessageOnCanvas(String msg, int time)
        {
            if (timerMessage.IsEnabled)
            {
                timerMessage.Stop();
                TextContainer.Visibility = Visibility.Hidden;                
            }
                
            messageCount = time;
            TextContainer.Visibility = Visibility.Visible;
            TextContainer.Orientation = defaultOrientation;            
            messageLabel.Text = msg;           
            timerMessage.Interval = TimeSpan.FromSeconds(1);
            timerMessage.Tick += Message_Tick;
            timerMessage.Start();
        }

        private void Message_Tick(object sender, EventArgs e)
        {
            messageCount--;
            if (messageCount == 0)
            {
                TextContainer.Visibility = Visibility.Hidden;
                timerMessage.Stop();
            }
        }

       
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ASGPage pagetoRemove = ActiveSessionManager.CurrentPage;
            

            if (! ActiveSessionManager.RemovePage(pagetoRemove))    {       
                ShowMessageOnCanvas("Cannot remove last page in the project", MESSAGE_TIME_PERIOD);
                return;
            }

            long id = pagetoRemove.UniqueId;
            setContainerItemsVisibility(pagetoRemove, Visibility.Hidden);
            foreach (PrototypeElement item in pagetoRemove.PrototypeElementDictionary.Values)
            {
                Container.Items.Remove(item);
            }

            ASGPage firstPage = ActiveSessionManager.CurrentProject.PageDictionary.First().Value;
            
            ActiveSessionManager.LoadPage(firstPage);
            PageNumber.Text = firstPage.PageNumber + "/" + ActiveSessionManager.CurrentProject.PageDictionary.Count;
            SetupAndRenderPage(firstPage);
            ShowMessageOnCanvas("Page " + id + " successfully removed" + Environment.NewLine
                                + "Page " + firstPage.UniqueId + " was loaded", Main.MESSAGE_TIME_PERIOD);
        }
        


        private void ItemsControl_TouchUp(object sender, TouchEventArgs e)
        {
            e.Handled = true;
        }



        private void lefthelp_TouchClick(object sender, RoutedEventArgs e)
        {
            GoToPreviousPage();
        }



        private void righthelp_TouchClick(object sender, RoutedEventArgs e)
        {
            GoToNextPage();
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            List<string> filter = new List<string>( new string[] {"*.gif","*.jpg","*.jpeg","*.png"});
            load = new LoadDialog(ActiveSessionManager, this, filter, LoadDialog.LoadDialogType.BackgroundImage);
            Container.Items.Add(load);

            ScatterViewItem newItem = Container.ItemContainerGenerator.ContainerFromItem(load) as ScatterViewItem;
            newItem.Orientation = 0;
            newItem.Height = load.MinHeight + 50;
            newItem.Width = load.MinWidth + 50;

            newItem.Center = CanvasItem.ActualCenter;
        }

        private void ClearImage_Click(object sender, RoutedEventArgs e)
        {

            ActiveSessionManager.CurrentPage.BackgroundImageSource = null;
            loadPageBackground(ActiveSessionManager.CurrentPage);

        }


        private void ScatterViewItem_ScatterManipulationCompleted(object sender, ContainerManipulationCompletedEventArgs e)
        {

            if (canvasHelper.PointInsideContainer((sender as ScatterViewItem).ActualCenter, TrashContainer))
            {
                PrototypeElement selectedPrototypeElement = (sender as ScatterViewItem).Tag as PrototypeElement;
                if (selectedPrototypeElement != null)
                {
                    ActiveSessionManager.RemovePrototypeElementFromPage(selectedPrototypeElement);
                    Container.Items.Remove((sender as ScatterViewItem));
                }
            }
            else
            {
                PrototypeElement instance = ((ScatterViewItem)sender).Tag as PrototypeElement;
                events.SetupEventsPanel(instance);
            }

        }

        public void loadProjectGestureArea()
        {
            IEnumerable<ASGPage> list = from object item in
                                            ActiveSessionManager.CurrentProject.PageDictionary.Values
                                        select item as ASGPage;
            List<ASGPage> PageList = list.ToList();
            foreach (ASGPage page in PageList)
            {

                IEnumerable<PrototypeElement> items = from object item in page.PrototypeElementDictionary.Values
                                                      select item as PrototypeElement;
                List<PrototypeElement> ItemsList = items.ToList();
                foreach (PrototypeElement item in ItemsList)
                {
                    addThisElementToCanvas(page, item, item.Orientation, Visibility.Hidden);

                }
            }

        }

        /*
       * ADDS AN ELEMENT TO CANVAS (GESTURE AREA)
       * ADDS IT TO CONTAINER AND ACTIVESESSIONMANAGER
       * AND BINDS IT TO A PROTOTYPE ELEMENT
       */
        public void addThisElementToCanvas(ASGPage page, PrototypeElement _element,
            double _orientation, System.Windows.Visibility _visibility = System.Windows.Visibility.Visible)
        {
            ScatterViewItem svi = new ScatterViewItem();
            PrototypeElement element;

            Container.Items.Add(svi);

            svi.Visibility = _visibility;

            svi.BorderThickness = new Thickness(20, 20, 20, 20);
            svi.Background = new SolidColorBrush(Colors.Transparent);
            svi.ContainerManipulationCompleted += ScatterViewItem_ScatterManipulationCompleted;

            if (_element == null)
                element = new PrototypeElement { ElementType = ElementTypes.None, Orientation = _orientation, Center = svi.ActualCenter };
            else
            {
                element = new PrototypeElement
                {
                    ElementType = _element.ElementType,
                    Orientation = _element.Orientation,
                    Center = _element.Center,
                    Width = _element.Width,
                    Height = _element.Height,
                    Content = _element.Content,
                    GestureTargetPageMap = _element.GestureTargetPageMap
                };
            }

            canvasHelper.BindScatterViewItemAndElement(svi, element);          

            ActiveSessionManager.AddPrototypeElementToPage(page, element);
        }


     


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (preview != null)
                preview.Close();

            Application.Current.Shutdown();
        }

     



  

  
    }

    
}
