using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Validators
{
    public class TouchPathBoundingBoxValidator : IPrimitiveConditionValidator
    {
        TouchPathBoundingBox _data;
       public  int Order()
        {
            return 1;
        }

        #region IRuleValidator Members

        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as TouchPathBoundingBox;
        }

        public bool Equals(IPrimitiveConditionValidator rule)
        {
            throw new NotImplementedException();
        }

        public ValidSetOfPointsCollection Validate(System.Collections.Generic.List<TouchPoint2> points)
        {
            ValidSetOfPointsCollection sets = new ValidSetOfPointsCollection();

            if (points.Count > 0)
            {
                Rect parent = points[0].Stroke.GetBounds();
                for (int i = 1; i < points.Count; i++)
                {
                    Rect rect = points[i].Stroke.GetBounds();
                    parent.Union(rect);
                }


                if (parent.Height >= _data.MinHeight && parent.Height <= _data.MaxHeight
                    && parent.Width >= _data.MinWidth && parent.Width <= _data.MaxWidth)
                {
                    ValidSetOfTouchPoints set = new ValidSetOfTouchPoints(points);
                    sets.Add(set);
                }
            }
            return sets;
        }

        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            //TODO: move this code block to base class or extension methods. its common in many validators
            ValidSetOfPointsCollection validSets = new ValidSetOfPointsCollection();
            foreach (var set in sets)
            {
                var list = Validate(set);
                foreach (var item in list)
                {
                    validSets.Add(item);
                }
            }

            return validSets;
        }

        #endregion


        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {
            if (points.Count > 0)
            {
                Rect parent = points[0].Stroke.GetBounds();
                for (int i = 1; i < points.Count; i++)
                {
                    Rect rect = points[i].Stroke.GetBounds();
                    parent.Union(rect);
                }

                TouchPathBoundingBox data = new TouchPathBoundingBox();
                data.MinHeight = parent.Height;
                data.MinWidth = parent.Width;

                return data;

            }
            return null;
        }

        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            return null;
        }
    }
}
