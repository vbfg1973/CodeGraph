using CodeGraph.Domain.Graph.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.Nodes
{
    public class ProjectNode : Node
    {
        public ProjectNode(string name)
            : this(name, name)
        {
        }

        public ProjectNode(string fullName, string name)
            : base(fullName, name)
        {
        }

        public override string Label { get; } = "Project";
    }
}