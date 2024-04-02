using CodeGraph.Domain.Graph.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.Nodes
{
    public class PackageNode : Node
    {
        public PackageNode(string fullName, string name, string version)
            : base(fullName, name)
        {
            Version = version;
            SetPrimaryKey();
        }

        public override string Label { get; } = "Package";

        public string Version { get; }

        public override string Set(string node)
        {
            return $"{base.Set(node)}, {node}.version = \"{Version}\"";
        }

        protected override void SetPrimaryKey()
        {
            Pk = $"{FullName}{Version}".GetHashCode().ToString();
        }
    }
}