using System.Reflection;

namespace CodeGraph.Domain
{
    public sealed class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}