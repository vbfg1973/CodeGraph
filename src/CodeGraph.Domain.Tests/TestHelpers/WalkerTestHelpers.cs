using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Graph.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGraph.Domain.Tests.TestHelpers
{
    public static class WalkerTestHelpers
    {
        public static (DotnetOptions, FileNode) GetCSharpCompilation(string path)
        {
            SyntaxTree tree = GetCSharpSyntaxTree(path);
            CSharpCompilation compilation = CSharpCompilation.Create("CodeToTest")
                .AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location))
                .AddSyntaxTrees(tree);
            SemanticModel model = compilation.GetSemanticModel(tree);
            FileNode fileNode = new(path, Path.GetFileName(path));

            DotnetOptions dotnetOptions = new DotnetOptions(tree, model);
            
            return (dotnetOptions, fileNode);
        }

        private static SyntaxTree GetCSharpSyntaxTree(string path)
        {
            string code = new StreamReader(path).ReadToEnd();
            return CSharpSyntaxTree.ParseText(code);
        }
    }
}