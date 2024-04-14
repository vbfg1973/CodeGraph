using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Relationships;
using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Graph.Triples
{
    public class TripleUsesWord : Triple
    {
        public TripleUsesWord(TypeNode nodeA, WordNode nodeB) : base(nodeA, nodeB, new HasWordInNameRelationship())
        {
        }
        
        public TripleUsesWord(MethodNode nodeA, WordNode nodeB) : base(nodeA, nodeB, new HasWordInNameRelationship())
        {
        }
    }
}