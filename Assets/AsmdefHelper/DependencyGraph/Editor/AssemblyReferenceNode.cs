using System;
using GraphProcessor;

namespace AsmdefHelper.DependencyGraph.Editor
{
    [Serializable, NodeMenuItem("Assembly Node")]
    public class AssemblyReferenceNode : BaseNode
    {
        [Input(name = "Untitled Input", allowMultiple = true)]
        public string input;

        [Output(name = "Untitled Output", allowMultiple = true)]
        public string output;

        public override string name => "Untitled Assembly Node";
    }
}
