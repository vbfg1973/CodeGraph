using CodeGraph.Domain.Graph.Database;

namespace CodeGraph.Api.Config
{
    public class AppSettings
    {
        public CredentialsConfig Database { get; set; } = null!;
    }
}