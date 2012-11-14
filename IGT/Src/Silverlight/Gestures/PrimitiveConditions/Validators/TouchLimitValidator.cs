using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Combinatorial;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;
using TouchToolkit.GestureProcessor.Objects;
using System.Linq;
using TouchToolkit.GestureProcessor.Utility;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions
{
    public class TouchLimitValidator : IPrimitiveConditionValidator
    {
        class TouchLimitType
        {
            public const string Range = "Range";
            public const string FixedValue = "Fixed";
            
        }

       public  int Order()
        {
            return 1;
        }

        private TouchLimit _data;


        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as TouchLimit;
        }

        public ValidSetOfPointsCollection Validate(List<TouchPoint2> points)
        {
            ValidSetOfPointsCollection list = new ValidSetOfPointsCollection();

          
            if (_data.Type == TouchLimitType.FixedValue)
            {
                List<TouchPoint2> analyze = PointTranslator.removeHandRedundancy(points);

                if (analyze.Count == _data.Min)
                {
                    list.Add(new ValidSetOfTouchPoints(analyze));
                }
                else if (analyze.Count > _data.Min)
                {
                    // Generate possible valid combinitions
                    Combinations c = new Combinations(analyze.ToArray(), _data.Min);
                    while (c.MoveNext())
                    {
                        TouchPoint2[] arr = c.Current as TouchPoint2[];
                        ValidSetOfTouchPoints set = new ValidSetOfTouchPoints(arr);
                        list.Add(set);
                    }
                }
            }
            else if (_data.Type == TouchLimitType.Range)
            {
                if (points.Count == _data.Min)
                {
                    list.Add(new ValidSetOfTouchPoints(points));
                }
                else if (points.Count > _data.Min && points.Count <= _data.Max)
                {
                    // All possible combinitions of size between min & max-1 
                    for (int size = _data.Min; size < points.Count; size++)
                    {
                        // Generate possible valid combinitions
                        Combinations c = new Combinations(points.ToArray(), size);
                        while (c.MoveNext())
                        {
                            TouchPoint2[] arr = c.Current as TouchPoint2[];
                            ValidSetOfTouchPoints set = new ValidSetOfTouchPoints(arr);
                            list.Add(set);
                        }
                    }
                }
            }

            return list;
        }

        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            ValidSetOfPointsCollection list = new ValidSetOfPointsCollection();
            foreach (var item in sets)
            {
                var tlist = Validate(item);
                list.AddRange(tlist);
            }

            return list;
        }


        public bool Equals(IPrimitiveConditionValidator rule)
        {
            if (rule is TouchLimitValidator)
            {
                TouchLimitValidator r1 = rule as TouchLimitValidator;

                return (r1._data.Equals(this._data));
            }
            else
                return false;
        }




        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {

            return null;
            
            
        }
        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            List<IPrimitiveConditionData> rules = new List<IPrimitiveConditionData>();

            if (points.Count == 1)
            {
                _data.Min = 1;
                rules.Add(_data);
                return rules;            
            }

            int touchCount = 1;
            for (int i = 1; i < points.Count; i++)
            {
                if ((points[i].TouchDeviceId != points[i - 1].TouchDeviceId)
                    && points[i].StartTime < points[i-1].EndTime) // this makes sure that the second point starts before the previous finishes
                {
                    touchCount++;
                }
            
            }

            _data.Min = touchCount;
            rules.Add(_data);
            
            
            if (rules.Count > 0)
               return rules;
            else
               return null;
        }
    }
}
