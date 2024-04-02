using CodeGraph.Domain.Graph.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.Relationships
{
    public class InvokesRelationship : Relationship
    {
        public override string Type => "INVOKES";
    }
}