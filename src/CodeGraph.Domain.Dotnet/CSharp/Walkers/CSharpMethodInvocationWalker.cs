﻿using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpMethodInvocationWalker(TypeDeclarationSyntax declarationSyntax, WalkerOptions walkerOptions, ILoggerFactory loggerFactory)
        : CSharpBaseTypeWalker(walkerOptions), ICodeWalker
    {
        private readonly TypeDeclarationSyntax _declarationSyntax = declarationSyntax;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;

        private readonly List<Triple> _triples = new();

        private readonly ILogger<CSharpMethodInvocationWalker> _logger =
            loggerFactory.CreateLogger<CSharpMethodInvocationWalker>();

        public IEnumerable<Triple> Walk()
        {
            base.Visit(_declarationSyntax);

            return _triples;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax syntax)
        {
            _logger.LogDebug("{Method} {SyntaxType} {NameFromSyntax} {FilePath}", nameof(VisitMethodDeclaration), nameof(MethodDeclarationSyntax), syntax.Identifier.ToString(), syntax.SyntaxTree.FilePath);

            MethodNode methodNode = GetMethodNode(syntax);

            foreach (ExpressionSyntax expressionSyntax in syntax.DescendantNodes().OfType<ExpressionSyntax>())
            {
                switch (expressionSyntax)
                {
                    case ObjectCreationExpressionSyntax creation:
                        ClassNode classNode = GetTypeNodeFromInstantiation(creation);
                        _triples.Add(new TripleConstruct(methodNode, classNode));
                        break;
                    case InvocationExpressionSyntax invocation:
                        AddInvokedMethodTriple(invocation, methodNode);
                        break;
                }
            }

            base.VisitMethodDeclaration(syntax);
        }

        private ClassNode GetTypeNodeFromInstantiation(ObjectCreationExpressionSyntax creationExpressionSyntax)
        {
            _logger.LogDebug("{Method} {SyntaxType} {FilePath}", nameof(GetTypeNodeFromInstantiation), nameof(ObjectCreationExpressionSyntax), creationExpressionSyntax.SyntaxTree.FilePath);

            return _walkerOptions.DotnetOptions.SemanticModel.GetTypeInfo(creationExpressionSyntax).CreateClassNode();
        }

        private void AddInvokedMethodTriple(InvocationExpressionSyntax invocation, MethodNode parentMethodNode)
        {
            _logger.LogDebug("{Method} {SyntaxType} {FilePath}", nameof(AddInvokedMethodTriple), nameof(InvocationExpressionSyntax), invocation.SyntaxTree.FilePath);
            
            ISymbol? symbol = _walkerOptions
                .DotnetOptions
                .SemanticModel
                .GetSymbolInfo(invocation)
                .Symbol;

            if (symbol is not IMethodSymbol invokedMethodSymbol) return;


            if (!invokedMethodSymbol.TryCreateMethodNode(_walkerOptions.DotnetOptions.SemanticModel,
                    out MethodNode? invokedMethod)) return;

            int location = invocation.GetLocation().SourceSpan.Start;

            string invocationNodeName = parentMethodNode.FullName + "_" + invokedMethod!.FullName;
            InvocationNode invocationNode = new(parentMethodNode, invokedMethod);
            InvocationLocationNode invocationLocationNode = new(location);

            // Ignore dotnet's core methods
            if (invokedMethod.FullName.StartsWith("System", StringComparison.InvariantCultureIgnoreCase) ||
                invokedMethod.FullName.StartsWith("Microsoft.Asp", StringComparison.InvariantCultureIgnoreCase) ||
                invokedMethod.FullName.StartsWith("Microsoft.EntityFrameworkCore.Metadata",
                    StringComparison.InvariantCultureIgnoreCase) ||
                invokedMethod.FullName.StartsWith("Microsoft.EntityFrameworkCore.Migrations",
                    StringComparison.InvariantCultureIgnoreCase) ||
                invokedMethod.FullName.StartsWith("Microsoft.Extensions",
                    StringComparison.InvariantCultureIgnoreCase) ||
                invokedMethod.FullName.StartsWith("Moq", StringComparison.InvariantCultureIgnoreCase)
               ) return;

            _triples.Add(new TripleInvoke(parentMethodNode, invocationNode));
            _triples.Add(new TripleInvokedAt(invocationNode, invocationLocationNode));
            _triples.Add(new TripleInvocationOf(invocationNode, invokedMethod));
        }
    }
}