using Recipe_Finder;
using System.Text.Json;

namespace RecipeFinder_WebApp.Components
{
    public class APIClass
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APIClass(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Recipe>> GetRecipesAsync(string query)
        {
            var client = _httpClientFactory.CreateClient("RecipeClient");
            string requestUrl = $"https://tasty.p.rapidapi.com/recipes/list?from=0&size=20&tags=under_30_minutes{Uri.EscapeDataString(query)}";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl),
                Headers =
            {
                { "x-rapidapi-key", "1e20d608ccmsh8ca6ccbac500b8ep16ab54jsne0a8c87f1bb1" },
                { "x-rapidapi-host", "tasty.p.rapidapi.com" },
            },
            };

            var recipes = new List<Recipe>();

            try
            {
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    recipes = apiResponse.Results.Select(result => new Recipe
                    {
                        RecipeName = result.RecipeName,
                        Image = result.Image,
                        CookingInstructions = result.CookingInstructions
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return recipes;
        }
    }
}