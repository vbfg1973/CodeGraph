using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes
{
    public class WordRootNode : Node, IEquatable<WordRootNode>
    {
        public WordRootNode(string fullName, string name) : base(fullName, name)
        {
        }

        public override string Label => "WordRoot";

        public bool Equals(WordRootNode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Label == other.Label;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((WordRootNode)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Label);
        }

        public static bool operator ==(WordRootNode? left, WordRootNode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WordRootNode? left, WordRootNode? right)
        {
            return !Equals(left, right);
        }
    }
}