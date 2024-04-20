using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpTypeDefinitionWalker(TypeDeclarationSyntax typeDeclarationSyntax, WalkerOptions walkerOptions)
        : CSharpSyntaxWalker, ICodeWalker
    {
        private readonly List<Triple> _triples = new();
        
        public IEnumerable<Triple> Walk()
        {
            base.Visit(typeDeclarationSyntax);

            return _triples;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var a = node.Arity;

            base.VisitMethodDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            base.VisitPropertyDeclaration(node);
        }

        public override void VisitBaseExpression(BaseExpressionSyntax node)
        {
            var a = node.Token;
            base.VisitBaseExpression(node);
        }
    }
}