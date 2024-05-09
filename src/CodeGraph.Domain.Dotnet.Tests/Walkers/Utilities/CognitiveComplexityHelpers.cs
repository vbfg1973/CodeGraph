using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Dotnet.Walkers.CSharp;
using CodeGraph.Domain.Dotnet.Walkers.VisualBasic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.Utilities
{
    public static class CognitiveComplexityHelpers
    {
        public static SyntaxNode ParseSyntaxTreeRoot(string path, Language language)
        {
            var text = File.ReadAllText(path);

            return language switch
            {
                Language.CSharp => CSharpSyntaxTree.ParseText(text).GetRoot(),
                Language.VisualBasic => VisualBasicSyntaxTree.ParseText(text).GetRoot(),
                _ => throw new ArgumentException("Unknown language", nameof(language))
            };
        }

        public static MethodDeclarationSyntax FindMethod(CSharpSyntaxNode syntaxNode, string methodName)
        {
            return syntaxNode
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .First(method => method.Identifier.ToString() == methodName);
        }

        public static MethodBlockSyntax FindMethod(VisualBasicSyntaxNode syntaxNode, string methodName)
        {
            return syntaxNode
                .DescendantNodes()
                .OfType<MethodBlockSyntax>()
                .First(method => method.SubOrFunctionStatement.Identifier.ToString() == methodName);
        }

        // public static IComplexityAnalyzer GetMethodAnalyzer(Language language, SyntaxNode syntaxNode)
        // {
        //     return language switch
        //     {
        //         Language.CSharp => new CSharpCognitiveComplexityWalker((MethodDeclarationSyntax)syntaxNode),
        //         Language.VisualBasic => new VisualBasicCognitiveComplexityAnalyzer((MethodBlockSyntax)syntaxNode),
        //         _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        //     };
        // }

        public static string FileNameFromClass(string className)
        {
            return string.Join(".", className, "cs");
        }
    }
}