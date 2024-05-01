using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGraph.Domain.Dotnet.Abstract
{
    public abstract class AbstractCSharpWalker : CSharpSyntaxWalker, ICodeWalker
    {
        public abstract IEnumerable<Triple> Walk();
    }
}