using System.Text.Json;
using CodeGraph.Domain.Dotnet;
using CodeGraph.Domain.Dotnet.Analyzers;
using CodeGraph.Domain.Dotnet.OriginalImplementation;
using CodeGraph.Domain.Graph.Database;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CodeGraph
{
    public static class Program
    {
        private static IConfiguration s_configuration;
        private static IServiceCollection s_serviceCollection;
        private static IServiceProvider s_serviceProvider;

        internal static void Main(string[] args)
        {
            BuildConfiguration();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(s_configuration)
                .CreateLogger();
            ConfigureServices();

            ParseCommandLine(args);
        }

        private static void ParseCommandLine(string[] args)
        {
        }

        private static void ConfigureServices()
        {
            s_serviceCollection = new ServiceCollection();

            // var appSettings = new AppSettings();
            // s_configuration.Bind("Settings", appSettings);

            s_serviceCollection.AddLogging(configure => configure.AddSerilog());


            s_serviceProvider = s_serviceCollection.BuildServiceProvider();
        }
        
        private static void BuildConfiguration()
        {
            ConfigurationBuilder configuration = new();

            s_configuration = configuration.AddJsonFile("appsettings.json", true, true)
                // .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static async Task OldMain(string[] args)
        {
            string solutionPath = args[0];

            try
            {
                AnalysisConfig analysisConfig = new(solutionPath);
                Analyzer analyzer = new(analysisConfig);
                IList<Triple> triples = await analyzer.Analyze();

                // Console.WriteLine(JsonSerializer.Serialize(triples,
                //     new JsonSerializerOptions { WriteIndented = true }));

                CredentialsConfig creds = new("neo4j:neo4j:AdminPassword");
                await DbManager.InsertData(triples, creds, true);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}