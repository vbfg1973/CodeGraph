using CodeGraph.Domain.Graph.Database.Repositories.Results;
using CodeGraph.UiServer.Config;

namespace CodeGraph.UiServer.Clients
{
    public class CodeGraphApiClient
    {
        private readonly HttpClient _httpClient;

        public CodeGraphApiClient(HttpClient httpClient, CodeGraphApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(apiSettings.BaseUri);
        }

        public async ValueTask<MethodQueryResult> GetMethodByPk(int pk)
        {
            return await _httpClient.GetFromJsonAsync<MethodQueryResult>($"method/pk/{pk}");
        }
    }
}