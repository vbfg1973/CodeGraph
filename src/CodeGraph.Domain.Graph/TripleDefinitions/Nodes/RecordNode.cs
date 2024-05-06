using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes
{
    public class RecordNode : TypeNode, IEquatable<RecordNode>
    {
        public RecordNode(string fullName, string name, string[] modifiers = null!)
            : base(fullName, name, modifiers)
        {
        }

        public RecordNode() : base(string.Empty, string.Empty)
        {
        }

        public override string Label { get; } = "Record";

        public bool Equals(RecordNode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Label == other.Label;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RecordNode)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Label);
        }

        public static bool operator ==(RecordNode? left, RecordNode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RecordNode? left, RecordNode? right)
        {
            return !Equals(left, right);
        }
    }
}