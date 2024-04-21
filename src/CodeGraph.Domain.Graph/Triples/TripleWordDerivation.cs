using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Relationships;
using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Graph.Triples
{
    public class TripleWordDerivation : Triple
    {
        public TripleWordDerivation(WordNode nodeA, WordRootNode nodeB) : base(nodeA, nodeB, new WordRootRelationship())
        {
        }
    }
}