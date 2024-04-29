using CodeGraph.Domain.Graph.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.Relationships
{
    public class ImplementationOfRelationship : Relationship
    {
        public override string Type => "IMPLEMENTS";
    }
}