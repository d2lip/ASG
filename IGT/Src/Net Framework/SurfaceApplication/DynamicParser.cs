using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.M;
using System.IO;
using System.Dataflow;
using MGraphXamlReader;

using TouchToolkit.GestureProcessor.Objects.LanguageTokens;

using System.Threading;
using System.Reflection;
using TouchToolkit.Framework;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.Framework.Utility;
using Microsoft.Build.Framework;

namespace SurfaceApplication
{
    public static class DynamicParser
    {
        private static String GDL = String.Empty;
        private static string ProjectDir = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        private static string CompiledGestureDefPath = ProjectDir + "\\Resources\\Custom\\";
        private static string RuleNamesFilePath = ProjectDir + "\\Resources\\rulenames.gx";
        private static  string gestureName;
        private static class GrammarTokens
        {
            public const string PrimitiveConditions = "//##pricon##";
            public const string ReturnTypes = "//##rettype##";
            public const string PrimitiveConditionSyntaxes = "//##priconsyntax##";
        }

        private static class FilePaths
        {
            public static string CustomPrimitiveConditions { get { return ProjectDir + "\\Resources\\PrimitiveConditions.pd"; } }
            public static string CustomPrimitiveConditionSyntax { get { return ProjectDir + "\\Resources\\PrimitiveConditionSyntax.pd"; } }
            public static string CustomReturnTypes { get { return ProjectDir + "\\Resources\\ReturnTypes.pd"; } }
            public static string LanguageDefinition { get { return ProjectDir + "\\Resources\\GDL.mg"; } }
        }

        public static string Parser(string _gdl, string _gestureName)
        {
            gestureName = _gestureName;
            List<GestureToken> gestures = new List<GestureToken>();
            // Load language grammar            


            // Update grammar for custom primitive conditions & rules
            GDL = File.ReadAllText(FilePaths.LanguageDefinition);


            // Build parser   
            Parser parser = null;
            try
            {
                parser = GetParser();
            }
            catch (Exception e)
            {
                return e.Message;

            }

            List<string> ruleNames = GetRuleNames(parser);

            // Build the dictionary with known types: all rule & return types            
            IEnumerable<Type> premitiveConditionTypes = SerializationHelper.GetAllPrimitiveConditionDataTypes();

            var map = new Dictionary<Identifier, Type>();

            // Adding all types that implements 'IRuleData'
            foreach (var type in premitiveConditionTypes)
            {
                map.Add(type.Name, type);
            }

            // Adding additional types
            map.Add("Return", typeof(ReturnToken));
            map.Add("Gesture", typeof(GestureToken));
            map.Add("Validate", typeof(ValidateToken));

            string data = _gdl;

            // Parse

            var list = parser.Parse<List<object>>(data, map);
            foreach (GestureToken g in list)
            {
                gestures.Add(g);
            }
            UpdateRuleNamesList(ruleNames);
            UpdateGestureDefFile(gestures);

            return "Gesture succesfully exported";

        }



        internal static Parser GetParser()
        {
            using (var options = new CompilerOptions())
            {
                var sourceItems = new SourceItem[]{

                    new TextItem()
                    {
                        Name = "GdlGrammar", ContentType = TextItemType.MGrammar, Reader = new StringReader(GDL)
                    }
                };
                
                options.AddSourceItems(sourceItems);
                
                CompilationResults results = null;
                results = Compiler.Compile(options);
                if (results.HasErrors)
                {
                    throw new Exception("Failed to compile GDL ...."
                                      + Environment.NewLine
                                      + results.Errors[0]);
                }
                else
                {
                    foreach (var parserFactory in results.ParserFactories)
                    {                 
                        return parserFactory.Value.Create();
                    }

                }


            }

            return null;
        }



        private static List<string> GetRuleNames(Parser parser)
        {
            List<string> ruleNames = new List<string>();
            foreach (var productionInfo in parser.ProductionTable)
            {
                // Get the rule name from production table
                if (productionInfo.Description.StartsWith("PrimitiveCondition ="))
                {
                    // Remove the prefix "rule =" from the description
                    string ruleName = productionInfo.Description.Substring(6).Trim();

                    ruleNames.Add(ruleName);
                }
            }

            return ruleNames;
        }


        private static void UpdateRuleNamesList(List<string> ruleNames)
        {
            string contentToWrite = string.Join("|", ruleNames);

            File.WriteAllText(RuleNamesFilePath, contentToWrite);
        }

        private static void UpdateGestureDefFile(List<GestureToken> gestures)
        {
            string jsonContent = SerializationHelper.Serialize(gestures);

            File.WriteAllText(CompiledGestureDefPath + gestureName+".gx", jsonContent);
        }



    }
}
