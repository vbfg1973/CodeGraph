using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes
{
    public class InvocationNode : Node
    {
        public InvocationNode(MethodNode parentMethodNode, MethodNode methodNode) : base(
            $"{parentMethodNode.FullName}->{methodNode.FullName}",
            $"{parentMethodNode.FullName}->{methodNode.FullName}")
        {
            Arguments = methodNode.Arguments;
            ReturnType = methodNode.ReturnType;
        }

        public InvocationNode() : base(string.Empty, string.Empty)
        {
        }

        public string Arguments { get; }

        public string ReturnType { get; }

        public override string Label { get; } = "Invocation";

        protected sealed override void SetPrimaryKey()
        {
            Pk = $"{FullName}{Arguments}{ReturnType}".GetHashCode().ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Label, Arguments, ReturnType);
        }
    }
}