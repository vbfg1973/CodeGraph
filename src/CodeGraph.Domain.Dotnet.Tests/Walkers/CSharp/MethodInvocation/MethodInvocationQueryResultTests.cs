using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Tests.TestHelpers;
using CodeGraph.Domain.Dotnet.Walkers.CSharp;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CSharp.MethodInvocation
{
    public class MethodInvocationQueryResultTests
    {
        private readonly string[] _path =
        {
            "CodeToTest", "CSharp", "MethodInvocation"
        };

        [Theory]
        [InlineData("ClassWithSelfInvocation.csharp", 1)]
        [InlineData("ClassWithInvocationFromAnotherClass.csharp", 1)]
        [InlineData("ClassWithMultipleSelfInvocation.csharp", 2)]
        public async Task Given_Class_With_Method_Invocations_Correct_Number_Is_Found(string fileName,
            int expectedInvocationCount)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) =
                await WalkerTestHelpers.GetWalkerOptions(_path, fileName);
            TypeDeclarationSyntax declaration = (await walkerOptions
                    .DotnetOptions
                    .SyntaxTree
                    .GetRootAsync())
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .First();

            CSharpMethodInvocationWalker walker = new(declaration, walkerOptions, A.Fake<ILoggerFactory>());

            List<Triple> triples = walker.Walk().ToList();
            List<TripleInvoke> results = triples.OfType<TripleInvoke>().ToList();
            results.Count().Should().Be(expectedInvocationCount);
        }

        [Theory]
        [InlineData("ClassWithInvocationFromAnotherClass.csharp", 1)]
        public async Task Given_Class_With_Object_Creations_Correct_Number_Is_Found(string fileName,
            int expectedObjectConstructions)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) =
                await WalkerTestHelpers.GetWalkerOptions(_path, fileName);
            TypeDeclarationSyntax declaration = (await walkerOptions
                    .DotnetOptions
                    .SyntaxTree
                    .GetRootAsync())
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .First();

            CSharpMethodInvocationWalker walker = new(declaration, walkerOptions, A.Fake<ILoggerFactory>());
            List<Triple> triples = walker.Walk().ToList();
            List<TripleConstruct> results = triples.OfType<TripleConstruct>().ToList();
            results.Count().Should().Be(expectedObjectConstructions);
        }
    }
}