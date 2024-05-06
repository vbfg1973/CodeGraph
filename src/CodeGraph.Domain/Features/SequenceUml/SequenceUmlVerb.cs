using System.Diagnostics;
using System.Text.Json;
using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator;
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
                options: new JsonSerializerOptions { WriteIndented = true }));

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}