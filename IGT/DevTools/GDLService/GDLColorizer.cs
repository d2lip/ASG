using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Dataflow;
using Microsoft.M;
using System.IO;
using GestureDefinitionLanguage;

namespace TouchToolKit.GDLService
{
    class GDLColorizer : Colorizer
    {
        private IVsTextLines _buffer;

        private Parser _parser;
        private Parser Parser
        {
            get
            {
                if (_parser == null)
                {
                    GDL gdl = new GDL();

                    _parser = gdl.Parser;
                }
                return _parser;
            }
        }

        private TokenColor[] _colorTable;
        private TokenColor[] ColorTable
        {
            get
            {
                if (_colorTable == null)
                {
                    //Set colorTable

                    Dictionary<string, TokenColor> colorDict = new Dictionary<string, TokenColor>();
                    int length = Parser.Lexer.TokenTable.Length;

                    var commentColor = (TokenColor)3;
                    var keywordColor = (TokenColor)2;
                    var typesColor = (TokenColor)4;
                    var miscColor = (TokenColor)5;
                    var black = (TokenColor)1;

                    _colorTable = new TokenColor[length];

                    colorDict.Add("ValidName", black);
                    colorDict.Add("ValidNum", black);
                    colorDict.Add(",", black);

                    foreach (var entry in Parser.Lexer.TokenTable)
                    {
                        if (colorDict.ContainsKey(entry.Name))
                        {
                            continue;
                        }

                        if (entry.InterleaveGroup == "Comment")
                        {
                            colorDict.Add(entry.Name, commentColor);
                            continue;
                        }

                        if (entry.IsFinal)
                        {
                            colorDict.Add(entry.Name, keywordColor);
                            continue;
                        }

                        if (entry.Name.Length < 2)
                        {
                            colorDict.Add(entry.Name, black);
                            continue;
                        }

                        if (entry.Name.Contains("\""))
                        {
                            colorDict.Add(entry.Name, typesColor);
                        }
                        else
                        {
                            colorDict.Add(entry.Name, miscColor);
                        }
                    }

                    foreach (var entry in Parser.Lexer.TokenTable)
                    {
                        if (colorDict.ContainsKey(entry.Name))
                        {
                            _colorTable[entry.Tag] = colorDict[entry.Name];
                        }
                    }
                }
                return _colorTable;
            }
        }

        public GDLColorizer(LanguageService svc, IVsTextLines buffer, IScanner scanner)
            : base(svc, buffer, scanner)
        {
            _buffer = buffer;
        }

        public override int ColorizeLine(int line, int length, IntPtr ptr, int state, uint[] attrs)
        {
            string text = "";
            _buffer.GetLineText(line, 0, line, length, out text);
            var tokens = Parser.GetTokens(new TextReaderTextStream(new StringReader(text)),
                new SilentReporter(), 0, false);

            if (attrs == null)
            {
                attrs = new uint[text.Length];
            }

            int i = 0;

            while (tokens.MoveNext())
            {
                var token = tokens.Current[tokens.Current.Count - 1];

                int tag = token.Tag;
                if (tag < 0 || tag >= ColorTable.Length)
                {
                    continue;
                }
                var attr = ColorTable[tag];
                int tokenSize = token.Text.Length;
                for (int j = 0; j < tokenSize; j++)
                {
                    attrs[i] = (uint)attr;
                    i++;
                }
            }

            return 0;
        }
    }
}
