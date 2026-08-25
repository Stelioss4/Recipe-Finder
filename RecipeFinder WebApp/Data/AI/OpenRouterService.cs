using RecipeFinder_WebApp.Data.AI.Models;
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


        public async Task<WeeklyPlanAgentResponseDto> GenerateWeeklyPlanAsync(WeeklyPlanAgentRequestDto agentRequest)
        {
            var requestJson = JsonSerializer.Serialize(agentRequest);

            var prompt = """
        You are the weekly meal planning agent for Recipe Finder.

        Create a weekly meal plan using ONLY recipes from the candidate recipes provided below.

        Rules:
        - Select exactly {agentRequest.WeeklyPlanDays} unique recipes.
        - Never invent recipe IDs.
        - Use only recipe IDs contained in CandidateRecipes.
        - Prefer variety across the week.
        - Consider cuisine, ingredients, preparation time and calories.
        - Prefer approximately {agentRequest.PreferredFavoriteRecipesPerWeek} favorite recipes when possible.
        - All hard calorie and preparation-time filtering has already been handled by the application.
        - Return ONLY valid JSON.
        - Do not include markdown.
        - Do not include explanations.

        Required response format:

        {{
        
                    "recipeIds": [1, 2, 3]
        }}

        Candidate data:

        {requestJson}
        """;

            var response = await SendMessageAsync(prompt);

            var result = JsonSerializer.Deserialize<WeeklyPlanAgentResponseDto>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result == null)
            {
                throw new Exception(
                    "AI weekly plan response could not be parsed.");
            }

            return result;
        }
    }
}