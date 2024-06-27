namespace CodeGraph.Domain.Graph.Database.Repositories.Common
{
    public abstract class BaseRepository
    {
        protected string FullName(string fullName)
        {
            return string.IsNullOrEmpty(fullName) ? string.Empty : $" {{fullName: \"{fullName}\"}}";
        }

        protected string Name(string name)
        {
            return string.IsNullOrEmpty(name) ? string.Empty : $" {{name: \"{name}\"}}";
        }

        protected string Pk(string pk)
        {
            return string.IsNullOrEmpty(pk) ? string.Empty : $" {{pk: \"{pk}\"}}";
        }
    }
}