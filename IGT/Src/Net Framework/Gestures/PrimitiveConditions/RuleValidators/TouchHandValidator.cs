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
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions
{
    public class TouchHandValidator : IPrimitiveConditionValidator
    {
       public  int Order()
        {
            return 1;
        }

        private TouchHand _data;

        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as TouchHand;
        }

        public ValidSetOfPointsCollection Validate(List<TouchPoint2> points)
        {
            ValidSetOfPointsCollection list = new ValidSetOfPointsCollection();
            foreach (TouchPoint2 point in points)
            {

                if (!point.isFinger && point.Snapshot!= null && point.Tag == null)
                {

                    string _type = string.Empty;
                    string _side = string.Empty;
                    ImageHelper.getHandType(out _type, out _side,  point);

                    if (_type == "" && _side == "")
                        continue;



                    if (_data.Type == string.Empty && _data.Side == string.Empty)
                    {
                        list.Add(new ValidSetOfTouchPoints(points));
                        continue;
                    }




                    if (_data.Side != string.Empty)
                    {
                        if (_type == _data.Type && _data.Side == _side)
                            list.Add(new ValidSetOfTouchPoints(points));
                    }
                    else // checks only the type
                    {
                        if (_type == _data.Type)
                            list.Add(new ValidSetOfTouchPoints(points));

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
            return false;
        }




        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {

            return null;


        }
        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {            
            List<IPrimitiveConditionData> rules = new List<IPrimitiveConditionData>();
            foreach (TouchPoint2 point in points)
            {
                if (point.Tag != null)
                {
                    rules.Add(null);
                    continue;
                }
                if (!point.isFinger && point.Snapshot!= null )
                {
                    TouchHand hand = new TouchHand();
                    string _type = "";
                    string _side = "";
                    hand.Kind = TouchHand.HAND;
                    
                    ImageHelper.getHandType(out _type, out _side,  point);
                    if (_type != "")
                    {
                        hand.Type = _type;
                        hand.Side = _side;
                        rules.Add(hand);
                    }
                }
            }

            if (rules.Count > 0)
                return rules;
            else
                return null;
        }

      
    }
}
