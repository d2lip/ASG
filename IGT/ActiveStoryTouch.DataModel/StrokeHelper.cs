using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Ink;
using System.Windows.Input;

namespace ActiveStoryTouch.DataModel
{
    public class StrokeHelper
    {
        public static StrokeCollection ConvertToStrokeCollection(SerializableStrokeCollection strokeCollection)
        {
            StrokeCollection resultCollection = new StrokeCollection();
            foreach (var stroke in strokeCollection)
            {
                DrawingAttributes drawingAttr = new DrawingAttributes
                {
                    Color = stroke.DrawingAttributes.Color,
                    FitToCurve = stroke.DrawingAttributes.FitToCurve,
                    Height = stroke.DrawingAttributes.Height,
                    Width = stroke.DrawingAttributes.Width,
                    IgnorePressure = stroke.DrawingAttributes.IgnorePressure,
                    IsHighlighter = stroke.DrawingAttributes.IsHighlighter,
                    StylusTipTransform = stroke.DrawingAttributes.StylusTipTransform
                };
                switch (stroke.DrawingAttributes.StylusTip)
                {
                    case SerializableDrawingAttributes.StylusTips.Ellipse:
                        drawingAttr.StylusTip = StylusTip.Ellipse;
                        break;
                    case SerializableDrawingAttributes.StylusTips.Rectangle:
                        drawingAttr.StylusTip = StylusTip.Rectangle;
                        break;
                    default:
                        break;
                }

                StylusPointCollection spc = new StylusPointCollection();
                foreach (var stylusPoint in stroke.StylusPoints)
                {
                    StylusPoint sp = new StylusPoint { X = stylusPoint.X, Y = stylusPoint.Y, PressureFactor = stylusPoint.PressureFactor };
                    spc.Add(sp);
                }
                Stroke newStroke = new Stroke(spc);
                newStroke.DrawingAttributes = drawingAttr;
                resultCollection.Add(newStroke);
            }
            return resultCollection;
        }
        public static SerializableStrokeCollection ConvertToSerializableStrokeCollection(StrokeCollection strokeCollection)
        {
            SerializableStrokeCollection resultCollection = new SerializableStrokeCollection();
            foreach (var stroke in strokeCollection)
            {
                SerializableDrawingAttributes drawingAttr = new SerializableDrawingAttributes
                {
                    Color = stroke.DrawingAttributes.Color,
                    FitToCurve = stroke.DrawingAttributes.FitToCurve,
                    Height = stroke.DrawingAttributes.Height,
                    Width = stroke.DrawingAttributes.Width,
                    IgnorePressure = stroke.DrawingAttributes.IgnorePressure,
                    IsHighlighter = stroke.DrawingAttributes.IsHighlighter,
                    StylusTipTransform = stroke.DrawingAttributes.StylusTipTransform
                };
                switch (stroke.DrawingAttributes.StylusTip)
                {
                    case StylusTip.Ellipse:
                        drawingAttr.StylusTip = SerializableDrawingAttributes.StylusTips.Ellipse;
                        break;
                    case StylusTip.Rectangle:
                        drawingAttr.StylusTip = SerializableDrawingAttributes.StylusTips.Rectangle;
                        break;
                    default:
                        break;
                }

                SerializableStroke newStroke = new SerializableStroke { DrawingAttributes = drawingAttr };

                foreach (var stylusPoint in stroke.StylusPoints)
                {
                    newStroke.StylusPoints.Add(new SerializableStylusPoint { PressureFactor = stylusPoint.PressureFactor, X = stylusPoint.X, Y = stylusPoint.Y });
                }
                resultCollection.Add(newStroke);
            }
            return resultCollection;
        }
    }
}
