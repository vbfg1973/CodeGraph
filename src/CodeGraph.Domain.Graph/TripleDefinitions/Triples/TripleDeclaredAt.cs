using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleDeclaredAt : Triple
    {
        public TripleDeclaredAt(
            TypeNode typeA,
            FileNode fileNodeB)
            : base(typeA, fileNodeB, new DeclaredAtRelationship())
        {
        }
    }
}