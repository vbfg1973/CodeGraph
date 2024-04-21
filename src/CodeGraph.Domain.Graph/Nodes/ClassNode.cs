namespace CodeGraph.Domain.Graph.Nodes
{
    public class ClassNode : TypeNode, IEquatable<ClassNode>
    {
        public ClassNode(string fullName, string name, string[] modifiers = null!)
            : base(fullName, name, modifiers)
        {
        }

        public override string Label { get; } = "Class";

        public bool Equals(ClassNode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Label == other.Label;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((ClassNode)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Label);
        }

        public static bool operator ==(ClassNode? left, ClassNode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ClassNode? left, ClassNode? right)
        {
            return !Equals(left, right);
        }
    }
}