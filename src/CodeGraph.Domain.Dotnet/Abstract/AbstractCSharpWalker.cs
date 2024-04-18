using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGraph.Domain.Dotnet.Abstract
{
    public abstract class AbstractCSharpWalker(WalkerOptions walkerOptions) : CSharpSyntaxWalker, ICSharpCodeWalker
    {
        public WalkerOptions WalkerOptions { get; } = walkerOptions;

        public abstract IEnumerable<Triple> Walk();
    }
}