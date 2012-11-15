using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiUnification
{
    public static class VariableName
    {
        private static string[] letters = new string [] { "x", "y", "z", "a", "b", "c", "d", "e", "f" };

        private static int currentIndex = -1;

        public static string getAvailableLetter()
        {
            currentIndex++;
            return letters[currentIndex];
         
        }


        public static void reset()
        {
            currentIndex = 0;
        }
    }
}
