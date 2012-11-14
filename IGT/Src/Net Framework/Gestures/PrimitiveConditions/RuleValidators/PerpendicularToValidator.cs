using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.GestureProcessor.Utility;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.ShapeRecognizers;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.RuleValidators
{
    public class PerpendicularToValidator : IPrimitiveConditionValidator
    {
        PerpendicularTo _data = null;

        public  int Order()
        {
            return 1;
        }
        public void Init(Objects.IPrimitiveConditionData ruleData)
        {
            _data = ruleData as PerpendicularTo;
        }

        private bool Validate(ValidateBlockResult firstBlockResult, ValidateBlockResult secBlockResult)
        {
            foreach (var firstTouches in firstBlockResult.Data)
            {
                foreach (var firstTouch in firstTouches)
                {
                    foreach (var secTouches in secBlockResult.Data)
                    {
                        foreach (var secTouch in secTouches)
                        {   
                            var stylusPoints1 = firstTouch.Stroke.StylusPoints;
                            var stylusPoints2 = secTouch.Stroke.StylusPoints;

                            // Calculate slope of each line represented by the two sets of touch points
                            double set1Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints1[0], stylusPoints1[stylusPoints1.Count - 1]);
                            double set2Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints2[0], stylusPoints2[stylusPoints2.Count - 1]);

                            double angularDiff = (set1Angle - set2Angle) * 180 / 3.14;
                            if (Math.Abs(angularDiff) > 70 && Math.Abs(angularDiff) < 110)
                            {
                                return true;
                            }
                            else
                            {
                                angularDiff = Math.Abs(angularDiff) - 180;
                                if (Math.Abs(angularDiff) > 70 && Math.Abs(angularDiff) < 110)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
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
            var result = false;
            var firstBlockResults = PartiallyEvaluatedGestures.Get(sets.ExpectedGestureName, _data.Gesture1);
            var secBlockResults = PartiallyEvaluatedGestures.Get(sets.ExpectedGestureName, _data.Gesture2);

            foreach (var firstBlockResult in firstBlockResults)
            {
                foreach (var secBlockResult in secBlockResults)
                {
                    if (Validate(firstBlockResult, secBlockResult))
                    {
                        result = true;
                        // TODO:???
                        firstBlockResult.AssociatedResults.Add(secBlockResult.Id);
                        secBlockResult.AssociatedResults.Add(firstBlockResult.Id);
                        break;
                    }
                }
            }

            if (result)
                return sets;
            else
                return new ValidSetOfPointsCollection();
        }

        private ValidSetOfTouchPoints ValidateLine(TouchPoint2 points)
        {
            ValidSetOfTouchPoints ret = new ValidSetOfTouchPoints();
            Correlation recognizer = new Correlation(points);
            if (Math.Abs(recognizer.RSquared) > .1)
            {
                ret.Add(points);
            }
            return ret;
        }

        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {
          
            /*
            // check to see if the two sets of points are lines
            ValidSetOfTouchPoints line1 = ValidateLine(points[0]);
            ValidSetOfTouchPoints line2 = ValidateLine(points[1]);

            if ((line1.Count == 0) || (line2.Count == 0))
                return null;


            // considering now that they are lines....
            var stylusPoints1 = points[0].Stroke.StylusPoints;
            var stylusPoints2 = points[1].Stroke.StylusPoints;

            // Calculate slope of each line represented by the two sets of touch points
            double set1Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints1[0], stylusPoints1[stylusPoints1.Count - 1]);
            double set2Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints2[0], stylusPoints2[stylusPoints2.Count - 1]);

            double angularDiff = (set1Angle - set2Angle) * 180 / 3.14;
            if (Math.Abs(angularDiff) > 70 && Math.Abs(angularDiff) < 110)
            {
                PerpendicularTo result = new PerpendicularTo();
                result.Gesture1 = "rule1";
                result.Gesture2 = "rule2";                 
                return result;
            }
            else
            {
                angularDiff = Math.Abs(angularDiff) - 180;
                if (Math.Abs(angularDiff) > 70 && Math.Abs(angularDiff) < 110)
                {
                    PerpendicularTo result = new PerpendicularTo();
                    result.Gesture1 = "rule1";
                    result.Gesture2 = "rule2";
                    return result;
                }
            } */

            return null;
                    
        }
        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            return null;
        }



        

        #endregion
    }
}
