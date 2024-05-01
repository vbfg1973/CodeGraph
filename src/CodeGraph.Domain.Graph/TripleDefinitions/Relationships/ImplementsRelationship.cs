using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class ImplementsRelationship : Relationship
    {
        public override string Type => "IMPLEMENTS";
    }
}