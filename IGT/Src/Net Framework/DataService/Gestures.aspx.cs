using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace DataService
{
    public partial class Gestures : System.Web.UI.Page
    {
        //RunTest = 0;
        //GestureSet = 1;
        //AccountName = 2;
        //ProjectName = 3;
        //GestureName = 4;
        const string URLTemplate = "http://sandbox.shahed.me/p/gesture-language/default.aspx?rt={0}&gs={1}&ac={2}&p={3}&g={4}&sr=false";

        DataAccess _dataAccess = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            // Load account names
            List<User> users = _dataAccess.GetUsers();
            accountListBox.Items.Clear();
            foreach (var user in users)
            {
                accountListBox.Items.Add(new ListItem() { Value = user.Id.ToString(), Text = user.UserName });
            }
        }

        public void LoadProjects()
        {
            int userId = accountListBox.GetSelectedId();

            var projects = _dataAccess.GetProjects(userId);
            projectListBox.Items.Clear();
            foreach (var proj in projects)
            {
                projectListBox.Items.Add(new ListItem() { Text = proj.Name, Value = proj.Id.ToString() });
            }
        }

        public void LoadGestures()
        {
            int projectId = projectListBox.GetSelectedId();
            var gestures = _dataAccess.GetGestures(projectId);
            gestureListBox.Items.Clear();
            foreach (var g in gestures)
            {
                gestureListBox.Items.Add(new ListItem() { Value = g.Id.ToString(), Text = g.GestureName });
            }
        }

        protected void accountListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void projectListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGestures();
        }

        protected void gestureListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateTestPageUrl();
        }

        private void GenerateTestPageUrl()
        {
            //RunTest = 0;
            //GestureSet = 1;
            //AccountName = 2;
            //ProjectName = 3;
            //GestureName = 4;

            StringBuilder sb = new StringBuilder();
            foreach (ListItem item in enabledGesturesListBox.Items)
            {
                sb.Append(item.Value);
            }

            string url = string.Format(URLTemplate, "true",
                sb.ToString(), accountListBox.GetSelectedText(), projectListBox.GetSelectedText(), gestureListBox.GetSelectedText());

            testPageUrl.NavigateUrl = url;
            testPageUrl.Text = url;
        }

        protected void generatePageButton_Click(object sender, EventArgs e)
        {
            GenerateTestPageUrl();
        }
    }
}
