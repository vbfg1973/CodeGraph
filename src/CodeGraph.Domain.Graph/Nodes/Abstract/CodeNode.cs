namespace CodeGraph.Domain.Graph.Nodes.Abstract
{
    public abstract class CodeNode : Node, IEquatable<CodeNode>
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

        public bool Equals(CodeNode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Modifiers == other.Modifiers;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((CodeNode)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Modifiers);
        }

        public static bool operator ==(CodeNode? left, CodeNode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CodeNode? left, CodeNode? right)
        {
            return !Equals(left, right);
        }
    }
}