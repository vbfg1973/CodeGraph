using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpTypeDiscoveryWalker(FileNode fileNode, WalkerOptions walkerOptions)
        : CSharpBaseTypeWalker(walkerOptions), ICodeWalker
    {
        private readonly List<Triple> _triples = new();


        public IEnumerable<Triple> Walk()
        {
            base.Visit(walkerOptions.DotnetOptions.SyntaxTree.GetRoot());

            return _triples.Distinct();
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            GetTypeDeclarationTriples(node);

            SubWalkers(node);

            base.VisitClassDeclaration(node);
        }


        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            GetTypeDeclarationTriples(node);

            SubWalkers(node);

            base.VisitInterfaceDeclaration(node);
        }

        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
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
            TypeNode typeNode = GetTypeNode(node);

            ProjectNode projectNode = new(walkerOptions.DotnetOptions.Project!.Name);

            _triples.Add(new TripleDeclaredAt(typeNode, fileNode));
            _triples.Add(new TripleBelongsTo(typeNode, projectNode));
            _triples.AddRange(node.GetInherits(typeNode, walkerOptions.DotnetOptions.SemanticModel));
            _triples.AddRange(WordTriples(typeNode));
        }

        private void SubWalkers(TypeDeclarationSyntax node)
        {
            if (!_walkerOptions.DescendIntoSubWalkers) return;

            CSharpTypeDefinitionWalker typeDefinitionWalker = new(node, _walkerOptions);
            _triples.AddRange(typeDefinitionWalker.Walk().Distinct());

            CSharpMethodInvocationWalker methodInvocationWalker = new(node, _walkerOptions);
            _triples.AddRange(methodInvocationWalker.Walk().Distinct());
        }
    }
}