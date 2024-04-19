namespace CodeGraph.Domain.Graph.Nodes
{
    public class ClassNode : TypeNode
    {
        public ClassNode(string fullName, string name, string[] modifiers = null!)
            : base(fullName, name, modifiers)
        {
        }

        public override string Label { get; } = "Class";
    }
    
    public class RecordNode : TypeNode
    {
        public RecordNode(string fullName, string name, string[] modifiers = null!)
            : base(fullName, name, modifiers)
        {
        }

        public override string Label { get; } = "Record";
    }
}