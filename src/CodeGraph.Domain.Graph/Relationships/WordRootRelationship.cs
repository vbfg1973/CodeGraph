using CodeGraph.Domain.Graph.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.Relationships
{
    public class WordRootRelationship : Relationship
    {
        public override string Type => "WORD_DERIVES_FROM";
    }
}