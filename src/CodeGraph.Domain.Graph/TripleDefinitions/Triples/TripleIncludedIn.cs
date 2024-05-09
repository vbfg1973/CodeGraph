using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleIncludedIn : Triple
    {
        public TripleIncludedIn(
            ProjectNode contentA,
            FolderNode contentNodeB)
            : base(contentA, contentNodeB, new IncludedInRelationship())
        {
        }

        public TripleIncludedIn(
            FolderNode contentA,
            FolderNode contentNodeB)
            : base(contentA, contentNodeB, new IncludedInRelationship())
        {
        }

        public TripleIncludedIn(
            FileNode contentA,
            FolderNode contentNodeB)
            : base(contentA, contentNodeB, new IncludedInRelationship())
        {
        }
    }
}