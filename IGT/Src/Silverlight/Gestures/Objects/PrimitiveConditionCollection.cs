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
using System.Linq;

using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;

namespace TouchToolkit.GestureProcessor.Objects
{


    public class PrimitiveConditionCollection : List<IPrimitiveConditionValidator>
    {
        public ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets)
        {
            // TODO: Consider sorting the list so that less expensive conditions are evaluated first
            string gestureName=  sets.ExpectedGestureName;
            IOrderedEnumerable < IPrimitiveConditionValidator > ordered =  this.OrderBy(x => x.Order());


            foreach (var rule in ordered)
            {
                /* TODO: Temporary work around for testing new types
                 */
                sets.ExpectedGestureName = gestureName;
                sets = rule.Validate(sets);

                if (sets.Count == 0)
                    break;
            }

            return sets;
        }

        public ValidSetOfPointsCollection Validate(ValidSetOfTouchPoints set)
        {
            ValidSetOfPointsCollection sets = new ValidSetOfPointsCollection();

            sets.Add(set);

            return Validate(sets);
        }
    }
}
