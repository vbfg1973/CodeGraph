using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes
{
    public class InvocationNode : Node
    {
        public InvocationNode(string fullName) : base(fullName, fullName)
        {
        }

        public override string Label { get; } = "Invocation";
    }
}