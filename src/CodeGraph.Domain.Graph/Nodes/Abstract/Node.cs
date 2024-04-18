namespace CodeGraph.Domain.Graph.Nodes.Abstract
{
    public abstract class Node
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

        protected virtual void SetPrimaryKey()
        {
            Pk = FullName.GetHashCode().ToString();
        }

        public virtual string Set(string node)
        {
            return $"{node}.pk = \"{Pk}\", {node}.fullName = \"{FullName}\", {node}.name = \"{Name}\"";
        }
    }
}