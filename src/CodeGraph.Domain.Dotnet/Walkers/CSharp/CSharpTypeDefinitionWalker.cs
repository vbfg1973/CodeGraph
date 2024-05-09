using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;

namespace CodeGraph.Domain.Dotnet.Walkers.CSharp
{
    public class CSharpTypeDefinitionWalker(
        TypeDeclarationSyntax typeDeclarationSyntax,
        WalkerOptions walkerOptions,
        ILoggerFactory loggerFactory)
        : CSharpBaseTypeWalker(walkerOptions), ICodeWalker
    {
        private readonly ILogger<CSharpTypeDefinitionWalker> _logger =
            loggerFactory.CreateLogger<CSharpTypeDefinitionWalker>();

        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly List<Triple> _triples = new();

        public IEnumerable<Triple> Walk()
        {
            _logger.LogTrace("{Method}", nameof(Walk));
            base.Visit(typeDeclarationSyntax);

            return _triples;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax syntax)
        {
            _logger.LogTrace("{Method} {SyntaxType} {NameFromSyntax} {FilePath}", nameof(VisitMethodDeclaration),
                nameof(MethodDeclarationSyntax), syntax.Identifier.ToString(), syntax.SyntaxTree.FilePath);

            GetComplexity(syntax);
            GetHasTriple(syntax);
            GetImplementationOfTriples(syntax);

            base.VisitMethodDeclaration(syntax);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax syntax)
        {
            _logger.LogTrace("{Method} {SyntaxType} {NameFromSyntax} {FilePath}", nameof(VisitPropertyDeclaration),
                nameof(PropertyDeclarationSyntax), syntax.Identifier.ToString(), syntax.SyntaxTree.FilePath);

            GetHasTriple(syntax);

            base.VisitPropertyDeclaration(syntax);
        }

        private void GetComplexity(MethodDeclarationSyntax syntax)
        {
            CSharpCognitiveComplexityWalker walker = new(syntax, _walkerOptions);

            MethodNode methodNode = GetMethodNode(syntax);
            CognitiveComplexityNode cognitiveComplexityNode = new(walker.ComplexityScore);

            _triples.Add(new TripleHasComplexity(methodNode, cognitiveComplexityNode));
        }

        private void GetHasTriple(MethodDeclarationSyntax syntax)
        {
            _logger.LogTrace("{Method} {SyntaxType} {NameFromSyntax} {FilePath}", nameof(GetHasTriple),
                nameof(MethodDeclarationSyntax), syntax.Identifier.ToString(), syntax.SyntaxTree.FilePath);

            TypeNode typeNode = GetTypeNode(typeDeclarationSyntax);
            MethodNode methodNode = GetMethodNode(syntax);

            _logger.LogTrace(
                "{Method} {SyntaxType} {NameFromSyntax} {FilePath} {MethodNodeFullName} {MethodNodeReturnType}",
                nameof(GetHasTriple), nameof(MethodDeclarationSyntax), syntax.Identifier.ToString(),
                syntax.SyntaxTree.FilePath, methodNode.FullName, methodNode.ReturnType);

            _triples.Add(new TripleHas(typeNode, methodNode));
            _triples.AddRange(WordTriples(methodNode));
        }

        private void GetHasTriple(PropertyDeclarationSyntax syntax)
        {
            _logger.LogTrace("{Method} {SyntaxType} {NameFromSyntax} {FilePath}", nameof(GetHasTriple),
                nameof(PropertyDeclarationSyntax), syntax.Identifier.ToString(), syntax.SyntaxTree.FilePath);

            TypeNode typeNode = GetTypeNode(typeDeclarationSyntax);
            IPropertySymbol propertySymbol =
                CSharpExtensions.GetDeclaredSymbol(_walkerOptions.DotnetOptions.SemanticModel, syntax)!;
            PropertyNode propertyNode = propertySymbol.CreatePropertyNode();
            _logger.LogTrace(
                "{Method} {SyntaxType} {NameFromSyntax} {FilePath} {PropertyNodeFullName} {PropertyNodeReturnType}",
                nameof(GetHasTriple), nameof(MethodDeclarationSyntax), syntax.Identifier.ToString(),
                syntax.SyntaxTree.FilePath, propertyNode.FullName, propertyNode.ReturnType);

            _triples.Add(new TripleHas(typeNode, propertyNode));
            _triples.AddRange(WordTriples(propertyNode));
        }

        private void GetImplementationOfTriples(MethodDeclarationSyntax syntax)
        {
            _logger.LogTrace("{Method} {SyntaxType} {NameFromSyntax} {FilePath}", nameof(GetImplementationOfTriples),
                nameof(MethodDeclarationSyntax), syntax.Identifier.ToString(), syntax.SyntaxTree.FilePath);

            MethodNode methodNode = GetMethodNode(syntax);
            IMethodSymbol methodSymbol =
                CSharpExtensions.GetDeclaredSymbol(_walkerOptions.DotnetOptions.SemanticModel, syntax)!;

            if (!methodSymbol.TryGetInterfaceMethodFromImplementation(_walkerOptions.DotnetOptions.SemanticModel,
                    out MethodNode interfaceMethodNode)) return;

            _logger.LogTrace(
                "{Method} {SyntaxType} {NameFromSyntax} {FilePath} {MethodNodeFullName} {MethodNodeReturnType}",
                nameof(GetHasTriple), nameof(MethodDeclarationSyntax), syntax.Identifier.ToString(),
                syntax.SyntaxTree.FilePath, interfaceMethodNode.FullName, interfaceMethodNode.ReturnType);
            _triples.Add(new TripleImplementationOf(methodNode, interfaceMethodNode));
        }
    }
}