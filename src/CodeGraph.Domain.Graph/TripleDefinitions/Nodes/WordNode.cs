using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes
{
    public class WordNode : Node, IEquatable<WordNode>
    {
        public WordNode(string fullName, string name) : base(fullName, name)
        {
        }

        public WordNode() : base(string.Empty, string.Empty)
        {
        }

        public override string Label => "Word";

        public bool Equals(WordNode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Label == other.Label;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((WordNode)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Label);
        }

        public static bool operator ==(WordNode? left, WordNode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WordNode? left, WordNode? right)
        {
            return !Equals(left, right);
        }
    }
}