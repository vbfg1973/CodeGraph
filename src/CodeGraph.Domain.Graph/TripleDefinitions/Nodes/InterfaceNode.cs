namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes
{
    public class InterfaceNode : TypeNode, IEquatable<InterfaceNode>
    {
        public InterfaceNode(string fullName, string name, string[] modifiers = null!)
            : base(fullName, name, modifiers)
        {
        }

        public override string Label { get; } = "Interface";

        public bool Equals(InterfaceNode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Label == other.Label;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((InterfaceNode)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Label);
        }

        public static bool operator ==(InterfaceNode? left, InterfaceNode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InterfaceNode? left, InterfaceNode? right)
        {
            return !Equals(left, right);
        }
    }
}