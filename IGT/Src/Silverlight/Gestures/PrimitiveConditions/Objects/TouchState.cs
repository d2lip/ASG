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

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Objects
{
    public class TouchState : IPrimitiveConditionData
    {
       

        private List<string> _touchState = new List<string>();
        public List<string> States
        {
            get
            {
                return _touchState;
            }
            set
            {
                _touchState = value;
            }
        }

        #region IRuleData Members

        public bool Equals(IPrimitiveConditionData rule)
        {
            throw new NotImplementedException();
        }

        #endregion


        public void Union(IPrimitiveConditionData value)
        {
            TouchState touchState = value as TouchState;

            bool matchFound = false;
            foreach (string newState in touchState.States)
            {
                foreach (string existingState in this.States)
                {
                    matchFound = string.Equals(newState, existingState);
                    if (matchFound)
                        break;
                }

                if (!matchFound)
                    this.States.Add(newState);
            }
        }

        public string ToGDL()
        {
            if (States.Count == 0)
                return string.Format("Touch state");


            string uniqueStates = string.Join(" ", States);
            return string.Format("Touch state: {0}", States[0]);
            
        }


        public bool isComplex()
        {
            return false;
        }
    }
}
