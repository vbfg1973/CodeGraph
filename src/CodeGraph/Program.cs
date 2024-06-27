using CodeGraph.Domain;
using CodeGraph.Domain.Features.FolderHierarchy;
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
                    FolderHierarchyOptions,
                    ImportSolutionOptions,
                    SequenceUmlOptions
                >(args)
                .WithParsed<FolderHierarchyOptions>(options =>
                {
                    FolderHierarchyVerb? verb = s_serviceProvider.GetService<FolderHierarchyVerb>();

                    verb?.Run(options)
                        .Wait();
                })
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

            // var appSettings = new AppSettings();
            // s_configuration.Bind("Settings", appSettings);

            s_serviceCollection.AddLogging(configure => configure.AddSerilog());

            CredentialsConfig credentialsConfig = new()
            {
                Host = "localhost",
                Port = 7687,
                Database = "neo4j",
                UserName = "neo4j",
                Password = "AdminPassword"
            };

            s_serviceCollection.AddSingleton(credentialsConfig);

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