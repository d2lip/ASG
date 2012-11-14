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
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;
using TouchToolkit.GestureProcessor.Exceptions;
using TouchToolkit.GestureProcessor.Utility;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions
{
    /// <summary>
    /// Validates the lifetime of a touch
    /// </summary>
    public class TouchTimeValidator : IPrimitiveConditionValidator
    {
       public  int Order()
        {
            return 1;
        }

        TouchTime _data;
        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as TouchTime;
        }

        public ValidSetOfPointsCollection Validate(List<TouchPoint2> points)
        {
            ValidSetOfPointsCollection sets = new ValidSetOfPointsCollection();
            bool result = true;

            foreach (var point in points)
            {
                // TODO: We need to check the unit type (i.e. sec, min,...) and compare accordingly
                
                if (_data.Unit.StartsWith("msec"))
                {
                    if (point.Age.TotalMilliseconds <= _data.Value)
                        result = false;
                }
                else if (_data.Unit.StartsWith("sec"))
                {
                    if (point.Age.TotalSeconds <= _data.Value)
                        result = false;
                }
                else
                {
                    throw new LanguageSyntaxErrorException("Invalid unit for \"TouchTime\" primitive condition!");
                }
            }

            if (result)
                sets.Add(new ValidSetOfTouchPoints(points));

            return sets;
        }

        public bool Equals(IPrimitiveConditionValidator rule)
        {
            throw new NotImplementedException();
        }

        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            ValidSetOfPointsCollection validSets = new ValidSetOfPointsCollection();
            foreach (var item in sets)
            {
                // Because we know it will return only one item in the list
                var results = Validate(item);
                if (results.Count > 0)
                    validSets.Add(results[0]);
            }

            return validSets;
        }

        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            List<IPrimitiveConditionData> result = new List<IPrimitiveConditionData>();
            TouchTime ruleData = new TouchTime();

            ruleData.Unit = "secs";
            foreach (TouchPoint2 point in points)
            {
                if (point.Action == TouchAction.Move)
                {
                    if (point.Tag == null) {
                        if (point.isFinger)
                        {
                            double length = TrigonometricCalculationHelper.CalculatePathLength(point);
                            if (length >= 5)
                                continue;
                        }
                       

                    }
                    ruleData.Value = point.Age.Seconds;
                    result.Add(ruleData);               
                }
            
            }



            return result;
        }


        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {
            return null;
         
        }

        private float GetAverageTouchPointAge(List<TouchPoint2> points)
        {
            double val = 0f;
            foreach (var p in points)
            {
                val += p.Age.TotalSeconds;
            }

            return (float)val / points.Count;
        }

    }
}
