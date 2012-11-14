using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.M;
using System.IO;
using System.Dataflow;
using MGraphXamlReader;
using System.Runtime.Serialization.Json;
using TouchToolkit.GestureProcessor.Objects.LanguageTokens;

using System.Threading;
using System.Reflection;
using TouchToolkit.Framework;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using TouchToolkit.Framework.Utility;


namespace LanguageParser
{
    class Program
    {
        private static string RuleNamesFilePath { get; set; }
        private static string CompiledGestureDefPath { get; set; }
        private static string GestureDefSourcePath { get; set; }
        private static string LanguageDefPath { get; set; }
        private static string _language = string.Empty;

        // i.e. ProjectOutput = bin\Debug\
        private static string OutDir { get; set; }
        private static string ProjectDir = string.Empty;
        private static string ProjectName = string.Empty;
        private static string FrameworkType = string.Empty;

        private class FrameworkTypes
        {
            public const string Silverlight = "SL";
            public const string DotNetFullFramework = "NET"; // Default
        }

        private class GrammarTokens
        {
            public const string PrimitiveConditions = "//##pricon##";
            public const string ReturnTypes = "//##rettype##";
            public const string PrimitiveConditionSyntaxes = "//##priconsyntax##";
        }

        private class FilePaths
        {
            public static string CustomPrimitiveConditions { get { return ProjectDir + "TouchToolkit/Extensions/Language Syntax/PrimitiveConditions.pd"; } }
            public static string CustomPrimitiveConditionSyntax { get { return ProjectDir + "TouchToolkit/Extensions/Language Syntax/PrimitiveConditionSyntax.pd"; } }
            public static string CustomReturnTypes { get { return ProjectDir + "TouchToolkit/Extensions/Language Syntax/ReturnTypes.pd"; } }
            public static string LanguageDefinition { get { return ProjectDir + "TouchToolkit/Lib/GDL.mg"; } }
        }

        static void Main(string[] args)
        {
            //TODO: Refactor and split into smaller methods

            try
            {
                if (args.Length == 3 || args.Length == 4)
                {
                    /*
                     * Used by the VS Post Build Event
                     * 
                     * CD ..\..\Framework
                     * LanguageParser.exe "..\\" $(OutDir) $(ProjectName)
                     * 
                     */
                    ProjectDir = args[0];
                    OutDir = args[1];
                    ProjectName = args[2];

                    if (args.Length == 4)
                        FrameworkType = args[3];

                    CompiledGestureDefPath = ProjectDir + @"bin/gestures.gx";
                    RuleNamesFilePath = ProjectDir + @"bin/rulenames.gx";
                    GestureDefSourcePath = ProjectDir + @"/TouchToolkit/Gesture Definitions";
                    LanguageDefPath = ProjectDir + @"/TouchToolkit/Lib/GDL.mg";
                }
                else if (args.Length == 5)
                {
                    /*
                     * LanguageParser.exe [0: framework-type] [1:gesture-def-path] [2:name-file] [3:gesture-source] [4:lang-def]
                     */

                    var root = @"..\..\..\..\";
                    FrameworkType = args[0];
                    CompiledGestureDefPath = root + args[1];
                    RuleNamesFilePath = root + args[2];
                    GestureDefSourcePath = root + args[3];
                    LanguageDefPath = root + args[4];

                }
                else if (args.Length == 0)
                {
                    WriteMessage("No args provided. Using default paths!");

                    ProjectDir = @"..\..\..\LanguageParser.TestApp\";
                    OutDir = @"bin\Debug\";
                    ProjectName = "LanguageParser.TestApp";

                    //FrameworkType = FrameworkTypes.Silverlight;

                    DirectoryInfo projDir = new DirectoryInfo(ProjectDir);
                    if (projDir.Exists)
                        ProjectName = projDir.Name;

                    CompiledGestureDefPath = ProjectDir + @"bin/gestures.gx";
                    RuleNamesFilePath = ProjectDir + @"bin/rulenames.gx";
                    GestureDefSourcePath = ProjectDir + @"TouchToolkit/Gesture Definitions";
                    LanguageDefPath = ProjectDir + @"TouchToolkit/Lib/GDL.mg";

                }
                else
                {
                    WriteMessage("Invalid args!");
                    int i = 0;
                    foreach (var item in args)
                        WriteMessage(string.Format("{0}::{1}" + Environment.NewLine, i++, item));

                    return;
                }

                // Print all parameters
                foreach (var item in args)
                    WriteMessage(item);

                DirectoryInfo gestureDefinitions = new DirectoryInfo(GestureDefSourcePath);

                if (!gestureDefinitions.Exists)
                {
                    WriteMessage("Could not find the \"Gesture Definitions\" folder.");
                    return;
                }
                else
                {
                    List<GestureToken> gestures = new List<GestureToken>();

                    int gestureDefCount = gestureDefinitions.GetFiles("*.g").Count();
                    if (gestureDefCount > 0)
                    {
                        // TODO: Temporary work-around to get exexuting assembly 
                        //if (FrameworkType != FrameworkTypes.Silverlight)
                        GestureFramework.HostAssembly = GetHostAssembly();

                        // Load language grammar
                        WriteMessage("Loading language grammar...");
                        _language = File.ReadAllText(LanguageDefPath);

                        // Update grammar for custom primitive conditions & rules
                        _language = AddUserDefinedSyntax(_language);

                        // Build parser
                        WriteMessage("Building parser from language...");
                        Parser parser = GetParser();
                        List<string> ruleNames = GetRuleNames(parser);

                        // Build the dictionary with known types: all rule & return types
                        WriteMessage("Building list of rule types...");
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

                        WriteMessage("Compiling gestures (*.g files) ...");
                        DirectoryInfo gestureFolder = new DirectoryInfo(GestureDefSourcePath);
                        FileInfo[] files = gestureFolder.GetFiles("*.g");
                        foreach (var file in files)
                        {
                            string data = File.ReadAllText(file.FullName);

                            // Parse
                            var list = parser.Parse<List<object>>(data, map);
                            foreach (GestureToken g in list)
                            {
                                if (gestures.Where(existingGesture => existingGesture.Name == g.Name).Count() > 0)
                                {
                                    WriteMessage(string.Format("Duplicate definition found for gesture: \"{0}\". Skipping the definition defined in \"{1}\"", g.Name, file.FullName));
                                }
                                else
                                {
                                    gestures.Add(g);
                                }
                            }
                        }

                        UpdateRuleNamesList(ruleNames);
                    }


                    WriteMessage(string.Format("{0} gesture definitions found. Updating deployment folder...", gestureDefCount));
                    UpdateGestureDefFile(gestures);
                    WriteMessage("Done.");

#if DEBUG
                    if (args.Count() == 0) // if its in local test mode
                    {
                        // Let users read the console message
                        Thread.Sleep(1000);
                    }
#endif
                }
            }
            catch (Exception ex)
            {
                WriteMessage(ex.Message);
            }
        }

        private  static string AddUserDefinedSyntax(string language)
        {
            string priCons = string.Empty;
            string syntax = string.Empty;
            string returnTypes = string.Empty;

            if (File.Exists(FilePaths.CustomPrimitiveConditions) && File.Exists(FilePaths.CustomPrimitiveConditionSyntax))
            {
                priCons = File.ReadAllText(FilePaths.CustomPrimitiveConditions);
                syntax = File.ReadAllText(FilePaths.CustomPrimitiveConditionSyntax);
            }

            language = language.Replace(GrammarTokens.PrimitiveConditions, priCons);
            language = language.Replace(GrammarTokens.PrimitiveConditionSyntaxes, syntax);

            if (File.Exists(FilePaths.CustomReturnTypes))
            {
                returnTypes = File.ReadAllText(FilePaths.CustomReturnTypes);
            }

            return language.Replace(GrammarTokens.ReturnTypes, returnTypes);
        }

        private static Assembly GetHostAssembly()
        {
            Assembly hostAssembly = null;
            FileInfo appAssemblyFile = null;
            DirectoryInfo outDirInfo = new DirectoryInfo(ProjectDir + OutDir);
            var assemblies = outDirInfo.GetFiles("*.dll");
            foreach (var assembly in assemblies)
            {
                if (assembly.Name.Contains(ProjectName))
                {
                    appAssemblyFile = assembly;
                    break;
                }
            }

            if (appAssemblyFile == null)
            {
                var exes = outDirInfo.GetFiles("*.exe");
                foreach (var exe in exes)
                {
                    if (exe.Name.Contains(ProjectName))
                    {
                        appAssemblyFile = exe;
                        break;
                    }
                }
            }

            if (appAssemblyFile != null)
            {
                hostAssembly = Assembly.LoadFile(appAssemblyFile.FullName);
            }

            return hostAssembly;
        }

        private static Parser GetParser()
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
                        return parserFactory.Value.Create();
                    }
                }
            }

            return null;
        }

        private static void WriteMessage(string msg)
        {
            Console.WriteLine(msg);
        }

        #region Get the list of rules from assembly

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




        #endregion

        #region Update resources for executables
        private static void UpdateRuleNamesList(List<string> ruleNames)
        {
            string contentToWrite = string.Join("|", ruleNames);

            File.WriteAllText(RuleNamesFilePath, contentToWrite);
        }

        private static void UpdateGestureDefFile(List<GestureToken> gestures)
        {
            string jsonContent = SerializationHelper.Serialize(gestures);

            File.WriteAllText(CompiledGestureDefPath, jsonContent);
        }
        #endregion
    }
}
