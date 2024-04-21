using CodeGraph.Domain.Graph.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.Relationships
{
    public class DependsOnRelationship : Relationship
    {
        public override string Type => "DEPENDS_ON";
    }
}