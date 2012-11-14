using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ActiveStoryTouch.DataModel.Helpers
{
    public class PageSorter : IComparer
    {
        #region IComparer Members

        int IComparer.Compare(object first, object second)
        {
            ASGPage firstPage = first as ASGPage;
            ASGPage secondPage = second as ASGPage;
            if (firstPage == null && secondPage == null)
                return 0;
            else if (firstPage == null)
                return -1;
            else if (secondPage == null)
                return 1;

            int compareResult = firstPage.PageNumber.CompareTo(secondPage.PageNumber);

            if (compareResult != 0)
            {
                return compareResult;
            }
            else
            {
                // go deeper since they're equal
                return firstPage.Name.CompareTo(secondPage.Name);
            }

        }

        #endregion
    }
}
