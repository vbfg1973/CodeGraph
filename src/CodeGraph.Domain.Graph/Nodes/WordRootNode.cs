using CodeGraph.Domain.Graph.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.Nodes
{
    public class WordRootNode : Node
    {
        public WordRootNode(string fullName, string name) : base(fullName, name)
        {
        }

        public override string Label => "WordRoot";
    }
}