using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dataflow;
using Microsoft.M;
using System.IO;
using System.Reflection;

namespace GestureDefinitionLanguage
{
    public class GDL
    {
        public GDL()
        {
            var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("GestureDefinitionLanguage.Resources.GDL.mg"));

            Language = stream.ReadToEnd();
            Parser = GetParser();
            
        }
        public string Language;
        public Parser Parser;

        private Parser _parser;
        private Parser GetParser()
        {
                if (_parser == null)
                {
                    using (var options = new CompilerOptions())
                    {
                        var sourceItems = new SourceItem[]{
                            new TextItem()
                            {

                                Name = "GdlGrammer", ContentType = TextItemType.MGrammar, Reader = new StringReader(Language)
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
}