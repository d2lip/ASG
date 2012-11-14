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
using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Validators
{
    public interface IPrimitiveConditionValidator
    {
        /// <summary>
        /// Sets the validator to use the provided data while validating any gesture
        /// </summary>
        /// <param name="ruleData"></param>
        void Init(IPrimitiveConditionData ruleData);

        /// <summary>
        /// Returns whether two validators are logically equal
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        bool Equals(IPrimitiveConditionValidator rule);

        /// <summary>
        /// Returns one or more sets of points that satisfies the condition
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        ValidSetOfPointsCollection Validate(List<TouchPoint2> points);

        /// <summary>
        /// Returns one or more sets of points that satisfies the condition
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        ValidSetOfPointsCollection Validate(ValidSetOfPointsCollection sets);

        /// <summary>
        /// Builds approximate rule data for the given gesture
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        List<IPrimitiveConditionData> GenerateRules(List<TouchPoint2> points);

        
        IPrimitiveConditionData GenerateRuleData(List<TouchPoint2> points);

         int Order();
    }
}
