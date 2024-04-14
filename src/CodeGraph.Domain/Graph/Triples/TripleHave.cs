using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Nodes.Abstract;
using CodeGraph.Domain.Graph.Relationships;
using CodeGraph.Domain.Graph.Relationships.Abstract;
using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Graph.Triples
{
    public class TripleHave : Triple
    {
        public TripleHave(
            TypeNode typeA,
            MethodNode methodB)
            : base(typeA, methodB, new HasRelationship())
        {
        }
    }
}