using CodeGraph.Domain.Graph.TripleDefinitions.Relationships.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Relationships
{
    public class HasWordInNameRelationship : Relationship
    {
        public override string Type => "HAS_WORD_IN_NAME";
    }
}