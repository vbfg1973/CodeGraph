using CodeGraph.Clients.Config;

namespace CodeGraph.UiServer.Config
{
    public class AppSettings
    {
        public CodeGraphApiSettings CodeGraphApi { get; init; } = null!;
    }
}