using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleHas : Triple
    {
        public TripleHas(
            TypeNode typeA,
            MethodNode methodNodeB)
            : base(typeA, methodNodeB, new HasRelationship())
        {
        }

        public TripleHas(
            TypeNode typeA,
            PropertyNode methodNodeB)
            : base(typeA, methodNodeB, new HasRelationship())
        {
        }
    }
}