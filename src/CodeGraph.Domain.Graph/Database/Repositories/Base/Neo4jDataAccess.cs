using System.Text.Json;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace CodeGraph.Domain.Graph.Database.Repositories.Base
{
    public sealed class Neo4jDataAccess : INeo4jDataAccess
    {
        private readonly string _database;

        private readonly ILogger<Neo4jDataAccess> _logger;

        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true
        };

        private readonly IAsyncSession _session;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Neo4jDataAccess" /> class.
        /// </summary>
        public Neo4jDataAccess(CredentialsConfig credentialsConfig, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Neo4jDataAccess>();
            _database = credentialsConfig.Database ?? "neo4j";
            IDriver driver = GraphDatabase.Driver($"neo4j://{credentialsConfig.Host}:{credentialsConfig.Port}",
                AuthTokens.Basic(credentialsConfig.UserName, credentialsConfig.Password));

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
            _logger.LogTrace("{Query}", query.ReplaceLineEndings("").Replace("\t", ""));

            T result = default;
            try
            {
                parameters = parameters ?? new Dictionary<string, object>();

                result = await _session.ExecuteReadAsync(async tx =>
                {
                    IResultCursor? res = await tx.RunAsync(query, parameters);
                    IRecord? record = await res.SingleAsync();
                    _logger.LogTrace("{Method} {DatabaseResultBody}", nameof(ExecuteReadScalarAsync),
                        JsonSerializer.Serialize(record, _options));

                    return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(record, _options))!;
                });

                return result;
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("The result set is empty")) result = default;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }

            return result;
        }

        /// <summary>
        ///     Execute write transaction
        /// </summary>
        public async Task<T> ExecuteWriteTransactionAsync<T>(string query,
            IDictionary<string, object>? parameters = null)
        {
            _logger.LogTrace("{Query}", query.ReplaceLineEndings("").Replace("\t", ""));

            T? result = default;
            try
            {
                parameters = parameters ?? new Dictionary<string, object>();

                result = await _session.ExecuteWriteAsync(async tx =>
                {
                    IResultCursor? res = await tx.RunAsync(query, parameters);
                    IRecord? record = await res.SingleAsync();
                    _logger.LogTrace("{Method} {DatabaseResultBody}", nameof(ExecuteWriteTransactionAsync),
                        JsonSerializer.Serialize(record, _options));

                    return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(record, _options))!;
                });

                return result;
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("The result set is empty")) result = default;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }

            return result;
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
            _logger.LogTrace("{Query}", query.ReplaceLineEndings("").Replace("\t", ""));

            List<T>? result = Enumerable.Empty<T>().ToList();
            try
            {
                parameters = parameters ?? new Dictionary<string, object>();

                result = await _session.ExecuteReadAsync(async tx =>
                {
                    List<T> data = new();
                    IResultCursor? res = await tx.RunAsync(query, parameters);
                    List<IRecord>? records = await res.ToListAsync();

                    _logger.LogTrace("{Method} {DatabaseResultBody}", nameof(ExecuteReadTransactionAsync),
                        JsonSerializer.Serialize(records, _options));

                    data = JsonSerializer.Deserialize<List<T>>(JsonSerializer.Serialize(records, _options))!.ToList();

                    return data;
                });
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("The result set is empty")) result = Enumerable.Empty<T>().ToList();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }

            return result;
        }
    }
}