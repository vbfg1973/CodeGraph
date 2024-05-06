using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Dotnet.Tests.TestHelpers;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Dotnet.Tests.Walkers.CSharp.TypeDefinition
{
    public class TypeDefinitionWalkerInterfaceImplementationTests
    {
        private readonly string[] _path =
        {
            "CodeToTest", "CSharp", "InterfaceImplementation"
        };

        [Theory]
        [InlineData("BasicMethod.csharp")]
        [InlineData("GenericReturnType.csharp")]
        [InlineData("GenericReturnTypeSeparateNamespaces.csharp")]
        public async Task Given_Class_With_Methods_Fully_Implements_Interface(string fileName)
        {
            // Arrange
            (WalkerOptions walkerOptions, FileNode fileNode) =
                await WalkerTestHelpers.GetWalkerOptions(_path, fileName, true);
            List<TypeDeclarationSyntax> declarations = await GetDeclarations(walkerOptions);

            List<Triple> discoveredTriples = new();

            CSharpTypeDiscoveryWalker walker = new(fileNode, new ProjectNode("FakeProject"), walkerOptions,
                A.Fake<ILoggerFactory>());
            discoveredTriples.AddRange(walker.Walk());

            List<TripleHas> interfaceMethods =
                discoveredTriples.OfType<TripleHas>().Where(x => x.NodeA is InterfaceNode).ToList();
            List<TripleImplementationOf> implementations = discoveredTriples.OfType<TripleImplementationOf>().ToList();
            List<TripleInvocationOf> invocations = discoveredTriples.OfType<TripleInvocationOf>().ToList();

            interfaceMethods.Count().Should().Be(implementations.Count);

            if (invocations.Any()) interfaceMethods.Count().Should().Be(invocations.Count);

            HashSet<string> interfaceMethodPks = interfaceMethods.Select(x => x.NodeB.Pk).ToHashSet();
            HashSet<string> implementationPks = implementations.Select(x => x.NodeB.Pk).ToHashSet();

            interfaceMethodPks.SetEquals(implementationPks).Should().BeTrue();

            if (invocations.Any())
            {
                HashSet<string> invocationPks = invocations.Select(x => x.NodeB.Pk).ToHashSet();
                interfaceMethodPks.SetEquals(invocationPks).Should().BeTrue();
            }
        }

        private static async Task<List<TypeDeclarationSyntax>> GetDeclarations(WalkerOptions walkerOptions)
        {
            return (await walkerOptions
                    .DotnetOptions
                    .SyntaxTree
                    .GetRootAsync())
                .DescendantNodes()
                .OfType<TypeDeclarationSyntax>()
                .ToList();
        }
    }
}