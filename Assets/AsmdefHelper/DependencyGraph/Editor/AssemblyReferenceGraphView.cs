using System;
using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEditor;
using UnityEngine;

namespace AsmdefHelper.DependencyGraph.Editor
{
    public class AssemblyReferenceGraphView : BaseGraphView
    {
        private const float RepulsionStrength = 500f;
        private const float AttractionStrength = 0.1f;
        private const float DesiredDistance = 200f;
        private const int Iterations = 100;

        public AssemblyReferenceGraphView(EditorWindow window, BaseGraph graph)
            : base(window) => InitializeGraph(graph);

        private void InitializeGraph(BaseGraph graph)
        {
            var nodes = CreateNodes(graph);
            CreateEdges(graph, nodes);
            ApplyForceDirectedLayout(nodes, graph);
        }

        private List<BaseNode> CreateNodes(BaseGraph graph)
        {
            var nodes = Enumerable
                .Range(0, 10)
                .Select(_ => BaseNode.CreateFromType<AssemblyReferenceNode>(Vector2.zero))
                .ToList();

            foreach (var node in nodes)
                graph.AddNode(node);

            return nodes.Cast<BaseNode>().ToList();
        }

        private void CreateEdges(BaseGraph graph, List<BaseNode> nodes)
        {
            var edges = new List<(int from, int to)> { (0, 1), (0, 2), (1, 3), (0, 4), (4, 5) };
            foreach (var (from, to) in edges)
            {
                var outputPort = nodes[from].GetPort("output", null);
                var inputPort = nodes[to].GetPort("input", null);
                if (outputPort != null && inputPort != null)
                    graph.Connect(inputPort, outputPort);
            }
        }

        private void ApplyForceDirectedLayout(List<BaseNode> nodes, BaseGraph graph)
        {
            var positions = CalculateForceDirectedPositions(nodes, graph);
            for (var i = 0; i < nodes.Count; i++)
                nodes[i].position = new Rect(positions[i], new Vector2(100, 100));
        }

        private List<Vector2> CalculateForceDirectedPositions(List<BaseNode> nodes, BaseGraph graph)
        {
            var nodePositions = InitializeNodePositions(nodes);
            var forces = new Vector2[nodes.Count];

            for (var i = 0; i < Iterations; i++)
            {
                Array.Clear(forces, 0, nodes.Count);

                CalculateRepulsiveForces(nodes, nodePositions, forces);
                CalculateAttractiveForces(graph, nodes, nodePositions, forces);

                UpdateNodePositions(nodePositions, forces);
            }

            return nodePositions;
        }

        private List<Vector2> InitializeNodePositions(List<BaseNode> nodes)
        {
            return nodes
                .Select(
                    _ =>
                        new Vector2(
                            UnityEngine.Random.Range(0, 500),
                            UnityEngine.Random.Range(0, 500)
                        )
                )
                .ToList();
        }

        private void CalculateRepulsiveForces(
            List<BaseNode> nodes,
            List<Vector2> nodePositions,
            Vector2[] forces
        )
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                for (var j = i + 1; j < nodes.Count; j++)
                {
                    var direction = nodePositions[i] - nodePositions[j];
                    var distance = direction.magnitude;
                    if (distance == 0)
                        distance = 0.1f;

                    var repulsion =
                        RepulsionStrength * direction.normalized / (distance * distance);
                    forces[i] += repulsion;
                    forces[j] -= repulsion;
                }
            }
        }

        private void CalculateAttractiveForces(
            BaseGraph graph,
            List<BaseNode> nodes,
            List<Vector2> nodePositions,
            Vector2[] forces
        )
        {
            foreach (var edge in graph.edges)
            {
                var i = nodes.IndexOf(edge.outputNode);
                var j = nodes.IndexOf(edge.inputNode);
                if (i == -1 || j == -1)
                    continue;

                var direction = nodePositions[j] - nodePositions[i];
                var distance = direction.magnitude;
                var attraction =
                    AttractionStrength * (distance - DesiredDistance) * direction.normalized;

                forces[i] += attraction;
                forces[j] -= attraction;
            }
        }

        private void UpdateNodePositions(List<Vector2> nodePositions, Vector2[] forces)
        {
            for (var i = 0; i < nodePositions.Count; i++)
                nodePositions[i] += forces[i] * Time.deltaTime;
        }
    }
}
