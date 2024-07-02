using System.Diagnostics;
using CodeGraph.Domain.Common;
using CodeGraph.Domain.Graph.TripleDefinitions;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Neo4j.Driver;

namespace CodeGraph.Domain.Graph.Database
{
    public static class DbManager
    {
        private const int BatchSize = 1000;

        public static async Task InsertData(IList<Triple> triples, CredentialsConfig credentialsConfig, bool isDelete)
        {
            if (credentialsConfig == null) throw new ArgumentException("Please, provide credentials.");
            Console.WriteLine($"Code Knowledge Graph use \"{credentialsConfig.Database}\" Neo4j database.");
            
            IDriver? driver =
                GraphDatabase.Driver(credentialsConfig.Host, AuthTokens.Basic(credentialsConfig.UserName, credentialsConfig.Password));
            
            IAsyncSession? session = driver.AsyncSession(o => o.WithDatabase(credentialsConfig.Database));
            try
            {
                if (isDelete)
                {
                    await Console.Error.WriteLineAsync(
                        $"Deleting graph data of \"{credentialsConfig.Database}\" database...");
                    await session.RunAsync("MATCH (n) DETACH DELETE n;");
                    await Console.Error.WriteLineAsync(
                        $"Deleting graph data of \"{credentialsConfig.Database}\" database complete.");
                }

                Stopwatch sw = new();
                sw.Start();

                await RecreateIndices(session);

                await Console.Error.WriteLineAsync($"Inserting {triples.Count} triples...");

                int count = 0;
                long last_ms = 0;

                IOrderedEnumerable<Triple> orderedTriples = triples
                    .OrderBy(x => x.Relationship.Type)
                    .ThenBy(x => x.NodeA.FullName)
                    .ThenBy(x => x.NodeB.FullName);

                foreach (IEnumerable<Triple> tripleBatch in orderedTriples.Batch(BatchSize).ToList())
                {
                    await session.ExecuteWriteAsync(async tx =>
                    {
                        foreach (Triple triple in tripleBatch)
                        {
                            await tx.RunAsync(triple.ToString());
                        }

                        count += BatchSize;
                    });

                    await Console.Error.WriteAsync(
                        $"Inserted {count} triples - {sw.Elapsed} - {Math.Round((decimal)(sw.ElapsedMilliseconds - last_ms) / 1000, 2)} secs  \r");
                    last_ms = sw.ElapsedMilliseconds;
                }

                sw.Stop();

                await Console.Error.WriteAsync(new string(' ', 80) + "\r"); // Clear line
                await Console.Error.WriteLineAsync($"Inserted {triples.Count} triples complete - {sw.Elapsed}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                await session.CloseAsync();
                await driver.DisposeAsync();
            }
        }

        private static async Task RecreateIndices(IAsyncSession session)
        {
            foreach (string label in NodeUtilities.NodeLabels())
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    string dropIndexQuery = $"DROP INDEX {label.ToLower()}_pk  IF EXISTS";
                    await Console.Error.WriteLineAsync(dropIndexQuery);
                    await tx.RunAsync(dropIndexQuery);
                });

                await session.ExecuteWriteAsync(async tx =>
                {
                    string createIndexQuery = $"CREATE INDEX {label.ToLower()}_pk  FOR (n:{label}) ON (n.pk)";
                    await Console.Error.WriteLineAsync(createIndexQuery);
                    await tx.RunAsync(createIndexQuery);
                });
            }
        }
    }
}