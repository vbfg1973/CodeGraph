using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes
{
    public class CognitiveComplexityNode : Node
    {
        public CognitiveComplexityNode(int complexityScore) : base(complexityScore.ToString("D10"), complexityScore.ToString("D10"))
        {
            ComplexityScore = complexityScore;
        }

        public CognitiveComplexityNode() : base(string.Empty, string.Empty)
        {
        }

        public int ComplexityScore { get; }

        public override string Label { get; } = "CognitiveComplexity";
    }
}