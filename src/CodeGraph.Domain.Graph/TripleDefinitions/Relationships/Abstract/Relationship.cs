namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract
{
    public abstract class Relationship : IEquatable<Relationship>
    {
        public abstract string Type { get; }

        public bool Equals(Relationship? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Relationship)obj);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public static bool operator ==(Relationship? left, Relationship? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Relationship? left, Relationship? right)
        {
            return !Equals(left, right);
        }
    }
}