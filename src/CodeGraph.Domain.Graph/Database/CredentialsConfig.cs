namespace CodeGraph.Domain.Graph.Database
{
    public class CredentialsConfig
    {
        public CredentialsConfig(string credentials)
        {
            if (string.IsNullOrEmpty(credentials)) return;
            string[] args = credentials.Split(";");
            if (args.Length != 4) return;
            Host = args[0];
            Database = args[1];
            UserName = args[2];
            Password = args[3];
        }

        public string Host { get; }
        public string Database { get; }
        public string UserName { get; }
        public string Password { get; }
    }
}