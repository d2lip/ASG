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
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.GestureProcessor.Utility;
using TouchToolkit.GestureProcessor.Utility.TouchHelpers;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Validators
{
    public class TouchDirectionValidator : IPrimitiveConditionValidator
    {
        private TouchDirection _data;
       public  int Order()
        {
            return 1;
        }
       

        #region IRuleValidator Members

        public void Init(IPrimitiveConditionData ruleData)
        {
            _data = ruleData as TouchDirection;
        }

        public bool Equals(IPrimitiveConditionValidator rule)
        {
            throw new NotImplementedException();
        }

        public ValidSetOfPointsCollection Validate(List<TouchPoint2> points)
        {
            ValidSetOfPointsCollection sets = new ValidSetOfPointsCollection();
            ValidSetOfTouchPoints list = new ValidSetOfTouchPoints();
            List<Queue<int>> PointList = new List<Queue<int>>();
            Queue<int> AddQueue;

            foreach (var point in points)
            {
                
                int length = point.Stroke.StylusPoints.Count;
                bool continuous = true;
                int step = 1;
                AddQueue = new Queue<int>();
                for (int i = 0; i < length - step; i = i + step)
                {
                    var p1 = point.Stroke.StylusPoints[i];
                    var p2 = point.Stroke.StylusPoints[i + step];
                    double slope = TrigonometricCalculationHelper.GetSlopeBetweenPoints(p1, p2);
                    String stringSlope = TouchPointExtensions.SlopeToDirection(slope);
                    double dist = TrigonometricCalculationHelper.GetDistanceBetweenPoints(p1, p2);
                    if (dist == 0)
                    {

                        continue;
                    }
                    if (stringSlope.Equals(_data.Values))
                    {
                        if (!continuous)
                        {
                            continuous = true;
                            PointList.Add(AddQueue);
                            AddQueue = new Queue<int>();
                        }
                        AddQueue.Enqueue(i);
                    }
                    else
                    {
                        continuous = false;
                    }
                }

                if (AddQueue.Count > 0)
                {
                    PointList.Add(AddQueue);
                }

                //Add seperate points for each queue made
                foreach (var queue in PointList)
                {
                    TouchPoint2 p = point.GetEmptyCopy();
                    while (queue.Count > 0)
                    {
                        int i = queue.Dequeue();
                        StylusPoint newPoint = point.Stroke.StylusPoints[i];
                        p.Stroke.StylusPoints.Add(newPoint);
                    }
                    if (p.Stroke.StylusPoints.Count > 1)
                    {
                        list.Add(p);
                    }
                }
            }
            if (list.Count > 0)
            {
                sets.Add(new ValidSetOfTouchPoints(points));
               // sets.Add(list);
            }
            return sets;
        }

        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            ValidSetOfPointsCollection ret = new ValidSetOfPointsCollection();
            foreach (var item in sets)
            {
                ValidSetOfPointsCollection list = Validate(item);
                foreach (var set in list)
                {
                    ret.Add(set);
                }
            }
            return ret;
        }

        #endregion

        public IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points)
        {
            return null;
            
         }
        public List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points)
        {
            // Paths generally contain a lot of points, we are skipping some points 
           // to improve performance. The 'step' variable decides how much we should skip
            

            List<IPrimitiveConditionData> primitives = new List<IPrimitiveConditionData>();
            bool singlePoint = false;
            foreach (TouchPoint2 point in points)
            {
                if (!point.isFinger)
                {
                    primitives.Add(null);
                    continue;
                }

                singlePoint = TrigonometricCalculationHelper.isASinglePoint(point);
                if (singlePoint)
                {
                    primitives.Add(null);
                    continue;
                }

                StylusPoint p1 = point.Stroke.StylusPoints[0];
                StylusPoint p2 = point.Stroke.StylusPoints[point.Stroke.StylusPoints.Count - 1];

                double slope = TrigonometricCalculationHelper.GetSlopeBetweenPoints(p1, p2);
                string sDirection = TouchPointExtensions.SlopeToDirection(slope);
                TouchDirection direction = new TouchDirection();
                direction.Values = sDirection;
                primitives.Add(direction);
            }

            return primitives;
           


        }

        }
}
