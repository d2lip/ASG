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
using TouchToolkit.GestureProcessor.Utility;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions
{
    public class TagValidator : IPrimitiveConditionValidator
    {
       public  int Order()
        {
            return 0;
        }

        private Tag _data;

        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as Tag;
        }

        public ValidSetOfPointsCollection Validate(List<TouchPoint2> points)
        {
            ValidSetOfPointsCollection list = new ValidSetOfPointsCollection();            
            foreach (TouchPoint2 point in points)
            {
                if (point.Tag != null)
                {
                    if (point.Tag == _data.Value)
                        list.Add(new ValidSetOfTouchPoints(points));
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
            return false;
        }


        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {
            return null;
        }
        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            List<IPrimitiveConditionData> rules = new List<IPrimitiveConditionData>();

            Tag tag = new Tag();

            foreach (TouchPoint2 point in points)
            {
                if (point.Tag != null)
                {
                    tag.Value = point.Tag;
                    rules.Add(tag);
                }
            }


            if (rules.Count > 0)
                return rules;
            else
                return null;
        }

      
    }
}
