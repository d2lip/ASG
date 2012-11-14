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
    public class AngleBetweenValidator : IPrimitiveConditionValidator
    {
       public  int Order()
        {
            return 1;
        }

        AngleBetween _data = null;
        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as AngleBetween;
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
                            double angleValueMin = _data.Min;
                            double angleValueMax = _data.Max;
                            // Calculate slope of each line represented by the two sets of touch points
                            double set1Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints1[0], stylusPoints1[stylusPoints1.Count - 1]);
                            double set2Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints2[0], stylusPoints2[stylusPoints2.Count - 1]);

                            double angularDiff = (set1Angle - set2Angle) * 180 / 3.14;
                            if (Math.Abs(angularDiff) > angleValueMin && Math.Abs(angularDiff) < angleValueMax)
                            {
                                return true;
                            }
                            else
                            {
                                angularDiff = Math.Abs(angularDiff) - 180;
                                if (Math.Abs(angularDiff) > angleValueMin && Math.Abs(angularDiff) < angleValueMax)
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
            Console.WriteLine("AngleName:" + sets.ExpectedGestureName);

            List<ValidateBlockResult> firstBlockResults = PartiallyEvaluatedGestures.Get(sets.ExpectedGestureName, _data.Gesture1);
            List<ValidateBlockResult> secBlockResults = PartiallyEvaluatedGestures.Get(sets.ExpectedGestureName, _data.Gesture2);

            if (firstBlockResults.Count == 0 || secBlockResults.Count == 0)
                return new ValidSetOfPointsCollection();

            ValidSetOfPointsCollection step1 = firstBlockResults[0].Data;

            ValidSetOfTouchPoints line1 = ValidateLine(step1[0][0]);

            if (line1.Count > 0)
            {
                ValidSetOfPointsCollection step2 = secBlockResults[0].Data;
                ValidSetOfTouchPoints line2 = ValidateLine(step2[0][0]);

                if (line2.Count == 0)
                {
                    return new ValidSetOfPointsCollection();
                }
                var stylusPoints1 = TrigonometricCalculationHelper.removeRedundancy(line1[0].Stroke.StylusPoints);
                var stylusPoints2 = TrigonometricCalculationHelper.removeRedundancy(line2[0].Stroke.StylusPoints);
                if (stylusPoints2.Count == 0)
                {
                    return new ValidSetOfPointsCollection();
                }

                // Considering that they are two lines, the angle will only be checked if their edges are  nearby
                if (!linesTouch(stylusPoints1, stylusPoints2))
                {
                    return new ValidSetOfPointsCollection();
                }

                // Calculate slope of each line represented by the two sets of touch points
                // as the final of a line is usually fuzzy, get the middle points(1/4,3/4), that have a more fixed pattern
                int avg1 = stylusPoints1.Count;
                int avg2 = stylusPoints2.Count;

                double set1Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints1[avg1 / 4], stylusPoints1[avg1 * 3 / 4]);
                double set2Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints2[avg2 / 4], stylusPoints2[avg2 * 3 / 4]);

                double angularDiff = (set1Angle - set2Angle) * 180 / 3.14;

                if (angularDiff < 0)
                    angularDiff = 180 + angularDiff;

                if (angularDiff > 180)
                    angularDiff = angularDiff - 180;

                double angleValueMin = _data.Min;
                double angleValueMax = _data.Max;

                if (Math.Abs(angularDiff) > angleValueMin && Math.Abs(angularDiff) < angleValueMax)
                {
                    return sets;
                }
                else
                {
                    angularDiff = Math.Abs(angularDiff) - 180;
                    if (Math.Abs(angularDiff) > angleValueMin && Math.Abs(angularDiff) < angleValueMax)
                    {
                        return sets;
                    }
                }
            }
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
            return null;
        }

        private bool linesTouch(StylusPointCollection line1, StylusPointCollection line2)
        {
            const int THRESHOLD = 20;


            foreach (var point1 in line1)
            {
                foreach (var point2 in line2)
                {
                    double XValue = Math.Abs(point1.X - point2.X);
                    double YValue = Math.Abs(point1.Y - point2.Y);

                    if (XValue <= THRESHOLD && YValue <= THRESHOLD)
                    {
                        return true;
                    }
                }
            }
            return false;


        }

        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            List<IPrimitiveConditionData> returnList = new List<IPrimitiveConditionData>();

            // Do all the possible combinations
            for (int j = 0; j < points.Count - 1; j++)
            {
                //If it's a blob, leave it
                if (!points[j].isFinger)
                {
                    returnList.Add(null);
                    continue;
                }

                var stylusPoints1 = TrigonometricCalculationHelper.removeRedundancy(points[j].Stroke.StylusPoints);
                if (stylusPoints1.Count < 4)  // It's a point
                {
                    returnList.Add(null);
                    continue;
                }


                ValidSetOfTouchPoints line1 = ValidateLine(points[j]);
                if (line1.Count > 0)
                {
                    for (int i = j + 1; i < points.Count; i++)
                    {
                        //Check to see if it is a point
                        var stylusPoints2 = TrigonometricCalculationHelper.removeRedundancy(points[i].Stroke.StylusPoints);
                        if (stylusPoints2.Count == 0)
                            continue;

                        // check to see if the two sets of points are lines
                        ValidSetOfTouchPoints line2 = ValidateLine(points[i]);
                        if (line2.Count == 0)
                            continue;

                        // Considering that they are two lines, the angle will only be check if their edges are  nearby
                        if (!linesTouch(stylusPoints1, stylusPoints2))
                        {
                            returnList.Add(null);
                            continue;
                        }

                        // Calculate slope of each line represented by the two sets of touch points
                        // as the final of a line is usually fuzzy, get the middle points(1/4,3/4), that have a more fixed pattern
                        int avg1 = stylusPoints1.Count;
                        int avg2 = stylusPoints2.Count;

                        double set1Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints1[avg1 / 4], stylusPoints1[avg1 * 3 / 4]);
                        double set2Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints2[avg2 / 4], stylusPoints2[avg2 * 3 / 4]);

                        double angularDiff = (set1Angle - set2Angle) * 180 / 3.14;

                        if (angularDiff < 0)
                            angularDiff = 180 + angularDiff;

                        if (angularDiff > 180)
                            angularDiff = angularDiff - 180;

                        AngleBetween result = new AngleBetween();
                        result.Gesture1 = (j + 1) + "";
                        result.Gesture2 = (i + 1) + "";
                        result.Min = angularDiff;
                        returnList.Add(result);
                    }
                }
            }

            return returnList;

        }

        public bool isComplex()
        {
            return true;
        }



        #endregion
    }
}
