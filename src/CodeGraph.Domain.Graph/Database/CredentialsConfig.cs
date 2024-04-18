namespace CodeGraph.Domain.Graph.Database
{
    public class CredentialsConfig
    {
        public CredentialsConfig(string credentials)
        {
            if (string.IsNullOrEmpty(credentials)) return;
            string[] args = credentials.Split(":");
            if (args.Length != 3) return;
            Database = args[0];
            User = args[1];
            Password = args[2];
        }

        public string Database { get; }
        public string User { get; }
        public string Password { get; }
    }
}