using System;
using GraphProcessor;

namespace AsmdefHelper.DependencyGraph.Editor
{
    [Serializable, NodeMenuItem("Assembly Node")]
    public class AssemblyReferenceNode : BaseNode
    {
        [Input(name = "Ref By", allowMultiple = true)]
        public AssemblyReferenceNode input;

        [Output(name = "Ref To", allowMultiple = true)]
        public AssemblyReferenceNode output;

        public override string name => "Untitled Assembly Node";
    }
}
