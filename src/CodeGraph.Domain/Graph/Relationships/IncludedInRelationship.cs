using CodeGraph.Domain.Graph.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.Relationships
{
    public class IncludedInRelationship : Relationship
    {
        public override string Type => "INCLUDED_IN";
    }
}