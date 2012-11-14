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

namespace TouchToolkit.GestureProcessor.Objects
{
    public class ValidSetOfPointsCollection : List<ValidSetOfTouchPoints>
    {
        public string ExpectedGestureName { get; set; }


        public void Remove(ValidSetOfPointsCollection list)
        {
            var itemsToRemove = new ValidSetOfPointsCollection();   
            // Build the unique set of touch points
            foreach (var touches in list)
            {
                foreach (var touch in touches)
                {
                    // Remove the sets that contain any of the above selected touchPoints
                    // TODO: its a work-around for development purpose... Re-think!!

                    foreach (var item in this)
                    {
                        if (item.Contains(touch))
                            itemsToRemove.Add(item);
                    }
                }
            }


            foreach (var item in itemsToRemove)
            {
                this.Remove(item);
            }

        }
    }
}
