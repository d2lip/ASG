using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;

namespace TouchToolkit.GestureProcessor.Objects.LanguageTokens
{
    public class ValidateToken : LanguageToken
    {
        private List<IPrimitiveConditionData> _primitiveConditions = new List<IPrimitiveConditionData>();
        public List<IPrimitiveConditionData> PrimitiveConditions
        {
            get
            {
                return _primitiveConditions;
            }
            set
            {
                _primitiveConditions = value;
            }
        }
    }
}
