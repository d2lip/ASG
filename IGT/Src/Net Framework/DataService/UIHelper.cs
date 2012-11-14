using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace DataService
{
    public static class UIHelper
    {
        public static int GetSelectedId(this ListBox self)
        {
            int id = 0;
            int.TryParse(self.SelectedValue, out id);
            return id;
        }

        public static string GetSelectedText(this ListBox self)
        {
            if (self.SelectedItem != null)
                return self.SelectedItem.Text;
            else
                return string.Empty;
        }
    }
}
