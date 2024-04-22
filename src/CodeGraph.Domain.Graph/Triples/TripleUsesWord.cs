using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Nodes.Abstract;
using CodeGraph.Domain.Graph.Relationships;
using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Graph.Triples
{
    public class TripleUsesWord(CodeNode nodeA, WordNode nodeB) : Triple(nodeA, nodeB, new HasWordInNameRelationship());
}