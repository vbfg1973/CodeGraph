using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.CSharp.Walkers
{
    public class CSharpTypeDiscoveryWalker(WalkerOptions walkerOptions) : CSharpSyntaxWalker, ICodeWalker
    {
        private readonly WalkerOptions _walkerOptions = walkerOptions;

        public IEnumerable<Triple> Walk()
        {
            base.Visit(walkerOptions.SyntaxTree.GetRoot());
            
            return Enumerable.Empty<Triple>();
        }
        
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            TypeNode typeNode = walkerOptions
                .SemanticModel
                .GetDeclaredSymbol(node)
                .CreateTypeNode(node);
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
                TypeNode parentNode = walkerOptions.SemanticModel.GetTypeInfo(baseType).CreateTypeNode();
                
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