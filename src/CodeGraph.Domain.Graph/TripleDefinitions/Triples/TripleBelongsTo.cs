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
            ProjectNode projectB)
            : base(typeA, projectB, new BelongsToRelationship())
        {
        }
    }
}