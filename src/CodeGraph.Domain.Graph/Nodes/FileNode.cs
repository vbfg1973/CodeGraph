using CodeGraph.Domain.Graph.Nodes.Abstract;

namespace CodeGraph.Domain.Graph.Nodes
{
    public class FileNode : Node, IEquatable<FileNode>
    {
        public FileNode(string fullName, string name)
            : base(fullName, name)
        {
        }

        public override string Label { get; } = "File";

        public bool Equals(FileNode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Label == other.Label;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((FileNode)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Label);
        }

        public static bool operator ==(FileNode? left, FileNode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FileNode? left, FileNode? right)
        {
            return !Equals(left, right);
        }
    }
}