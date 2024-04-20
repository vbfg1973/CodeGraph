using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.CSharp.Walkers;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Tests.TestHelpers;
using FakeItEasy;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Tests.Walkers.CSharp
{
    public class TypeDefinitionWalkerTests
    {
        private readonly string[] _path =
        {
            "CodeToTest", "CSharp"
        };

        [Theory]
        [InlineData("ClassWithInterfaceDefinedMethods.csharp")]
        public async Task Given(string fileName)
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

            var walker = new CSharpTypeDefinitionWalker(declaration, walkerOptions);

            var results = walker.Walk();
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