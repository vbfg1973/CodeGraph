using CodeGraph.Domain.Graph.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.Nodes
{
    public abstract class TypeNode : CodeNode
    {
        protected TypeNode(string fullName, string name, string[] modifiers = null)
            : base(fullName, name, modifiers)
        {
        }
    }
}