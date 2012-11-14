using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TouchToolkit.GestureProcessor.Utility;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.ShapeRecognizers;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Validators
{
    public class ClosedLoopValidator : IPrimitiveConditionValidator
    {
        private ClosedLoop _data;
        int threshHold = 80; // 100 pixel
       public  int Order()
        {
            return 1;
        }

        #region IRuleValidator Members

        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as ClosedLoop;
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
                if (IsClosedLoop(point))
                    set.Add(point);
            }

            if (set.Count > 0)
                sets.Add(set);

            return sets;
        }

        private bool IsClosedLoop(TouchPoint2 point)
        {

            double length = TrigonometricCalculationHelper.CalculatePathLength(point);

            if (length < 50)
                return false;
            // Check the distance between start and end point
            if (point.Stroke.StylusPoints.Count > 1)
            {
                StylusPoint firstPoint = point.Stroke.StylusPoints[0];
                StylusPoint lastPoint = point.Stroke.StylusPoints[point.Stroke.StylusPoints.Count - 1];

                double distance = TrigonometricCalculationHelper.GetDistanceBetweenPoints(firstPoint, lastPoint);
                Correlation recognizer = new Correlation(point);
                if (Math.Abs(recognizer.RSquared) < 0.1)
                {
                    if (distance < threshHold)
                    {
                        return true;
                    }
                }
               
            }
            return false;
        }

        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            return sets.ForEachSet(Validate);
        }



        #endregion


        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {
            bool singlePoint = false;
            foreach (TouchPoint2 point in points) {
                singlePoint = TrigonometricCalculationHelper.isASinglePoint(point);
                  if(singlePoint)
                    return null;  // A point can't be a closed loop

                  // if it is not a finger, it can't be a closed loop
                  if (!point.isFinger)
                      continue;
            }                   
            
             ValidSetOfPointsCollection valids =  Validate(points);
             if ((valids != null) && (valids.Count > 0)) { 
                 _data.State = "true";
                 return _data;
             }
             return null;
        }

        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            List<IPrimitiveConditionData> result = new List<IPrimitiveConditionData>();
            bool singlePoint = false;
            foreach (TouchPoint2 point in points)
            {
                singlePoint = TrigonometricCalculationHelper.isASinglePoint(point);
                if (singlePoint)
                {
                    result.Add(null);
                    continue;
                } // A point can't be a closed loop

                // if it is not a finger, it can't be a closed loop
                if (!point.isFinger)
                {
                    result.Add(null);
                    continue;
                }

                ValidSetOfPointsCollection valids = Validate(points);
                if ((valids != null) && (valids.Count > 0))
                {
                    _data.State = "true";
                    result.Add(_data);
                }
            }

           
            return result;
        }
    }
}
