using System;
using GraphProcessor;

namespace AsmdefHelper.DependencyGraph.Editor
{
    [Serializable, NodeMenuItem("Assembly Node")]
    public class AssemblyReferenceNode : BaseNode
    {
        [Input(name = "Ref By")]
        public AssemblyReferenceNode input;

        [Output(name = "Ref To")]
        public AssemblyReferenceNode output;

        public override string name => "Untitled Assembly Node";
    }
}
