using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class InvokesRelationship : Relationship
    {
        public override string Type => "INVOKES";
    }
}