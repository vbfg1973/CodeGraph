using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class InvocationOfRelationship : Relationship
    {
        public override string Type => "INVOCATION_OF";
    }
}