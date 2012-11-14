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
using TouchToolkit.GestureProcessor.ReturnTypes;


namespace TouchToolkit.GestureProcessor.Objects
{
    public class ReturnTypeInfoCollection : List<ReturnTypeInfo>
    {
        /// <summary>
        /// Calculates the specified return objects for each item in the list
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public List<IReturnType> Calculate(ValidSetOfTouchPoints set)
        {
            List<IReturnType> results = new List<IReturnType>();

            foreach (var retInfo in this)
            {
                IReturnTypeCalculator calc = Activator.CreateInstance(retInfo.CalculatorType) as IReturnTypeCalculator;

                //TODO: temp work-around for Info type
                if (!string.IsNullOrEmpty(retInfo.AdditionalInfo))
                {
                    if (calc is InfoCalculator)
                    {
                        (calc as InfoCalculator).Message = retInfo.AdditionalInfo;
                    }
                }

                IReturnType  result = calc.Calculate(set);
                results.Add(result);
            }

            return results;
        }
    }
}
