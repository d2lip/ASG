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

        #region Drawing variables
        private DrawingCanvasHelper canvasHelper;
        private Stroke selectedStrokes;
        private StrokeCollection lastStrokes;

        private Rect originalSelection;
        #endregion



        #region Dialog variables
        private LoadDialog load;
        private SaveDialog save;
        private EventsGesture events;
        private Thumbnails thumbnails;
        public IGT igt;
        private PreviewWindow preview;
        #endregion


        #region System variables
        private int messageCount;
        private double defaultOrientation = 0;
        private DispatcherTimer timerMessage;
        public const int MESSAGE_TIME_PERIOD = 2;

        public SessionManager ActiveSessionManager { get; set; }
        public Window handler;
        public Canvas mainCanvasRoot;
        #endregion




        public Main(Window _handler, Canvas root)
        {
            InitializeComponent();
            handler = _handler;
            igt = new SurfaceApplication.IGT(handler, mainCanvasRoot, Container);

            CreateASG(root);
        }


        #region Initial methods setting font and screen size

        private void CreateASG(Canvas root)
        {
            ActiveSessionManager = new SessionManager(PageInkCanvas, root);

            canvasHelper = new DrawingCanvasHelper(ActiveSessionManager, PageInkCanvas, Container, CurrentColor, ColorWheel);
            this.mainCanvasRoot = root;
            events = new EventsGesture(this);
            thumbnails = new Thumbnails(this);


            setMenuSize(root);

            CanvasItem.Height = root.Height - 50;
            CanvasItem.Width = root.Width - 50;


            PageInkCanvas.Height = root.Height;
            PageInkCanvas.Width = root.Width;


            canvasHelper.RemoveScatterViewItemEffects(TextContainer);
            canvasHelper.RemoveScatterViewItemEffects(TopMenu);
            canvasHelper.RemoveScatterViewItemEffects(LeftMenu);
            canvasHelper.RemoveScatterViewItemEffects(BottomMenu);
            canvasHelper.RemoveScatterViewItemEffects(TrashContainer);

            TextContainer.Visibility = Visibility.Hidden;
            timerMessage = new DispatcherTimer();


            PageInkCanvas.ReleaseAllCaptures();
            canvasHelper.loadPenSettings();
            SetFontSize(root);
        }

        private void setMenuSize(Canvas root)
        {
            TextContainer.Center = new Point((double)root.Width / 2, (double)root.Height / 2);
            CanvasItem.Center = new Point((double)root.Width / 2, (double)root.Height / 2);

            TrashContainer.Center = new Point((double)root.Width - 30, (double)root.Height / 2);
            TrashContainer.Height = root.Height;
            TrashContainer.Width = 50;

            LeftMenu.Width = 50;
            LeftMenu.Center = new Point(10, (double)root.Height / 2.2);
            LeftMenu.Height = root.Height / 1.3;

            TopMenu.Width = root.Width - 50;
            TopMenu.Center = new Point((double)root.Width / 2, 25);
            TopMenu.Height = 50;

            BottomMenu.Width = root.Width;
            BottomMenu.Height = 50;
            BottomMenu.Center = new Point((double)root.Width / 2 + 50, root.Height - 25);
        }




        private void SetFontSize(Canvas root)
        {
            Double _FontSize;

            // based on screen size
            if (root.Width > 1300)
                _FontSize = 35;
            else if (root.Width > 1000)
                _FontSize = 30;
            else
                _FontSize = 25;

            this.Resources["MyFontSize"] = _FontSize;
        }


        #endregion



        #region Main System Functionalities



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


        public void SetupAndRenderPage(ASGPage pageToLoad)
        {
            events.SetupEventsPanel();
            RenderPage(pageToLoad);
            canvasHelper.loadPenSettings();
        }


        public void setContainerItemsVisibility(ASGPage page, Visibility visibility)
        {
            IEnumerable<GestureArea> list = from object item in Container.Items
                                            where (item is GestureArea)
                                                         && ((Container.ItemContainerGenerator.ContainerFromItem(item as GestureArea) as ScatterViewItem).Tag is PrototypeElement)
                                                         && (page.PrototypeElementDictionary.Values.Contains(
                                                                (Container.ItemContainerGenerator.ContainerFromItem(item as GestureArea) as ScatterViewItem).Tag as PrototypeElement)
                                                                )
                                            select item as GestureArea;
            List<GestureArea> ItemsList = list.ToList();
            foreach (GestureArea area in ItemsList)
            {
                ScatterViewItem svi = Container.ItemContainerGenerator.ContainerFromItem(area) as ScatterViewItem;
                svi.Visibility = visibility;
            }
        }

        public void ClearCanvas()
        {
            IEnumerable<GestureArea> list = from object item in Container.Items
                                            where (item is GestureArea)

                                            select item as GestureArea;
            List<GestureArea> ItemsList = list.ToList();
            foreach (GestureArea area in ItemsList)
            {
                Container.Items.Remove(area);
            }
        }




        public void loadPageBackground(ASGPage pageToLoad)
        {
            try
            {
                BackgroundImage.Source = new BitmapImage(new Uri(EnvironmentFolder.getImagesFolder() + pageToLoad.BackgroundImageSource));
                PageInkCanvas.Background = new SolidColorBrush(Colors.Transparent);
            }
            catch (Exception ex)
            {
                PageInkCanvas.Background = new SolidColorBrush(Colors.White);
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
            messageLabel.Width = 800;
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


        private void ScatterViewItem_ScatterManipulationCompleted(object sender, ContainerManipulationCompletedEventArgs e)
        {

            if (canvasHelper.PointInsideContainer((sender as ScatterViewItem).ActualCenter, TrashContainer))
            {
                PrototypeElement selectedPrototypeElement = (sender as ScatterViewItem).Tag as PrototypeElement;
                if (selectedPrototypeElement != null)
                {
                    ActiveSessionManager.RemovePrototypeElementFromPage(selectedPrototypeElement);
                    Container.Items.Remove((sender as ScatterViewItem).Content as GestureArea);
                }
            }
            else
            {
                PrototypeElement instance = ((ScatterViewItem)sender).Tag as PrototypeElement;
                events.SetupEventsPanel(instance);
            }

        }

        public void loadGestureAreasFromCurrentProject()
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
                 Visibility visibility;
                foreach (PrototypeElement item in ItemsList)
                {
                    if (ActiveSessionManager.CurrentPage.UniqueId == page.UniqueId)
                        visibility= Visibility.Visible;
                    else
                         visibility = Visibility.Hidden;
                    addThisElementToCanvas(page, item, item.Orientation, visibility);

                }
            }

        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }



        private GestureArea getNewGestureArea(PrototypeElement elementBase = null)
        {
            GestureArea g = new GestureArea();

            if ((elementBase != null) && (elementBase.BackgroundImage != ""))
            {
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(EnvironmentFolder.getImagesFolder() + elementBase.BackgroundImage, UriKind.Absolute);
                src.EndInit();
                g.BackgroundImage.Source = src;
            }
            g.GestureItem.Click += new RoutedEventHandler(CallGestureListFromMenuItem);
            g.ImageItem.Click += new RoutedEventHandler(CallLoadImageFromMenuItem);

            return g;
        }

        private ScatterViewItem CreateGestureAreaContainer(GestureArea area)
        {
            Container.Items.Add(area);
            ScatterViewItem svi = Container.ItemContainerGenerator.ContainerFromItem(area) as ScatterViewItem;

            svi.Background = new SolidColorBrush(Colors.Transparent);
            svi.ContainerManipulationCompleted += ScatterViewItem_ScatterManipulationCompleted;
            return svi;
        }



        /*
       * ADDS AN ELEMENT TO CANVAS (GESTURE AREA)
       * ADDS IT TO CONTAINER AND ACTIVESESSIONMANAGER
       * AND BINDS IT TO A PROTOTYPE ELEMENT
       */
        public void addThisElementToCanvas(ASGPage page, PrototypeElement _element,
            double _orientation, System.Windows.Visibility _visibility = System.Windows.Visibility.Visible)
        {
            GestureArea area = getNewGestureArea(_element);
            ScatterViewItem svi = CreateGestureAreaContainer(area);
            svi.Visibility = _visibility;
            canvasHelper.BindScatterViewItemAndElement(svi, _element);

           
        }

        #endregion



        #region Drawing Menu Functionalities

        private void OnColorWheelTouchDown(object sender, TouchEventArgs args)
        {
            canvasHelper.HandleInputDown(sender, args, false);
        }
        private void OnColorWheelMouseDown(object sender, MouseButtonEventArgs args)
        {
            canvasHelper.HandleInputDown(sender, args, false);
        }

        private void OnColorWheelMouseUp(object sender, MouseButtonEventArgs args)
        {
            // If the mouse was already captured to the sender, release it
            IInputElement element = sender as IInputElement;
            if (args.Device.GetCaptured() == element)
            {
                element.ReleaseMouseCapture();
            }
        }
        private void OnCurrentColorTouchDown(object sender, TouchEventArgs args)
        {
            canvasHelper.HandleInputDown(sender, args, true);
        }

        private void OnCurrentColorMouseDown(object sender, MouseButtonEventArgs args)
        {
            canvasHelper.HandleInputDown(sender, args, true);
        }




        public void CallGestureListFromMenuItem(object sender, RoutedEventArgs e)
        {
            GestureArea area = Main.FindParent<GestureArea>(sender as ElementMenuItem);
            ScatterViewItem svi = Container.ItemContainerGenerator.ContainerFromItem(area) as ScatterViewItem;
            ShowGestureList(svi.Tag as PrototypeElement);
        }


        public void CallLoadImageFromMenuItem(object sender, RoutedEventArgs e)
        {
            GestureArea area = Main.FindParent<GestureArea>(sender as ElementMenuItem);
            ScatterViewItem svi = Container.ItemContainerGenerator.ContainerFromItem(area) as ScatterViewItem;
            ShowLoadImage(LoadDialog.LoadDialogType.GestureAreaImage, svi);
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            ShowLoadImage(LoadDialog.LoadDialogType.BackgroundImage);
        }

        private void ShowLoadImage(LoadDialog.LoadDialogType type, ScatterViewItem svi = null)
        {
            if (load != null)
            {
                Container.Items.Remove(load);
            }

            List<string> filter = new List<string>(new string[] { "*.gif", "*.jpg", "*.jpeg", "*.png" });
            load = new LoadDialog(ActiveSessionManager, this, filter, type, svi);

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

        private void ClearImage_Click(object sender, RoutedEventArgs e)
        {

            ActiveSessionManager.CurrentPage.BackgroundImageSource = null;
            loadPageBackground(ActiveSessionManager.CurrentPage);

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
                    if (lastStrokes != null)
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




        private void OnStrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs args)
        {

            if (lastStrokes == null)
                lastStrokes = new StrokeCollection();
            lastStrokes.Add(args.Stroke);

            if (ActiveSessionManager.ActivePenMode == PenMode.Selection
                || ActiveSessionManager.ActivePenMode == PenMode.GestureArea)
                selectedStrokes = args.Stroke;
        }

        private void CreateSelection()
        {
            Rect b = selectedStrokes.GetBounds();
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
            else
            {
                DisableSelect();
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

        private void CreateGestureArea()
        {
            Rect b = selectedStrokes.GetBounds();
            PrototypeElement element = new PrototypeElement { BackgroundImage = "", Orientation = defaultOrientation, Width = b.Width, Height = b.Height, Center = new Point(b.Location.X + b.Width / 2, b.Location.Y + b.Height / 2) };
            ActiveSessionManager.AddPrototypeElementToPage(ActiveSessionManager.CurrentPage, element);
            addThisElementToCanvas(ActiveSessionManager.CurrentPage, element, element.Orientation);
        }


        private void StrokeUpCanvas()
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

        }


        private void PageInkCanvas_StylusUp(object sender, StylusEventArgs e)
        {
            StrokeUpCanvas();
            e.Handled = true;
        }

        private void PageInkCanvas_StylusDown(object sender, StylusDownEventArgs e)
        {
            StrokeDownCanvas(null);
            e.Handled = true;
        }


        private void StrokeDownCanvas(TouchEventArgs e)
        {
            canvasHelper.loadPenSettings(e);

        }

        private void PageInkCanvas_TouchUp(object sender, TouchEventArgs e)
        {
            StrokeUpCanvas();
            e.Handled = true;
        }

        private void PageInkCanvas_TouchDown(object sender, TouchEventArgs e)
        {
            StrokeDownCanvas(e);
            e.Handled = true;
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


        #endregion



        #region Bottom Menu Functionalities



        private void Record_Click(object sender, RoutedEventArgs e)
        {            
            var svi = Container.ItemContainerGenerator.ContainerFromItem(igt) as ScatterViewItem;

            if (svi == null)
            {
                Container.Items.Add(igt);
                svi = Container.ItemContainerGenerator.ContainerFromItem(igt) as ScatterViewItem;
                svi.Orientation = 0;
                svi.Width = Container.ActualWidth * 2.5;
                svi.Height = Container.ActualHeight * 2.5;
                igt.Height = Container.ActualHeight / 1.03;
                igt.Width = Container.ActualWidth / 1.5;
                svi.Center = new Point(Width / 2, Height / 2);
                svi.CanMove = false;
                svi.CanScale = false;
                svi.CanRotate = false;
                svi.ShowsActivationEffects = false;
            }
            svi.Visibility = Visibility.Visible;
        }


        private void ShowGestureList(PrototypeElement instance = null)
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
                svi.Height = this.Height / 1.3;
                svi.Width = this.Width / 1.7;
                svi.Visibility = System.Windows.Visibility.Hidden;
                svi.Center = new Point(svi.Width / 2, svi.Height / 2); // CanvasItem.ActualCenter;
            }

            events.SetupEventsPanel(instance);

            if (svi.Visibility == System.Windows.Visibility.Visible)
                Container.Items.Remove(events);
            else
            {
                svi.Visibility = System.Windows.Visibility.Visible;
            }
        }



        private void lefthelp_TouchClick(object sender, RoutedEventArgs e)
        {
            GoToPreviousPage();
        }



        private void righthelp_TouchClick(object sender, RoutedEventArgs e)
        {
            GoToNextPage();
        }

        // REMOVE PAGE       
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ASGPage pagetoRemove = ActiveSessionManager.CurrentPage;


            if (!ActiveSessionManager.RemovePage(pagetoRemove))
            {
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


        // DUPLICATE PAGE
        private void DuplicatePage_Click(object sender, RoutedEventArgs e)
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
                addDuplicateOfThisElementToCanvas(ActiveSessionManager.CurrentPage, item);
            }
            PageNumber.Text = ActiveSessionManager.CurrentPage.PageNumber + "/" + ActiveSessionManager.CurrentProject.PageDictionary.Count;
            ShowMessageOnCanvas("Page successfully copied", MESSAGE_TIME_PERIOD);
        }

        private void addDuplicateOfThisElementToCanvas(ASGPage aSGPage, PrototypeElement item)
        {
            PrototypeElement duplicatedElement = new PrototypeElement
            {
                BackgroundImage = item.BackgroundImage,
                ElementType = item.ElementType,
                Orientation = item.Orientation,
                Center = item.Center,
                Width = item.Width,
                Height = item.Height,

            };

            foreach (string key in item.GestureTargetPageMap.Keys)
            {
                long page;
                if (item.GestureTargetPageMap.TryGetValue(key, out page))
                    duplicatedElement.GestureTargetPageMap.Add(key, page);

            }


            GestureArea area = getNewGestureArea(duplicatedElement);
            ScatterViewItem svi = CreateGestureAreaContainer(area);

            canvasHelper.BindScatterViewItemAndElement(svi, duplicatedElement);

            ActiveSessionManager.AddPrototypeElementToPage(aSGPage, duplicatedElement);
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
                svi.Width = this.Width / 1.3;
                svi.CanMove = false;
                svi.CanRotate = false;
                svi.CanScale = false;

                svi.Visibility = System.Windows.Visibility.Hidden;
                svi.Center = CanvasItem.ActualCenter;
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
            ShowGestureList();
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

        #endregion



        #region Top Menu Functionalities




        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (preview != null)
                preview.Close();

            Application.Current.Shutdown();
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            preview = new PreviewWindow(this);
            preview.Show();
            preview.previewPage();
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
                newItem.Height = save.MaxHeight;
                newItem.Width = this.Width / 3;
                newItem.Center = CanvasItem.ActualCenter;
            }

        }

        private void LoadProject_Click(object sender, RoutedEventArgs e)
        {
            if (load != null)
            {
                Container.Items.Remove(load);

            }
            List<string> filter = new List<string>(new string[] { "*.asgproj" });
            load = new LoadDialog(ActiveSessionManager, this, filter, LoadDialog.LoadDialogType.Project);

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

        #endregion




    }


}
