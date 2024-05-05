namespace CodeGraph.Domain.Features.SequenceUml.SequenceGenerator
{
    public interface ISequenceGeneratorService
    {
        Task<MethodInvocationHierarchy> FindSequenceFromStartingMethod(string methodFullName);
    }
}