namespace CodeGraph.Api.Config
{
    public static class EnvironmentUtility
    {
        private const string EnvironmentTypeVariableName = "EnvironmentType";

        public static string GetEnvironmentName()
        {
            ValidEnvironmentTypes env = GetEnvVar();
            return env switch
            {
                ValidEnvironmentTypes.Local => "local",
                ValidEnvironmentTypes.DockerCompose => "dockercompose",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static ValidEnvironmentTypes GetEnvVar()
        {
            string env = (Environment.GetEnvironmentVariable(EnvironmentTypeVariableName) ?? string.Empty).ToLower();

            return env switch
            {
                "dockercompose" => ValidEnvironmentTypes.DockerCompose,
                _ => ValidEnvironmentTypes.Local
            };
        }

        private enum ValidEnvironmentTypes
        {
            Local,
            DockerCompose
        }
    }
}