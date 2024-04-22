using Microsoft.CodeAnalysis;

namespace CodeGraph.Domain.Dotnet.Abstract
{
    public class DotnetOptions(
        SyntaxTree syntaxTree,
        SemanticModel semanticModel,
        // Solution? solution = null,
        Project? project = null)
    {
        public SyntaxTree SyntaxTree { get; } = syntaxTree;
        public SemanticModel SemanticModel { get; } = semanticModel;
        // public Solution? Solution { get; } = solution;
        public Project? Project { get; } = project;
    }
}