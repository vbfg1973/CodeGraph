using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using CodeGraph.Domain.Tests.TestHelpers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Tests.Walkers.CSharp.TypeDiscovery
{
    public class TypeDiscoveryWalkerTests
    {
        private readonly string[] _path =
        {
            "CodeToTest", "CSharp", "TypeDiscovery"
        };

        [Theory]
        [InlineData("ClassWithAbstractBaseClass.csharp")]
        [InlineData("ClassWithoutInterface.csharp")]
        [InlineData("ClassWithInterface.csharp")]
        [InlineData("ClassWithInterfaceDefinedMethods.csharp")]
        [InlineData("HasGenericCustomInterface.csharp")]
        [InlineData("HasGenericInBuiltInterface.csharp")]
        [InlineData("RecordDefinition.csharp")]
        [InlineData("InterfaceDerivingFromInterface.csharp")]
        public async Task Given_File_With_Class_Definition_At_Least_One_TripleDeclaredAt(string fileName)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) = await GetWalkerOptions(fileName);

            // Act
            CSharpTypeDiscoveryWalker walker = new(fileNode, walkerOptions);
            List<TripleDeclaredAt> results = walker.Walk().OfType<TripleDeclaredAt>().ToList();

            // Assert
            results.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("ClassWithAbstractBaseClass.csharp")]
        [InlineData("ClassWithoutInterface.csharp")]
        [InlineData("ClassWithInterface.csharp")]
        [InlineData("ClassWithInterfaceDefinedMethods.csharp")]
        [InlineData("HasGenericCustomInterface.csharp")]
        [InlineData("HasGenericInBuiltInterface.csharp")]
        [InlineData("RecordDefinition.csharp")]
        [InlineData("InterfaceDerivingFromInterface.csharp")]
        public async Task Given_File_With_Class_Definition_No_Triples_Have_Null_Nodes(string fileName)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) = await GetWalkerOptions(fileName);

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

        [Theory]
        [InlineData("ClassWithAbstractBaseClass.csharp", "Class", "Class")]
        [InlineData("ClassWithInterface.csharp", "Class", "Interface")]
        [InlineData("ClassWithInterfaceDefinedMethods.csharp", "Class", "Interface")]
        [InlineData("HasGenericCustomInterface.csharp", "Class", "Interface")]
        [InlineData("HasGenericInBuiltInterface.csharp", "Class", "Interface")]
        [InlineData("InterfaceDerivingFromInterface.csharp", "Interface", "Interface")]
        public async Task Given_File_With_Class_Definition_Derived_From_BaseType_Found_BaseType_Is_Correct(
            string fileName, string subType, string parentType)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) = await GetWalkerOptions(fileName);

            // Act
            CSharpTypeDiscoveryWalker walker = new(fileNode, walkerOptions);
            List<TripleOfType> results = walker.Walk().OfType<TripleOfType>().ToList();

            // Assert
            results.Count.Should().Be(1);

            results.First().NodeA.Label.Should().Be(subType);
            results.First().NodeB.Label.Should().Be(parentType);
        }

        private async Task<(WalkerOptions, FileNode)> GetWalkerOptions(string fileName)
        {
            (DotnetOptions dotnetOptions, FileNode fileNode) = await
                WalkerTestHelpers.GetCSharpCompilation(Path.Combine(Path.Combine(_path), fileName));

            ICodeWalkerFactory codeWalkerFactory = A.Fake<ICodeWalkerFactory>();
            ILoggerFactory loggerFactory = A.Fake<ILoggerFactory>();
            WalkerOptions walkerOptions = new(dotnetOptions, codeWalkerFactory, loggerFactory);

            return (walkerOptions, fileNode);
        }
    }
}