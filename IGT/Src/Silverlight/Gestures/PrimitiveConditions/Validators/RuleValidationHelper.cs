using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TouchToolkit.GestureProcessor.Utility.TouchHelpers;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Validators
{
    public class RuleValidationHelper
    {
        public static Rect GetBoundingBox(TouchPoint2 point)
        {
            Rect location = point.Stroke.GetBounds();
            location.Height += 60;
            location.Width += 60;
            location.X -= 30;
            location.Y -= 30;

            return location;
        }

        public static bool HasPreviousTouchPoints(Rect location, TouchPoint2 point, int depth, DateTime endTime)
        {
            if (depth == 1)
            {
                return true;
            }
            else
            {
                depth--;
                TouchPoint2 oldTouchPoint = TouchHistoryTracker.GetTouchPoint(location, endTime, point.StartTime);
                if (oldTouchPoint != null)
                    return HasPreviousTouchPoints(location, oldTouchPoint, depth, endTime);
                else
                    return false;
            }

        }

        public static bool HasPreviousTouchPoints(Rect location, TouchPoint2 point, int depth, DateTime endTime, List<TouchPoint2> selectedPoints)
        {
            if (depth == 0)
            {
                return true;
            }
            else
            {
                depth--;
                TouchPoint2 oldTouchPoint = TouchHistoryTracker.GetTouchPoint(location, endTime, point.StartTime);
                if (oldTouchPoint != null)
                {
                    selectedPoints.Add(oldTouchPoint);
                    return HasPreviousTouchPoints(location, oldTouchPoint, depth, endTime, selectedPoints);
                }
                else
                    return false;
            }
        }
    }

    public static class RuleValidatorExtensions
    {
        public delegate ValidSetOfPointsCollection Validate(List<TouchPoint2> points);
        public static ValidSetOfPointsCollection ForEachSet(this ValidSetOfPointsCollection self, Validate validateMethod)
        {
            ValidSetOfPointsCollection validSets = new ValidSetOfPointsCollection();
            foreach (var item in self)
            {
                ValidSetOfPointsCollection list = validateMethod(item);
                foreach (var set in list)
                {
                    validSets.Add(set);
                }
            }

            return validSets;
        }

        public static Point[] ToFilteredPoints(this StylusPointCollection self, int skip)
        {
            if (self.Count < skip)
            {
                // it contains too few items, don't skip any
                skip = 1;
            }

            int index = 0;
            Point[] points = new Point[self.Count / skip + 1];

            for (int i = 0; i < self.Count; i += skip)
            {
                points[index++] = new Point(self[i].X, self[i].Y);
            }

            return points;
        }


    }
}
