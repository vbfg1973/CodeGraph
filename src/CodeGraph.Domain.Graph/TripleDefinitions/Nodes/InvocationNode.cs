using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes
{
    public class InvocationNode : Node
    {
        public InvocationNode(MethodNode parentMethodNode, MethodNode methodNode, int location) : base(
            $"{parentMethodNode.FullName}->{methodNode.FullName}",
            $"{parentMethodNode.FullName}->{methodNode.FullName}")
        {
            Location = location;
            Arguments = methodNode.Arguments;
            ReturnType = methodNode.ReturnType;
        }

        public InvocationNode() : base(string.Empty, string.Empty)
        {
        }

        public int Location { get; }

        private string Arguments { get; }

        private string ReturnType { get; }

        public override string Label { get; } = "Invocation";

        public override string Set(string node)
        {
            return $"{base.Set(node)}, {node}.location = \"{Location}\"";
        }

        protected sealed override void SetPrimaryKey()
        {
            Pk = $"{FullName}{Arguments}{ReturnType}{Location}".GetHashCode().ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Label, Arguments, ReturnType, Location);
        }
    }
}