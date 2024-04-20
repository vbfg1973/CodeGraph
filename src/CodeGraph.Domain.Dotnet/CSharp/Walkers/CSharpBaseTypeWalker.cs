using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Graph.Nodes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public abstract class CSharpBaseTypeWalker(WalkerOptions walkerOptions) : CSharpSyntaxWalker
    {
        protected readonly WalkerOptions _walkerOptions = walkerOptions;

        protected TypeNode GetTypeNode(TypeDeclarationSyntax node)
        {
            return _walkerOptions
                .DotnetOptions
                .SemanticModel
                .GetDeclaredSymbol(node)!
                .CreateTypeNode(node);
        }
    }
}