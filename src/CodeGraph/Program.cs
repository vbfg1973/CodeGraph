using CodeGraph.Domain.Features.ImportSolution;
using CodeGraph.Domain.Features.SequenceUml;
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
            ConfigureServices();

            ParseCommandLine(args);
        }

        private static void ParseCommandLine(string[] args)
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

            // var appSettings = new AppSettings();
            // s_configuration.Bind("Settings", appSettings);

            s_serviceCollection.AddLogging(configure => configure.AddSerilog());

            s_serviceCollection.AddTransient<ImportSolutionVerb>();
            s_serviceCollection.AddTransient<SequenceUmlVerb>();

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