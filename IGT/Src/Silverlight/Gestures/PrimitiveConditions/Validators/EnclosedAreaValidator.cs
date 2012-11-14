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
using ShapeRecognizers.ConvexHull;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Validators
{
    public class EnclosedAreaValidator : IPrimitiveConditionValidator
    {
       public  int Order()
        {
            return 1;
        }

        private EnclosedArea _data;
        #region IRuleValidator Members

        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as EnclosedArea;
        }

        public bool Equals(IPrimitiveConditionValidator rule)
        {
            throw new NotImplementedException();
        }

        public ValidSetOfPointsCollection Validate(System.Collections.Generic.List<TouchPoint2> points)
        {
            ValidSetOfPointsCollection sets = new ValidSetOfPointsCollection();
            ValidSetOfTouchPoints set = new ValidSetOfTouchPoints();
            
            foreach (var point in points)
            {
                if (IsEnclosedAreaWithinRange(point.Stroke.StylusPoints))
                    set.Add(point);
            }

            if (set.Count > 0)
                sets.Add(set);

            return sets;
        }

        private bool IsEnclosedAreaWithinRange(StylusPointCollection stylusPointCollection)
        {
            // TODO: Move the magic number to configuration
            Point[] points = stylusPointCollection.ToFilteredPoints(5);

            double area  = ConvexHullArea.GetArea(points);

            if (area >= _data.Min && area <= _data.Max)
                return true;
            else
                return false;
        }

        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            return sets.ForEachSet(Validate);
        }



        #endregion


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
