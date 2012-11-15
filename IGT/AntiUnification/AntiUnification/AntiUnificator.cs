using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.GestureProcessor.PrimitiveConditions.RuleValidators;
using TouchToolkit.Framework.ShapeRecognizers;
using TouchToolkit.Framework;
using System.Windows.Input;
using System.Windows.Media;
using TouchToolkit.GestureProcessor.Utility;
using TouchToolkit.GestureProcessor.Utility.TouchHelpers;
using System.Windows.Ink;


namespace AntiUnification
{
    public class AntiUnificator
    {

        // p11 = (i11,n11,v11)   --Gestures primitives and its values - Ex: gesture1,"Touch path length",1.234

        // S1 = (p11,p12,p13)
        // S2 = (p21,p22,p23)

        // In the same G, compare different v's that have the same n and different i
        // create a v' that express the relation between the two v's
        // creates a new expression that contains the relation within v' was created        


        /*
         
         
         */

        //  samples will hold a list and each element of this list 
        // if the sample gesture, therefore each element will hold a list itself with the gestures captured for that sample



        private static List<SetOfPrimitives> samples;
        public static string GestureName { get; set; }
        private static SetOfPrimitives solution;
        private const int NO_ID = 0;
        private const string STEP = "step";


        public static List<SetOfPrimitives> getAllSamples()
        {

            return samples;
        }

        public static SetOfPrimitives getSample(int i)
        {

            return samples[i];
        }


        public static void Add(SetOfPrimitives data)
        {
            if (samples == null)
            {
                samples = new List<SetOfPrimitives>();

            }
            samples.Add(data);
        }

        private static double matchingAccuracy;
        public static void setMatchingAccuracy(double percent)
        {
            if (percent > 1)
                percent = percent / 100;

            matchingAccuracy = 1 - percent;
        }

        public static void checkRules()
        {

            if ((samples == null) || (samples.Count < 1))
            {
                return;
            }
            else if (samples.Count == 1)
            {
                if (solution != null)
                    solution.Clear();
                solution = samples[0];
                return;
            }


            // Always create a new solution based on the gestures
            solution = new SetOfPrimitives();

            // All the samples will be compared to the first as 
            // the anti-unification will look for all the elements that
            // are common through all the samples
            SetOfPrimitives firstSample = samples[0];

            foreach (PrimitiveData primitive in firstSample)
            {

                checkProportionforPrimitive(primitive);

                PrimitiveData antiUnifiedString = checkStringListPrimitives(primitive);
                if (antiUnifiedString != null)
                    solution.Add(antiUnifiedString);


               PrimitiveData antiUnifiedComplex =  checkValueComplexPrimitives(primitive);
               if (antiUnifiedComplex != null)
                   solution.Add(antiUnifiedComplex);             
                
              

            }



        }

        private static PrimitiveData checkValueComplexPrimitives(PrimitiveData reference)
        {
            //calculate the difference between the alphas            
            // if the difference is than TOLERANCE -- that means that all the alpha have similar values
            // if alpha is about 1 then use a fixed value (that means that all the examples have a similar value)
            // if create a mutiplicity rule using the average of SUM(alphas)                        


            if (reference.ID.Count() > 1)
                return null;

            IEnumerable<PrimitiveData> similarValues = from set in samples
                                                       from primitive in set
                                                       where primitive.Name == reference.Name
                                                       && reference.Value != 0
                                                       && reference.IDcompare(primitive.ID)
                                                       && !(from s in solution
                                                            select s.Name)
                                                            .Contains(reference.Name)
                                                       select primitive;

            PrimitiveData newPrimitive = null;

            if ((similarValues != null) && (similarValues.Count() == samples.Count))
            {
                List<double> values = new List<double>();
                foreach (PrimitiveData p in similarValues)
                {
                    values.Add(p.Value);
                }
                double value = Math.Round(values.Average(), 2);
                double maxDifference = values.Max() - values.Min();
                double tolerance = Math.Round(value * matchingAccuracy, 2);

              

                if (maxDifference > tolerance)
                    return newPrimitive;
                else if (maxDifference == 0)
                    {
                        newPrimitive = new PrimitiveData();
                        newPrimitive.ID = reference.ID;
                        newPrimitive.Name = reference.Name;
                        newPrimitive.Value = value;
                    
                    }
                else if (maxDifference <= tolerance)
                {
                    newPrimitive = new PrimitiveData();
                    newPrimitive.ID = reference.ID;
                    newPrimitive.Name = reference.Name;
                    newPrimitive.Value = 0;
                    newPrimitive.ListValue.Add((value - tolerance) + ".." + (value + tolerance));
                }
      

            }
            return newPrimitive;








        }



        private static void checkStringComplexPrimitives(PrimitiveData data)
        {
            //calculate the difference between the alphas            
            // if the difference is than TOLERANCE -- that means that all the alpha have similar values
            // if alpha is about 1 then use a fixed value (that means that all the examples have a similar value)
            // if create a mutiplicity rule using the average of SUM(alphas)           

            if (solution.Contains(data))
                return;

            IEnumerable<PrimitiveData> similarStrings = from set in samples
                                                        from primitive in set
                                                        where primitive.Name == data.Name
                                                        && data.Value == 0
                                                        && data.IDcompare(primitive.ID)
                                                        && data.StrListcompare(primitive.ListValue)

                                                        select primitive;



            if ((similarStrings != null) && (similarStrings.Count() == samples.Count))
            {
                PrimitiveData newPrimitive = new PrimitiveData();
                newPrimitive.ID = data.ID;
                newPrimitive.Name = data.Name;
                newPrimitive.ListValue = data.ListValue;
                solution.Add(newPrimitive);
            }

        }




        private static void checkConstantValuePrimitives(PrimitiveData data)
        {
            /*
             * On the same sample, compare to see the relation between the basic primitives, if there is any proportion,  create a new complex primitive that will
             * reference the basic similar ones
             */

            SetOfPrimitives gesture;
            List<double> values = new List<double>();

            values.Add(data.Value);
            for (int i = 0; i < samples.Count; i++)
            {
                gesture = samples.ElementAt(i);
                foreach (PrimitiveData compare in gesture)
                {

                    if (!(data.Equals(compare)) && (data.Name == compare.Name) && (data.IDcompare(compare.ID)))
                    {
                        values.Add(compare.Value);
                    }
                }
            }


            double value = Math.Round(values.Average(), 2);
            double maxDifference = values.Max() - values.Min();
            double tolerance = Math.Round(value * matchingAccuracy, 2);

            if (maxDifference > tolerance)
                return;

            PrimitiveData newPrimitive = new PrimitiveData();

            if (maxDifference == 0)
            {
                newPrimitive.ID = data.ID;
                newPrimitive.Name = data.Name;
                newPrimitive.Value = data.Value;
            }
            else
            {
                newPrimitive.ID = data.ID;
                newPrimitive.Name = data.Name;
                newPrimitive.ListValue.Add((value - tolerance) + ".." + (value + tolerance));
            }
            solution.checkAndAdd(newPrimitive);
        }


        private static PrimitiveData checkStringListPrimitives(PrimitiveData reference)
        {


            // gets all the string primitives with the same name, step and with at least one value in its list of values that is similar
            IEnumerable<PrimitiveData> antiUnified = (from sample in samples
                                                      from primitive in sample
                                                      where primitive.IDcompare(reference.ID)
                                                      && primitive.Name == reference.Name
                                    
                                                      select primitive);

           // List<PrimitiveData> toCompare = antiUnified.ToList();




            // this primitive exists on the same step for all the samples
            if (antiUnified.Count() == samples.Count())
            {
                IEnumerable<string> intersectedValue = reference.ListValue;

                foreach (PrimitiveData value in antiUnified)
                {
                    intersectedValue = value.ListValue.Intersect(intersectedValue);
                }

                if (intersectedValue.Count() > 0)
                    return new PrimitiveData()
                    {
                        ID = reference.ID,
                        Name = reference.Name,
                        ListValue = intersectedValue.ToList(),
                        PointsRecognized = reference.PointsRecognized,
                        Value = reference.Value
                    };
            }

            return null;
        }



        /*         
         Check a way so that the proportion:
         * Compares the data with all the data in the same gesture with the same name.
         * Find the alpha between the first found
         * add alpha to alphas
         * Then it has to do the same for all the other gestures
         */

        private static PrimitiveData getPrimitiveWithSameName(PrimitiveData data, SetOfPrimitives gesture)
        {
            foreach (PrimitiveData result in gesture)
            {
                if ((data.Name == result.Name) && (data.IDcompare(result.ID)))
                    return result;
            }
            return null;
        }



        private static void checkProportionforPrimitive(PrimitiveData reference)
        {
            /*
             * On the same sample, compare to see the relation between the basic primitives, if there is any proportion, if find any, create a new complex primitive that will
             * reference the basic similar ones
             */
            if (reference.Value == 0)
            {
                return;

            }

            /*  IEnumerable<PrimitiveData> similarInSameStep = from set in samples
                                                             from primitive in set
                                                             where primitive.Name == data.Name
                                                             && primitive.ID.Count == 1
                                                             && data.IDcompare(primitive.ID)
                                                             select primitive;
              */



            PrimitiveData originalData = reference;
            SetOfPrimitives gesture;
            List<SetOfPrimitives> proportions = new List<SetOfPrimitives>();
            PrimitiveData newPrimitive = null;
            List<double> values = new List<double>();

            values.Add(reference.Value);

            int i = 0;
            gesture = samples.ElementAt(i);

            while (i < samples.Count)
            {
                PrimitiveData compare;
                for (int j = 0; j < gesture.Count; j++)
                {
                    compare = gesture[j];


                    if ((reference.Name == compare.Name) && (reference.IDcompare(compare.ID)))
                    {
                        values.Add(compare.Value);
                    }

                    if (reference.Equals(compare))
                    {
                        continue;
                    }

                    if ((reference.Name == compare.Name) && (compare.ID.Count == 1) && (reference.ID[0] < compare.ID[0]))
                    {
                        newPrimitive = new PrimitiveData();
                        newPrimitive.Name = reference.Name;
                        newPrimitive.IDadd(reference.ID[0]);
                        newPrimitive.IDadd(compare.ID[0]);
                        newPrimitive.Value = compare.Value / reference.Value;

                        while (proportions.Count <= i)
                        {
                            proportions.Add(new SetOfPrimitives());
                        }

                        proportions[i].Add(newPrimitive);
                    }
                }
                i++;
                if (i < samples.Count)
                {
                    gesture = samples.ElementAt(i);
                    reference = getPrimitiveWithSameName(reference, gesture);
                    if (reference == null)
                        return;

                    values.Add(reference.Value);

                }
            }
            // After the loops, the proportions should have the same amount of items as samples
            // and each setofprimitive of proportion should have the same amount of primitives
            // If not, then nothing will be add to solution
            // if it is, the same comparison should be done in proportions

            if (proportions.Count < samples.Count)
            {
                // No proportion was found, try to see if the primitive is a constant among the samples
                checkConstantValuePrimitives(originalData);
                return;
            }
            List<double> alphas;
            int setCount = proportions[0].Count;
            for (i = 0; i < proportions[0].Count; i++)
            {
                alphas = new List<double>();

                foreach (SetOfPrimitives set in proportions)
                {
                    if (setCount != set.Count)
                        return;

                    alphas.Add(set[i].Value);
                }

                double value = Math.Round(values.Average(), 2);
                double maxDifference = alphas.Max() - alphas.Min();
                double tolerance = Math.Round(value * matchingAccuracy, 2);

                if (maxDifference > tolerance)
                {
                    newPrimitive = new PrimitiveData();
                    newPrimitive.ID = originalData.ID;
                    newPrimitive.Name = originalData.Name;
                    newPrimitive.ListValue.Add((value - tolerance) + ".." + (value + tolerance));
                    solution.checkAndAdd(newPrimitive);
                }


                else if ((maxDifference != 0))
                {
                    string variable = VariableName.getAvailableLetter();
                    // Adds the x primitive
                    newPrimitive = new PrimitiveData();
                    newPrimitive.IDadd(proportions[0][i].ID[0]);
                    newPrimitive.Name = proportions[0][i].Name;
                    newPrimitive.Value = 0;
                    newPrimitive.ListValue.Add(variable);
                    solution.checkAndAdd(newPrimitive);

                    // adds the alpha*x primitive
                    newPrimitive = new PrimitiveData();
                    newPrimitive.IDadd(proportions[0][i].ID[1]);
                    newPrimitive.Name = proportions[0][i].Name;
                    newPrimitive.Value = 0;
                    double alpha = Math.Round(alphas.Average(), 2);
                    //newPrimitive.strValue = alpha + "x";
                    tolerance = Math.Round(alpha * matchingAccuracy, 2);
                    newPrimitive.ListValue.Add((alpha - tolerance) + variable + ".." + (alpha + tolerance) + variable);
                    solution.checkAndAdd(newPrimitive);

                }
                else
                {
                    newPrimitive = new PrimitiveData();
                    newPrimitive.IDadd(proportions[0][i].ID[0]);
                    newPrimitive.Name = proportions[0][i].Name;
                    newPrimitive.Value = proportions[0][i].Value;
                    solution.checkAndAdd(newPrimitive);

                    newPrimitive = new PrimitiveData();
                    newPrimitive.IDadd(proportions[0][i].ID[1]);
                    newPrimitive.Name = proportions[0][i].Name;
                    newPrimitive.Value = proportions[0][i].Value;
                    solution.checkAndAdd(newPrimitive);
                }



            }

        }

        public static bool removeSample(int sampleIDX)
        {
            try
            {
                samples.RemoveAt(sampleIDX);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        public static bool removeAllSamples()
        {
            try
            {
                if (samples != null)
                    samples.Clear();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        private static string getStrValueFromGDL(string gdl)
        {
            string result = gdl.Replace(" ", "");
            int idx = result.IndexOf(":");
            if (idx >= 0)
                result = result.Substring(idx + 1);
            else
                result = "";

            return result;
        }

        private static string getPrimitiveName(IPrimitiveConditionData primitive)
        {
            string fullname = primitive.ToGDL();
            int pos = fullname.LastIndexOf(":");
            if (pos == -1)
                return fullname;
            else
                return fullname.Remove(pos);
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




        public static List<String> TouchPointsToGDL(ValidSetOfTouchPoints allpoints)
        {
            if (allpoints == null)
                return null;

            List<IPrimitiveConditionData> primitives;
            SetOfPrimitives setOfPrimitives = new SetOfPrimitives();
            List<TouchPoint2> simplegestures = new List<TouchPoint2>();
            primitives = GestureLanguageProcessor.GetAllPrimitives();

            SetOfPrimitives sample = new SetOfPrimitives();

            if ((allpoints.Count == 1) && (PointTranslator.BreakIntoSteps))
                simplegestures = PointTranslator.FindLines(allpoints[0]);
            else
            {
                simplegestures = PointTranslator.analyzeTags(allpoints);
                simplegestures = PointTranslator.removeHandRedundancy(simplegestures);
            }
            //=============================            

            var simplePrimitives = from p in primitives where p.isComplex() == false select p;
            var complexPrimitives = from p in primitives where p.isComplex() == true select p;
            sample.AddRange(loadPrimitives(simplegestures, simplePrimitives));
            sample.AddRange(loadPrimitives(simplegestures, complexPrimitives));

            Add(sample);
            return PrimitiveToGDL(sample);
        }




        private static SetOfPrimitives loadPrimitives(List<TouchPoint2> simplegestures, IEnumerable<IPrimitiveConditionData> primitives)
        {
            List<IPrimitiveConditionData> returnList;
            SetOfPrimitives output = new SetOfPrimitives();

            foreach (IPrimitiveConditionData primitive in primitives)
            {
                returnList = null;
                IPrimitiveConditionValidator ruleValidator = GestureLanguageProcessor.GetPrimitiveConditionValidator(primitive);
                returnList = ruleValidator.GenerateRules(simplegestures);
                if (returnList != null)
                {
                    int index = 1;
                    foreach (IPrimitiveConditionData resultData in returnList)
                    {
                        if (resultData != null)
                        {
                            PrimitiveData primitiveData = new PrimitiveData();
                            if (primitive.isComplex())
                            {
                                primitiveData.ID = getIDsFromGDL(resultData.ToGDL());
                            }
                            else
                            {
                                primitiveData.IDadd(index++);
                            }
                            primitiveData.Name = getPrimitiveName(primitive);
                            string strValue = getStrValueFromGDL(resultData.ToGDL());
                            double value;
                            bool isDouble = double.TryParse(strValue, out value);
                            if (isDouble)
                                primitiveData.Value = value;
                            else
                            {
                                int idx = strValue.IndexOf(",");
                                if (idx >= 0)
                                    primitiveData.ListValue = new List<string>(strValue.Split(','));
                                else
                                    primitiveData.ListValue.Add(strValue);
                            }
                            output.Add(primitiveData);

                        }
                        else
                            index++;
                    }
                }
            }
            return output;
        }


        public static List<String> GetSolutionGDL()
        {
            return PrimitiveToGDL(solution);
        }

        private static List<String> PrimitiveToGDL(SetOfPrimitives primitives)
        {
            if (primitives == null)
                throw new ArgumentNullException();

            string name = "";// "name:" + GestureName + Environment.NewLine;
            List<String> result = new List<String>();
            const string SPACE = "      ";

            List<string> steps = new List<string>();
            List<string> complexSteps = new List<string>();

            int step = 1;

            IdComparer comparer = new IdComparer();
            for (int i = 0; i < primitives.Count; i++)
            {
                if (primitives[i] == null)
                    primitives[i] = new PrimitiveData();

            }

            primitives.Sort(comparer);



            foreach (PrimitiveData primitive in primitives)
            {
                if ((primitive.ID.Count == 1))   // Simple primitives
                {
                    if (primitive.ID[0] != step)
                    {
                        steps.Add("validate as step" + primitive.ID[0]);
                        step++;
                    }

                    if (primitive.ListValue.Count == 0)
                    {
                        if (primitive.Value != 0)
                            steps.Add(SPACE + primitive.Name + ":" + Math.Round(primitive.Value, 2));
                        else
                            steps.Add(SPACE + primitive.Name);

                    }
                    else
                    {
                        steps.Add(SPACE + primitive.Name + ":" + string.Join(",", primitive.ListValue));

                    }

                }
                else
                {
                    string primitiveID = primitive.IDtoString();
                    if (primitiveID != "")
                        primitiveID = " " + primitiveID;

                    if (primitive.ListValue.Count == 0)
                    {
                        complexSteps.Add(SPACE + primitive.Name + primitiveID + ":" + Math.Round(primitive.Value, 2));

                    }
                    else
                    {
                        complexSteps.Add(SPACE + primitive.Name + primitiveID + ":" + string.Join(",", primitive.ListValue));

                    }
                }
            }

            if (step == 1)
                result.Insert(0, "validate");
            else
                result.Insert(0, "validate as step1");

            result.AddRange(steps);
            if (complexSteps.Count > 1)
            {
                if (step > 1)
                    result.Add("validate");

                result.AddRange(complexSteps);
            }

            result.Add("return" + Environment.NewLine + SPACE + "Touch points");


            return result;


        }











    }
}


