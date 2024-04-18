using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers.Classes
{
    public class NullCSharpClassWalker(WalkerOptions walkerOptions)
        : AbstractCSharpWalker(walkerOptions)
    {
        private readonly SemanticModel? _semanticModel;

        private readonly SyntaxTree? _syntaxTree;

        public override IEnumerable<Triple> Walk()
        {
            return Enumerable.Empty<Triple>();
        }
    }
}