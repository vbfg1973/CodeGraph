namespace CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract
{
    public abstract class Node : IEquatable<Node>
    {
        protected Node(string fullName, string name)
        {
            FullName = fullName;
            Name = name;
            SetPrimaryKey();
        }

        public abstract string Label { get; }
        public virtual string FullName { get; }
        public virtual string Name { get; }

        /// <summary>
        ///     Used to compare matching nodes on merge operations
        /// </summary>
        public virtual string Pk { get; protected set; } = null!;

        public bool Equals(Node? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Label == other.Label && FullName == other.FullName && Name == other.Name && Pk == other.Pk;
        }

        protected virtual void SetPrimaryKey()
        {
            Pk = FullName.GetHashCode().ToString();
        }

        public virtual string Set(string node)
        {
            return $"{node}.pk = \"{Pk}\", {node}.fullName = \"{FullName}\", {node}.name = \"{Name}\"";
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Node)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Label, FullName, Name, Pk);
        }

        public static bool operator ==(Node? left, Node? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Node? left, Node? right)
        {
            return !Equals(left, right);
        }
    }
}