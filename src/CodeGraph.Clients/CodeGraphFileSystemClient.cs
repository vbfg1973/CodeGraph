using System.Net.Http.Json;
using CodeGraph.Clients.Config;
using CodeGraph.Clients.Dto.FileSystem;

namespace CodeGraph.Clients
{
    public class CodeGraphFileSystemClient
    {
        private const string BaseRoute = "fileSystem";
        private readonly HttpClient _httpClient;

        public CodeGraphFileSystemClient(HttpClient httpClient, CodeGraphApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(apiSettings.BaseUri);
        }

        public async ValueTask<FileSystemEntryDto> GetRootFolders()
        {
            return await _httpClient.GetFromJsonAsync<FileSystemEntryDto>($"{BaseRoute}/root");
        }
    }
}