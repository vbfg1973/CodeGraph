using CodeGraph.Domain.Graph.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.Relationships
{
    public class HasRelationship : Relationship
    {
        public override string Type => "HAS";
    }
}