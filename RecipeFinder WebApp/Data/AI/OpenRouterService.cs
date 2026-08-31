using RecipeFinder_WebApp.Data.AI.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Net;

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

            Console.WriteLine($"========== OPENROUTER MODEL: {model} ==========");

            for (int attempt = 1; attempt <= 2; attempt++)
            {
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
            },
                    response_format = new
                    {
                        type = "json_object"
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);

                using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://openrouter.ai/api/v1/chat/completions");

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                request.Content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.SendAsync(request);

                var responseContent =
                    await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using var document = JsonDocument.Parse(responseContent);

                    var responseMessage = document.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    return responseMessage ?? string.Empty;
                }

                Console.WriteLine("========== OPENROUTER ERROR ==========");
                Console.WriteLine($"Attempt: {attempt}/2");
                Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Console.WriteLine(responseContent);
                Console.WriteLine("======================================");

                bool transientError =
                    response.StatusCode == HttpStatusCode.TooManyRequests ||
                    (int)response.StatusCode >= 500;

                if (transientError && attempt == 1)
                {
                    await Task.Delay(2000);
                    continue;
                }

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    throw new AiServiceUnavailableException(
                        "The AI service is currently busy.");
                }

                if (response.StatusCode == HttpStatusCode.PaymentRequired)
                {
                    throw new AiServiceUnavailableException(
                        "The AI service has reached its current usage limit.");
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new AiServiceUnavailableException(
                        "The AI service configuration is currently unavailable.");
                }

                if ((int)response.StatusCode >= 500)
                {
                    throw new AiServiceUnavailableException(
                        "The AI provider is temporarily unavailable.");
                }

                throw new Exception(
                    "The AI weekly plan could not be generated right now.");
            }

            throw new Exception(
                "The AI weekly plan could not be generated right now.");
        
        }


        public async Task<WeeklyPlanAgentResponseDto> GenerateWeeklyPlanAsync(WeeklyPlanAgentRequestDto agentRequest)
        {
            var requestJson = JsonSerializer.Serialize(agentRequest);

            var prompt = $$"""
    You are the weekly meal planning agent for Recipe Finder.

    Create a weekly meal plan using ONLY recipes from the candidate recipes provided below.

    Rules:
    - Select exactly {{agentRequest.WeeklyPlanDays}} unique recipes.
    - Never invent recipe IDs.
    - Use only recipe IDs contained in CandidateRecipes.
    - Prefer variety across the week.
    - Consider cuisine, ingredients, preparation time and calories.
    - Prefer approximately {{agentRequest.PreferredFavoriteRecipesPerWeek}} favorite recipes when possible.
    - All hard calorie and preparation-time filtering has already been handled by the application.
    - Return ONLY valid JSON.
    - Do not include markdown.
    - Do not include explanations.

    Required response format:

    {
        "recipeIds": [1, 2, 3]
    }

    Candidate data:

    {{requestJson}}
    """;

            var response = await SendMessageAsync(prompt);

            Console.WriteLine("========== AI RAW RESPONSE ==========");
            Console.WriteLine(response);
            Console.WriteLine("=====================================");

            if (string.IsNullOrWhiteSpace(response))
            {
                throw new Exception("AI returned an empty response.");
            }

            var cleanedResponse = response.Trim();

            if (cleanedResponse.StartsWith("```"))
            {
                cleanedResponse = cleanedResponse
                    .Replace("```json", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("```", "")
                    .Trim();
            }

            if (!cleanedResponse.StartsWith("{"))
            {
                throw new Exception(
                    "The AI returned an unexpected response.");
            }

            WeeklyPlanAgentResponseDto? result;

            try
            {
                result = JsonSerializer.Deserialize<WeeklyPlanAgentResponseDto>(
                    cleanedResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (JsonException ex)
            {
                Console.WriteLine(
                    $"AI JSON parsing failed: {ex.Message}");

                throw new Exception(
                    "The AI returned an invalid weekly plan response.");
            }

            if (result == null)
            {
                throw new Exception(
                    "The AI weekly plan response could not be parsed.");
            }

            return result;
        }
    }
}