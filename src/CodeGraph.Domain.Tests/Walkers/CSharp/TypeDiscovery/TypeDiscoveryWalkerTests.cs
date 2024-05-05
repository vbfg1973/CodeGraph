using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples;
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
            (WalkerOptions walkerOptions, FileNode fileNode) =
                await WalkerTestHelpers.GetWalkerOptions(_path, fileName);

            // Act
            CSharpTypeDiscoveryWalker walker = new(fileNode, walkerOptions, A.Fake<ILoggerFactory>());
            List<TripleDeclaredAt> results = walker.Walk().OfType<TripleDeclaredAt>().ToList();

            // Assert
            results.Should().NotBeEmpty();
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
            (WalkerOptions walkerOptions, FileNode fileNode) =
                await WalkerTestHelpers.GetWalkerOptions(_path, fileName);

            // Act
            CSharpTypeDiscoveryWalker walker = new(fileNode, walkerOptions, A.Fake<ILoggerFactory>());
            List<TripleOfType> results = walker.Walk().OfType<TripleOfType>().ToList();

            // Assert
            results.Count.Should().Be(1);

            results.First().NodeA.Label.Should().Be(subType);
            results.First().NodeB.Label.Should().Be(parentType);
        }
    }
}