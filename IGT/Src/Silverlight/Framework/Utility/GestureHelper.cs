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

using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.Framework.Utility
{
    public static class GestureHelper
    {
        /// <summary>
        /// Finds the specified gesture from the list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Gesture Get(this List<Gesture> list, string name)
        {
            foreach (var g in list)
            {
                if (g.Name.ToLower() == name.ToLower())
                    return g;
            }
            return null;
        }

        /// <summary>
        /// Returns the list of unique source ui-elements from the specified touch points
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static List<UIElement> GetUIElements(this List<TouchPoint2> self)
        {
            List<UIElement> list = new List<UIElement>();
            foreach (var touchPoint in self)
            {
                if (!list.Contains(touchPoint.Source))
                {
                    list.Add(touchPoint.Source);
                }
            }

            return list;
        }
    }
}
