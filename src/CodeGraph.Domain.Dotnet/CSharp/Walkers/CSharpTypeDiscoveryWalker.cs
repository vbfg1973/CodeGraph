using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpTypeDiscoveryWalker(FileNode fileNode, WalkerOptions walkerOptions)
        : CSharpSyntaxWalker, ICodeWalker
    {
        private List<Triple> _triples = new();

        public IEnumerable<Triple> Walk()
        {
            base.Visit(walkerOptions.DotnetOptions.SyntaxTree.GetRoot());

            return _triples;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            TypeNode typeNode = walkerOptions
                .DotnetOptions
                .SemanticModel
                .GetDeclaredSymbol(node)!
                .CreateTypeNode(node);

            _triples.Add(new TripleDeclaredAt(typeNode, fileNode));

            _triples.AddRange(node.GetInherits(typeNode, walkerOptions.DotnetOptions.SemanticModel));
            
            base.VisitClassDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            TypeNode typeNode = walkerOptions
                .DotnetOptions
                .SemanticModel
                .GetDeclaredSymbol(node)!
                .CreateTypeNode(node);

            _triples.Add(new TripleDeclaredAt(typeNode, fileNode));
            
            _triples.AddRange(node.GetInherits(typeNode, walkerOptions.DotnetOptions.SemanticModel));
            
            base.VisitInterfaceDeclaration(node);
        }

        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            TypeNode typeNode = walkerOptions
                .DotnetOptions
                .SemanticModel
                .GetDeclaredSymbol(node)!
                .CreateTypeNode(node);

            _triples.Add(new TripleDeclaredAt(typeNode, fileNode));
            
            _triples.AddRange(node.GetInherits(typeNode, walkerOptions.DotnetOptions.SemanticModel));
            
            base.VisitRecordDeclaration(node);
        }
    }
}