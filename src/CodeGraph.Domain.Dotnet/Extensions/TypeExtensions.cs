using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.Extensions
{
    public static class TypeExtensions
    {
        public static TypeNode CreateTypeNode(this ISymbol symbol, TypeDeclarationSyntax declaration)
        {
            (string fullName, string name) = (symbol.ContainingNamespace.ToString() + '.' + symbol.Name, symbol.Name);
            return declaration switch
            {
                ClassDeclarationSyntax _ => new ClassNode(fullName, name, symbol.MapModifiers()),
                InterfaceDeclarationSyntax _ => new InterfaceNode(fullName, name, symbol.MapModifiers()),
                RecordDeclarationSyntax _ => new RecordNode(fullName, name, symbol.MapModifiers()),
                _ => throw new ArgumentOutOfRangeException(nameof(declaration),
                    "Invalid TypeDeclarationSyntax in CreateTypeNode")
            };
        }

        public static IEnumerable<Triple> GetInherits(this TypeDeclarationSyntax typeDeclarationSyntax, TypeNode node,
            SemanticModel semanticModel)
        {
            if (typeDeclarationSyntax.BaseList == null) yield break;

            foreach (BaseTypeSyntax baseTypeSyntax in typeDeclarationSyntax.BaseList.Types)
            {
                TypeSyntax baseType = baseTypeSyntax.Type;
                TypeNode parentNode = baseType.CreateTypeNode(semanticModel);

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

        public static TypeNode CreateTypeNode(this TypeSyntax syntaxNode, SemanticModel semanticModel)
        {
            TypeInfo identifiedType = semanticModel.GetTypeInfo(syntaxNode);

            if (identifiedType.ConvertedType is not INamedTypeSymbol namedType) return null!;

            return namedType.TypeKind switch
            {
                TypeKind.Interface => identifiedType.CreateInterfaceNode(),
                TypeKind.Class => identifiedType.CreateClassNode(),
                TypeKind.Error => identifiedType
                    .CreateInterfaceNode(), // TODO - In built generic interface types. This must be wrong, but why?!?!? 
                // Maybe records show as classes? No TypeKind for them! Makes sense if syntactical sugar
                _ => null!
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