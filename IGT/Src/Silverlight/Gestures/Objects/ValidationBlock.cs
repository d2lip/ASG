using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;

namespace TouchToolkit.GestureProcessor.Objects
{
    public class ValidationBlock
    {
        public string Name = string.Empty;
        public PrimitiveConditionCollection PrimitiveConditions = new PrimitiveConditionCollection();
    }
}
