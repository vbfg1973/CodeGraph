using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleConstruct : Triple
    {
        //public TripleConstruct(
        //    ClassNode classA,
        //    ClassNode classB)
        //    : base(classA, classB, new ConstructRelationship())
        //{ }

        public TripleConstruct(
            MethodNode methodA,
            ClassNode classNodeB)
            : base(methodA, classNodeB, new ConstructRelationship())
        {
        }
    }
}