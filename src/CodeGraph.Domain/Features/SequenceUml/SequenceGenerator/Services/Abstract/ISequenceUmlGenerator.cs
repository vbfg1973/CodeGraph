using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Models;

namespace CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Services.Abstract
{
    public interface ISequenceUmlGenerator
    {
        Task<string> GenerateUmlDescription(MethodInvocationHierarchy methodInvocationHierarchy);
    }
}