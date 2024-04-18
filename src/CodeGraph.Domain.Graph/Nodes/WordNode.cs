using CodeGraph.Domain.Graph.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.Nodes
{
    public class WordNode : Node
    {
        public WordNode(string fullName, string name) : base(fullName, name)
        {
        }

        public override string Label => "Word";
    }
}