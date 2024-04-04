namespace CodeGraph.Domain.Dotnet
{
    public class AnalysisConfig
    {
        public AnalysisConfig(string solution, string csvFile)
        {
            Solution = solution;
            CsvFile = csvFile;
        }

        public string Solution { get; }
        public string CsvFile { get; }
    }
}