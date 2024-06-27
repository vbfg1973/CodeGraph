using System.Diagnostics;
using System.Text.Json;
using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Models;
using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Services.Abstract;
using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Services.Strategies;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CodeGraph.Domain.Features.SequenceUml
{
    public class SequenceUmlVerb(ISequenceGeneratorService sequenceGeneratorService, ILogger<SequenceUmlVerb> logger)
    {
        public async Task Run(SequenceUmlOptions options)
        {
            logger.LogInformation("{Method}", nameof(Run));

            Stopwatch sw = new();
            sw.Start();

            MethodInvocationHierarchy methodInvocationHierarchy =
                await sequenceGeneratorService.FindSequenceFromStartingMethod(options.StartingMethodFullName);

            Console.WriteLine(JsonSerializer.Serialize(methodInvocationHierarchy,
                new JsonSerializerOptions { WriteIndented = true }));

            ISequenceUmlGenerator sequenceUmlGenerator = new PlantUmlSequenceUmlGenerator();

            string umlDescription = await sequenceUmlGenerator.GenerateUmlDescription(methodInvocationHierarchy);

            Console.WriteLine(umlDescription);

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}