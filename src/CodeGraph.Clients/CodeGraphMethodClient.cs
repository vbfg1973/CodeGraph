using System.Net.Http.Json;
using CodeGraph.Clients.Config;
using CodeGraph.Clients.Dto.Methods;

namespace CodeGraph.Clients
{
    public class CodeGraphMethodClient
    {
        private const string BaseRoute = "method";
        private readonly HttpClient _httpClient;

        public CodeGraphMethodClient(HttpClient httpClient, CodeGraphApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(apiSettings.BaseUri);
        }

        public async ValueTask<MethodResultDto> GetMethodByPk(int pk)
        {
            return await _httpClient.GetFromJsonAsync<MethodResultDto>($"{BaseRoute}/pk/{pk}");
        }

        public async ValueTask<MethodResultDto> GetMethodByFullName(string fullName)
        {
            return await _httpClient.GetFromJsonAsync<MethodResultDto>($"{BaseRoute}/fullName/{fullName}");
        }
    }
}