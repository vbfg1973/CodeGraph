using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class HasRelationship : Relationship
    {
        public override string Type => "HAS";
    }
}