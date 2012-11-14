using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Surface.Presentation.Controls;
using ActiveStoryTouch.DataModel;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Data;
using ASG.UI.Enums;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using Microsoft.Surface.Presentation.Input;


namespace ASG.UI
{

   

    public class DrawingCanvasHelper
    {
        private SessionManager ActiveSessionManager;
        private InkCanvas PageInkCanvas;
        private ScatterView Container;
        private Color SELECTION_COLOR = Colors.GreenYellow;
        private Color GESTURE_COLOR = Colors.SkyBlue;
        private Ellipse CurrentColor;
        private Image ColorWheel;


        // REFACTOR THIS TO A STATIC CLASS

        public DrawingCanvasHelper(SessionManager _activeSessionManager, InkCanvas _PageInkCanvas, ScatterView _Container, Ellipse _CurrentColor, Image _ColorWheel)
        {
            this.ActiveSessionManager = _activeSessionManager;
            this.PageInkCanvas = _PageInkCanvas;
            this.Container = _Container;
            this.CurrentColor = _CurrentColor;
            this.ColorWheel = _ColorWheel;
        }


        public bool PointInsideContainer(Point p, ScatterViewItem _container)
        {
            return (p.X > (_container.ActualCenter.X - _container.ActualWidth / 2));
        }

      



        public void loadPenSettings(TouchEventArgs e = null)
        {
            double _width = 0;
            double _height = 0;

            if (e != null)
            {
                _width = e.TouchDevice.GetEllipse(PageInkCanvas).Width;
                _height = e.TouchDevice.GetEllipse(PageInkCanvas).Height;
            }

            if ((_width > 3 || _height > 3) ||
                (_width == 0.0 || _height == 0.0))
            {
                _width = 2.5;
                _height = 2.5;
            }

            switch (ActiveSessionManager.ActivePenMode)
            {
                case PenMode.Draw:
                    {
                        PageInkCanvas.DefaultDrawingAttributes.Height = _height ;
                        PageInkCanvas.DefaultDrawingAttributes.Width = _width ;
                        PageInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                        PageInkCanvas.DefaultDrawingAttributes.Color = getColorFromSelector();
                        break;
                    }
                case PenMode.StrokeEraser:
                    {
                        PageInkCanvas.EraserShape = new EllipseStylusShape(_width * 30, _height * 30);
                        PageInkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                        break;

                    }
                case PenMode.Selection:
                    {
                        PageInkCanvas.DefaultDrawingAttributes.Height = _height;
                        PageInkCanvas.DefaultDrawingAttributes.Width = _width ;
                        PageInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                        PageInkCanvas.DefaultDrawingAttributes.Color = SELECTION_COLOR;
                        break;
                    }
                case PenMode.GestureArea:
                    {
                        PageInkCanvas.DefaultDrawingAttributes.Height = _height ;
                        PageInkCanvas.DefaultDrawingAttributes.Width = _width ;
                        PageInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                        PageInkCanvas.DefaultDrawingAttributes.Color = GESTURE_COLOR;
                        break;
                    }
            }
        }




        public void BindScatterViewItemAndElement(ScatterViewItem svi,  PrototypeElement element)
        {
            svi.Tag = element;

            PrototypeElement _element = new PrototypeElement() { Width = element.Width, Height = element.Height, Center = element.Center, Orientation = element.Orientation};

            Binding elementBindings = new Binding();
            elementBindings.Mode = BindingMode.OneWayToSource;
            elementBindings.Source = element;
            elementBindings.Path = new PropertyPath("Width");
            svi.SetBinding(ScatterViewItem.WidthProperty, elementBindings);

            elementBindings = new Binding();
            elementBindings.Mode = BindingMode.OneWayToSource;
            elementBindings.Source = element;
            elementBindings.Path = new PropertyPath("Height");
            svi.SetBinding(ScatterViewItem.HeightProperty, elementBindings);

            elementBindings = new Binding();
            elementBindings.Mode = BindingMode.OneWayToSource;
            elementBindings.Source = element;
            elementBindings.Path = new PropertyPath("Center");
            svi.SetBinding(ScatterViewItem.CenterProperty, elementBindings);

            elementBindings = new Binding();
            elementBindings.Mode = BindingMode.OneWayToSource;
            elementBindings.Source = element;
            elementBindings.Path = new PropertyPath("Orientation");
            svi.SetBinding(ScatterViewItem.OrientationProperty, elementBindings);



            if (element != null)
            {                
                svi.Width = _element.Width;
                svi.Center = _element.Center;
                svi.Height = _element.Height;               
                svi.Orientation = _element.Orientation;
            }
   
        
        }




        public ScatterViewItem RemoveScatterViewItemEffects(ScatterViewItem svi)
        {
            Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome ssc;

            svi.ShowsActivationEffects = false;
            svi.ApplyTemplate();
            ssc = svi.Template.FindName("shadow", svi) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
            ssc.Visibility = Visibility.Hidden;
            svi.ApplyTemplate();

            return svi;
        }
        


        private Color getColorFromSelector()
        {
            SolidColorBrush newBrush = (SolidColorBrush)CurrentColor.Fill;
            Color imageColor = newBrush.Color;
            return imageColor;
        }





     
        /// <summary>
        /// Select a color based on the postion of args.TouchDevice if hit.
        /// </summary>
        /// <param name="args">The arguments for the input event.</param>
        /// <param name="closeOnlyOnHit">Indicates if the ColorWheel should
        /// be kept open when an actual color is chosen.</param>
        /// <returns> true if a color was actually chosen.</returns>
        private bool ChooseColor(InputEventArgs args, bool closeOnlyOnHit)
        {
            // If the color wheel is not visible, bail out
            if (ColorWheel.Visibility == Visibility.Hidden)
            {
                return false;
            }

            // Set the color on the CurrentColor indicator and on the SurfaceInkCanvas
            Color color = GetPixelColor(args.Device);

            // Black means the user touched the transparent part of the wheel. In that 
            // case, leave the color set to its current value
            bool hit = color != Colors.White;

            // close the colorwheel if caller always requests or only if a
            // color is actually chosen.
            bool close = !closeOnlyOnHit || hit;

            if (hit)
            {
                PageInkCanvas.DefaultDrawingAttributes.Color = color;
                CurrentColor.Fill = new SolidColorBrush(color);
            }

            if (close)
            {
                //CurrentEditingMode = SurfaceInkEditingMode.Ink;

                // Replace the color wheel with the current color button
                ColorWheel.Visibility = Visibility.Hidden;
            }

            args.Handled = true;
            return hit;
        }


        public void HandleInputDown(object sender, InputEventArgs args, bool makeVisible)
        {
            // Capture the touch device and handle the event 
            IInputElement element = sender as IInputElement;
            if (element != null && args.Device.Capture(element))
            {
                args.Handled = true;
            }

            if (makeVisible)
            {
                // Overlay the current color button with the color wheel
                ColorWheel.Visibility = Visibility.Visible;
            }
            else
            {
                // Overlay the current color button with the color wheel                
                ChooseColor(args, true);
                ColorWheel.Visibility = Visibility.Hidden;

            }
        }


        private System.Windows.Media.Color GetPixelColor(InputDevice inputDevice)
        {
            // Translate the input point to bitmap coordinates
            double transformFactor = ColorWheel.Source.Width / ColorWheel.ActualWidth;
            Point inputPoint = inputDevice.GetPosition(ColorWheel);
            Point bitmapPoint = new Point(inputPoint.X * transformFactor, inputPoint.Y * transformFactor);

            // The point is outside the color wheel. Return black.
            if (bitmapPoint.X < 0 || bitmapPoint.X >= ColorWheel.Source.Width ||
                bitmapPoint.Y < 0 || bitmapPoint.Y >= ColorWheel.Source.Height)
            {
                return Colors.Black;
            }

            // The point is inside the color wheel. Find the color at the point.
            CroppedBitmap cb = new CroppedBitmap(ColorWheel.Source as BitmapSource, new Int32Rect((int)bitmapPoint.X, (int)bitmapPoint.Y, 1, 1));
            byte[] pixels = new byte[4];
            cb.CopyPixels(pixels, 4, 0);
            return Color.FromRgb(pixels[2], pixels[1], pixels[0]);
        }
        


      
    }
}
