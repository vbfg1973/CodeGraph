namespace CodeGraph.Domain.Graph.Nodes.Abstract
{
    public abstract class CodeNode : Node
    {
        protected CodeNode(string fullName, string name, string[] modifiers) : base(fullName, name)
        {
            Modifiers = modifiers == null ? "" : string.Join(", ", modifiers);
        }

        public string Modifiers { get; }

        public override string Set(string node)
        {
            return
                $"{base.Set(node)}{(string.IsNullOrEmpty(Modifiers) ? "" : $", {node}.modifiers = \"{Modifiers}\"")}";
        }
    }
}