using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActiveStoryTouch.DataModel.Helpers
{
    public static class ElementTypesHelper
    {
        public static ElementTypes ElementTypeFromString(String elementTypeString)
        {
            ElementTypes resultType;
            Enum.TryParse<ElementTypes>(elementTypeString, true, out resultType);
            return resultType;
        }
        public static String StringFromElementType(ElementTypes elementType)
        {
            return elementType.ToString();
        }
    }
}
