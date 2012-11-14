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
using AntiUnification;
using System.Windows.Ink;

namespace SurfaceApplication
{
    /// <summary>
    /// Interaction logic for RuleSample.xaml
    /// </summary>
    public partial class RuleSample : UserControl
    {
        private IGTRules parent;

        public RuleSample()
        {
            InitializeComponent();
            
        }

        public RuleSample(IGTRules _parent, List<String> gdl, SurfaceInkCanvas canvas)
        {
            InitializeComponent();
            foreach (string s in gdl)
            {
                rule.Items.Add(s);
            
            }

            parent = _parent;
            thumbnail.Strokes.Add(canvas.Strokes);
            viewbox.Width = canvas.ActualWidth / 3;
            viewbox.Height = canvas.ActualHeight / 3;
            thumbnail.EditingMode = SurfaceInkEditingMode.None;            

        }


        public RuleSample(IGTRules _parent, List<String> gdl, InkCanvas canvas)
        {
            InitializeComponent();
            foreach (string s in gdl)
            {
                rule.Items.Add(s);

            }

            parent = _parent;
            thumbnail.Strokes.Add(canvas.Strokes);
            viewbox.Width = canvas.ActualWidth / 3;
            viewbox.Height = canvas.ActualHeight / 3;
            thumbnail.EditingMode = SurfaceInkEditingMode.None;

        }

        private void DeleteLine_Click(object sender, RoutedEventArgs e)
        {
            if (rule.SelectedIndex != 0 && rule.SelectedIndex != rule.Items.Count-1)       
                rule.Items.RemoveAt(rule.SelectedIndex);

            int i = parent.rules.SelectedIndex;
          //  SetOfPrimitives primitives =  AntiUnificator.getSample(i);

           // primitives.RemoveAt(rule.SelectedIndex);
        }

      


  

      

    }
}
