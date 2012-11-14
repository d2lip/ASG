using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;

namespace TouchToolkit.Framework.Utility
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Sets the value of a property based on the property-name using reflection
        /// </summary>
        /// <param name="self"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetValue(this object self, string propertyName, object value)
        {
            Type myType = self.GetType();
            PropertyInfo[] propInfos = myType.GetProperties();
            foreach (PropertyInfo propInfo in propInfos)
            {
                if (propInfo.Name == propertyName)
                {
                    if (propInfo.CanWrite)
                    {
                        propInfo.SetValue(self, value, null);
                        break;
                    }
                }
            }
        }

    }
}
