using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleBelongsTo : Triple
    {
        public TripleBelongsTo(
            TypeNode typeA,
            ProjectNode projectNodeB)
            : base(typeA, projectNodeB, new BelongsToRelationship())
        {
        }
    }
}