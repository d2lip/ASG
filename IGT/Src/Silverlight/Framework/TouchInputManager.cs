using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TouchToolkit.Framework.GestureEvents;
using TouchToolkit.Framework.TouchInputProviders;

namespace TouchToolkit.Framework
{
    public class TouchInputManager
    {
        private static TouchInputProvider _listener = null;

        internal static void SetTouchInputHardware(TouchInputProvider listener)
        {
            
            _listener = listener;
            GestureFramework.EventManager.SubscribeTouchEvents();
        }

        /// <summary>
        /// Holds the reference of active touch input provider
        /// </summary>
        public static TouchInputProvider ActiveTouchProvider
        {
            get
            {
                return _listener;
            }
        }
    }
}
