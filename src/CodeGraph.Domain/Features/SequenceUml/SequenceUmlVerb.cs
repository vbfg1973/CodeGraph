using System.Text.Json;
using CodeGraph.Domain.Graph.Database;
using CodeGraph.Domain.Graph.Database.Repositories;
using CodeGraph.Domain.Graph.Database.Repositories.Base;
using CodeGraph.Domain.Graph.QueryModels;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Features.SequenceUml
{
    [Verb("SequenceUml", HelpText = "Generate Sequence UML from nominated starting point")]
    public class SequenceUmlOptions
    {
    }

    public class SequenceUmlVerb
    {
        private readonly ILoggerFactory _loggerFactory;

        public SequenceUmlVerb(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public async Task Run(SequenceUmlOptions options)
        {
            CredentialsConfig creds = new("neo4j://localhost:7687;neo4j;neo4j;AdminPassword");
            Neo4jDataAccess dataAccess = new(creds, _loggerFactory);

            InterfaceRepository interfaceRepository = new(dataAccess, _loggerFactory);

            List<InterfaceMethodImplementation> res = await interfaceRepository.Query();

            Console.WriteLine(JsonSerializer.Serialize(res, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine(res.Count);
        }
    }
}