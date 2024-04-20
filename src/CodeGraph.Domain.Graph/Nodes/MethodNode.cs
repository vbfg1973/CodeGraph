using CodeGraph.Domain.Graph.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.Nodes
{
    public class MethodNode : CodeNode, IEquatable<MethodNode>
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

        public bool Equals(MethodNode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Label == other.Label && Arguments == other.Arguments && ReturnType == other.ReturnType;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((MethodNode)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Label, Arguments, ReturnType);
        }

        public static bool operator ==(MethodNode? left, MethodNode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MethodNode? left, MethodNode? right)
        {
            return !Equals(left, right);
        }
    }
}