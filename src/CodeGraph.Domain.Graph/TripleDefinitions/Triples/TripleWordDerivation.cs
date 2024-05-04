using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleWordDerivation : Triple
    {
        public TripleWordDerivation(WordNode nodeA, WordRootNode nodeB) : base(nodeA, nodeB, new WordRootRelationship())
        {
        }
    }
}