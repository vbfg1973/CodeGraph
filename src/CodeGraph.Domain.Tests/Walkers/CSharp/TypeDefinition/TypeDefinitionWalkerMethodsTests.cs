using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using CodeGraph.Domain.Tests.TestHelpers;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Tests.Walkers.CSharp.TypeDefinition
{
    public class TypeDefinitionWalkerMethodTests
    {
        private readonly string[] _path =
        {
            "CodeToTest", "CSharp", "TypeDefinition"
        };

        [Theory]
        [InlineData("ClassWithCustomMethodReturnType.csharp", 1)]
        [InlineData("ClassWithBuiltInMethodReturnType.csharp", 1)]
        public async Task Given_Class_With_Properties_Correct_Number_Found(string fileName, int expectedPropertyCount)
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

            CSharpTypeDefinitionWalker walker = new(declaration, walkerOptions);
            List<TripleHas> results = walker.Walk().OfType<TripleHas>().Where(x => x.NodeB is MethodNode).ToList();
            results.Count().Should().Be(expectedPropertyCount);
        }

        [Theory]
        [InlineData("ClassWithCustomMethodReturnType.csharp",
            "CodeGraph.Domain.Tests.CodeToTest.CSharp.TypeDefinition.CustomPropertyType")]
        [InlineData("ClassWithBuiltInMethodReturnType.csharp",
            "string")]
        public async Task Given_Class_With_Properties_Correct_Return_Types(string fileName, string expectedReturnType)
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
        
            CSharpTypeDefinitionWalker walker = new(declaration, walkerOptions);
            List<MethodNode> propertyNodes = walker.Walk()
                .OfType<TripleHas>()
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                .Where(x => x.NodeB != null)
                .Where(x => x.NodeB is MethodNode)
                .Select(x => x.NodeB as MethodNode)
                .ToList()!;
        
            propertyNodes.First().ReturnType.Should().Be(expectedReturnType);
        }
    }
}