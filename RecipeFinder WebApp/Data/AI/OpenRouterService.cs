using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RecipeFinder_WebApp.Data.AI
{
    public class OpenRouterService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenRouterService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> SendMessageAsync(string message)
        {
            var apiKey = _configuration["OpenRouter:ApiKey"];
            var model = _configuration["OpenRouter:Model"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception("OpenRouter API key is missing.");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new Exception("OpenRouter model is missing.");
            }

            var requestBody = new
            {
                model = model,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = message
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://openrouter.ai/api/v1/chat/completions");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            var responseContent =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"OpenRouter request failed: {response.StatusCode} - {responseContent}");
            }

            using var document = JsonDocument.Parse(responseContent);

            var responseMessage = document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return responseMessage ?? string.Empty;
        }
    }
}