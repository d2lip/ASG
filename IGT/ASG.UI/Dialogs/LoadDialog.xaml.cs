using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.IO;
using ASG.UI;
using ActiveStoryTouch.DataModel;


namespace ASG.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for LoadDialog.xaml
    /// </summary>
    public partial class LoadDialog : UserControl
    {
        private SessionManager sessionManager;
        private Main instance;
        private LoadDialogType type;
        
        private ScatterViewItem Svi;

        public enum LoadDialogType
        {
            BackgroundImage,
            GestureAreaImage,
            Project
        }

        //"*.asgproj"

        public LoadDialog(SessionManager _sessionManager, Main _instance,List<string> _filter, LoadDialogType _type, ScatterViewItem svi = null)
        {
            InitializeComponent();
            sessionManager = _sessionManager;
            instance = _instance;
            type = _type;           
            DirectoryInfo di = null;
            switch (type)
            { 
                   
                case LoadDialogType.Project:
                     di = new DirectoryInfo(EnvironmentFolder.getProjectsFolder());
                    LoadButton.Click += LoadProjectButton_Click;
                    Title.Content = "LOAD PROJECT";
                    break;
                case LoadDialogType.BackgroundImage:
                    di = new DirectoryInfo(EnvironmentFolder.getImagesFolder());
                    LoadButton.Click += LoadImageButton_Click;
                    Title.Content = "BACKGROUND IMAGE";
                    break;
                case LoadDialogType.GestureAreaImage:
                    di = new DirectoryInfo(EnvironmentFolder.getImagesFolder());
                    Svi = svi;
                    LoadButton.Click += LoadImageGestureAreaButton_Click;
                    Title.Content = "GESTURE AREA IMAGE";
                    break; 
            }
         
            ProjectList.Items.Clear();
            foreach (string s in _filter)
            {
                FileInfo[] rgFiles = di.GetFiles(s);

                foreach (FileInfo fi in rgFiles)
                {
                    ProjectList.Items.Add(fi.Name);
                }
            }

        }



        private void LoadImageGestureAreaButton_Click(object sender, RoutedEventArgs e)
        {
            string msg = "";
            if (ProjectList.SelectedItem == null)
            {
                msg = "No image was selected";
            }
            else
            {               
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(EnvironmentFolder.getImagesFolder() + ProjectList.SelectedItem.ToString(), UriKind.Absolute);
                src.EndInit();
                (Svi.Content as GestureArea).BackgroundImage.Source =  src;
                src.CacheOption = BitmapCacheOption.OnDemand;
                (Svi.Tag as PrototypeElement).BackgroundImage = ProjectList.SelectedItem.ToString();              
            }

            ((ScatterView)this.Parent).Items.Remove(this);
            instance.ShowMessageOnCanvas(msg, Main.MESSAGE_TIME_PERIOD);
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {

            string msg = "";
            if (ProjectList.SelectedItem == null)
            {
                msg = "No image was selected";
            }
            else
            {
                sessionManager.CurrentPage.BackgroundImageSource = ProjectList.SelectedItem.ToString();
                instance.loadPageBackground(sessionManager.CurrentPage);
            }

            ((ScatterView)this.Parent).Items.Remove(this);
            instance.ShowMessageOnCanvas(msg, Main.MESSAGE_TIME_PERIOD);
        }

        private void LoadProjectButton_Click(object sender, RoutedEventArgs e)
        {

            string msg = "";
            if (ProjectList.SelectedItem == null)
            {
                msg = "No project was selected";
            }
            else
            {
                instance.ClearCanvas();
                try
                {
                    sessionManager.LoadProject(EnvironmentFolder.getProjectsFolder() +ProjectList.SelectedItem.ToString());
                    instance.SetupAndRenderPage(sessionManager.CurrentPage);
                    instance.loadGestureAreasFromCurrentProject();

                    msg = "Project loaded";
                }
                catch (Exception er)
                {
                    msg = er.Message;
                
                }

            }

            ((ScatterView)this.Parent).Items.Remove(this);
            instance.ShowMessageOnCanvas(msg, Main.MESSAGE_TIME_PERIOD);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ((ScatterView)this.Parent).Items.Remove(this);
        }
    }
}
