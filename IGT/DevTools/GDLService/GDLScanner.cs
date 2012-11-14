using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using System.Dataflow;
using Microsoft.M;
using System.IO;
using Microsoft.VisualStudio.TextManager.Interop;

namespace TouchToolKit.GDLService
{
    //This is a stub class that isn't defined but is required to exist to comply with the MPF
    class GDLScanner : IScanner
    {
        /*
        private static string _language = File.ReadAllText("../../Resources/GDL.mg");

        private static Parser _parser;
        private static Parser Parser
        {
            get
            {
                if (_parser == null)
                {
                    using (var options = new CompilerOptions())
                    {
                        var sourceItems = new SourceItem[]{
                            new TextItem()
                            {
                                Name = "GdlGrammer", ContentType = TextItemType.MGrammar, Reader = new StringReader(_language)
                            }
                        };

                        options.AddSourceItems(sourceItems);

                        CompilationResults results = Compiler.Compile(options);
                        if (results.HasErrors)
                        {
                            //TODO: show meaningful error message
                            throw new Exception("Failed to compile GDL ....");
                        }
                        else
                        {
                            foreach (var parserFactory in results.ParserFactories)
                            {
                                //TODO: Why inside foreach loop!!!
                                _parser = parserFactory.Value.Create();
                            }
                        }
                    }
                }
                return _parser;
            }
        }

        private static TokenColor[] _colorTable;
        private static TokenColor[] ColorTable
        {
            get
            {
                if (_colorTable == null)
                {
                    //Set colorTable
                    
                    Dictionary<string, TokenColor> colorTable = new Dictionary<string, TokenColor>();
                    int length = Parser.Lexer.TokenTable.Length;

                    var commentColor = (TokenColor) 3;
                    var keywordColor = (TokenColor) 2;
                    var typesColor = (TokenColor) 4;
                    var miscColor = (TokenColor) 1;
                    var black = (TokenColor) 1;

                    _colorTable = new TokenColor[length];

                    colorTable.Add("ValidName", black);
                    colorTable.Add("ValidNum", black);
                    colorTable.Add(",", black);

                    foreach (var entry in Parser.Lexer.TokenTable)
                    {
                        if (colorTable.ContainsKey(entry.Name))
                        {
                            continue;
                        }

                        if (entry.InterleaveGroup == "Comment")
                        {
                            colorTable.Add(entry.Name, commentColor);
                            continue;
                        }
                        else if (entry.InterleaveGroup != null)
                        {
                            colorTable.Add(entry.Name, black);
                            continue;
                        }

                        if (entry.IsFinal)
                        {
                            colorTable.Add(entry.Name, keywordColor);
                            continue;
                        }

                        if (entry.Name.Contains("\""))
                        {
                            colorTable.Add(entry.Name, miscColor);
                        }
                        else
                        {
                            colorTable.Add(entry.Name, typesColor);
                        }
                    }

                    foreach (var entry in Parser.Lexer.TokenTable)
                    {
                        if (colorTable.ContainsKey(entry.Name))
                        {
                            _colorTable[entry.Tag] = colorTable[entry.Name];
                        }
                    }
                }
                return _colorTable;

            }
        }

        private string _line;
        private int _offset;

        private IVsTextLines _buffer;
        */
        public GDLScanner()
        {
            //_buffer = buffer;
        }

        // Summary:
        //     Scan the next token and fill in syntax coloring details about it in tokenInfo.
        //
        // Parameters:
        //   tokenInfo:
        //     Keeps information about token.
        //
        //   state:
        //     Keeps track of scanner state. In: state after last token. Out: state after
        //     current token.
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            /*
            string line = _line.Substring(_offset);
            var tokens = Parser.GetTokens(new TextReaderTextStream(new StringReader(line)),
                new SilentReporter(), 0, false);

            bool ret = tokens.MoveNext();
            
            var current = tokens.Current;
            var item = current[0];
            string desc = item.Description;
            string text = item.Text;
            int length = text.Length;
            uint color = (uint)item.Tag;

            if (tokenInfo == null)
                tokenInfo = new TokenInfo();

            tokenInfo.StartIndex = _offset;
            tokenInfo.EndIndex = length;
            _offset = length;

            if (color < 0 || color >= ColorTable.Length)
                tokenInfo.Color = TokenColor.Text;
            else
                tokenInfo.Color = ColorTable[color];

            tokenInfo.Type = ColorToType(tokenInfo.Color);
            tokenInfo.Token = item.Tag;
             */
            
            return false;
        }

        // Summary:
        //     Used to (re)initialize the scanner before scanning a small portion of text,
        //     such as single source line for syntax coloring purposes
        //
        // Parameters:
        //   source:
        //     The source text portion to be scanned
        //
        //   offset:
        //     The index of the first character to be scanned
        public void SetSource(string source, int offset)
        {
            /*
            _line = source;
            _offset = offset;
             */
        }
        /*
        private TokenType ColorToType(TokenColor color)
        {
            TokenType ret = TokenType.Text;
            switch (color)
            {
                case TokenColor.Comment:
                    ret = TokenType.Comment;
                    break;
                case TokenColor.Identifier:
                    ret = TokenType.Identifier;
                    break;
                case TokenColor.Keyword:
                    ret = TokenType.Keyword;
                    break;
                case TokenColor.Number:
                    ret = TokenType.Text;
                    break;
                case TokenColor.String:
                    ret = TokenType.Text;
                    break;
                case TokenColor.Text:
                    ret = TokenType.Text;
                    break;
            }

            return ret;
        }
         */
    }
}
