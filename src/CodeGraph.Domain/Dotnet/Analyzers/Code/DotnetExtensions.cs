using CodeGraph.Domain.Graph.Nodes;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.Analyzers.Code
{
    public static class DotnetExtensions
    {
        public static TypeNode CreateTypeNode(this TypeInfo typeInfo)
        {
            return typeInfo.ConvertedType.TypeKind switch
            {
                TypeKind.Interface => CreateInterfaceNode(typeInfo),
                TypeKind.Class => CreateClassNode(typeInfo),
                _ => null
            };
        }

        public static ClassNode CreateClassNode(this TypeInfo typeInfo)
        {
            return new ClassNode(GetFullName(typeInfo), GetName(typeInfo));
        }

        public static InterfaceNode CreateInterfaceNode(this TypeInfo typeInfo)
        {
            return new InterfaceNode(GetFullName(typeInfo), GetName(typeInfo));
        }

        public static string GetName(this TypeInfo typeInfo)
        {
            return typeInfo.Type!.Name;
        }

        public static string GetFullName(this TypeInfo typeInfo)
        {
            return typeInfo.Type?.ContainingNamespace + "." + GetName(typeInfo);
        }

        public static string GetNamespaceName(this INamespaceSymbol namespaceSymbol, string name)
        {
            string? nextName = namespaceSymbol?.Name;
            return string.IsNullOrEmpty(nextName)
                ? name
                : GetNamespaceName(namespaceSymbol.ContainingNamespace, $"{nextName}.{name}");
        }
    }
}