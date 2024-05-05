namespace CodeGraph.Domain.Features.SequenceUml.SequenceGenerator
{
    public class MethodInvocationHierarchy(
        string parentTypeFullname,
        string parentTypePk,
        string methodFullName,
        string methodPk)
    {
        public string ParentTypeFullname { get; } = parentTypeFullname;
        public string ParentTypePk { get; } = parentTypePk;
        public string MethodFullName { get; } = methodFullName;
        public string MethodPk { get; } = methodPk;

        public List<MethodInvocationHierarchy> MethodInvocations { get; } = new();
    }
}