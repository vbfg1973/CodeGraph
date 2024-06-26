﻿using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Dotnet.Walkers.CSharp
{
    public class CSharpTypeDiscoveryWalker(
        FileNode fileNode,
        ProjectNode projectNode,
        WalkerOptions walkerOptions,
        ILoggerFactory loggerFactory)
        : CSharpBaseTypeWalker(walkerOptions), ICodeWalker
    {
        private readonly ILogger<CSharpTypeDiscoveryWalker> _logger =
            loggerFactory.CreateLogger<CSharpTypeDiscoveryWalker>();

        private readonly ProjectNode _projectNode = projectNode;
        private readonly List<Triple> _triples = new();

        public IEnumerable<Triple> Walk()
        {
            _logger.LogTrace("{Method}", nameof(Walk));

            base.Visit(walkerOptions.DotnetOptions.SyntaxTree.GetRoot());

            return _triples;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            _logger.LogTrace("{Method} {SyntaxType} {NameFromSyntax} {FilePath}", nameof(VisitClassDeclaration),
                nameof(ClassDeclarationSyntax), node.Identifier.ToString(), node.SyntaxTree.FilePath);

            GetTypeDeclarationTriples(node);

            SubWalkers(node);

            base.VisitClassDeclaration(node);
        }


        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            _logger.LogTrace("{Method} {SyntaxType} {NameFromSyntax} {FilePath}", nameof(VisitInterfaceDeclaration),
                nameof(InterfaceDeclarationSyntax), node.Identifier.ToString(), node.SyntaxTree.FilePath);

            GetTypeDeclarationTriples(node);

            SubWalkers(node);

            base.VisitInterfaceDeclaration(node);
        }

        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            _logger.LogTrace("{Method} {SyntaxType} {NameFromSyntax} {FilePath}", nameof(VisitRecordDeclaration),
                nameof(RecordDeclarationSyntax), node.Identifier.ToString(), node.SyntaxTree.FilePath);

            GetTypeDeclarationTriples(node);

            SubWalkers(node);

            base.VisitRecordDeclaration(node);
        }

        // public override void VisitStructDeclaration(StructDeclarationSyntax node)
        // {
        //     GetTypeDeclarationTriples(node);
        //
        //     base.VisitStructDeclaration(node);
        // }

        private void GetTypeDeclarationTriples(TypeDeclarationSyntax node)
        {
            _logger.LogTrace("{Method} {SyntaxType} {NameFromSyntax} {FilePath}", nameof(GetTypeDeclarationTriples),
                nameof(TypeDeclarationSyntax), node.Identifier.ToString(), node.SyntaxTree.FilePath);

            TypeNode typeNode = GetTypeNode(node);

            _triples.Add(new TripleDeclaredAt(typeNode, fileNode));
            _triples.Add(new TripleBelongsTo(typeNode, _projectNode));
            _triples.AddRange(node.GetInherits(typeNode, walkerOptions.DotnetOptions.SemanticModel));
            _triples.AddRange(WordTriples(typeNode));
        }

        private void SubWalkers(TypeDeclarationSyntax node)
        {
            _logger.LogTrace("{Method} {SyntaxType} {NameFromSyntax} {FilePath}", nameof(SubWalkers),
                nameof(TypeDeclarationSyntax), node.Identifier.ToString(), node.SyntaxTree.FilePath);

            if (!_walkerOptions.DescendIntoSubWalkers) return;

            CSharpTypeDefinitionWalker typeDefinitionWalker = new(node, _walkerOptions, loggerFactory);
            _triples.AddRange(typeDefinitionWalker.Walk());

            CSharpMethodInvocationWalker methodInvocationWalker = new(node, _walkerOptions, loggerFactory);
            _triples.AddRange(methodInvocationWalker.Walk());
        }
    }
}