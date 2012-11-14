using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Ink;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using Microsoft.Surface.Presentation.Controls;

namespace ASG.UI
{
    public class PenSettings : INotifyPropertyChanged
    {
        private InkCanvas _targetInkCanvas;
        private SurfaceInkCanvas _SurfacetargetInkCanvas;
        public PenSettings(InkCanvas targetInkCanvas)
        {
            _targetInkCanvas = targetInkCanvas;
        }



        

        public PenSettings(SurfaceInkCanvas targetInkCanvas)
        {
            _SurfacetargetInkCanvas = targetInkCanvas;
        }

        /// <summary>
        /// Used for bindings from the UI to set the eraser width
        /// </summary>
        public double EraserWidth
        {
            get
            {
                if (_targetInkCanvas != null)
                    return _targetInkCanvas.EraserShape.Width;
                else return 0;
            }
            set
            {
                if (_targetInkCanvas.EraserShape.GetType() == typeof(EllipseStylusShape))
                {
                    _targetInkCanvas.EraserShape = new EllipseStylusShape(value, _targetInkCanvas.EraserShape.Height);
                }
                else
                    _targetInkCanvas.EraserShape = new RectangleStylusShape(value, _targetInkCanvas.EraserShape.Height);
                InkCanvasEditingMode temp = _targetInkCanvas.ActiveEditingMode;
                _targetInkCanvas.EditingMode = InkCanvasEditingMode.None;
                _targetInkCanvas.EditingMode = temp;
                NotifyPropertyChanged("EraserWidth");
            }
        }

        /// <summary>
        /// Used for bindings from the UI to set the eraser height
        /// </summary>
        public double EraserHeight
        {
            get
            {
                if (_targetInkCanvas != null)
                    return _targetInkCanvas.EraserShape.Height;
                else return 0;
            }
            set
            {
                if (_targetInkCanvas.EraserShape.GetType() == typeof(System.Windows.Ink.EllipseStylusShape))
                    _targetInkCanvas.EraserShape = new System.Windows.Ink.EllipseStylusShape(_targetInkCanvas.EraserShape.Width, value);
                else
                    _targetInkCanvas.EraserShape = new System.Windows.Ink.RectangleStylusShape(_targetInkCanvas.EraserShape.Width, value);
                InkCanvasEditingMode temp = _targetInkCanvas.ActiveEditingMode;
                _targetInkCanvas.EditingMode = InkCanvasEditingMode.None;
                _targetInkCanvas.EditingMode = temp;
                NotifyPropertyChanged("EraserHeight");
            }
        }
        public SolidColorBrush PenBrush
        {
            get
            {
                if (_targetInkCanvas != null)
                    return new SolidColorBrush(_targetInkCanvas.DefaultDrawingAttributes.Color);
                else
                    return null;
            }
            set
            {
                _targetInkCanvas.DefaultDrawingAttributes.Color = value.Color;
                NotifyPropertyChanged("PenBrush");
            }
        }
        public double PenColorRed
        {
            get
            {
                if (_targetInkCanvas != null)
                    return _targetInkCanvas.DefaultDrawingAttributes.Color.R;
                else 
                    return 255;
            }
            set
            {
                _targetInkCanvas.DefaultDrawingAttributes.Color = Color.FromArgb(_targetInkCanvas.DefaultDrawingAttributes.Color.A, (byte)value, _targetInkCanvas.DefaultDrawingAttributes.Color.G, _targetInkCanvas.DefaultDrawingAttributes.Color.B);
                NotifyPropertyChanged("PenBrush");
                NotifyPropertyChanged("PenColorRed");
            }
        }
        public double PenColorGreen
        {
            get
            {
                if (_targetInkCanvas != null)
                    return _targetInkCanvas.DefaultDrawingAttributes.Color.G;
                else
                    return 255;
            }
            set
            {
                _targetInkCanvas.DefaultDrawingAttributes.Color = Color.FromArgb(_targetInkCanvas.DefaultDrawingAttributes.Color.A, _targetInkCanvas.DefaultDrawingAttributes.Color.R, (byte)value, _targetInkCanvas.DefaultDrawingAttributes.Color.B);
                NotifyPropertyChanged("PenBrush");
                NotifyPropertyChanged("PenColorGreen");
            }
        }
        public double PenColorBlue
        {
            get
            {
                if (_targetInkCanvas != null)
                    return _targetInkCanvas.DefaultDrawingAttributes.Color.B;
                return 255;
            }
            set
            {
                _targetInkCanvas.DefaultDrawingAttributes.Color = Color.FromArgb(_targetInkCanvas.DefaultDrawingAttributes.Color.A, _targetInkCanvas.DefaultDrawingAttributes.Color.R, _targetInkCanvas.DefaultDrawingAttributes.Color.G, (byte)value);
                NotifyPropertyChanged("PenBrush");
                NotifyPropertyChanged("PenColorBlue");
            }
        }

        private double _currentAngle = 0;

        public double PenRotation
        {
            get 
            {
                return _currentAngle;
            }
            set
            {
                _targetInkCanvas.DefaultDrawingAttributes.StylusTipTransform = new RotateTransform(value).Value;
                _currentAngle = value;
                NotifyPropertyChanged("PenRotation");
            }
        }

        private double _currentSkew = 0;

        public double PenSkew
        {
            get
            {
                return _currentSkew;
            }
            set
            {
                _targetInkCanvas.DefaultDrawingAttributes.StylusTipTransform = new SkewTransform(value, value).Value;
                _currentSkew = value;
                NotifyPropertyChanged("PenSkew");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
