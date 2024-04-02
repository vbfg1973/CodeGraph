using CodeGraph.Domain.Graph.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.Relationships
{
    public class DeclaredAtRelationship : Relationship
    {
        public override string Type => "DECLARED_AT";
    }
}