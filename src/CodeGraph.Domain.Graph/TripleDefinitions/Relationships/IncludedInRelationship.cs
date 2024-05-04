using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class IncludedInRelationship : Relationship
    {
        public override string Type => "INCLUDED_IN";
    }
}