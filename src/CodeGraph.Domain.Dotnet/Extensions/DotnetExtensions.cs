using CodeGraph.Domain.Graph.Nodes;
using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.Extensions
{
    public static class DotnetExtensions
    {
        public static string[] MapModifiers(this SyntaxTokenList syntaxTokens)
        {
            return syntaxTokens.Select(x => x.ValueText).ToArray();
        }

        public static string[] MapModifiers(this ISymbol symbol)
        {
            List<string> list = new();

            list.Add(symbol.DeclaredAccessibility.ToString().ToLower());

            if (symbol.IsAbstract) list.Add("abstract");

            if (symbol.IsStatic) list.Add("static");
            
            if (symbol.IsSealed) list.Add("sealed");

            if (symbol.IsOverride) list.Add("override");

            if (symbol.IsVirtual) list.Add("virtual");

            if (symbol.IsExtern) list.Add("extern");

            return list.ToArray();
        }

        public static string GetName(this TypeInfo typeInfo)
        {
            return typeInfo.Type.Name;
        }

        public static string GetFullName(this TypeInfo typeInfo)
        {
            return typeInfo.Type.ContainingNamespace + "." + GetName(typeInfo);
        }

        private static string GetNamespaceName(this INamespaceSymbol namespaceSymbol, string name)
        {
            string? nextName = namespaceSymbol?.Name;
            return string.IsNullOrEmpty(nextName)
                ? name
                : GetNamespaceName(namespaceSymbol.ContainingNamespace, $"{nextName}.{name}");
        }

        public static bool TryCreateMethodNode(this IMethodSymbol methodSymbol, SemanticModel semanticModel,
            out MethodNode? methodNode)
        {
            methodNode = null;

            methodNode = methodSymbol.CreateMethodNode();
            return true;
        }

        public static MethodNode CreateMethodNode(this IMethodSymbol symbol)
        {
            string fullName =
                symbol.ContainingNamespace.GetNamespaceName($"{symbol.ContainingType.Name}.{symbol.Name}");

            (string name, string? type)[] args = symbol
                .Parameters
                .Select(x => (name: x.Name, type: x.Type.ToString()))
                .ToArray();

            string returnType = symbol.ReturnType.ToString() ?? "Unknown";

            string symbolName = symbol.Name;
            string[] modifiers = symbol.MapModifiers();

            return new MethodNode(fullName,
                symbolName,
                args,
                returnType,
                modifiers);
        }

        public static PropertyNode CreatePropertyNode(this IPropertySymbol symbol)
        {
            string fullName =
                symbol.ContainingNamespace.GetNamespaceName($"{symbol.ContainingType.Name}.{symbol.Name}");

            string returnType = symbol.Type.ToString() ?? "Unknown";

            return new PropertyNode(fullName,
                symbol.Name,
                returnType,
                symbol.MapModifiers());
        }
    }
}