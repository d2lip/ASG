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
using System.Windows.Threading;
using Microsoft.Surface.Presentation.Input;


namespace SurfaceApplication
{
    /// <summary>
    /// Interaction logic for IGTRules.xaml
    /// </summary>
    public partial class IGTRules : UserControl
    {
        
        public double accuracy = 0;
        private IGT igt;


        public IGTRules(IGT main)
        {
            InitializeComponent();
            igt = main;
           
            
        }


        private void GenerateRule_Click(object sender, RoutedEventArgs e)
        {
            AntiUnificator.setMatchingAccuracy(Convert.ToDouble(igt.options.accuracy.Text));
            AntiUnificator.checkRules();
            rules.Items.Clear();

            try
            {

                List<String> gdl = AntiUnificator.GetSolutionGDL();
                string rule = "";
                foreach (string s in gdl)
                    rule += s + Environment.NewLine;

                rules.Items.Add(rule);
            }

            catch (ArgumentNullException er)
            {
                igt.ShowMessageOnCanvas("There are no samples", 3);

            }

          
        }


        public void addNewRule(List<String> rule, InkCanvas canvas)
        {
            RuleSample sample = new RuleSample(this, rule, canvas);
            sample.RemoveSample.Click += RemoveSample_Click;
            gestures.Items.Add(sample);
        }
    
  


        public void addNewRule(List<String> rule, SurfaceInkCanvas canvas)
        {
            RuleSample sample = new RuleSample(this, rule, canvas);
            sample.RemoveSample.Click += RemoveSample_Click; 
            gestures.Items.Add(sample);
        }

        private void RemoveSample_Click(object sender, RoutedEventArgs e)
        {
            Grid grid = (sender as SurfaceButton).Parent as Grid;
            SurfaceScrollViewer scroll = grid.Parent as SurfaceScrollViewer;
            RuleSample sample = scroll.Parent as RuleSample;

            int idx = gestures.Items.IndexOf(sample);

            if (AntiUnificator.removeSample(idx))
            {
                gestures.Items.Remove(sample);
            }

        }

   


      

        private void RemoveAll_Click(object sender, RoutedEventArgs e)
        {         
            if (AntiUnificator.removeAllSamples())
            {
                gestures.Items.Clear();
            }
            
        }


    }
}
