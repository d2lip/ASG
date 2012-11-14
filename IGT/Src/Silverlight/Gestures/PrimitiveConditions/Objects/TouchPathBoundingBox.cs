using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Objects
{
    public class TouchPathBoundingBox : IPrimitiveConditionData
    {
        double _minHeight, _minWidth, _maxHeight, _maxWidth;
        public double MinHeight
        {
            get
            {
                return _minHeight;
            }
            set
            {
                _minHeight = value;
            }
        }

        public double MinWidth
        {
            get
            {
                return _minWidth;
            }
            set
            {
                _minWidth = value;
            }
        }

        public double MaxHeight
        {
            get
            {
                return _maxHeight;
            }
            set
            {
                _maxHeight = value;
            }
        }

        public double MaxWidth
        {
            get
            {
                return _maxWidth;
            }
            set
            {
                _maxWidth = value;
            }
        }
        #region IRuleData Members

        public bool Equals(IPrimitiveConditionData rule)
        {
            throw new NotImplementedException();
        }

        #endregion


        public void Union(IPrimitiveConditionData value)
        {
            if (value == null)
            {
                throw new Exception("Null Value Exception");
            }
            if (!(value is TouchPathBoundingBox))
            {
                throw new Exception("Wrong Input Type Exception");
            }
            TouchPathBoundingBox touchPBox = value as TouchPathBoundingBox;

            if (this.MinHeight > touchPBox.MinHeight)
                this.MinHeight = touchPBox.MinHeight;

            if (this.MaxHeight < touchPBox.MaxHeight)
                this.MaxHeight = touchPBox.MaxHeight;

            if (this.MinWidth > touchPBox.MinWidth)
                this.MinWidth = touchPBox.MinWidth;

            if (this.MaxWidth < touchPBox.MaxWidth)
                this.MaxWidth = touchPBox.MaxWidth;

        }


        public string ToGDL()
        {
            if (this.MinWidth == 0 && this.MinHeight == 0)
                return string.Format("Touch path bounding box: {0}x{1}", this.MaxHeight, this.MaxWidth);
            else if (this.MaxWidth == 0 && this.MaxHeight == 0)
                return string.Format("Touch path bounding box: {0}x{1}", this.MinHeight, this.MinWidth);
            else
                return string.Format("Touch path bounding box: {0}x{1}..{2}x{3}", this.MinHeight, this.MinWidth, this.MaxHeight, this.MaxWidth);
        }


        public bool isComplex()
        {
            return true;
        }
    }
}
