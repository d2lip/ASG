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

namespace TouchToolkit.GestureProcessor.Base
{
    public class PrimitiveConditionResultCache
    {
        List<Tuple<ValidSetOfTouchPoints, ValidSetOfTouchPoints>> _resultCache;

        public PrimitiveConditionResultCache()
        {
            _resultCache = new List<Tuple<ValidSetOfTouchPoints, ValidSetOfTouchPoints>>();
        }

        public ValidSetOfTouchPoints Get(ValidSetOfTouchPoints points)
        {
            throw new NotImplementedException();
        }
    }
}
