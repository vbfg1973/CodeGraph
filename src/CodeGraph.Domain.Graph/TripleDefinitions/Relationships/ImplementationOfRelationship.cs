using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class ImplementationOfRelationship : Relationship
    {
        public override string Type => "IMPLEMENTS";
    }
}