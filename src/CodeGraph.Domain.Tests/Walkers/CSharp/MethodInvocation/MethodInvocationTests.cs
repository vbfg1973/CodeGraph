using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Tests.TestHelpers;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Tests.Walkers.CSharp.MethodInvocation
{
    public class MethodInvocationTests
    {
        private readonly string[] _path =
        {
            "CodeToTest", "CSharp", "MethodInvocation"
        };
        
        [Theory]
        [InlineData("ClassWithSelfInvocation.csharp", 1)]
        [InlineData("ClassWithInvocationFromAnotherClass.csharp", 1)]
        [InlineData("ClassWithMultipleSelfInvocation.csharp", 2)]
        public async Task Given_Class_With_Method_Invocations_Correct_Number_Is_Found(string fileName, int expectedInvocationCount)
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

            CSharpMethodInvocationWalker walker = new(declaration, walkerOptions);
            List<TripleInvoke> results = walker.Walk().OfType<TripleInvoke>().Where(x => x.NodeB is MethodNode).ToList();
            results.Count().Should().Be(expectedInvocationCount);
        }

        [Theory]
        [InlineData("ClassWithInvocationFromAnotherClass.csharp", 1)]
        public async Task Given_Class_With_Object_Creations_Correct_Number_Is_Found(string fileName, int expectedObjectConstructions)
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
            
            CSharpMethodInvocationWalker walker = new(declaration, walkerOptions);
            var results = walker.Walk().OfType<TripleConstruct>().ToList();
            results.Count().Should().Be(expectedObjectConstructions);
        }
    }
}