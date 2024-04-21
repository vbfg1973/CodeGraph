using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Graph.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGraph.Domain.Tests.TestHelpers
{
    public static class WalkerTestHelpers
    {
        public static async Task<(DotnetOptions, FileNode)> GetCSharpCompilation(string path)
        {
            SyntaxTree tree = await GetCSharpSyntaxTree(path);
            CSharpCompilation compilation = CSharpCompilation.Create("CodeToTest")
                .AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location))
                .AddSyntaxTrees(tree);
            SemanticModel model = compilation.GetSemanticModel(tree);
            FileNode fileNode = new(path, Path.GetFileName(path));

            DotnetOptions dotnetOptions = new(tree, model);

            return (dotnetOptions, fileNode);
        }

        private static async Task<SyntaxTree> GetCSharpSyntaxTree(string path)
        {
            string code = await new StreamReader(path).ReadToEndAsync();
            return CSharpSyntaxTree.ParseText(code);
        }
    }
}