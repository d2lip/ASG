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
    public class IntersectTouchPathValidator : IPrimitiveConditionValidator
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
            throw new NotImplementedException();
        }

        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            throw new NotImplementedException();
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
