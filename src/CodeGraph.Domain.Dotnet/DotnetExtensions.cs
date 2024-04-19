using CodeGraph.Domain.Graph.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet
{
    public static class DotnetExtensions
    {
        public static string[] MapModifiers(this SyntaxTokenList syntaxTokens)
        {
            return syntaxTokens.Select(x => x.ValueText).ToArray();
        }

        private static ClassNode CreateClassNode(this TypeInfo typeInfo)
        {
            return new ClassNode(GetFullName(typeInfo), GetName(typeInfo));
        }

        private static InterfaceNode CreateInterfaceNode(this TypeInfo typeInfo)
        {
            return new InterfaceNode(GetFullName(typeInfo), GetName(typeInfo));
        }

        private static string GetName(this TypeInfo typeInfo)
        {
            return typeInfo.Type.Name;
        }

        private static string GetFullName(this TypeInfo typeInfo)
        {
            return typeInfo.Type.ContainingNamespace + "." + GetName(typeInfo);
        }

        public static TypeNode CreateTypeNode(this ISymbol symbol, TypeDeclarationSyntax declaration)
        {
            (string fullName, string name) = (symbol.ContainingNamespace.ToString() + '.' + symbol.Name, symbol.Name);
            return declaration switch
            {
                ClassDeclarationSyntax _ => new ClassNode(fullName, name, declaration.Modifiers.MapModifiers()),
                InterfaceDeclarationSyntax _ => new InterfaceNode(fullName, name, declaration.Modifiers.MapModifiers()),
                // Records
                _ => null
            };
        }

        public static TypeNode CreateTypeNode(this TypeInfo typeInfo)
        {
            return typeInfo.ConvertedType.TypeKind switch
            {
                TypeKind.Interface => CreateInterfaceNode(typeInfo),
                TypeKind.Class => CreateClassNode(typeInfo),
                _ => null
            };
        }

        private static string GetNamespaceName(this INamespaceSymbol namespaceSymbol, string name)
        {
            string? nextName = namespaceSymbol?.Name;
            return string.IsNullOrEmpty(nextName)
                ? name
                : GetNamespaceName(namespaceSymbol.ContainingNamespace, $"{nextName}.{name}");
        }

        private static MethodNode CreateMethodNode(this IMethodSymbol symbol,
            MethodDeclarationSyntax declaration = null)
        {
            string temp = $"{symbol.ContainingType}.{symbol.Name}";
            string fullName =
                symbol.ContainingNamespace.GetNamespaceName($"{symbol.ContainingType.Name}.{symbol.Name}");
            (string name, string? type)[] args = symbol.Parameters.Select(x => (name: x.Name, type: x.Type.ToString()))
                .ToArray();
            string? returnType = symbol.ReturnType.ToString();
            return new MethodNode(fullName,
                symbol.Name,
                args,
                returnType,
                declaration?.Modifiers.MapModifiers());
        }
    }
}