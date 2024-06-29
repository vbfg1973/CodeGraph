using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Dotnet.Tests.TestHelpers;
using CodeGraph.Domain.Dotnet.Tests.Walkers.CognitiveComplexity.ClassData;
using CodeGraph.Domain.Dotnet.Walkers.CSharp;
using CodeGraph.Domain.Dotnet.Walkers.VisualBasic;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using CognitiveComplexityHelpers = CodeGraph.Domain.Dotnet.Tests.Walkers.Utilities.CognitiveComplexityHelpers;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CognitiveComplexity
{
    public class CognitiveComplexityTests
    {
        private readonly string[] _path = { "CodeToTest", "TestClasses" };

        [Theory]
        [ClassData(typeof(CatchClauseCSharp))]
        [ClassData(typeof(CatchClauseVisualBasic))]
        [ClassData(typeof(BinaryExpressionCSharp))]
        [ClassData(typeof(BinaryExpressionVisualBasic))]
        [ClassData(typeof(DoWhileLoopCSharp))]
        [ClassData(typeof(DoWhileLoopVisualBasic))]
        [ClassData(typeof(ForeachLoopCSharp))]
        [ClassData(typeof(ForeachLoopVisualBasic))]
        [ClassData(typeof(ForLoopCSharp))]
        [ClassData(typeof(ForLoopVisualBasic))]
        [ClassData(typeof(GotoCSharp))]
        [ClassData(typeof(GotoVisualBasic))]
        [ClassData(typeof(IfElseCSharp))]
        [ClassData(typeof(IfElseVisualBasic))]
        [ClassData(typeof(LambdaCSharp))]
        [ClassData(typeof(LambdaVisualBasic))]
        [ClassData(typeof(MethodCSharp))]
        [ClassData(typeof(MethodVisualBasic))]
        [ClassData(typeof(SwitchCSharp))]
        [ClassData(typeof(SwitchVisualBasic))]
        [ClassData(typeof(WhileLoopCSharp))]
        [ClassData(typeof(WhileLoopVisualBasic))]
        public async Task GivenClassMethodHasCorrectCognitiveComplexity(string fileName, string methodName,
            int expectedComplexityScore, Language language)
        {
            SyntaxNode treeRoot = CognitiveComplexityHelpers.ParseSyntaxTreeRoot(
                Path.Combine(Path.Combine(_path), string.Join(".", fileName)),
                language);

            int complexityScore = await GetComplexityScore(treeRoot, methodName, fileName, language);

            complexityScore
                .Should()
                .Be(expectedComplexityScore);
        }

        private async Task<int> GetComplexityScore(SyntaxNode syntaxNode, string methodName, string fileName,
            Language language)
        {
            return language switch
            {
                Language.CSharp => await GetCSharpComplexityScore(syntaxNode, methodName, fileName),
                Language.VisualBasic => await GetVisualBasicComplexityScore(syntaxNode, methodName, fileName),
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            };
        }

        private async Task<int> GetCSharpComplexityScore(SyntaxNode syntaxNode, string methodName, string fileName)
        {
            MethodDeclarationSyntax methodDeclarationSyntax = syntaxNode
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .First(x => x.Identifier.ToString() == methodName);

            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) =
                await WalkerTestHelpers.GetWalkerOptions(_path, fileName);

            CSharpCognitiveComplexityWalker analyzer = new(methodDeclarationSyntax, walkerOptions);

            return analyzer.ComplexityScore;
        }

        private async Task<int> GetVisualBasicComplexityScore(SyntaxNode syntaxNode, string methodName, string fileName)
        {
            MethodBlockSyntax methodDeclarationSyntax = syntaxNode
                .DescendantNodes()
                .OfType<MethodBlockSyntax>()
                .First(x => x.SubOrFunctionStatement.Identifier.ToString() == methodName);

            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) =
                await WalkerTestHelpers.GetWalkerOptions(_path, fileName);

            VisualBasicCognitiveComplexityAnalyzer analyzer = new(methodDeclarationSyntax);

            return analyzer.ComplexityScore;
        }
    }
}