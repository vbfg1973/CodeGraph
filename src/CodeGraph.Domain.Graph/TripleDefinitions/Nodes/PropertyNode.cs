using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes
{
    public class PropertyNode : CodeNode
    {
        public PropertyNode(string fullName, string name, string returnType, string[] modifiers) : base(fullName, name,
            modifiers)
        {
            ReturnType = returnType;
        }

        public string ReturnType { get; }

        public override string Label { get; } = "Property";
    }
}