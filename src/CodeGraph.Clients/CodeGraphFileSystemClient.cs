using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CodeGraph.Clients.Config;
using CodeGraph.Clients.Dto.FileSystem;
using Microsoft.Extensions.Logging;

namespace CodeGraph.Clients
{
    public class CodeGraphFileSystemClient
    {
        private const string BaseRoute = "fileSystem";
        private readonly HttpClient _httpClient;
        private readonly ILogger<CodeGraphFileSystemClient> _logger;

        public CodeGraphFileSystemClient(HttpClient httpClient, CodeGraphApiSettings apiSettings,
            ILogger<CodeGraphFileSystemClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(apiSettings.BaseUri);
        }

        public async ValueTask<FileSystemEntryDto> GetRootFolders()
        {
            return await _httpClient.GetFromJsonAsync<FileSystemEntryDto>($"{BaseRoute}/root");
        }

        public async ValueTask<List<FileSystemHierarchyDto>> GetHierarchy()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"{BaseRoute}/hierarchy");

            if (!responseMessage.IsSuccessStatusCode)
            {
                _logger.LogError("{Method} Failed to get response from API: {StatusCode} {ReasonPhrase}",
                    nameof(GetHierarchy), responseMessage.StatusCode, responseMessage.ReasonPhrase);

                return null!;
            }

            string jsonString = await responseMessage.Content.ReadAsStringAsync();
            _logger.LogTrace("{Method} API returned: {JsonResponse}", nameof(GetHierarchy), jsonString);
            List<FileSystemHierarchyDto>? result = JsonSerializer.Deserialize<List<FileSystemHierarchyDto>>(jsonString,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = false,
                    Converters = { new JsonStringEnumConverter() }
                })!;
            
            _logger.LogTrace("{Method} Deserialised: {JsonResponse}", nameof(GetHierarchy),
                JsonSerializer.Serialize(result));
            return result;
        }
    }
}