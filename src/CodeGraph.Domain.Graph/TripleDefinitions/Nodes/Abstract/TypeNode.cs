namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract
{
    public abstract class TypeNode : CodeNode, IEquatable<TypeNode>
    {
        protected TypeNode(string fullName, string name, string[] modifiers = null!)
            : base(fullName, name, modifiers)
        {
        }

        public bool Equals(TypeNode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Label == other.Label;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TypeNode)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Label);
        }

        public static bool operator ==(TypeNode? left, TypeNode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TypeNode? left, TypeNode? right)
        {
            return !Equals(left, right);
        }
    }
}