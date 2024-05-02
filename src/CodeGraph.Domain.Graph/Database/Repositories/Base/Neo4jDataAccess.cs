using System.Text.Json;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace CodeGraph.Domain.Graph.Database.Repositories.Base
{
    public sealed class Neo4jDataAccess : INeo4jDataAccess
    {
        private readonly string _database;

        private readonly ILogger<Neo4jDataAccess> _logger;
        private readonly IAsyncSession _session;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Neo4jDataAccess" /> class.
        /// </summary>
        public Neo4jDataAccess(CredentialsConfig credentialsConfig, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Neo4jDataAccess>();
            _database = credentialsConfig.Database ?? "neo4j";
            IDriver driver = GraphDatabase.Driver(credentialsConfig.Host, AuthTokens.Basic(credentialsConfig.UserName, credentialsConfig.Password));

            _session = driver.AsyncSession(o => o.WithDatabase(_database));
        }

        /// <summary>
        ///     Execute read list as an asynchronous operation.
        /// </summary>
        public async Task<List<string>> ExecuteReadListAsync(string query, string returnObjectKey,
            IDictionary<string, object>? parameters = null)
        {
            return await ExecuteReadTransactionAsync<string>(query, returnObjectKey, parameters);
        }

        /// <summary>
        ///     Execute read dictionary as an asynchronous operation.
        /// </summary>
        public async Task<List<T>> ExecuteReadDictionaryAsync<T>(string query,
            string returnObjectKey, IDictionary<string, object>? parameters = null)
        {
            return await ExecuteReadTransactionAsync<T>(query, returnObjectKey, parameters);
        }

        /// <summary>
        ///     Execute read scalar as an asynchronous operation.
        /// </summary>
        public async Task<T> ExecuteReadScalarAsync<T>(string query, IDictionary<string, object>? parameters = null)
        {
            try
            {
                parameters = parameters ?? new Dictionary<string, object>();

                T result = await _session.ExecuteReadAsync(async tx =>
                {
                    T scalar = default;
                    IResultCursor? res = await tx.RunAsync(query, parameters);
                    scalar = (await res.SingleAsync())[0].As<T>();
                    return scalar;
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }
        }

        /// <summary>
        ///     Execute write transaction
        /// </summary>
        public async Task<T> ExecuteWriteTransactionAsync<T>(string query,
            IDictionary<string, object>? parameters = null)
        {
            try
            {
                parameters = parameters ?? new Dictionary<string, object>();

                T result = await _session.ExecuteWriteAsync(async tx =>
                {
                    T scalar = default;
                    IResultCursor? res = await tx.RunAsync(query, parameters);
                    scalar = (await res.SingleAsync())[0].As<T>();
                    return scalar;
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or
        ///     resetting unmanaged resources asynchronously.
        /// </summary>
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _session.CloseAsync();
        }

        /// <summary>
        ///     Execute read transaction as an asynchronous operation.
        /// </summary>
        private async Task<List<T>> ExecuteReadTransactionAsync<T>(string query, string returnObjectKey,
            IDictionary<string, object>? parameters)
        {
            try
            {
                parameters = parameters ?? new Dictionary<string, object>();

                List<T>? result = await _session.ExecuteReadAsync(async tx =>
                {
                    List<T> data = new List<T>();
                    IResultCursor? res = await tx.RunAsync(query, parameters);
                    List<IRecord>? records = await res.ToListAsync();
                    data = JsonSerializer.Deserialize<List<T>>(JsonSerializer.Serialize(records))!.ToList();
                    
                    
                    // data = records.Select(x => (T)x.Values[returnObjectKey]).ToList();
                    return data;
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }
        }
    }
}