using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;


namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpTypeDefinitionWalker(TypeDeclarationSyntax typeDeclarationSyntax, WalkerOptions walkerOptions)
        : CSharpBaseTypeWalker(walkerOptions), ICodeWalker
    {
        private readonly List<Triple> _triples = new();

        public IEnumerable<Triple> Walk()
        {
            base.Visit(typeDeclarationSyntax);

            return _triples;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            GetHasTriple(node);
            GetImplementationOfTriples(node);

            base.VisitMethodDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            GetHasTriple(node);

            base.VisitPropertyDeclaration(node);
        }

        private void GetHasTriple(MethodDeclarationSyntax syntax)
        {
            TypeNode typeNode = GetTypeNode(typeDeclarationSyntax);
            MethodNode methodNode = GetMethodNode(syntax);
            _triples.Add(new TripleHas(typeNode, methodNode));
            _triples.AddRange(WordTriples(methodNode));
        }

        private void GetHasTriple(PropertyDeclarationSyntax syntax)
        {
            TypeNode typeNode = GetTypeNode(typeDeclarationSyntax);
            IPropertySymbol propertySymbol =
                CSharpExtensions.GetDeclaredSymbol(_walkerOptions.DotnetOptions.SemanticModel, syntax)!;
            PropertyNode propertyNode = propertySymbol.CreatePropertyNode();
            _triples.Add(new TripleHas(typeNode, propertyNode));
            _triples.AddRange(WordTriples(propertyNode));
        }

        private void GetImplementationOfTriples(MethodDeclarationSyntax syntax)
        {
            MethodNode methodNode = GetMethodNode(syntax);
            IMethodSymbol methodSymbol =
                CSharpExtensions.GetDeclaredSymbol(_walkerOptions.DotnetOptions.SemanticModel, syntax)!;

            if (methodSymbol.TryGetInterfaceMethodFromImplementation(_walkerOptions.DotnetOptions.SemanticModel,
                    out MethodNode interfaceMethodNode))
                _triples.Add(new TripleImplementationOf(methodNode, interfaceMethodNode));
        }
    }
}