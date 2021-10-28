using System;
using IronPython;
using IronPython.Compiler;
using IronPython.Compiler.Ast;
using IronPython.Hosting;
using Microsoft.ProgramSynthesis.Wrangling.Constraints;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Utils;
using Node = Microsoft.ProgramSynthesis.Wrangling.Tree.Node;

namespace Python_Refazer
{
    class MainClass
    {
        static void Main(string[] args)
        { 
            //var ast = ParseFile("/Users/malinda/test.py");
            var code1 = @"for pos in pronounce:
    count = count + pos";
            var code2 = @"np.sum(pronounce)";
            var ast1 = ParsePythonString(code1);
            var ast2 = ParsePythonString(code2);

            Node node1 =Translator.translateCode(ast1);
            Node node2 = Translator.translateCode(ast2);
            var example = Translator.createExample(node1, node2);
            Example<Node, Node>[] examples = new[] { example };
            var rule =  Synthesizer.Learn(examples);
            Assert.NotNull(rule);
            Console.WriteLine(rule);
            Node output = rule.Run(node1);
            Console.WriteLine(output.ToText());
        }

        public static PythonAst ParseFile(string path)
        {
            var py = Python.CreateEngine();
            var src = HostingHelpers.GetSourceUnit(py.CreateScriptSourceFromFile(path));
            return Parse(src, py);
        }

        public static PythonAst ParsePythonString(string code)
        {
            var py = Python.CreateEngine();
            var src = HostingHelpers.GetSourceUnit(py.CreateScriptSourceFromString(code));
            return Parse(src, py);
        }

        private static PythonAst Parse(SourceUnit src, ScriptEngine py)
        {
            var pylc = HostingHelpers.GetLanguageContext(py);
            var compilerContext = new CompilerContext(src, pylc.GetCompilerOptions(), ErrorSink.Default);
            var parser = Parser.CreateParser(compilerContext,(PythonOptions)pylc.Options);
            return parser.ParseFile(true);
        }
    }
}
