using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.Exceptions;
using TouchToolkit.GestureProcessor.ReturnTypes;

namespace TouchToolkit.GestureProcessor.Utility.TouchHelpers
{
    public static class TouchPointExtensions
    {
        public static TouchPath ToTouchPath(this StylusPointCollection self, int deviceId)
        {
            TouchPath touchPath = new TouchPath();

            touchPath.TouchDeviceId = deviceId;

            foreach (var item in self)
                touchPath.Points.Add(new Point(item.X, item.Y));

            return touchPath;
        }

        public static TouchAction2 ToTouchAction(this TouchAction action)
        {
            if (action == TouchAction.Down)
                return TouchAction2.Down;
            else if (action == TouchAction.Up)
                return TouchAction2.Up;
            else if (action == TouchAction.Move)
                return TouchAction2.Move;
            else
                throw new InvalidDataSetException("Unknown touch action type");
        }

        public static TouchAction ToTouchAction(this TouchAction2 action)
        {
            if (action == TouchAction2.Down)
                return TouchAction.Down;
            else if (action == TouchAction2.Up)
                return TouchAction.Up;
            else if (action == TouchAction2.Move)
                return TouchAction.Move;
            else
                throw new InvalidDataSetException("Unknown touch action type");
        }

        public static String SlopeToDirection(double slope)
        {
            String direction = "";
            if (slope != Math.PI)
            {
                slope = slope % (Math.PI);
            }

            if ((slope >= 7 * Math.PI / 8 /*&& slope < Math.Math.PI +0.01*/) ||
                (/*slope >= -Math.PI - 0.01 &&*/ slope < -7 * Math.PI / 8))
            {
                direction = "Left";
            }
            else if ((slope >= 0 && slope < (Math.PI / 8)) ||
                (slope >= -(Math.PI / 8) && slope < 0))
            {
                direction = "Right";
            }
            else if (slope >= Math.PI / 8 && slope < 3 * Math.PI / 8)
            {
                direction = "DownRight";
            }
            else if (slope >= 3 * Math.PI / 8 && slope < 5 * Math.PI / 8)
            {
                direction = "Down";
            }
            else if (slope >= 5 * Math.PI / 8 && slope < 7 * Math.PI / 8)
            {
                direction = "DownLeft";
            }

            else if (slope >= -7 * Math.PI / 8 && slope < -5 * Math.PI / 8)
            {
                direction = "UpLeft";
            }
            else if (slope >= -5 * Math.PI / 8 && slope < -3 * Math.PI / 8)
            {
                direction = "Up";
            }
            else if (slope >= -3 * Math.PI / 8 && slope <= -1 * Math.PI / 8)
            {
                direction = "UpRight";
            }
            return direction;
        }
    }
}
