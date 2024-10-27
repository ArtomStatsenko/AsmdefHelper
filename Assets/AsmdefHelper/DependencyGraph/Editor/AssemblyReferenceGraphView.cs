using GraphProcessor;
using UnityEditor;
using UnityEngine;

namespace AsmdefHelper.DependencyGraph.Editor
{
    public class AssemblyReferenceGraphView : BaseGraphView
    {
        public AssemblyReferenceGraphView(EditorWindow window, BaseGraph graph)
            : base(window) => InitializeGraph(graph);

        private void InitializeGraph(BaseGraph graph)
        {
            var node1 = BaseNode.CreateFromType<AssemblyReferenceNode>(new Vector2(100, 100));
            var node2 = BaseNode.CreateFromType<AssemblyReferenceNode>(new Vector2(400, 100));

            node1.SetCustomName("Custom Node Name 1");
            node2.SetCustomName("Custom Node Name 2");

            graph.AddNode(node1);
            graph.AddNode(node2);

            var outputPort = node1.GetPort("output", null);
            var inputPort = node2.GetPort("input", null);

            if (outputPort == null || inputPort == null)
            {
                Debug.LogError(
                    "One of the ports is null. Check initialization of input and output ports."
                );
                return;
            }

            graph.Connect(inputPort, outputPort);
        }
    }
}
