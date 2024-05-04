using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class DependsOnRelationship : Relationship
    {
        public override string Type => "DEPENDS_ON";
    }
}