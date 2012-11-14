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
using ActiveStoryTouch.DataModel;
using ASG.UI;

namespace ASG.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for SaveDialog.xaml
    /// </summary>
    public partial class SaveDialog : UserControl
    {

        private SessionManager sessionManager;
        Main instance;
        public SaveDialog(SessionManager _sessionManager, Main _instance)
        {

            InitializeComponent();

            sessionManager = _sessionManager;
            instance = _instance;
        }

        
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ((ScatterView)this.Parent).Items.Remove(this);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            sessionManager.SaveCurrentPage(instance.PageInkCanvas.Strokes);
            String pathName = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Projects\\";
            
            try
            {
            sessionManager.SaveProject(pathName + FileName.Text + ".asgproj");
            }
            catch(Exception err)
            {
                instance.ShowMessageOnCanvas(err.Message, 20);
            }
            ((ScatterView)this.Parent).Items.Remove(this);
            
            
        }



    }
}
