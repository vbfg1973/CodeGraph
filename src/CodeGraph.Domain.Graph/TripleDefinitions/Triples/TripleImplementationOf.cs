using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleImplementationOf : Triple
    {
        public TripleImplementationOf(
            MethodNode methodA,
            MethodNode methodNodeB)
            : base(methodA, methodNodeB, new ImplementsRelationship())
        {
        }
    }
}