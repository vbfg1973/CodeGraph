using CodeGraph.Domain.Graph.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGraph.Domain.Tests.TestHelpers
{
    public static class WalkerTestHelpers
    {
        public static (Compilation, SyntaxTree, SemanticModel, FileNode) GetCSharpCompilation(string path)
        {
            SyntaxTree tree = GetSyntaxTree(path);
            CSharpCompilation compilation = CSharpCompilation.Create("CodeToTest")
                .AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location))
                .AddSyntaxTrees(tree);
            SemanticModel model = compilation.GetSemanticModel(tree);
            FileNode fileNode = new(path, Path.GetFileName(path));

            return (compilation, tree, model, fileNode);
        }

        private static SyntaxTree GetSyntaxTree(string path)
        {
            string code = new StreamReader(path).ReadToEnd();
            return CSharpSyntaxTree.ParseText(code);
        }
    }
}