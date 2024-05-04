namespace CodeGraph.Domain.Dotnet
{
    public class AnalysisConfig
    {
        public AnalysisConfig(string solution)
        {
            Solution = solution;
        }

        public string Solution { get; }
    }
}