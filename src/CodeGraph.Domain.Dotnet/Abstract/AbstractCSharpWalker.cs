using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGraph.Domain.Dotnet.Abstract
{
    public abstract class AbstractCSharpWalker : CSharpSyntaxWalker, ICodeWalker
    {
        public abstract IEnumerable<Triple> Walk();
    }
}