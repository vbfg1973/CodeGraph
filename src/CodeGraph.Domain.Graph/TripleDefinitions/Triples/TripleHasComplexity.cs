using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleHasComplexity : Triple
    {
        public TripleHasComplexity(
            MethodNode methodA,
            CognitiveComplexityNode cognitiveComplexityNodeB)
            : base(methodA, cognitiveComplexityNodeB, new HasComplexityRelationship())
        {
        }
    }
}