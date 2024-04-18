namespace CodeGraph.Domain.Graph.Nodes
{
    public class InterfaceNode : TypeNode
    {
        public InterfaceNode(string fullName, string name, string[] modifiers = null!)
            : base(fullName, name, modifiers)
        {
        }

        public override string Label { get; } = "Interface";
    }
}