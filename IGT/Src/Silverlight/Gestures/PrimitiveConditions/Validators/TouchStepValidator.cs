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
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;
using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions
{
    public class TouchStepValidator : IPrimitiveConditionValidator
    {
       public  int Order()
        {
            return 1;
        }

        TouchStep _data;

        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as TouchStep;
        }

        public bool Equals(IPrimitiveConditionValidator rule)
        {
            // TODO: temp work around
            return false;
        }

        /// <summary>
        /// Only checks if system has the number of history data as mentioned
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public ValidSetOfPointsCollection Validate(List<TouchPoint2> points)
        {
            ValidSetOfPointsCollection sets = new ValidSetOfPointsCollection();

            //Algo: For each point, we need to check if a point has n more touch points in the history.
            //For example, for a point p1 in points collections, we need to check if there is another point
            //p2 where p2.endTime is less than p1.start time. In the same way, is there any p3... and go on 
            //upto nth level where n = _data.TouchCount

            ValidSetOfTouchPoints set = new ValidSetOfTouchPoints();



            // Check if enough history data is recorded
            if (TouchHistoryTracker.Count >= _data.TouchCount)
            {
                foreach (var point in points)
                {
                    DateTime earliestValidTime = GetEarliestValidTime(point);
                    Rect location = RuleValidationHelper.GetBoundingBox(point);


                    if (RuleValidationHelper.HasPreviousTouchPoints(location, point, _data.TouchCount, earliestValidTime))
                        set.Add(point);
                }

            }

            if (set.Count > 0)
                sets.Add(set);

            return sets;
        }

        private DateTime GetEarliestValidTime(TouchPoint2 point)
        {
            DateTime time;

            if (_data.Unit.StartsWith("min"))
                time = point.StartTime.AddMinutes(-_data.TimeLimit);
            else if (_data.Unit.StartsWith("msec"))
                time = point.StartTime.AddMilliseconds(-_data.TimeLimit);
            else // default is seconds unit
                time = point.StartTime.AddSeconds(-_data.TimeLimit);

            return time;
        }


        

        /// <summary>
        /// Only checks if system has the number of history data as mentioned
        /// </summary>
        /// <param name="sets"></param>
        /// <returns></returns>
        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            ValidSetOfPointsCollection validSets = new ValidSetOfPointsCollection();
            foreach (var set in sets)
            {
                ValidSetOfPointsCollection results = Validate(set);
                foreach (var result in results)
                {
                    validSets.Add(result);
                }
            }

            return validSets;
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
