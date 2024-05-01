using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleHas : Triple
    {
        public TripleHas(
            TypeNode typeA,
            MethodNode methodB)
            : base(typeA, methodB, new HasRelationship())
        {
        }

        public TripleHas(
            TypeNode typeA,
            PropertyNode methodB)
            : base(typeA, methodB, new HasRelationship())
        {
        }
    }
}