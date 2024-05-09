using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.Extensions
{
    /// <summary>
    ///     Helpers for cognitive complexity analysers
    /// </summary>
    public static class CognitiveComplexityHelpers
    {
        /// <summary>
        ///     Tests if the syntax node is one of the nominated SyntaxKinds
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="syntaxKinds"></param>
        /// <returns></returns>
        internal static bool IsAnyKind(this SyntaxNode syntaxNode, IEnumerable<SyntaxKind> syntaxKinds)
        {
            return syntaxKinds.Contains((SyntaxKind)syntaxNode.RawKind);
        }

        /// <summary>
        ///     Removes the parentheses from a logical expression for ease of cognitive analysis. Assists with ensuring
        ///     a && b && c == 1 AND a && (b && c) also == 1
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal static SyntaxNode RemoveParentheses(this SyntaxNode node)
        {
            var current = node;
            while (current.IsAnyKind(new[] { SyntaxKind.ParenthesizedExpression, SyntaxKind.ParenthesizedPattern }))
                current = current.IsKind(SyntaxKind.ParenthesizedExpression)
                    ? ((ParenthesizedExpressionSyntax)current).Expression
                    : ((ParenthesizedPatternSyntax)current).Pattern;

            return current;
        }

        /// <summary>
        ///     Ensures a method invocation has the same number of arguments as the nominated amount. Assists with recursion
        ///     detection.
        /// </summary>
        /// <param name="invocationExpression"></param>
        /// <param name="argumentCount"></param>
        /// <returns></returns>
        public static bool ArgumentCountMatches(this InvocationExpressionSyntax invocationExpression, int argumentCount)
        {
            return invocationExpression.ArgumentList.Arguments.Count == argumentCount;
        }
    }
}