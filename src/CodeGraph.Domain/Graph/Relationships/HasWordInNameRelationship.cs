using CodeGraph.Domain.Graph.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.Relationships
{
    public class HasWordInNameRelationship : Relationship
    {
        public override string Type => "HAS_WORD_IN_NAME";
    }
    
    public class WordRootRelationship : Relationship
    {
        public override string Type => "WORD_DERIVES_FROM";
    }
}