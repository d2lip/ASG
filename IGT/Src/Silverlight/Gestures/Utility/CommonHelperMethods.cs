using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;


namespace TouchToolkit.GestureProcessor.Utility
{
    //TODO: Change to more meaningful name
    public static class CommonHelperMethods
    {
        //public static bool IsTypeOf(this object self, Type type)
        //{
        //    // TODO: Check with base class


        //    // Check with implemented interfaces
        //    Type value = self.GetType().GetInterface(type.Name, false);

        //    if (value != null)
        //        return true;
        //    else
        //        return false;
        //}







        public static Point ToPoint(this StylusPoint point)
        {
            return new Point(point.X, point.Y);
        }

        public static bool IsTypeOf(this Type type, Type typeToMatch)
        {
            bool result = false;
            Type[] interfaces = type.GetInterfaces();
            foreach (var i in interfaces)
            {
                if (i.IsAssignableFrom(typeToMatch))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
