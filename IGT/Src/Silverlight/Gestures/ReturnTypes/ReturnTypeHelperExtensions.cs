using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TouchToolkit.GestureProcessor.Feedbacks.GestureFeedbacks;
using System.Collections.Generic;

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public static class ReturnTypeHelperExtensions
    {

        public static T Get<T>(this List<IReturnType> self)
        {
            foreach (IReturnType item in self)
                if (item != null)
                    if (item.GetType() == typeof(T))
                        return (T)item;

            return default(T);
        }
    }
}
