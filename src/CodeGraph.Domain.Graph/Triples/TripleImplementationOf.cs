using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Relationships;
using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Graph.Triples
{
    public class TripleImplementationOf : Triple
    {
        public TripleImplementationOf(
            MethodNode methodA,
            MethodNode methodB)
            : base(methodA, methodB, new ImplementsRelationship())
        {
        }
    }
}