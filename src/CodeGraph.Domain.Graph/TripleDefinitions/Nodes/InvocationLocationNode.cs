using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes
{
    public class InvocationLocationNode : Node
    {
        public InvocationLocationNode(int location) : base(location.ToString("D10"), location.ToString("D10"))
        {
            Location = location;
        }

        public int Location { get; }

        public override string Label { get; } = "InvocationLocation";
    }
}