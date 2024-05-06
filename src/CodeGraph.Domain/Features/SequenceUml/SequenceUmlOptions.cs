using CommandLine;

namespace CodeGraph.Domain.Features.SequenceUml
{
    [Verb("sequence", HelpText = "Generate Sequence UML from nominated starting point")]
    public class SequenceUmlOptions
    {
        [Option('m', nameof(StartingMethodFullName), HelpText = "Fullname of the start point method of the sequence. Form: {fullyQualifiedNamespace}.{className}.{methodName}", Required = true)]
        public string StartingMethodFullName { get; set; }
    }
}