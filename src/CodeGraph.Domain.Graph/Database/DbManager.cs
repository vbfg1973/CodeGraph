﻿using System.Diagnostics;
using CodeGraph.Domain.Common;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Neo4j.Driver;

namespace CodeGraph.Domain.Graph.Database
{
    public static class DbManager
    {
        private const int BatchSize = 250;

        public static async Task InsertData(IList<Triple> triples, CredentialsConfig credentials, bool isDelete)
        {
            if (credentials == null) throw new ArgumentException("Please, provide credentials.");
            Console.WriteLine($"Code Knowledge Graph use \"{credentials.Database}\" Neo4j database.");
            IDriver? driver =
                GraphDatabase.Driver(credentials.Host, AuthTokens.Basic(credentials.UserName, credentials.Password));
            IAsyncSession? session = driver.AsyncSession(o => o.WithDatabase(credentials.Database));
            try
            {
                if (isDelete)
                {
                    await Console.Error.WriteLineAsync(
                        $"Deleting graph data of \"{credentials.Database}\" database...");
                    await session.RunAsync("MATCH (n) DETACH DELETE n;");
                    await Console.Error.WriteLineAsync(
                        $"Deleting graph data of \"{credentials.Database}\" database complete.");
                }

                await Console.Error.WriteLineAsync($"Inserting {triples.Count} triples...");

                int count = 0;
                long last_ms = 0;
                Stopwatch sw = new();
                sw.Start();
                foreach (var tripleBatch in triples.OrderBy(x => x.Relationship.Type).Batch(BatchSize).ToList())
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
                        $"Inserted {count} triples - {sw.Elapsed} - {sw.ElapsedMilliseconds - last_ms}ms  \r");
                    last_ms = sw.ElapsedMilliseconds;
                }

                sw.Stop();

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
    }
}