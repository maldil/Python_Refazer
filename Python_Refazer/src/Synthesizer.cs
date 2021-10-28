using System;
using Node = Microsoft.ProgramSynthesis.Wrangling.Tree.Node;
using static Microsoft.ProgramSynthesis.Transformation.Tree.Utils.Utils;
using Microsoft.ProgramSynthesis.Wrangling.Constraints;
using Microsoft.ProgramSynthesis.Transformation.Tree;

namespace Python_Refazer
{
    public class Synthesizer
    {
        public Synthesizer()
        {
        }

        public static Program Learn(Example<Node, Node>[] examples)
        {
            return Learner.Instance.Learn(examples);
        }
    }
}
