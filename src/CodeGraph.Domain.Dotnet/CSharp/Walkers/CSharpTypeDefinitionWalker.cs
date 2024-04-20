using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;


namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpTypeDefinitionWalker : CSharpBaseTypeWalker, ICodeWalker
    {
        private readonly List<Triple> _triples = new();
        private readonly TypeDeclarationSyntax _typeDeclarationSyntax;

        public CSharpTypeDefinitionWalker(TypeDeclarationSyntax typeDeclarationSyntax, WalkerOptions walkerOptions) :
            base(walkerOptions)
        {
            _typeDeclarationSyntax = typeDeclarationSyntax;
        }

        public IEnumerable<Triple> Walk()
        {
            base.Visit(_typeDeclarationSyntax);

            return _triples;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            GetHasTriple(node);

            base.VisitMethodDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            GetHasTriple(node);
            
            base.VisitPropertyDeclaration(node);
        }
        
        private void GetHasTriple(MethodDeclarationSyntax node)
        {
            TypeNode typeNode = GetTypeNode(_typeDeclarationSyntax);
            IMethodSymbol methodSymbol = CSharpExtensions.GetDeclaredSymbol(_walkerOptions.DotnetOptions.SemanticModel, node)!;
            MethodNode methodNode = methodSymbol.CreateMethodNode(node);
            _triples.Add(new TripleHas(typeNode, methodNode));
        }
        
        private void GetHasTriple(PropertyDeclarationSyntax node)
        {
            TypeNode typeNode = GetTypeNode(_typeDeclarationSyntax);
            IPropertySymbol propertySymbol = CSharpExtensions.GetDeclaredSymbol(_walkerOptions.DotnetOptions.SemanticModel, node)!;
            PropertyNode propertyNode = propertySymbol.CreatePropertyNode(node);
            _triples.Add(new TripleHas(typeNode, propertyNode));
        }
    }
}