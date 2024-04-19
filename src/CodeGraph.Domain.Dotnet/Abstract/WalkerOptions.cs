using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Dotnet.Abstract
{
    public class WalkerOptions(
        SyntaxTree syntaxTree,
        SemanticModel semanticModel,
        ICodeWalkerFactory codeWalkerFactory,
        ILoggerFactory loggerFactory)
    {
        public SyntaxTree SyntaxTree { get; } = syntaxTree;
        public SemanticModel SemanticModel { get; } = semanticModel;
        public ICodeWalkerFactory CodeWalkerFactory { get; } = codeWalkerFactory;
        public ILoggerFactory LoggerFactory { get; } = loggerFactory;
    }
}