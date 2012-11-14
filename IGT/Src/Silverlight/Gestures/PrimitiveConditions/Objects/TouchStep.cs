using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Objects
{
    public class TouchStep : IPrimitiveConditionData
    {
        private int _touchCount = 0;
        public int TouchCount
        {
            get
            {
                return _touchCount;
            }
            set
            {
                _touchCount = value;
            }
        }

        private double _timeLimit = 0;
        public double TimeLimit
        {
            get
            {
                return _timeLimit;
            }
            set
            {
                _timeLimit = value;
            }
        }

        private string _unit = string.Empty;
        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
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
            TouchStep touchStep = value as TouchStep;

            // TouchCount
            if (this.TouchCount < touchStep.TouchCount)
                this.TouchCount = touchStep.TouchCount;

            // TimeLimit
            if (this.Unit == touchStep.Unit)
            {
                if (this.TimeLimit < touchStep.TimeLimit)
                    this.TimeLimit = touchStep.TimeLimit;
            }
            else if (this.Unit == string.Empty && touchStep.Unit != string.Empty)
            {
                this.Unit = touchStep.Unit;
                this.TouchCount = touchStep.TouchCount;
                this.TimeLimit = touchStep.TimeLimit;
            }
            else
            {
                if (this.Unit == "sec" && touchStep.Unit == "msec")
                {
                    if ((this.TimeLimit * 1000) < touchStep.TimeLimit)
                    {
                        this.TimeLimit = touchStep.TimeLimit/1000;
                    }

                }
                else if (this.Unit == "msec" && touchStep.Unit == "sec")
                {
                    if ((this.TimeLimit) < (touchStep.TimeLimit*1000))
                    {
                        this.TimeLimit = touchStep.TimeLimit * 1000;
                    }
                }
            }
        }

        public string ToGDL()
        {
            return string.Format("Touch step: {0} touches within {1} {2}", this.TouchCount, this.TimeLimit, this.Unit);
        }


        public bool isComplex()
        {
            return false;
        }
    }
}
