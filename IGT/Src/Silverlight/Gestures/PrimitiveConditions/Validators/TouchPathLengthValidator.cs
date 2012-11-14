using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.Utility;
using TouchToolkit.GestureProcessor.Objects;
using System.Text.RegularExpressions;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Validators
{
    public class TouchPathLengthValidator : IPrimitiveConditionValidator
    {
        private const double TOLERANCE = 1;
       public  int Order()
        {
            return 1;
        }
        private TouchPathLength _data;        

        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as TouchPathLength;
        }

        public bool Equals(IPrimitiveConditionValidator rule)
        {
            throw new NotImplementedException();
        }


        private bool checkForVariable(TouchPoint2 point, string gestureName) {

            string id = PartiallyEvaluatedGestures.getIDFromVariable(_data.VariableMin);
            double length = TrigonometricCalculationHelper.CalculatePathLength(point);
           
            if (id == "")
                return false;

            if (id == _data.VariableMin)
            {
                // It's the declaration of the variable, just read the lenght and store it there                               
                PartiallyEvaluatedGestures.addInBuffer(id, length);
                return true;
            }
            else {

              //  List<ValidateBlockResult> firstBlockResults = PartiallyEvaluatedGestures.Get(gestureName, "step1");
            //    ValidSetOfPointsCollection step1 = firstBlockResults[0].Data;
            //    double idValue = TrigonometricCalculationHelper.CalculatePathLength(step1[0][0]);
                double idValue = PartiallyEvaluatedGestures.getValueFromID(id);


              //  double idValue = PartiallyEvaluatedGestures.getValueFromID(id);
                double min = PartiallyEvaluatedGestures.getMultiplierFromVariable(_data.VariableMin);
                double max = PartiallyEvaluatedGestures.getMultiplierFromVariable(_data.VariableMax);
                min = idValue * min;
                max = idValue * max;

                if (max == 0)
                    max = Double.MaxValue;

                if (idValue > 0 && length >= min && length <= max)
                {                    
                    return true;
                }
                else
                    return false;                
            }
    
        }

        public ValidSetOfPointsCollection Validate(List<TouchPoint2> p)
        {

            return null;
        }
        public ValidSetOfPointsCollection Validate(ValidSetOfTouchPoints points, string gestureName)
        {
            ValidSetOfPointsCollection sets = new ValidSetOfPointsCollection();
            ValidSetOfTouchPoints set = new ValidSetOfTouchPoints();
            double length = 0;
            
                // the length can be calculated for a single touch path. So, we check each
                // touchPoint individually

                foreach (var point in points)
                {
                    bool singlePoint = TrigonometricCalculationHelper.isASinglePoint(point);
                    if (singlePoint)
                    {
                        continue;
                    }  // A point can't be a closed loop     



                    if (checkForVariable(point, gestureName))
                    {
                        set.Add(point);
                    }
                    else
                    {
                        length = TrigonometricCalculationHelper.CalculatePathLength(point);
                        if (length >= _data.Min && length <= _data.Max)
                        {
                            set.Add(point);
                        }
                    }


                }

            



            if (set.Count > 0)
            {
                sets.Add(set);
            }




            return sets;
        }

      

        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            
            ValidSetOfPointsCollection validSets = new ValidSetOfPointsCollection();
            //_data.currentMin = CalculatePathLength(validSets.);
            foreach (var item in sets)
            {
                ValidSetOfPointsCollection list = Validate(item,sets.ExpectedGestureName);
                foreach (var set in list)
                {
                    validSets.Add(set);
                }
            }

            return validSets;
        }


        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {
            //Validate(points);
            bool singlePoint;
            double length = 0;
            foreach (var point in points)
            {
                singlePoint = TrigonometricCalculationHelper.isASinglePoint(point);
                if (singlePoint)
                    return null;
                length = TrigonometricCalculationHelper.CalculatePathLength(point);
            }

            TouchPathLength _return = new TouchPathLength();
            _return.Max = length;
            _return.Min = 0;
            return _return;


        }
        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            List<IPrimitiveConditionData> data = new List<IPrimitiveConditionData>();
            //Validate(points);
            bool singlePoint;
            double length = 0;
            foreach (var point in points)
            {
                if (!point.isFinger)
                {
                    data.Add(null);
                    continue;
                }

                singlePoint = TrigonometricCalculationHelper.isASinglePoint(point);
                if (singlePoint)
                    continue;
                length = TrigonometricCalculationHelper.CalculatePathLength(point);
                TouchPathLength _return = new TouchPathLength();
                _return.Max = length;
                _return.Min = 0;
                data.Add(_return);
            }

          
            return data;
        }
    }
}
