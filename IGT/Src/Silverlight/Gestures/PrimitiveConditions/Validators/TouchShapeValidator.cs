using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.Utility;
using TouchToolkit.GestureProcessor.Utility.TouchHelpers;
using TouchToolkit.Framework.ShapeRecognizers;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Validators
{
    public class TouchShapeValidator : IPrimitiveConditionValidator
    {

        public const string BOX = "Box";
        public const string LINE = "Line";
        //public const string VERTICAL_LINE = "Vertical Line";
        public const string CIRCLE = "Circle";
        public const string CHECK = "Check";

       public  int Order()
        {
            return 1;
        }



        private const double TOLERANCE = .7;


        private TouchShape _data;



        public void Init(Objects.IPrimitiveConditionData ruleData)
        {
            _data = ruleData as TouchShape;
        }

        public bool Equals(IPrimitiveConditionValidator rule)
        {
            throw new NotImplementedException();
        }

        public ValidSetOfPointsCollection Validate(List<TouchPoint2> points)
        {
            ValidSetOfPointsCollection ret = new ValidSetOfPointsCollection();
            foreach (var point in points)
            {
                if (!point.isFinger)
                {
                    continue;
                }

                bool singlePoint = TrigonometricCalculationHelper.isASinglePoint(point);
                if (singlePoint)
                {
                    continue;
                }  // A point can't be a closed loop     
                
                ValidSetOfTouchPoints tps = null;
                if (_data.Values.Equals(CIRCLE))
                {
                    tps = ValidateCircle(point);
                }
                else if (_data.Values.Equals(BOX))
                {
                    tps = ValidateBox(point);
                }
                else if (_data.Values.Equals(CHECK))
                {
                    tps = ValidateCheck(point);
                }
                else if (_data.Values.Equals(LINE))
                {
                    tps = ValidateLine(point);
                }

                if ((tps != null) && (tps.Count > 0))
                {
                    ret.Add(tps);
                }
            }
            return ret;
        }

        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            ValidSetOfPointsCollection ret = new ValidSetOfPointsCollection();
            foreach (var set in sets)
            {
                ValidSetOfPointsCollection list = Validate(set);
                foreach (var item in list)
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

        private ValidSetOfTouchPoints ValidateBox(TouchPoint2 points)
        {
            ValidSetOfTouchPoints output = new ValidSetOfTouchPoints();
            int length = points.Stroke.StylusPoints.Count;
            if (length < 1)
            {
                return output;
            }

            List<string> slopes = new List<string>();
            TouchPoint2 newPoints = points.GetEmptyCopy();

            for (int i = 0; i < length - 1; i++)
            {
                var point1 = points.Stroke.StylusPoints[i];
                var point2 = points.Stroke.StylusPoints[i + 1];
                double slope = TrigonometricCalculationHelper.GetSlopeBetweenPoints(point1, point2);
                double distance = TrigonometricCalculationHelper.GetDistanceBetweenPoints(point1, point2);
                string stringSlope = TouchPointExtensions.SlopeToDirection(slope);
                if (distance > 0)
                {
                    newPoints.Stroke.StylusPoints.Add(point1);
                    Correlation recognizer = new Correlation(newPoints);
                    if (Math.Abs(recognizer.RSquared) < TOLERANCE + 0.15)
                    {
                        int linelength = newPoints.Stroke.StylusPoints.Count;
                        double lineSlope = TrigonometricCalculationHelper.GetSlopeBetweenPoints(newPoints.Stroke.StylusPoints[1],
                            newPoints.Stroke.StylusPoints[linelength - 1]);
                        string lineStringSlope = TouchPointExtensions.SlopeToDirection(lineSlope);
                        slopes.Add(lineStringSlope);
                        newPoints = newPoints.GetEmptyCopy();
                    }
                }
            }
            RectangleParser parser = new RectangleParser();
            bool hasRect = parser.Advance(slopes);
            if (hasRect)
            {
                output.Add(points);
            }
            return output;
        }

        private ValidSetOfTouchPoints ValidateLine(TouchPoint2 points)
        {
            ValidSetOfTouchPoints ret = new ValidSetOfTouchPoints();
            Correlation recognizer = new Correlation(points);
            if (Math.Abs(recognizer.RSquared) > 0.2)
            {
                ret.Add(points);
            }
            return ret;
        }

        //TODO
        /*    private ValidSetOfTouchPoints ValidateHorizontalLine(TouchPoint2 points)
            {
                ValidSetOfTouchPoints ret = new ValidSetOfTouchPoints();
                Correlation recognizer = new Correlation(points);
                if (Math.Abs(recognizer.RSquared) > TOLERANCE - 0.1)
                {
                    ret.Add(points);
                }
                return ret;
            }*/


        private ValidSetOfTouchPoints ValidateCircle(TouchPoint2 points)
        {
            ValidSetOfTouchPoints ret = new ValidSetOfTouchPoints();
            CircleRecognizer recognizer = new CircleRecognizer(points);
            if (recognizer.R > TOLERANCE + 0.265)
            {
                ret.Add(points);
            }

            return ret;
        }

        public Objects.IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {


            return null;



        }
        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            List<IPrimitiveConditionData> shapes = new List<IPrimitiveConditionData>();
            TouchShape shape = new TouchShape();
            ValidSetOfTouchPoints valids;
            bool singlePoint = false;
            foreach (TouchPoint2 point in points)
            {

                if (!point.isFinger)
                {
                    shapes.Add(null);
                    continue;
                }

                singlePoint = TrigonometricCalculationHelper.isASinglePoint(point);
                if (singlePoint)
                {
                    shapes.Add(null);
                    continue;
                }  // A point can't be a closed loop                    

                valids = ValidateCircle(point);
                if ((valids != null) && (valids.Count > 0))
                {
                    shape.Values = CIRCLE;
                    shapes.Add(shape);
                    continue;
                }
                valids = ValidateBox(point);
                if ((valids != null) && (valids.Count > 0))
                {
                    shape.Values = BOX;
                    shapes.Add(shape);
                    continue;
                }
                valids = ValidateCheck(point);
                if ((valids != null) && (valids.Count > 0))
                {
                    shape.Values = CHECK;
                    shapes.Add(shape);
                    continue;
                }
                valids = ValidateLine(point);
                if ((valids != null) && (valids.Count > 0))
                {
                    shape.Values = LINE;
                    shapes.Add(shape);
                    continue;
                }




            }

            return shapes;


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


        private ValidSetOfTouchPoints ValidateCheck(TouchPoint2 point)
        {
            ValidSetOfTouchPoints ret = new ValidSetOfTouchPoints();

            PointTranslator.NoiseReduction = PointTranslator.NoiseReductionType.Low;
            List<TouchPoint2> lines = PointTranslator.FindLines(point);

            if (lines.Count == 2)
            {
                // Calculate angle
                var stylusPoints1 = TrigonometricCalculationHelper.removeRedundancy(lines[0].Stroke.StylusPoints);
                var stylusPoints2 = TrigonometricCalculationHelper.removeRedundancy(lines[1].Stroke.StylusPoints);

                double length1 = TrigonometricCalculationHelper.CalculatePathLength(lines[0]);
                double length2 = TrigonometricCalculationHelper.CalculatePathLength(lines[1]);

                if (length1 > length2)
                    return ret;
                int avg1 = stylusPoints1.Count;
                int avg2 = stylusPoints2.Count;

                double set1Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints1[avg1 / 4], stylusPoints1[avg1 * 3 / 4]);
                double set2Angle = TrigonometricCalculationHelper.GetSlopeBetweenPoints(stylusPoints2[avg2 / 4], stylusPoints2[avg2 * 3 / 4]);

                double angularDiff = (set1Angle - set2Angle) * 180 / 3.14;

                if (angularDiff < 0)
                    angularDiff = 180 + angularDiff;

                if (angularDiff > 180)
                    angularDiff = angularDiff - 180;

                if (angularDiff < 100 && angularDiff > 75)
                    ret.Add(point);



            }

            return ret;


        }
    }
}
