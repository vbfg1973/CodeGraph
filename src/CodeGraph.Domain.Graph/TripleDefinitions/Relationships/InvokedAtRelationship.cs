using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class InvokedAtRelationship : Relationship
    {
        public override string Type => "INVOKED_AT";
    }
}