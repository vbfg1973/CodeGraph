namespace CodeGraph.Domain.Graph.Database.Repositories.FileSystem.Queries
{
    public class FolderQueryByPk
    {
        public string Pk { get; init; } = null!;
    }
    
    public class FolderQueryByFullName
    {
        public string FullName { get; init; } = null!;
    }
}