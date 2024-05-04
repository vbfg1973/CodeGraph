using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract
{
    public abstract class Triple : IEquatable<Triple>
    {
        protected Triple(Node nodeA, Node nodeB, Relationship relationship)
        {
            NodeA = nodeA;
            NodeB = nodeB;
            Relationship = relationship;
        }

        public Node NodeA { get; }

        public Node NodeB { get; }

        public Relationship Relationship { get; }

        public bool Equals(Triple? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return NodeA.Equals(other.NodeA) && NodeB.Equals(other.NodeB) && Relationship.Equals(other.Relationship);
        }

        public override string ToString()
        {
            return
                $"MERGE (a:{NodeA.Label}:Node {{ pk: \"{NodeA.Pk}\" }}) ON CREATE SET {NodeA.Set("a")} ON MATCH SET {NodeA.Set("a")} MERGE (b:{NodeB.Label}:Node {{ pk: \"{NodeB.Pk}\" }}) ON CREATE SET {NodeB.Set("b")} ON MATCH SET {NodeB.Set("b")} MERGE (a)-[:{Relationship.Type}]->(b);";
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Triple)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NodeA, NodeB, Relationship);
        }

        public static bool operator ==(Triple? left, Triple? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Triple? left, Triple? right)
        {
            return !Equals(left, right);
        }
    }
}