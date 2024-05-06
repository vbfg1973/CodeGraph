using CodeGraph.Domain.Features.ImportSolution;
using CodeGraph.Domain.Features.SequenceUml;
using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator;
using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Services;
using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace CodeGraph.Domain
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddFeatureCommandLineVerbs(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ImportSolutionVerb>();
            serviceCollection.AddTransient<SequenceUmlVerb>();

            return serviceCollection;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISequenceGeneratorService, SequenceGeneratorService>();

            return serviceCollection;
        }
    }
}