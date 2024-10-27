using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace AsmdefHelper.DependencyGraph.Editor
{
    public class AssemblyReferenceGraphView : BaseGraphView
    {
        private const string InputSocketId = "input";
        private const string OutputSocketId = "output";

        public AssemblyReferenceGraphView(EditorWindow window, BaseGraph graph)
            : base(window) => InitializeGraph(graph);

        private void InitializeGraph(BaseGraph graph)
        {
            var assemblies = CompilationPipeline.GetAssemblies();
            var asmdefNodeDict = CreateNodes(graph, assemblies);
            CreateDependencies(graph, assemblies, asmdefNodeDict);
            LayoutNodes(asmdefNodeDict.Values.ToArray());
        }

        private Dictionary<string, AssemblyReferenceNode> CreateNodes(
            BaseGraph graph,
            Assembly[] assemblies
        )
        {
            var asmdefNodeDict = new Dictionary<string, AssemblyReferenceNode>();
            foreach (var asm in assemblies)
            {
                var node = BaseNode.CreateFromType<AssemblyReferenceNode>(Vector2.zero);
                graph.AddNode(node);

                node.SetCustomName(asm.name);

                var sourcesCount = assemblies.Count(a => a.assemblyReferences.Contains(asm));
                var destinationsCount = asm.assemblyReferences.Length;

                node.SetPortLabel(InputSocketId, $"RefBy({sourcesCount})");
                node.SetPortLabel(OutputSocketId, $"RefTo({destinationsCount})");

                asmdefNodeDict[asm.name] = node;
            }
            return asmdefNodeDict;
        }

        private void CreateDependencies(
            BaseGraph graph,
            Assembly[] assemblies,
            Dictionary<string, AssemblyReferenceNode> asmdefNodeDict
        )
        {
            foreach (var asm in assemblies)
            {
                if (!asmdefNodeDict.TryGetValue(asm.name, out var fromNode))
                    continue;

                foreach (var reference in asm.assemblyReferences)
                {
                    if (!asmdefNodeDict.TryGetValue(reference.name, out var toNode))
                        continue;

                    var inputPort = toNode.GetPort(InputSocketId, null);
                    var outputPort = fromNode.GetPort(OutputSocketId, null);
                    if (inputPort != null && outputPort != null)
                        graph.Connect(inputPort, outputPort);
                }
            }
        }

        private void LayoutNodes(AssemblyReferenceNode[] nodes)
        {
            var positions = CalculateHierarchicalPositions(nodes);
            for (var i = 0; i < nodes.Length; i++)
                nodes[i].position = new Rect(positions[i], new Vector2(100, 100));
        }

        private List<Vector2> CalculateHierarchicalPositions(AssemblyReferenceNode[] nodes)
        {
            var positions = new List<Vector2>();
            var offset = new Vector2(200, 200);
            for (var i = 0; i < nodes.Length; i++)
                positions.Add(offset * i);
            return positions;
        }
    }
}
