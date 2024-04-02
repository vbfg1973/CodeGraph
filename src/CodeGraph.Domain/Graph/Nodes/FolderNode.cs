using CodeGraph.Domain.Graph.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.Nodes
{
    public class FolderNode : Node
    {
        public FolderNode(string fullName, string name)
            : base(fullName, name)
        {
        }

        public override string Label { get; } = "Folder";
    }
}