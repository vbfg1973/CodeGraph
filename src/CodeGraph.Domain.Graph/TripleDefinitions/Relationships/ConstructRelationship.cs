using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class ConstructRelationship : Relationship
    {
        public override string Type => "CONSTRUCT";
    }
}