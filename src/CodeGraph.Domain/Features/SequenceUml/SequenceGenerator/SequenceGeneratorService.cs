using CodeGraph.Domain.Graph.Database.Repositories;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Domain.Features.SequenceUml.SequenceGenerator
{
    public class SequenceGeneratorService : ISequenceGeneratorService
    {
        private readonly IInterfaceRepository _interfaceRepository;
        private readonly ILogger<SequenceGeneratorService> _logger;
        private InterfaceRepository interfaceRepository;

        public SequenceGeneratorService(IInterfaceRepository interfaceRepository,
            ILogger<SequenceGeneratorService> logger)
        {
            _interfaceRepository = interfaceRepository;
            _logger = logger;
        }

        public async Task<MethodInvocationHierarchy> FindSequenceFromStartingMethod(string fullClassName, string fullMethodName, string methodPk)
        {
            throw new NotImplementedException();
        }
    }
}