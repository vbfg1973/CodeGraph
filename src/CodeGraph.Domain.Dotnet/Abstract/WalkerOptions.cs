using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Dotnet.Abstract
{
    public class DotnetOptions(
        SyntaxTree syntaxTree,
        SemanticModel semanticModel,
        Solution? solution = null,
        Project? project = null)
    {
        public SyntaxTree SyntaxTree { get; } = syntaxTree;
        public SemanticModel SemanticModel { get; } = semanticModel;
        public Solution? Solution { get; } = solution;
        public Project? Project { get; } = project;
    }

    public class WalkerOptions(
        DotnetOptions dotnetOptions,
        ICodeWalkerFactory codeWalkerFactory,
        ILoggerFactory loggerFactory)
    {
        public DotnetOptions DotnetOptions { get; } = dotnetOptions;
        public ICodeWalkerFactory CodeWalkerFactory { get; } = codeWalkerFactory;
        public ILoggerFactory LoggerFactory { get; } = loggerFactory;
    }
}