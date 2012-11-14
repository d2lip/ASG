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
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;
using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions
{
    public class TouchStateValidator : IPrimitiveConditionValidator
    {
        TouchState _data;

       public  int Order()
        {
            return 0;
        }

        class TouchActionResult
        {
            public TouchAction Action = TouchAction.Up;
            public bool result = false;
        }


        List<TouchActionResult> requiredTouchStates = new List<TouchActionResult>();

        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as TouchState;

            foreach (string touchState in _data.States)
            {
                requiredTouchStates.Add(
                        new TouchActionResult()
                        {
                            Action = GetTouchActionType(touchState)
                        });
            }
        }

        private static TouchAction GetTouchActionType(string value)
        {
            TouchAction touchState = TouchAction.Down;
            switch (value.ToLower())
            {
                case "touchup":
                    touchState = TouchAction.Up;
                    break;
                case "touchdown":
                    touchState = TouchAction.Down;
                    break;
                case "touchmove":
                    touchState = TouchAction.Move;
                    break;
            }
            return touchState;
        }

        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            ValidSetOfPointsCollection list = new ValidSetOfPointsCollection();
            foreach (ValidSetOfTouchPoints set in sets)
            {
                ValidSetOfPointsCollection tmpList = Validate(set);
                list.AddRange(tmpList);
            }

            return list;
        }

        public ValidSetOfPointsCollection Validate(List<TouchPoint2> points)
        {
            ValidSetOfPointsCollection sets = new ValidSetOfPointsCollection();
            if (IsValid(points))
            {
                sets.Add(new ValidSetOfTouchPoints(points));
            }

            return sets;
        }

 
        private bool IsValid(List<TouchPoint2> points)
        {
            // Clear previous results
            foreach (var item in requiredTouchStates)
                item.result = false;

            // Check if current state satisfies the rule
            for (int i = 0; i < points.Count; i++)
            {
                foreach (var stateInfo in requiredTouchStates)
                {
                    if (!stateInfo.result)
                    {
                        if (i < points.Count)
                        {
                            stateInfo.result = (stateInfo.Action == points[i].Action);

                            // as current point is tested against one required state, advance the index 
                            i++;
                        }
                    }
                }
            }

            bool result = true;
            foreach (var stateInfo in requiredTouchStates)
            {
                if (!stateInfo.result)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }


        public bool Equals(IPrimitiveConditionValidator obj)
        {
            return Equals(obj);
        }



        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {
            return null;
       
                        
        }


        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            List<IPrimitiveConditionData> states = new List<IPrimitiveConditionData>();
            
            foreach (TouchPoint2 point in points)
            {
                TouchState state = new TouchState();
                switch (point.Action.ToString().ToLower())
                {
                    case "up":
                        state.States.Add("TouchUp");
                        break;

                    case "move":
                        state.States.Add("TouchMove");
                        break;

                    case "down":
                        state.States.Add("TouchDown");
                        break;
                }
                
                states.Add(state);
            }

           return states;
  
        }

    }
}
