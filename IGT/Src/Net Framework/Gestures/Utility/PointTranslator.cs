using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using TouchToolkit.GestureProcessor.Utility;
using TouchToolkit.GestureProcessor.Utility.TouchHelpers;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.ShapeRecognizers;
using System.Diagnostics;


namespace TouchToolkit.GestureProcessor.Utility
{
    public class PointTranslator
    {

        public enum NoiseReductionType
        {
            High,
            Medium,
            Low
        }


        private static int redundancy = 200;

        private static bool _breakIntoSteps;

        public static NoiseReductionType NoiseReduction { get; set; }

        public static bool BreakIntoSteps
        {
            get { return _breakIntoSteps; }
            set { _breakIntoSteps = value; }
        }

        private static double _noiseReduction;

       
        private static TouchPoint2 getSmallerRect(TouchPoint2 p1, TouchPoint2 p2)
        {
            double a1 = p1.Bounds.Size.Height * p1.Bounds.Size.Width;
            double a2 = p2.Bounds.Size.Height * p2.Bounds.Size.Width;

            if (a1 < a2)
                return p1;
            else
                return p2;        
        }



        private static List<TouchPoint2> removeConcentricBlobs(int pointIndex, List<TouchPoint2> points)
        {
            TouchPoint2 p1 = points[pointIndex];
            if (!p1.isFinger)
            {   
                var concentricBlobs = from p in points 
                                   where                                    
                                   p.Action == p1.Action
                                   && p.Tag == null 
                                   && p != p1
                                   && p.isFinger == false
                                   && (TrigonometricCalculationHelper.GetDistanceBetweenPoints(p1.Position, p.Position) <= redundancy)
                                   select p;
                List<TouchPoint2> temp = concentricBlobs.ToList();
               
                foreach (TouchPoint2 point in temp)
                {                   
                   if (getSmallerRect(p1, point) == p1)
                   {
                       points.Remove(p1);
                       p1 = point;
                   }
                   else
                       points.Remove(point);
                }                
            }
            
            pointIndex++;
            if ((points.Count > 1)&&(pointIndex < points.Count))
                return removeConcentricBlobs(pointIndex, points);
            else 
                return points;
        }


        public static List<TouchPoint2> removeHandRedundancy(List<TouchPoint2> points)
        {   
            var blobs = from p in points
                        where
                        p.isFinger == false &&
                        p.Tag == null
                        select p;

            if (blobs == null)
                return points;

            if (points.Count > 1)
            {
                points =  removeConcentricBlobs(0, points);
                points = removeFingers(points);
                return points;
            }
            else
                return points;
        }

        public static List<TouchPoint2> analyzeTags(ValidSetOfTouchPoints allpoints)
        {
            var tagTouch = from p in allpoints
                           where p.Tag != null
                           select p;            

            List<TouchPoint2> tagTouchList = tagTouch.ToList();
            List<TouchPoint2> pointsList = allpoints.ToList();
            if (tagTouch != null)
            {
                foreach (TouchPoint2 point in pointsList)
                {
                    foreach (TouchPoint2 tagPoint in tagTouchList)
                    {
                        if (point.Tag == tagPoint.Tag)
                            continue;

                        if (pointsSimilar(point, tagPoint))
                            allpoints.Remove(point);                        

                    }
                
                }
            
            }

            return allpoints;

        }

        private static bool pointsSimilar(TouchPoint2 point, TouchPoint2 tagPoint)
        {
            return (point.Bounds.Contains(tagPoint.Position));
                
        }

        private static List<TouchPoint2> removeFingers(List<TouchPoint2> points)
        {
            var blobs = from p in points
                                  where                                  
                                  p.isFinger == false                                 
                                  select p;

            var fingers = from p in points
                                  where                              
                                  p.isFinger == true                                
                                  select p;
             double distance = 0;

             List<TouchPoint2> blobsList = blobs.ToList();
             List<TouchPoint2> fingerList = fingers.ToList();


             foreach (TouchPoint2 finger in fingerList) 
            {
                foreach (TouchPoint2 blob in blobsList)
                {
                    distance = TrigonometricCalculationHelper.GetDistanceBetweenPoints(blob.Position,finger.Position);                    
                    if (distance <= 1.5 * redundancy)
                    points.Remove(finger);
                }
            }
            return points;
        }

        public static List<TouchPoint2> FindLinesOLD(TouchPoint2 points)
        {
            double length = TrigonometricCalculationHelper.CalculatePathLength(points);
            List<TouchPoint2> newPoints = new List<TouchPoint2>();

            if (length < 10)
            {
                newPoints.Add(points);
                return newPoints;
            }

            TouchPoint2 temp;
            StylusPointCollection simplerPoints = new StylusPointCollection();
            simplerPoints = TrigonometricCalculationHelper.removeRedundancy(points.Stroke.StylusPoints);

            TouchInfo info = new TouchInfo();
            info.ActionType = points.Action.ToTouchAction();
            info.TouchDeviceId = points.TouchDeviceId;
            info.Bounds = points.Bounds;
            info.IsFinger = points.isFinger;
            TouchPoint2 output = new TouchPoint2(info, points.Source, simplerPoints);
            string lastDirection = "";
            int idxIni = 0;
            int i;
            int diffCount = 0;
            int increment = 1;
            //default = 0.1
            int minimumLength = 4;
            switch (NoiseReduction)
            {
                case NoiseReductionType.Low: increment = 2;
                    break;
                case NoiseReductionType.Medium: increment = 4;
                    break;
                case NoiseReductionType.High: increment = 8;
                    break;

            }
            string sPreviousDirection = "";
            for (i = increment; i < simplerPoints.Count; i = i + increment)
            {
                StylusPoint p1 = simplerPoints[idxIni];
                StylusPoint p2 = simplerPoints[i];
                double slope = TrigonometricCalculationHelper.GetSlopeBetweenPoints(p1, p2);
                string sDirection = TouchPointExtensions.SlopeToDirection(slope);

                if (sPreviousDirection == "")
                    sPreviousDirection = sDirection;
                else if (sPreviousDirection != sDirection)
                    diffCount++;
                else  // This makes that it has to be different diffCount minimumLength times in a row                
                    diffCount = 0;

                if ((diffCount >= minimumLength))
                {
                    p1 = simplerPoints[idxIni];
                    p2 = simplerPoints[i - diffCount];
                    slope = TrigonometricCalculationHelper.GetSlopeBetweenPoints(p1, p2);
                    string sCurrentDirection = TouchPointExtensions.SlopeToDirection(slope);

                    temp = output.GetRange(idxIni, i - diffCount);
                    if (lastDirection == sCurrentDirection)
                    {
                        newPoints[newPoints.Count - 1].Stroke.StylusPoints.Add(temp.Stroke.StylusPoints);
                    }
                    else
                    {
                        if (lastDirection == "")
                            lastDirection = sCurrentDirection;
                        temp.Action = TouchAction.Move;
                        newPoints.Add(temp);
                    }
                    diffCount = 0;
                    lastDirection = sCurrentDirection;
                    sPreviousDirection = sDirection;
                    idxIni = i;

                }
            }
            // If nothing was added then, there is only one step which is the whole gesture
            if (newPoints.Count == 0)
                newPoints.Add(output);
            // If there are several points after the last recognized step add the last remaining points as a new step
            else if (i - idxIni > minimumLength)
            {
                temp = output.GetRange(idxIni, i - increment);
                if (lastDirection == sPreviousDirection)
                {
                    newPoints[newPoints.Count - 1].Stroke.StylusPoints.Add(temp.Stroke.StylusPoints);
                }
                else
                {
                    temp.Action = TouchAction.Move;
                    newPoints.Add(temp);
                }
            }
            newPoints[newPoints.Count - 1].Action = output.Action;
            return newPoints;

        }


        public static List<TouchPoint2> FindLines(TouchPoint2 points)
        {
            double length = TrigonometricCalculationHelper.CalculatePathLength(points);
            List<TouchPoint2> newPoints = new List<TouchPoint2>();

            if (length < 10)
            {
                newPoints.Add(points);
                return newPoints;
            }

            TouchPoint2 temp;
            StylusPointCollection simplerPoints = new StylusPointCollection();
            simplerPoints = TrigonometricCalculationHelper.removeRedundancy(points.Stroke.StylusPoints);
                                      
            TouchInfo info = new TouchInfo();
            info.ActionType = points.Action.ToTouchAction();            
            info.TouchDeviceId = points.TouchDeviceId;
            info.Bounds = points.Bounds;
            info.IsFinger = points.isFinger;
            TouchPoint2 output = new TouchPoint2(info, points.Source, simplerPoints);
            string lastDirection = "";
            int idxIni = 0;
            int i;
                      
            int diffCount = 0;
            int increment=1;
                                                                        //default = 0.1
            int minimumLength = (int)(simplerPoints.Count * .05);
            switch (NoiseReduction)
            {
                case NoiseReductionType.Low: increment = 3;
                    break;
                case NoiseReductionType.Medium: increment = 5;
                    break;
                case NoiseReductionType.High: increment = 10;
                    break;
            
            }
            string sPreviousDirection = "";
            for (i = increment; i < simplerPoints.Count; i = i + increment)
            {
                StylusPoint p1 = simplerPoints[i - 1];
                StylusPoint p2 = simplerPoints[i];            
                double slope = TrigonometricCalculationHelper.GetSlopeBetweenPoints(p1, p2);
                string sDirection = TouchPointExtensions.SlopeToDirection(slope);

                if (sPreviousDirection == "")
                    sPreviousDirection = sDirection;          
                
                if (sPreviousDirection != sDirection)
                     diffCount++;
                else  // This makes that it has to be different diffCount times in a row                
                    diffCount = 0;

                if ((diffCount >= minimumLength))
                {                     
                     p1 = simplerPoints[idxIni];
                     p2 = simplerPoints[i]; 
                     slope = TrigonometricCalculationHelper.GetSlopeBetweenPoints(p1, p2);
                     string sCurrentDirection = TouchPointExtensions.SlopeToDirection(slope);                   

                    temp = output.GetRange(idxIni, i);
                    if (lastDirection == sCurrentDirection)
                    {                      
                        newPoints[newPoints.Count - 1].Stroke.StylusPoints.Add(temp.Stroke.StylusPoints);
                    }
                    else {
                      if (lastDirection == "")
                          lastDirection = sCurrentDirection;
                      temp.Action = TouchAction.Move;
                      newPoints.Add(temp);                                          
                    }
                    diffCount = 0;
                    lastDirection = sCurrentDirection;
                    sPreviousDirection = sDirection;                    
                    idxIni = i;
                    
                }
            }
            // If nothing was added then, there is only one step which is the whole gesture
            if (newPoints.Count == 0)
                newPoints.Add(output);            
            // If there are several points after the last recognized step add the last remaining points as a new step
            else if (i - idxIni > minimumLength)
            {                
                temp = output.GetRange(idxIni, i - increment);
                if (lastDirection == sPreviousDirection)
                {
                    newPoints[newPoints.Count - 1].Stroke.StylusPoints.Add(temp.Stroke.StylusPoints);
                }
                else
                {
                    temp.Action = TouchAction.Move;
                    newPoints.Add(temp);
                }                
            }
            newPoints[newPoints.Count - 1].Action = output.Action;
            return newPoints;
        }
             


       




        private static List<int> getIDsFromGDL(string gdl)
        {
            //   name stepx,stepy,...stepz:
            // gets whatever is between "step" and "," or ":"

            int idxIni = 0;
            int idxEnd = 0;
            List<int> ids = new List<int>();
            idxIni = gdl.IndexOf("step", idxIni);
            while (idxIni >= 0)
            {
                idxIni = idxIni + 4;
                idxEnd = gdl.IndexOf(",", idxIni);
                if (idxEnd < 0)
                    idxEnd = gdl.IndexOf(":", idxIni); ;
                int id = Convert.ToInt16(gdl.Substring(idxIni, idxEnd - idxIni));
                ids.Add(id);
                idxIni = gdl.IndexOf("step", idxIni);
            }


            return ids;
        }



    }

}
