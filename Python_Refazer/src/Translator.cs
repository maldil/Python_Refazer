    using System;
    using IronPython.Compiler.Ast;
    using Microsoft.ProgramSynthesis.Wrangling.Tree;
    using Microsoft.ProgramSynthesis.Transformation.Tree;
    using Microsoft.ProgramSynthesis.Wrangling.Constraints;
    using Node = Microsoft.ProgramSynthesis.Wrangling.Tree.Node;
    using IPythonNode = IronPython.Compiler.Ast.Node;
    using static Microsoft.ProgramSynthesis.Transformation.Tree.Utils.Utils;

    namespace Python_Refazer
    {
    public class Translator
    {
        public Translator()
        {

        }

        public static Node translateCode(PythonAst ast) {

            if (ast.Body == null)
                return null;
            else if (((SuiteStatement)ast.Body).Statements.Count == 0) {
                return null;
            }
            else if (((SuiteStatement)ast.Body).Statements.Count == 1)
            {
                var f_stms = ((SuiteStatement)ast.Body).Statements[0];

                var dummyBlock = StructNode.Create("DummyBlock", BuildValueAttribute(""), null);

                StructNode child = translateNode(f_stms, dummyBlock);

                dummyBlock.AddChild(child);
                return dummyBlock;
            }
                return null;
            }

        private static StructNode translateNode(IPythonNode f_stms, StructNode parent)
        {
            if (f_stms is ForStatement)
            {
                //var kind = f_stms.k
                var kind = f_stms.NodeName;
                var leafNode = StructNode.Create(kind, BuildValueAttribute(""), null, parent);
                leafNode.AddChild(translateNode(((ForStatement)f_stms).Left, leafNode));
                leafNode.AddChild(translateNode(((ForStatement)f_stms).List, leafNode));
                leafNode.AddChild(translateNode(((ForStatement)f_stms).Body, leafNode));

                return leafNode;
                //((ForStatement)f_stms).Body.
            }
            else if (f_stms is NameExpression)
            {
                return StructNode.Create(f_stms.NodeName, BuildValueAttribute(((NameExpression)f_stms).Name), null, parent);
            }

            else if (f_stms is SuiteStatement)
            {
                var result = StructNode.Create(f_stms.NodeName, BuildValueAttribute(""), null, parent);
                foreach (var statement in ((SuiteStatement)f_stms).Statements)
                {
                    result.AddChild(translateNode(statement, result));
                }
                return result;
            }
            else if (f_stms is CallExpression)
            {
                var result = StructNode.Create(f_stms.NodeName, BuildValueAttribute(""), null, parent);
                result.AddChild(translateNode(((CallExpression)f_stms).Target, result));
                for (var i = 0; i < ((CallExpression)f_stms).Args.Count; i++)
                {
                    var arg = ((CallExpression)f_stms).Args[i];
                    result.AddChild(translateNode(arg, result));
                }
                return result;
            }
            else if (f_stms is AssignmentStatement)
            {

                var result = StructNode.Create(f_stms.NodeName, BuildValueAttribute(""), null, parent);
                foreach (var expression in ((AssignmentStatement)f_stms).Left)
                {
                    result.AddChild(translateNode(expression, result));
                }
                result.AddChild(translateNode(((AssignmentStatement)f_stms).Right, result));
                return result;
            }
            else if (f_stms is BinaryExpression)
            {
                var result = StructNode.Create(f_stms.NodeName, BuildValueAttribute(((BinaryExpression)f_stms).Operator.ToString()), null, parent);
                result.AddChild(translateNode(((BinaryExpression)f_stms).Left, result));
                result.AddChild(translateNode(((BinaryExpression)f_stms).Right, result));
                return result;
            }

            else if (f_stms is MemberExpression)
            {

                var result = StructNode.Create(f_stms.NodeName, BuildValueAttribute(((MemberExpression)f_stms).Name), null, parent);
                result.AddChild(translateNode(((MemberExpression)f_stms).Target, result));
                return result;
            }
            else if (f_stms is Arg)
            {
                var result = StructNode.Create(f_stms.NodeName, BuildValueAttribute(""), null, parent);
                result.AddChild(translateNode(((Arg)f_stms).Expression, result));
                return result;
            }
            else if (f_stms is ExpressionStatement) {
                var result = StructNode.Create(f_stms.NodeName, BuildValueAttribute(""), null, parent);
                result.AddChild(translateNode(((ExpressionStatement)f_stms).Expression, result));
                return result;
            }

            else
            {
                Console.WriteLine(f_stms.ToString());
                throw new NotImplementedException();

            }      
        }

    public static Example<Node, Node> createExample(Node inputNode, Node outputNode)
        {
            return new Example<Node, Node>(inputNode, outputNode);
        }

        public static Program learn(Example<Node, Node>[] examples)
        {
            return Learner.Instance.Learn(examples);
        }

        public static void storeNode(Node node, string fileName)
        {

        }

        public static void loadNode(string fileName)
        {
      
        }

        public static Program Learn(Example<Node, Node>[] examples)
        {
            return Learner.Instance.Learn(examples);
        }
    }
    }
