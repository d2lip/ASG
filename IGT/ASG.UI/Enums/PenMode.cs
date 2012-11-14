using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ASG.UI.Enums
{
    public enum PenMode
    {
        Draw, PointEraser, StrokeEraser, Selection, GestureArea, Circle, Rectangle          //Defaulting to Draw mode
    }

    [ValueConversion(typeof(PenMode), typeof(String))]
    public class PenModeHelper : IValueConverter
    {
        const String DRAWMODETEXT = "Drawing";
        const String ERASERMODETEXT = "Point Eraser";
        const String SELECTMODETEXT = "Selection";
        const String ERASERSTROKEMODETEXT = "Stroke Eraser";
        const String GESTUREAREAMODETEXT = "Gesture";
        const String CIRCLEMODETEXT = "Circle";
        const String RECTANGLEMODETEXT = "Rectangle";
        public static String EnumToString(PenMode enumValue)
        {
            String result = String.Empty;
            switch (enumValue)
            {
                case PenMode.Draw:
                    result = DRAWMODETEXT;
                    break;
                case PenMode.PointEraser:
                    result = ERASERMODETEXT;
                    break;
                case PenMode.StrokeEraser:
                    result = ERASERSTROKEMODETEXT;
                    break;
                case PenMode.Selection:
                    result = SELECTMODETEXT;
                    break;
                case PenMode.GestureArea:
                    result = GESTUREAREAMODETEXT;
                    break;

                case PenMode.Circle:
                    result = CIRCLEMODETEXT;
                    break;
                case PenMode.Rectangle:
                    result = RECTANGLEMODETEXT;
                    break;

                default:
                    break;
            }
            return result;
        }

        
        public static PenMode StringToEnum(String stringValue)
        {
            if (stringValue.Equals(DRAWMODETEXT))
            {
                return PenMode.Draw;
            }
            else if (stringValue.Equals(ERASERMODETEXT))
            {
                return PenMode.PointEraser;
            }
            else if (stringValue.Equals(SELECTMODETEXT))
            {
                return PenMode.Selection;
            }
            else if (stringValue.Equals(ERASERSTROKEMODETEXT))
            {
                return PenMode.StrokeEraser;
            }
            else if (stringValue.Equals(GESTUREAREAMODETEXT))
            {
                return PenMode.GestureArea;
            }
            else if (stringValue.Equals(CIRCLEMODETEXT))
            {
                return PenMode.Circle;
            }
            else if (stringValue.Equals(RECTANGLEMODETEXT))
            {
                return PenMode.Rectangle;
            }
            return PenMode.Draw;
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return EnumToString((PenMode)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return StringToEnum(value as String);
        }

        #endregion
    }
}
