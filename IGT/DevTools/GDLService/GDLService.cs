using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio;
using System.Dataflow;
using Microsoft.M;
using System.IO;
using Microsoft.VisualStudio.Package;

namespace TouchToolKit.GDLService
{
    public class GDLService : LanguageService
    {
        private LanguagePreferences _preferences;
        private GDLScanner _scanner;
        private ColorableItem[] _colorableItems;

        public GDLService()
            : base()
        {
            _colorableItems = new ColorableItem[]
            {
                //First item is never referenced
                new ColorableItem("Placeholder",
                    "Placeholder",
                    COLORINDEX.CI_SYSPLAINTEXT_FG,
                    COLORINDEX.CI_SYSPLAINTEXT_BK,
                    System.Drawing.Color.Empty,
                    System.Drawing.Color.Empty,
                    FONTFLAGS.FF_DEFAULT),
                new ColorableItem("(Gesture Definition Language) - Text",
                    "Text",
                    COLORINDEX.CI_SYSPLAINTEXT_FG,
                    COLORINDEX.CI_SYSPLAINTEXT_BK,
                    System.Drawing.Color.Empty,
                    System.Drawing.Color.Empty,
                    FONTFLAGS.FF_DEFAULT),
                    new ColorableItem("(Gesture Definition Language) - Keyword",
                    "Keyword",
                    COLORINDEX.CI_MAROON,
                    COLORINDEX.CI_SYSPLAINTEXT_BK,
                    System.Drawing.Color.Maroon,
                    System.Drawing.Color.Empty,
                    FONTFLAGS.FF_DEFAULT),
                    new ColorableItem("(Gesture Definition Language) - Comment",
                    "Comment",
                    COLORINDEX.CI_DARKGREEN,
                    COLORINDEX.CI_LIGHTGRAY,
                    System.Drawing.Color.DarkGreen,
                    System.Drawing.Color.Empty,
                    FONTFLAGS.FF_DEFAULT),
                    new ColorableItem("(Gesture Definition Language) - Identifier",
                    "Identifier",
                    COLORINDEX.CI_BLUE,
                    COLORINDEX.CI_SYSPLAINTEXT_BK,
                    System.Drawing.Color.Blue,
                    System.Drawing.Color.Empty,
                    FONTFLAGS.FF_DEFAULT),
                    new ColorableItem("(Gesture Definition Language) - Misc",
                    "Identifier",
                    COLORINDEX.CI_PURPLE,
                    COLORINDEX.CI_SYSPLAINTEXT_BK,
                    System.Drawing.Color.Violet,
                    System.Drawing.Color.Empty,
                    FONTFLAGS.FF_DEFAULT)
            };
        }

        public override string GetFormatFilterList()
        {
            return "Gesture Definition (*.g)\n*.g";
        }

        public override LanguagePreferences GetLanguagePreferences()
        {
            if (_preferences == null)
            {
                _preferences = new LanguagePreferences(this.Site,
                                                        typeof(GDLService).GUID,
                                                        this.Name);
                _preferences.Init();
            }
            return _preferences;
        }

        public override IScanner GetScanner(IVsTextLines buffer)
        {
            if (_scanner == null)
            {
                _scanner = new GDLScanner();
            }
            return _scanner;
        }

        public override string Name
        {
            get { return "Gesture Definition Language"; }
        }

        public override AuthoringScope ParseSource(ParseRequest req)
        {
            return new MyAuthoringScope();
        }

        #region ColoringService
        public override int GetItemCount(out int count)
        {
            count = _colorableItems.Length - 1;
            return VSConstants.S_OK;
        }

        public override int GetColorableItem(int index, out IVsColorableItem item)
        {
            item = _colorableItems[index];
            return VSConstants.S_OK;
        }

        public override Colorizer GetColorizer(IVsTextLines buffer)
        {
            return new GDLColorizer(this, buffer, new GDLScanner());
        }
        #endregion
    }

    //The minimum required to get this working
    internal class MyAuthoringScope : AuthoringScope
    {
        public override string GetDataTipText(int line, int col, out TextSpan span)
        {
            span = new TextSpan();
            return null;
        }

        public override Declarations GetDeclarations(IVsTextView view,
                                                     int line,
                                                     int col,
                                                     TokenInfo info,
                                                     ParseReason reason)
        {
            return null;
        }

        public override Methods GetMethods(int line, int col, string name)
        {
            return null;
        }

        public override string Goto(VSConstants.VSStd97CmdID cmd,
                                     IVsTextView textView,
                                     int line,
                                     int col,
                                     out TextSpan span)
        {
            span = new TextSpan();
            return null;
        }
    }

}
