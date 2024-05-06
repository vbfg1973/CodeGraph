using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Models;

namespace CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Services.Abstract
{
    public interface ISequenceGeneratorService
    {
        Task<MethodInvocationHierarchy> FindSequenceFromStartingMethod(string methodFullName);
    }
}