using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using CodeGraph.Domain.Tests.TestHelpers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Tests.Walkers.CSharp
{
    public class TypeDiscoveryWalkerTests
    {
        private readonly string[] _path =
        {
            "CodeToTest", "CSharp"
        };
        
        [Theory]
        [InlineData("ClassWithAbstractBaseClass.csharp")]
        [InlineData("ClassWithoutInterface.csharp")]
        [InlineData("ClassWithInterface.csharp")]
        [InlineData("ClassWithInterfaceDefinedMethods.csharp")]
        [InlineData("HasGenericCustomInterface.csharp")]
        [InlineData("HasGenericInBuiltInterface.csharp")]
        public async Task Given_File_With_Class_Definition_At_Least_One_TripleDeclaredAt(string fileName)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) = GetWalkerOptions(fileName);
            
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
        public async Task Given_File_With_Class_Definition_No_Triples_Have_Null_Nodes(string fileName)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) = GetWalkerOptions(fileName);
            
            // Act
            CSharpTypeDiscoveryWalker walker = new(fileNode, walkerOptions);
            List<Triple> results = walker.Walk().ToList();

            // Assert
            results.Should().NotBeEmpty();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            results.Select(x => x.NodeA).Any(x => x == null).Should().BeFalse();
            
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            results.Select(x => x.NodeB).Any(x => x == null).Should().BeFalse();
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