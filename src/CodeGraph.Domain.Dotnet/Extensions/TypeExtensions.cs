using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet
{
    public static class TypeExtensions
    {
        public static TypeNode CreateTypeNode(this ISymbol symbol, TypeDeclarationSyntax declaration)
        {
            (string fullName, string name) = (symbol.ContainingNamespace.ToString() + '.' + symbol.Name, symbol.Name);
            return declaration switch
            {
                ClassDeclarationSyntax _ => new ClassNode(fullName, name, declaration.Modifiers.MapModifiers()),
                InterfaceDeclarationSyntax _ => new InterfaceNode(fullName, name, declaration.Modifiers.MapModifiers()),
                RecordDeclarationSyntax _ => new RecordNode(fullName, name, declaration.Modifiers.MapModifiers()),
                _ => throw new ArgumentOutOfRangeException(nameof(declaration), "Invalid TypeDeclarationSyntax in CreateTypeNode")
            };
        }

        public static IEnumerable<Triple> GetInherits(this TypeDeclarationSyntax typeDeclarationSyntax, TypeNode node, SemanticModel semanticModel)
        {
            if (typeDeclarationSyntax.BaseList == null) yield break;

            foreach (BaseTypeSyntax baseTypeSyntax in typeDeclarationSyntax.BaseList.Types)
            {
                TypeSyntax baseType = baseTypeSyntax.Type;
                TypeNode parentNode = semanticModel.GetTypeInfo(baseType).CreateTypeNode();

                switch (node)
                {
                    case ClassNode classNode:
                        yield return new TripleOfType(classNode, parentNode);
                        break;
                    case RecordNode recordNode:
                        yield return new TripleOfType(recordNode, parentNode);
                        break;
                    case InterfaceNode interfaceNode when parentNode is InterfaceNode parentInterfaceNode:
                        yield return new TripleOfType(interfaceNode, parentInterfaceNode);
                        break;
                }
            }
        }

        public static TypeNode CreateTypeNode(this TypeInfo typeInfo)
        {
            return typeInfo.ConvertedType.TypeKind switch
            {
                TypeKind.Interface => typeInfo.CreateInterfaceNode(),
                TypeKind.Class => typeInfo.CreateClassNode(),
                // Maybe records show as classes? No TypeKind for them!
                _ => null
            };
        }
        
        public static ClassNode CreateClassNode(this TypeInfo typeInfo)
        {
            return new ClassNode(typeInfo.GetFullName(), typeInfo.GetName());
        }

        public static InterfaceNode CreateInterfaceNode(this TypeInfo typeInfo)
        {
            return new InterfaceNode(typeInfo.GetFullName(), typeInfo.GetName());
        }
        
        public static RecordNode CreateRecordNode(this TypeInfo typeInfo)
        {
            return new RecordNode(typeInfo.GetFullName(), typeInfo.GetName());
        }
    }
}