using System.Text.Json;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Neo4j.Driver;

namespace CodeGraph.Domain.Graph.Database
{
    public static class DbManager
    {
        private const string Connection = "neo4j://localhost:7687";

        public static async Task InsertData(IList<Triple> triples, CredentialsConfig credentials, bool isDelete)
        {
            if (credentials == null) throw new ArgumentException("Please, provide credentials.");
            Console.WriteLine($"Code Knowledge Graph use \"{credentials.Database}\" Neo4j database.");
            IDriver? driver =
                GraphDatabase.Driver(Connection, AuthTokens.Basic(credentials.User, credentials.Password));
            IAsyncSession? session = driver.AsyncSession(o => o.WithDatabase(credentials.Database));
            try
            {
                if (isDelete)
                {
                    await Console.Error.WriteLineAsync($"Deleting graph data of \"{credentials.Database}\" database...");
                    await session.RunAsync("MATCH (n) DETACH DELETE n;");
                    await Console.Error.WriteLineAsync($"Deleting graph data of \"{credentials.Database}\" database complete.");
                }

                await Console.Error.WriteLineAsync($"Inserting {triples.Count} triples...");

                int count = 0;
                foreach (Triple triple in triples)
                {
                    await session.RunAsync(triple.ToString());
                    count++;

                    if (count % 500 == 0)
                    {
                        await Console.Error.WriteAsync($"Inserting {triples.Count} triples\r");
                    }
                }

                await Console.Error.WriteLineAsync($"Inserted {triples.Count} triples complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                await session.CloseAsync();
                await driver.CloseAsync();
            }
        }
    }
}