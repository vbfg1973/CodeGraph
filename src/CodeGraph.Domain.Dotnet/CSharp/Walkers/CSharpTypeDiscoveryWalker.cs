using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpTypeDiscoveryWalker(FileNode fileNode, WalkerOptions walkerOptions) : CSharpBaseTypeWalker(walkerOptions), ICodeWalker
    {
        private readonly List<Triple> _triples = new();


        public IEnumerable<Triple> Walk()
        {
            base.Visit(walkerOptions.DotnetOptions.SyntaxTree.GetRoot());

            return _triples;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            GetTypeDeclarationTriples(node);

            base.VisitClassDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            GetTypeDeclarationTriples(node);

            base.VisitInterfaceDeclaration(node);
        }

        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            GetTypeDeclarationTriples(node);

            base.VisitRecordDeclaration(node);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            GetTypeDeclarationTriples(node);
            
            base.VisitStructDeclaration(node);
        }

        private void GetTypeDeclarationTriples(TypeDeclarationSyntax node)
        {
            TypeNode typeNode = GetTypeNode(node);

            _triples.Add(new TripleDeclaredAt(typeNode, fileNode));

            _triples.AddRange(node.GetInherits(typeNode, walkerOptions.DotnetOptions.SemanticModel));

            if (!walkerOptions.DescendIntoSubWalkers) return;
            
            CSharpTypeDefinitionWalker typeDefinitionWalker = new CSharpTypeDefinitionWalker(node, walkerOptions);
            _triples.AddRange(typeDefinitionWalker.Walk());
        }
    }
}