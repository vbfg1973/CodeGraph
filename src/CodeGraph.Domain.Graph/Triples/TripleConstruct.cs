using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Relationships;
using CodeGraph.Domain.Graph.Triples.Abstract;

namespace CodeGraph.Domain.Graph.Triples
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
            ClassNode classB)
            : base(methodA, classB, new ConstructRelationship())
        {
        }
    }
}