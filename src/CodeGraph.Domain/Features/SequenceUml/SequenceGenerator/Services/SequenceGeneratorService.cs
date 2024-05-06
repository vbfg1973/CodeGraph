using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Models;
using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Services.Abstract;
using CodeGraph.Domain.Graph.Database.Repositories;
using CodeGraph.Domain.Graph.QueryModels.Enums;
using CodeGraph.Domain.Graph.QueryModels.Queries;
using CodeGraph.Domain.Graph.QueryModels.Results;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Services
{
    public class SequenceGeneratorService : ISequenceGeneratorService
    {
        private readonly ILogger<SequenceGeneratorService> _logger;
        private readonly IMethodRepository _methodRepository;

        public SequenceGeneratorService(IMethodRepository methodRepository,
            ILogger<SequenceGeneratorService> logger)
        {
            _methodRepository = methodRepository;
            _logger = logger;
        }

        public async Task<MethodInvocationHierarchy> FindSequenceFromStartingMethod(string methodFullName)
        {
            _logger.LogTrace("{Method} {MethodFullName}", nameof(FindSequenceFromStartingMethod), methodFullName);
            MethodQueryResult methodQueryResult = await _methodRepository.LookupMethodByFullName(methodFullName);

            return await FindSequenceFromMethod(methodQueryResult);
        }

        private async Task<MethodInvocationHierarchy> FindSequenceFromMethod(MethodQueryResult methodQueryResult)
        {
            _logger.LogTrace("{Method} {MethodFullName} {Argument}", nameof(FindSequenceFromMethod),
                methodQueryResult.MethodFullName, nameof(MethodQueryResult));
            MethodInvocationHierarchy methodInvocationHierarchy = new(methodQueryResult);

            List<MethodInvocationQueryResult> invocations =
                await _methodRepository.MethodInvocations(new MethodInvocationQuery
                    { MethodPk = methodQueryResult.MethodPk });

            await ProcessInvocations(invocations, methodInvocationHierarchy);

            return methodInvocationHierarchy;
        }

        private async Task<MethodInvocationHierarchy> FindSequenceFromMethod(
            MethodInvocationQueryResult methodInvocationQueryResult)
        {
            _logger.LogTrace("{Method} {MethodFullName} {Argument}", nameof(FindSequenceFromMethod),
                methodInvocationQueryResult.InvokedMethodFullName, nameof(MethodInvocationQueryResult));

            MethodInvocationHierarchy methodInvocationHierarchy = new(methodInvocationQueryResult);

            List<MethodInvocationQueryResult> invocations = await _methodRepository.MethodInvocations(
                new MethodInvocationQuery { MethodPk = methodInvocationQueryResult.InvokedMethodPk });

            await ProcessInvocations(invocations, methodInvocationHierarchy);

            return methodInvocationHierarchy;
        }

        private async Task ProcessInvocations(List<MethodInvocationQueryResult> invocations,
            MethodInvocationHierarchy methodInvocationHierarchy)
        {
            foreach (MethodInvocationQueryResult invocation in invocations)
            {
                if (invocation.InvokedMethodPk == methodInvocationHierarchy.MethodPk)
                    continue;

                if (invocation.InvokedMethodOwnerType != ObjectType.Interface)
                {
                    methodInvocationHierarchy.MethodInvocations.Add(await FindSequenceFromMethod(invocation));
                }

                else
                {
                    MethodQueryResult methodFromInterfaceImplementation =
                        await GetMethodFromInterfaceImplementation(invocation);

                    methodInvocationHierarchy.MethodInvocations.Add(
                        await FindSequenceFromMethod(methodFromInterfaceImplementation));
                }
            }
        }

        private async Task<MethodQueryResult> GetMethodFromInterfaceImplementation(
            MethodInvocationQueryResult invocation)
        {
            _logger.LogTrace("{Method} {MethodFullName} {Argument}", nameof(GetMethodFromInterfaceImplementation),
                invocation.InvokedMethodFullName, nameof(MethodInvocationQuery));

            List<InterfaceMethodImplementationQueryResult> implementations = await _methodRepository
                .InterfaceMethodImplementations(new InterfaceImplementationQuery
                    { InterfaceMethodPk = invocation.InvokedMethodPk });

            if (implementations.Count > 1)
                throw new ArgumentOutOfRangeException(
                    $"Interface method {invocation.InvokedMethodFullName} has more that one implemetation");

            InterfaceMethodImplementationQueryResult impl = implementations.First();
            _logger.LogTrace("Looked for {InvokedMethodFullName} and found {ClassMethodFullName}",
                invocation.InvokedMethodFullName, impl.ClassMethodFullName);

            MethodQueryResult methodQueryResult =
                await _methodRepository.LookupMethodByPk(impl.ClassMethodPk);
            return methodQueryResult;
        }
    }
}