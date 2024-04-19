using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpTypeDiscoveryWalker(FileNode fileNode, WalkerOptions walkerOptions) : CSharpSyntaxWalker, ICodeWalker
    {
        private List<Triple> _triples = new();
        
        public IEnumerable<Triple> Walk()
        {
            base.Visit(walkerOptions.DotnetOptions.SyntaxTree.GetRoot());

            return _triples;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            TypeNode typeNode = walkerOptions.DotnetOptions
                .SemanticModel
                .GetDeclaredSymbol(node)
                .CreateTypeNode(node);
            
            _triples.Add(new TripleDeclaredAt(typeNode, fileNode));
            
            base.VisitClassDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            base.VisitInterfaceDeclaration(node);
        }

        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            base.VisitRecordDeclaration(node);
        }


        private IEnumerable<Triple> GetInherits(TypeNode node, TypeDeclarationSyntax typeDeclarationSyntax)
        {
            if (typeDeclarationSyntax.BaseList == null) yield break;

            foreach (BaseTypeSyntax baseTypeSyntax in typeDeclarationSyntax.BaseList.Types)
            {
                TypeSyntax baseType = baseTypeSyntax.Type;
                TypeNode parentNode = walkerOptions.DotnetOptions.SemanticModel.GetTypeInfo(baseType).CreateTypeNode();

                switch (node)
                {
                    case ClassNode classNode:
                        yield return new TripleOfType(classNode, parentNode);
                        break;
                    case InterfaceNode interfaceNode when parentNode is InterfaceNode parentInterfaceNode:
                        yield return new TripleOfType(interfaceNode, parentInterfaceNode);
                        break;
                }
            }
        }
    }
}