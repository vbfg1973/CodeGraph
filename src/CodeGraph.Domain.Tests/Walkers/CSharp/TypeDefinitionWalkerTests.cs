using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Tests.TestHelpers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Tests.Walkers.CSharp
{
    public class TypeDefinitionWalkerTests
    {
        private readonly string[] _path =
        {
            "CodeToTest", "CSharp", "TypeDefinition"
        };

        [Theory]
        [InlineData("ClassWithProperties.csharp", 1)]
        [InlineData("ClassWithCustomPropertyTypes.csharp", 1)]
        public async Task Given_Class_With_Properties_Correct_Number_Found(string fileName, int expectedPropertyCount)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) = GetWalkerOptions(fileName);
            TypeDeclarationSyntax declaration = (await walkerOptions
                    .DotnetOptions
                    .SyntaxTree
                    .GetRootAsync())
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .First();

            CSharpTypeDefinitionWalker walker = new(declaration, walkerOptions);
            List<TripleHas> results = walker.Walk().OfType<TripleHas>().Where(x => x.NodeB is PropertyNode).ToList();
            results.Count().Should().Be(expectedPropertyCount);
        }

        [Theory]
        [InlineData("ClassWithProperties.csharp", "string")]
        [InlineData("ClassWithCustomPropertyTypes.csharp",
            "CodeGraph.Domain.Tests.CodeToTest.CSharp.CustomPropertyType")]
        public async Task Given_Class_With_Properties_Correct_Return_Types(string fileName, string expectedPropertyType)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) = GetWalkerOptions(fileName);
            TypeDeclarationSyntax declaration = (await walkerOptions
                    .DotnetOptions
                    .SyntaxTree
                    .GetRootAsync())
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .First();

            CSharpTypeDefinitionWalker walker = new(declaration, walkerOptions);
            List<PropertyNode> propertyNodes = walker.Walk()
                .OfType<TripleHas>()
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                .Where(x => x.NodeB != null)
                .Where(x => x.NodeB is PropertyNode)
                .Select(x => x.NodeB as PropertyNode)
                .ToList()!;

            propertyNodes.First().ReturnType.Should().Be(expectedPropertyType);
        }

        private (WalkerOptions, FileNode) GetWalkerOptions(string fileName)
        {
            (DotnetOptions dotnetOptions, FileNode fileNode) =
                WalkerTestHelpers.GetCSharpCompilation(Path.Combine(Path.Combine(_path), fileName));

            ICodeWalkerFactory codeWalkerFactory = A.Fake<ICodeWalkerFactory>();
            ILoggerFactory loggerFactory = A.Fake<ILoggerFactory>();
            WalkerOptions walkerOptions = new(dotnetOptions, codeWalkerFactory, loggerFactory);

            return (walkerOptions, fileNode);
        }
    }
}