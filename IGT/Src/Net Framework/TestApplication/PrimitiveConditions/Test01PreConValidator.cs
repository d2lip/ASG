using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;

namespace TestApplication.PrimitiveConditions
{
    public class Test01PreConValidator : IPrimitiveConditionValidator
    {
        #region IPrimitiveConditionValidator Members

        public void Init(TouchToolkit.GestureProcessor.PrimitiveConditions.Objects.IPrimitiveConditionData ruleData)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IPrimitiveConditionValidator rule)
        {
            throw new NotImplementedException();
        }

        public TouchToolkit.GestureProcessor.Objects.ValidSetOfPointsCollection Validate(List<TouchToolkit.GestureProcessor.Objects.TouchPoint2> points)
        {
            throw new NotImplementedException();
        }

        public TouchToolkit.GestureProcessor.Objects.ValidSetOfPointsCollection Validate(TouchToolkit.GestureProcessor.Objects.ValidSetOfPointsCollection sets)
        {
            throw new NotImplementedException();
        }

        public TouchToolkit.GestureProcessor.PrimitiveConditions.Objects.IPrimitiveConditionData GenerateRuleData(List<TouchToolkit.GestureProcessor.Objects.TouchPoint2> points)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
