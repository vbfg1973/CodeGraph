﻿using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using CodeGraph.Domain.Tests.TestHelpers;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Tests.Walkers.CSharp.TypeDefinition
{
    public class TypeDefinitionWalkerPropertyTests
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
            List<PropertyNode> propertyNodes = walker.Walk()
                .OfType<TripleHas>()
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                .Where(x => x.NodeB != null)
                .Where(x => x.NodeB is PropertyNode)
                .Select(x => x.NodeB as PropertyNode)
                .ToList()!;

            propertyNodes.First().ReturnType.Should().Be(expectedPropertyType);
        }

        [Theory]
        [InlineData("ClassWithProperties.csharp")]
        [InlineData("ClassWithCustomPropertyTypes.csharp")]
        public async Task Given_File_With_Class_Definition_No_Triples_Have_Null_Nodes(string fileName)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) =
                await WalkerTestHelpers.GetWalkerOptions(_path, fileName);

            // Act
            CSharpTypeDiscoveryWalker walker = new(fileNode, walkerOptions);
            List<Triple> results = walker.Walk().ToList();

            // Assert
            results.Should().NotBeEmpty();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            results.Select(x => x.NodeA).Any(x => x == null).Should().BeFalse();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            results.Select(x => x.NodeB).Any(x => x == null).Should().BeFalse();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            results.Select(x => x.NodeA).Any(x => x.Label == null).Should().BeFalse();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            results.Select(x => x.NodeB).Any(x => x.Label == null).Should().BeFalse();
        }
    }
}