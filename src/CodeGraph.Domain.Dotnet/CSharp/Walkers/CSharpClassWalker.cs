using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpClassWalker(ClassDeclarationSyntax classDeclarationSyntax, WalkerOptions walkerOptions)
        : CSharpSyntaxWalker, ICodeWalker
    {
        public ClassDeclarationSyntax ClassDeclarationSyntax { get; } = classDeclarationSyntax;
        public WalkerOptions WalkerOptions { get; } = walkerOptions;

        public IEnumerable<Triple> Walk()
        {
            throw new NotImplementedException();
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            base.VisitMethodDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            base.VisitPropertyDeclaration(node);
        }

        public override void VisitBaseExpression(BaseExpressionSyntax node)
        {
            base.VisitBaseExpression(node);
        }
    }
}