using CodeGraph.Domain.Graph.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.Nodes
{
    public class MethodNode : CodeNode
    {
        public MethodNode(string fullName, string name, (string name, string type)[] args, string returnType,
            string[] modifiers = null!)
            : base(fullName, name, modifiers)
        {
            Arguments = string.Join(", ", args.Select(x => $"{x.type} {x.name}"));
            ReturnType = returnType;
            SetPrimaryKey();
        }

        public override string Label { get; } = "Method";

        public string Arguments { get; }

        public string ReturnType { get; }

        public override string Set(string node)
        {
            return $"{base.Set(node)}, {node}.returnType = \"{ReturnType}\", {node}.arguments = \"{Arguments}\"";
        }

        protected sealed override void SetPrimaryKey()
        {
            Pk = $"{FullName}{Arguments}{ReturnType}".GetHashCode().ToString();
        }
    }
}