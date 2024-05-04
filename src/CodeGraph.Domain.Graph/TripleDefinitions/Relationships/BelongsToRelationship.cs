using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class BelongsToRelationship : Relationship
    {
        public override string Type => "BELONGS_TO";
    }
}