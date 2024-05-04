using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleInvocationOf : Triple
    {
        public TripleInvocationOf(
            InvocationNode invocationA,
            MethodNode methodB)
            : base(invocationA, methodB, new InvocationOfRelationship())
        {
        }
    }
}