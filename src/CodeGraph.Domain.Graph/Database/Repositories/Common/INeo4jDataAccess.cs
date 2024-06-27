namespace CodeGraph.Domain.Graph.Database.Repositories.Common
{
    public interface INeo4jDataAccess : IAsyncDisposable
    {
        Task<List<string>> ExecuteReadListAsync(string query, string returnObjectKey,
            IDictionary<string, object>? parameters = null);

        Task<List<T>> ExecuteReadDictionaryAsync<T>(string query, string returnObjectKey,
            IDictionary<string, object>? parameters = null);

        Task<T> ExecuteReadScalarAsync<T>(string query, IDictionary<string, object>? parameters = null);

        Task<T> ExecuteWriteTransactionAsync<T>(string query, IDictionary<string, object>? parameters = null);
    }
}