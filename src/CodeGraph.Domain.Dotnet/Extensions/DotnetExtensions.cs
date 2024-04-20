﻿using CodeGraph.Domain.Graph.Nodes;
using Microsoft.CodeAnalysis;
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