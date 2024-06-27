namespace CodeGraph.UiServer.Config
{
    public class AppSettings
    {
        public CodeGraphApiSettings CodeGraphApi { get; init; } = null!;
    }

    public class CodeGraphApiSettings
    {
        public string BaseUri { get; init; } = null!;
    }
}