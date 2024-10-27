using GraphProcessor;

namespace AsmdefHelper.DependencyGraph.Editor
{
    public static class BaseNodeExtension
    {
        public static void SetPortLabel(this BaseNode node, string portName, string label)
        {
            var port = node.GetPort(portName, null);
            if (port != null)
                port.portData.displayName = label;
        }
    }
}
