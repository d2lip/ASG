using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TouchToolkit.GestureProcessor.Exceptions;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;
using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions
{
    public class TouchAreaValidator : IPrimitiveConditionValidator
    {
        private TouchArea _data;

       public  int Order()
        {
            return 1;
        }

        private double Radius
        {
            get
            {
                return double.Parse(_data.Value);
            }
        }

        public bool Equals(IPrimitiveConditionValidator rule)
        {
            throw new NotImplementedException();
        }

        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as TouchArea;
        }

        public ValidSetOfPointsCollection Validate(List<TouchPoint2> points)
        {
            ValidSetOfPointsCollection sets = IsValid(points);

            return sets;
        }

        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            ValidSetOfPointsCollection result = new ValidSetOfPointsCollection();
            foreach (var item in sets)
            {
                ValidSetOfPointsCollection list = Validate(item);
                foreach (var set in list)
                {
                    result.Add(set);
                }
            }

            return result;
        }

        private ValidSetOfPointsCollection IsValid(List<TouchPoint2> points)
        {
            bool result = true;

            ValidSetOfPointsCollection sets = new ValidSetOfPointsCollection();

            if (points.Count > 0)
            {
                //double minX = int.MinValue, minY = int.MinValue, maxX = int.MaxValue, maxY = int.MaxValue;

                Rect area = new Rect(points[0].Position, new Size(0, 0));

                // Calculate the bounding box that covers all points
                foreach (var point in points)
                {
                    if (_data.HistoryLevel > 0)
                    {
                        Rect location = RuleValidationHelper.GetBoundingBox(point);
                        List<TouchPoint2> selectedPoints = new List<TouchPoint2>();
                        result = RuleValidationHelper.HasPreviousTouchPoints(location, point, _data.HistoryLevel, point.StartTime.AddSeconds(-_data.HistoryTimeLine), selectedPoints);

                        if (result) // Has required number of previous touch points in selected location
                        {
                            foreach (var p in selectedPoints)
                            {
                                area.Union(p.Position);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    area.Union(point.Position);
                }

                // TODO: We need to implement circular area too

                if (result && area.Height <= _data.Height && area.Width <= _data.Width)
                {
                    sets.Add(new ValidSetOfTouchPoints(points));
                }

            }
            return sets;
        }




        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {
            return null;
        }

        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            return null;
        }
    }
}
