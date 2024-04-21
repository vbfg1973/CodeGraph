using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpMethodInvocationWalker(TypeDeclarationSyntax declarationSyntax, WalkerOptions walkerOptions)
        : CSharpBaseTypeWalker(walkerOptions), ICodeWalker
    {
        private readonly TypeDeclarationSyntax _declarationSyntax = declarationSyntax;

        private readonly List<Triple> _triples = new();

        public IEnumerable<Triple> Walk()
        {
            base.Visit(walkerOptions.DotnetOptions.SyntaxTree.GetRoot());

            return _triples;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            MethodNode methodNode = GetMethodNode(node);

            foreach (ExpressionSyntax syntax in node.DescendantNodes().OfType<ExpressionSyntax>())
            {
                switch (syntax)
                {
                    case ObjectCreationExpressionSyntax creation:
                        break;
                    case InvocationExpressionSyntax invocation:
                        AddInvokedMethodTriple(invocation, methodNode);
                        break;
                }
            }
            
            base.VisitMethodDeclaration(node);
        }

        private void AddInvokedMethodTriple(InvocationExpressionSyntax invocation, MethodNode parentMethodNode)
        {
            ISymbol? symbol = _walkerOptions
                .DotnetOptions
                .SemanticModel
                .GetSymbolInfo(invocation)
                .Symbol;

            if (symbol is not IMethodSymbol invokedMethodSymbol) return;

            if (invokedMethodSymbol.TryCreateMethodNode(_walkerOptions.DotnetOptions.SemanticModel,
                    out MethodNode? invokedMethod))
            {
                _triples.Add(new TripleInvoke(parentMethodNode, invokedMethod!));
            }
        }
    }
}