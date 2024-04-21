using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Relationships;
using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Graph.Triples
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