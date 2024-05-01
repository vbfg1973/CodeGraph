namespace CodeGraph.Domain.Dotnet.Abstract
{
    public class WalkerOptions(
        DotnetOptions dotnetOptions,
        // ICodeWalkerFactory codeWalkerFactory,
        // ILoggerFactory loggerFactory,
        bool descendIntoSubWalkers = false)
    {
        public DotnetOptions DotnetOptions { get; } = dotnetOptions;

        // public ICodeWalkerFactory CodeWalkerFactory { get; } = codeWalkerFactory;
        // public ILoggerFactory LoggerFactory { get; } = loggerFactory;
        public bool DescendIntoSubWalkers { get; } = descendIntoSubWalkers;
    }
}