using System.Text.Json;
using CodeGraph.Domain;
using CodeGraph.Domain.Features.ImportSolution;
using CodeGraph.Domain.Features.SequenceUml;
using CodeGraph.Domain.Graph;
using CodeGraph.Domain.Graph.Database;
using CommandLine;
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

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Log.Logger.Error(e.ExceptionObject as Exception, "UnhandledException");
            };

            ConfigureServices();

            ParseArgumentsIntoCommandLineVerbs(args);

            Log.CloseAndFlush();
        }

        private static void ParseArgumentsIntoCommandLineVerbs(string[] args)
        {
            Parser.Default
                .ParseArguments<
                    ImportSolutionOptions,
                    SequenceUmlOptions
                >(args)
                .WithParsed<ImportSolutionOptions>(options =>
                {
                    ImportSolutionVerb? verb = s_serviceProvider.GetService<ImportSolutionVerb>();

                    verb?.Run(options)
                        .Wait();
                })
                .WithParsed<SequenceUmlOptions>(options =>
                {
                    SequenceUmlVerb? verb = s_serviceProvider.GetService<SequenceUmlVerb>();

                    verb?.Run(options)
                        .Wait();
                })
                ;
        }

        private static void ConfigureServices()
        {
            s_serviceCollection = new ServiceCollection();

            // AppSettings appSettings = new AppSettings();
            // s_configuration.Bind("Settings", appSettings);

            s_serviceCollection.AddLogging(configure => configure.AddSerilog());

            // Log.Debug("{AppSettings}", JsonSerializer.Serialize(appSettings));

            s_serviceCollection.AddSingleton(new CredentialsConfig("neo4j://localhost:7687;neo4j;neo4j;AdminPassword"));

            s_serviceCollection.AddDatabase();

            s_serviceCollection.AddDomainServices();
            s_serviceCollection.AddFeatureCommandLineVerbs();

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
    }
}