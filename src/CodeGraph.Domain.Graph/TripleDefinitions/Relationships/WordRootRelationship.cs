using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class WordRootRelationship : Relationship
    {
        public override string Type => "WORD_DERIVES_FROM";
    }
}