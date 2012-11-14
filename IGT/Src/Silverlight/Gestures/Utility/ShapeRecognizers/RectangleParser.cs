using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace TouchToolkit.Framework.ShapeRecognizers
{
    public class RectangleParser
    {
        public bool IsRect = false;
        bool isCW = false;
        string next = "";
        string current = "";
        int sides = 0;
        private String GetNextCW(string direction)
        {
            string next = "";
            switch (direction)
            {
                case "Up":
                    next = "Right";
                    break;
                case "Down":
                    next = "Left";
                    break;
                case "Left":
                    next = "Up";
                    break;
                case "Right":
                    next = "Down";
                    break;
                case "UpRight":
                    next = "DownRight";
                    break;
                case "UpLeft":
                    next = "UpRight";
                    break;
                case "DownRight":
                    next = "DownLeft";
                    break;
                case "DownLeft":
                    next = "UpRight";
                    break;
            }
            return next;
        }
        private String GetNextCCW(string direction)
        {
            string next = "";
            switch (direction)
            {
                case "Up":
                    next = "Left";
                    break;
                case "Down":
                    next = "Right";
                    break;
                case "Left":
                    next = "Down";
                    break;
                case "Right":
                    next = "Up";
                    break;
                case "UpRight":
                    next = "UpLeft";
                    break;
                case "UpLeft":
                    next = "DownLeft";
                    break;
                case "DownRight":
                    next = "UpRight";
                    break;
                case "DownLeft":
                    next = "DownRight";
                    break;
            }
            return next;
        }
        public bool Advance(List<string> lines)
        {
            foreach (var line in lines)
            {
                Advance(line);
                if (IsRect)
                {
                    return true;
                }
            }
            
            return false;
        }
        public bool Advance(string lines)
        {
            
            bool isAdvanced = false;
            if (next.Equals("") || sides == 0)
            {
                next = lines;
                sides = 1;
                isAdvanced = true;
            }
            else if (current.Equals(lines))
            {
                isAdvanced = true;
            }
            else if (sides == 1)
            {
                if (lines.Equals(GetNextCCW(next)))
                {
                    isCW = false;
                    isAdvanced = true;
                    next = GetNextCCW(lines);
                    sides++;
                }
                else if (lines.Equals(GetNextCW(next)))
                {
                    isAdvanced = true;
                    isCW = true;
                    next = GetNextCW(lines);
                    sides++;
                }
            }
            else if (sides == 2 && isCW)
            {
                if (lines.Equals(next))
                {
                    isAdvanced = true;
                    next = GetNextCW(next);
                    sides++;
                }
                else
                {
                    sides = 0;
                    next = lines;
                }
            }
            else if (sides == 2 && !isCW)
            {
                if (lines.Equals(next))
                {
                    isAdvanced = true;
                    next = GetNextCCW(next);
                    sides++;
                }
                else
                {
                    sides = 0;
                    next = lines;
                }
            }
            else if (sides == 3)
            {
                if (lines.Equals(next))
                {
                    isAdvanced = true;
                    IsRect = true;
                }
                else
                {
                    sides = 0;
                    next = lines;
                }
            }
            if (isAdvanced)
            {
                current = lines;
            }
            return isAdvanced;
        }
    }
}
