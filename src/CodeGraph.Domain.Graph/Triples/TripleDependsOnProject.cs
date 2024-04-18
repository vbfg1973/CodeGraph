using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Relationships;
using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Graph.Triples
{
    public class TripleDependsOnProject : Triple
    {
        public TripleDependsOnProject(
            ProjectNode projectA,
            ProjectNode projectB)
            : base(projectA, projectB, new DependsOnRelationship())
        {
        }
    }
}