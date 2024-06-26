﻿using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Tests.TestHelpers;
using CodeGraph.Domain.Dotnet.Tests.Walkers.CSharp.Global.ClassData;
using CodeGraph.Domain.Dotnet.Walkers.CSharp;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CSharp.Global
{
    public class AllFileDotnetTests
    {
        private readonly string[] _path =
        {
            "CodeToTest", "CSharp"
        };

        [Fact]
        public void Some_Test_Files_Are_Present()
        {
            List<string> paths = Directory.EnumerateFiles(Path.Combine(_path), "*.csharp", SearchOption.AllDirectories)
                .ToList();

            paths.Should().NotBeEmpty();
        }

        [Theory]
        [ClassData(typeof(CSharpFileDiscoveryClassData))]
        public async Task Given_CSharp_File_No_Triples_Have_Null_Nodes(string fullPath)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) = await WalkerTestHelpers.GetWalkerOptions(fullPath, true);

            // Act
            CSharpTypeDiscoveryWalker walker = new(fileNode, new ProjectNode("FakeProject"), walkerOptions,
                A.Fake<ILoggerFactory>());
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