using CodeGraph.Domain.Graph.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.Relationships
{
    public class ImplementsRelationship : Relationship
    {
        public override string Type => "IMPLEMENTS";
    }
}