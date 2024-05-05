using System.Diagnostics;
using System.Text.Json;
using CodeGraph.Domain.Graph.Database;
using CodeGraph.Domain.Graph.Database.Repositories;
using CodeGraph.Domain.Graph.Database.Repositories.Base;
using CodeGraph.Domain.Graph.QueryModels;
using CodeGraph.Domain.Graph.QueryModels.Enums;
using CodeGraph.Domain.Graph.QueryModels.Queries;
using CodeGraph.Domain.Graph.QueryModels.Results;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CodeGraph.Domain.Features.SequenceUml
{
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
            List<InterfaceMethodImplementationQueryResult> interfaceMethodImplementations =
                await interfaceRepository.InterfaceMethodImplementations();
            List<MethodInvocationQueryResult> methodInvocations = await interfaceRepository.MethodInvocations();
            sw.Stop();

            Console.WriteLine(interfaceMethodImplementations.Count);
            Console.WriteLine(methodInvocations.Count);

            int classMethodInvocations = methodInvocations
                .Count(x => x.InvokedMethodOwnerType == ObjectType.Class);

            int interfaceMethodInvocations = methodInvocations
                .Count(x => x.InvokedMethodOwnerType == ObjectType.Interface);

            Console.WriteLine($"Class Invocations: {classMethodInvocations}");
            Console.WriteLine($"Interface Invocations: {interfaceMethodInvocations}");

            foreach (var methodInvocation in methodInvocations.Where(x => x.InvokedMethodOwnerType == ObjectType.Interface))
            {
                var implementations = await interfaceRepository.InterfaceMethodImplementations(new InterfaceImplementationQuery()
                    { InterfaceMethodPk = methodInvocation.InvokedMethodPk});

                if (implementations.Count() == 1)
                {
                    InterfaceMethodImplementationQueryResult i = implementations.First();
                    Console.WriteLine($"Called: {string.Join(".", methodInvocation.InvokedMethodOwnerFullName, methodInvocation.InvokedMethodName)} - Actually called: {string.Join(".", i.ClassFullName, i.ClassMethodName)}");
                }

                else
                {
                    Console.WriteLine($"---------- More than one {implementations.Count()} implementation Called: {string.Join(".", methodInvocation.InvokedMethodOwnerFullName, methodInvocation.InvokedMethodName)} ----------");
                }
            }
            

            Console.WriteLine(sw.Elapsed);
        }
    }
}