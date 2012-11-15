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

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Validators
{
    public class OnSameObjectValidator : IPrimitiveConditionValidator
    {
       public  int Order()
        {
            return 1;
        }
        #region IRuleValidator Members

        public void Init(IPrimitiveConditionData ruleData)
        {

        }

        public bool Equals(IPrimitiveConditionValidator rule)
        {
            throw new NotImplementedException();
        }

        public ValidSetOfPointsCollection Validate(System.Collections.Generic.List<TouchPoint2> points)
        {
            bool result = true;
            ValidSetOfPointsCollection sets = new ValidSetOfPointsCollection();

            if (points.Count > 1)
            {
                UIElement source = points[0].Source;
                for (int i = 1; i < points.Count; i++)
                {
                    if (source != points[i].Source)
                    {
                        result = false;
                        break;
                    }
                }
            }

            if (result)
                sets.Add(new ValidSetOfTouchPoints(points));

            return sets;
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



            //Always return on same object validator for generating a rule


            return new List<IPrimitiveConditionData>() { new OnSameObject(){Condition = true}};
        }
    }
}
