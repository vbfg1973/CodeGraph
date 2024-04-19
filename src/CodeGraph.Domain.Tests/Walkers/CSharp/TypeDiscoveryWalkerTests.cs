using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples.Abstract;
using CodeGraph.Domain.Tests.TestHelpers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Tests.Walkers.CSharp
{
    public class TypeDiscoveryWalkerTests
    {
        private readonly string[] _path = new[]
        {
            "CodeToTest", "CSharp"
        };

        [Theory]
        [InlineData("ClassWithoutInterface.csharp")]
        public async Task Given_File_Type_Detected(string fileName)
        {
            (Compilation compilation, SyntaxTree syntaxTree, SemanticModel semanticModel, FileNode fileNode) =
                WalkerTestHelpers.GetCSharpCompilation(Path.Combine(Path.Combine(_path), fileName));

            ICodeWalkerFactory factory = A.Fake<ICodeWalkerFactory>();
            ILoggerFactory loggerFactory = A.Fake<ILoggerFactory>();
            WalkerOptions walkerOptions = new WalkerOptions(syntaxTree, semanticModel, factory, loggerFactory);

            CSharpTypeDiscoveryWalker walker = new CSharpTypeDiscoveryWalker(walkerOptions);

            List<Triple> results = walker.Walk().ToList();

            results.Should().NotBeEmpty();
        }
    }
}