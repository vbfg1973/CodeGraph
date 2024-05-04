using System.Diagnostics;
using CodeGraph.Domain.Graph.Database;
using CodeGraph.Domain.Graph.Database.Repositories;
using CodeGraph.Domain.Graph.Database.Repositories.Base;
using CodeGraph.Domain.Graph.QueryModels;
using CodeGraph.Domain.Graph.QueryModels.Enums;
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

            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<InterfaceMethodImplementation> interfaceMethodImplementations =
                await interfaceRepository.InterfaceMethodImplementations();
            List<MethodInvocation> methodInvocations = await interfaceRepository.MethodInvocations();
            sw.Stop();

            Console.WriteLine(interfaceMethodImplementations.Count);
            Console.WriteLine(methodInvocations.Count);

            int classMethodInvocations = methodInvocations
                .Count(x => x.InvokedMethodParentType == ObjectType.Class);

            int interfaceMethodInvocations = methodInvocations
                .Count(x => x.InvokedMethodParentType == ObjectType.Interface);

            Console.WriteLine($"Class Invocations: {classMethodInvocations}");
            Console.WriteLine($"Interface Invocations: {interfaceMethodInvocations}");

            foreach (MethodInvocation interfaceMethodInvocation in methodInvocations.Where(x =>
                         x.InvokedMethodParentType == ObjectType.Interface))
            {
                List<InterfaceMethodImplementation> candidates = interfaceMethodImplementations.Where(x =>
                    x.InterfaceName == interfaceMethodInvocation.InvokedMethodParent &&
                    x.InterfaceMethodName == interfaceMethodInvocation.InvokedMethod).ToList();
            }

            Console.WriteLine(sw.Elapsed);
        }
    }
}