using CodeGraph.Domain.Graph.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.Extensions
{
    public static class DotnetExtensions
    {
        public static string[] MapModifiers(this SyntaxTokenList syntaxTokens)
        {
            return syntaxTokens.Select(x => x.ValueText).ToArray();
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

            SyntaxReference? syntaxReference = methodSymbol
                .DeclaringSyntaxReferences
                .FirstOrDefault();

            if (syntaxReference == null) return false;

            MethodDeclarationSyntax declarationSyntax = (syntaxReference.GetSyntax() as MethodDeclarationSyntax)!;
            IMethodSymbol symbol = semanticModel.GetDeclaredSymbol(declarationSyntax)!;
            methodNode = symbol.CreateMethodNode(declarationSyntax);
            return true;
        }

        public static MethodNode CreateMethodNode(this IMethodSymbol symbol,
            MethodDeclarationSyntax declaration)
        {
            string fullName =
                symbol.ContainingNamespace.GetNamespaceName($"{symbol.ContainingType.Name}.{symbol.Name}");

            (string name, string? type)[] args = symbol
                .Parameters
                .Select(x => (name: x.Name, type: x.Type.ToString()))
                .ToArray();

            string returnType = symbol.ReturnType.ToString() ?? "Unknown";

            string symbolName = symbol.Name;
            string[] modifiers = declaration.Modifiers.MapModifiers();

            return new MethodNode(fullName,
                symbolName,
                args,
                returnType,
                modifiers);
        }

        public static PropertyNode CreatePropertyNode(this IPropertySymbol symbol,
            PropertyDeclarationSyntax declaration = null!)
        {
            string fullName =
                symbol.ContainingNamespace.GetNamespaceName($"{symbol.ContainingType.Name}.{symbol.Name}");

            string returnType = symbol.Type.ToString() ?? "Unknown";

            return new PropertyNode(fullName,
                symbol.Name,
                returnType,
                declaration.Modifiers.MapModifiers());
        }
    }
}