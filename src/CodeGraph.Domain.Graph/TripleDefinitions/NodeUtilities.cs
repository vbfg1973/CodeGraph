using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions
{
    public static class NodeUtilities
    {
        public static IEnumerable<string> NodeLabels()
        {
            return GetInstances().Select(x => x.Label);
        }

        private static IEnumerable<Node> GetInstances()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where
                (
                    x => (x is { BaseType: not null, IsAbstract: false }
                          && x.BaseType == typeof(Node)) || x.BaseType == typeof(TypeNode)
                )
                .Select(t => Activator.CreateInstance(t) as Node)!;
        }
    }
}