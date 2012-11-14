using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.GestureProcessor.Utility;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.ShapeRecognizers;
using System.Windows.Input;
using TouchToolkit.GestureProcessor.Utility.TouchHelpers;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Validators
{
    public class TouchRelativePositionValidator : IPrimitiveConditionValidator
    {
        TouchRelativePosition _data = null;
       public  int Order()
        {
            return 1;
        }
        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as TouchRelativePosition;
        }

       


        #region IPrimitiveConditionValidator Members


        public bool Equals(IPrimitiveConditionValidator rule)
        {
            throw new NotImplementedException();
        }

        public ValidSetOfPointsCollection Validate(List<TouchPoint2> points)
        {
            throw new NotImplementedException();
        }


        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            List<ValidateBlockResult> firstBlockResults = PartiallyEvaluatedGestures.Get(sets.ExpectedGestureName, _data.Gesture1);
            List<ValidateBlockResult> secBlockResults = PartiallyEvaluatedGestures.Get(sets.ExpectedGestureName, _data.Gesture2);

            if (firstBlockResults.Count == 0 || secBlockResults.Count == 0)
                return new ValidSetOfPointsCollection();

            TouchPoint2 point1 = firstBlockResults[0].Data[0][0];
            TouchPoint2 point2 = secBlockResults[0].Data[0][0];

            if (_data.Position == TouchRelativePosition.LEFT)
                if (point1.Position.X < point2.Position.X)
                    return sets;

            if (_data.Position == TouchRelativePosition.RIGHT)
                if (point1.Position.X > point2.Position.X)
                    return sets;

           
            return new ValidSetOfPointsCollection();
        }





      

        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {
            return null;
        }

     
        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            List<IPrimitiveConditionData> returnList = new List<IPrimitiveConditionData>();

            // Do all the possible combinations
            for (int j = 0; j < points.Count - 1; j++)
            {
                for (int i = j + 1; i < points.Count; i++)
                {                   

                    TouchPoint2 point1 = points[j];
                    TouchPoint2 point2 = points[i];


                    
                    TouchRelativePosition result = new TouchRelativePosition();
                    result.Gesture1 = (j + 1) + "";
                    result.Gesture2 = (i + 1) + "";
                    double difference = point1.Position.X - point2.Position.X;

                    if (Math.Abs(difference) <= 250)
                        continue;

                    else if (point1.Position.X < point2.Position.X)
                    {                        
                        result.Position = TouchRelativePosition.LEFT;                        
                    }

                    else
                    {
                        result.Position = TouchRelativePosition.RIGHT;
                    }

                    returnList.Add(result);  
                }
               



            }

            return returnList;

        }

      


        #endregion
    }
}
