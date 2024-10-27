using GraphProcessor;
using UnityEditor;
using UnityEngine;

namespace AsmdefHelper.DependencyGraph.Editor
{
    public class AssemblyReferenceGraphWindow : BaseGraphWindow
    {
        [MenuItem("Window/Simple Graph")]
        public static BaseGraphWindow OpenWithTmpGraph()
        {
            var window = CreateWindow<AssemblyReferenceGraphWindow>();

            var tmpGraph = CreateInstance<BaseGraph>();
            tmpGraph.hideFlags = HideFlags.HideAndDontSave;
            window.InitializeGraph(tmpGraph);

            window.Show();
            return window;
        }

        protected override void InitializeWindow(BaseGraph initGraph)
        {
            titleContent = new GUIContent("Assembly Graph");
            graphView ??= new AssemblyReferenceGraphView(this, initGraph);
            rootView.Add(graphView);
        }
    }
}
