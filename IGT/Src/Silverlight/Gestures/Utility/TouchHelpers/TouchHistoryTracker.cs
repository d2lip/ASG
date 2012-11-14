using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.GestureProcessor.Utility.TouchHelpers
{
    // The history tracker works like a stack. But there is no max limit, instead it overrides old data
    public class TouchHistoryTracker
    {
        static int count = 0;
        public static int Count
        {
            get { return count; }
        }

        static int top = 0;

        // TODO: The history length should be decide dynamically based on the highest requirement of the active gestures
        static int size = 40;

        static TouchPoint2[] _History = new TouchPoint2[size];

        public static void Add(TouchPoint2 touch)
        {
            if (count < size)
                count++;

            top = top % size;
            _History[top] = touch;
            top++;
        }

        public static TouchPoint2 GetTouchPoint(Rect area, DateTime start, DateTime end)
        {
            TouchPoint2 selectedPoint = null;
            int pos = top % size;

            // Starting from most recent history, keep going back and match if any touch data is available
            // within given time frame & location
            for (int i = 1; i < size; i++)
            {
                pos -= i;

                // Since we are going backwards in a circular queue style data structure...
                if (pos < 0)
                    pos += size;

                TouchPoint2 touch = _History[pos];

                if (touch == null)
                    break;

                // Check if this touch point ended before the given time
                if (touch.EndTime < end && touch.StartTime > start)
                {
                    // Check if the touch point is within given area
                    Rect box = touch.Stroke.GetBounds();
                    if (area.Left < box.Left && area.Width > box.Width 
                        && area.Top < box.Top && area.Height > box.Height)
                    {
                        selectedPoint = touch;
                        break;
                    }
                }
            }

            return selectedPoint;
        }

        private static int GetStartPos(DateTime since)
        {
            throw new NotImplementedException();
        }
    }
}
