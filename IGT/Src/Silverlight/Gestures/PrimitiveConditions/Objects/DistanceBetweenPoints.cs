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
    public class DistanceBetweenPoints : IPrimitiveConditionData
    {
        public class BehaviourTypes
        {
            public const string Increasing = "increasing";
            public const string Decreasing = "decreasing";
            public const string Range = "range";
            public const string UnChanged = "unchanged";
        }

        private string _behaviour = string.Empty;
        public string Behaviour
        {
            get
            {
                return _behaviour;
            }
            set
            {
                _behaviour = value;
            }
        }

        int _minValue = 0;
        public int Min
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
            }
        }

        int _maxValue = 0;
        public int Max
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
            }
        }

        public bool Equals(IPrimitiveConditionData rule)
        {
            DistanceBetweenPoints value = rule as DistanceBetweenPoints;

            if (value == null)
            {
                // Failed to cast to same time
                return false;
            }
            else
            {
                if (value.Behaviour == this.Behaviour &&
                    value.Max == this.Max &&
                    value.Min == this.Min)
                    return true;
                else
                    return false;
            }
        }

        public void Union(IPrimitiveConditionData value)
        {
            DistanceBetweenPoints distanceBtwPoints = value as DistanceBetweenPoints;

            if (this.Behaviour != distanceBtwPoints.Behaviour)
            {

                if (this.Behaviour == BehaviourTypes.Increasing && distanceBtwPoints.Behaviour == BehaviourTypes.Decreasing)
                {
                    throw new Exception("Invalid Union Type");
                }
                else if (this.Behaviour == BehaviourTypes.Decreasing && distanceBtwPoints.Behaviour == BehaviourTypes.Increasing)
                {
                    throw new Exception("Invalid Union Type");
                }
                else if (this.Behaviour == BehaviourTypes.UnChanged && distanceBtwPoints.Behaviour == BehaviourTypes.Increasing)
                {
                    this.Max = distanceBtwPoints.Max;
                    this.Behaviour = BehaviourTypes.Increasing;
                }
                else if (this.Behaviour == BehaviourTypes.UnChanged && distanceBtwPoints.Behaviour == BehaviourTypes.Decreasing)
                {
                    this.Min = distanceBtwPoints.Min;
                    this.Behaviour = BehaviourTypes.Decreasing;
                }
            }
            else if (this.Behaviour == BehaviourTypes.Increasing && distanceBtwPoints.Behaviour == BehaviourTypes.Increasing)
            {
                this.Max = distanceBtwPoints.Max;
                this.Behaviour = BehaviourTypes.Increasing;
            }
            else if (this.Behaviour == BehaviourTypes.Decreasing && distanceBtwPoints.Behaviour == BehaviourTypes.Decreasing)
            {
                this.Min = distanceBtwPoints.Min;
                this.Behaviour = BehaviourTypes.Decreasing;
            }
        }


        public string ToGDL()
        {
            if (this.Behaviour == BehaviourTypes.UnChanged)
            {
                if (this.Min == 0)
                    return string.Format("Distance between points: {0}", BehaviourTypes.UnChanged);
                else
                    return string.Format("Distance between points: {0} {1}%", BehaviourTypes.UnChanged, this.Min);
            }
            else if (this.Behaviour == BehaviourTypes.Range)
                return string.Format("Distance between points: {0}..{1}", this.Min, this.Max);
            else
                return string.Format("Distance between points: {0}", this.Behaviour);
        }


        public bool isComplex()
        {
            return true;
        }
    }
}
