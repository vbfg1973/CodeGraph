using CodeGraph.Domain.Dotnet.Analyzers.Code.CSharp;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples.Abstract;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGraph.Domain.Tests
{
    public class CSharpCodeAnalyzerTests
    {

        [Theory]
        [InlineData("ClassWithoutInterface.csharp")]
        [InlineData("ClassWithInterface.csharp")]
        [InlineData("HasGenericCustomInterface.csharp")]
        [InlineData("HasGenericInBuiltInterface.csharp")]
        [InlineData("ClassWithInterfaceDefinedMethods.csharp")]
        public async Task Given_SyntaxTree(string fileName)
        {
            SyntaxTree syntaxTree = GetSyntaxTree(fileName);
            SyntaxNode root = await syntaxTree.GetRootAsync();
            List<SyntaxNode> nodes = root.DescendantNodes().ToList();

            nodes.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("ClassWithoutInterface.csharp", 1)]
        [InlineData("ClassWithInterface.csharp", 3)]
        [InlineData("HasGenericCustomInterface.csharp", 3)]
        [InlineData("HasGenericInBuiltInterface.csharp", 7)]
        [InlineData("ClassWithInterfaceDefinedMethods.csharp", 5)]

        public async Task Given_Code_Analysis_Triples_Count_Is_Correct(string fileName, int tripleCount)
        {
            CSharpCodeAnalyzer codeAnalyser = GetCodeAnalyzer(fileName);

            codeAnalyser.Should().NotBeNull();

            IList<Triple> triples = await codeAnalyser.Analyze();

            triples.Count.Should().Be(tripleCount);
        }
        
        [Theory]
        [InlineData("ClassWithInterface.csharp")]
        [InlineData("ClassWithoutInterface.csharp")]
        [InlineData("HasGenericCustomInterface.csharp")]
        [InlineData("HasGenericInBuiltInterface.csharp")]
        [InlineData("ClassWithInterfaceDefinedMethods.csharp")]

        public async Task Given_Code_Analysis_No_Node_Is_Null(string fileName)
        {
            CSharpCodeAnalyzer codeAnalyser = GetCodeAnalyzer(fileName);

            codeAnalyser.Should().NotBeNull();

            IList<Triple> triples = await codeAnalyser.Analyze();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            triples.All(x => x.NodeA != null).Should().BeTrue();
            
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            triples.All(x => x.NodeB != null).Should().BeTrue();
        }

        private static CSharpCodeAnalyzer GetCodeAnalyzer(string fileName)
        {
            SyntaxTree tree = GetSyntaxTree(fileName);
            CSharpCompilation compilation = CSharpCompilation.Create("CodeToTest")
                .AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location))
                .AddSyntaxTrees(tree);
            SemanticModel model = compilation.GetSemanticModel(tree);
            FileNode fileNode = new FileNode(GetPath(fileName), fileName);

            return new CSharpCodeAnalyzer(tree, model, fileNode);
        }

        private static SyntaxTree GetSyntaxTree(string fileName)
        {
            string code = new StreamReader(GetPath(fileName)).ReadToEnd();
            return CSharpSyntaxTree.ParseText(code);
        }

        private static string GetPath(string fileName)
        {
            return Path.Combine("CodeToTest", fileName);
        }
    }
}