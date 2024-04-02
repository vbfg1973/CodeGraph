using CodeGraph.Domain.Graph.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.Nodes
{
    public class FileNode : Node
    {
        public FileNode(string fullName, string name)
            : base(fullName, name)
        {
        }

        public override string Label { get; } = "File";
    }
}