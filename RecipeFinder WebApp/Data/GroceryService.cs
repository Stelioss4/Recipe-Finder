using Recipe_Finder;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RecipeFinder_WebApp.Data
{
    public class GroceryService
    {
        private Recipe recipe = new();
        private readonly HttpClient _httpClient;

        public GroceryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> SearchMarketplaceForIngredient(string ingredientName)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://edamam-food-and-grocery-database.p.rapidapi.com/api/food-database/v2/nutrients"),
                Headers =
                    {
                        { "x-rapidapi-key", "your_api_key_here" },
                        { "x-rapidapi-host", "edamam-food-and-grocery-database.p.rapidapi.com" },
                    },
                // Adjust this part to use the ingredient name and valid measure URI
                Content = new StringContent($"{{\"ingredients\":[{{\"quantity\":1,\"measureURI\":\"your_measure_uri_here\",\"qualifiers\":[],\"foodId\":\"{ingredientName}\"}}]}}")
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };

            using (var response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    return body;
                }
                else
                {
                    // Handle the error case
                    var errorBody = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error fetching data: {errorBody}");
                }
            }
        }
    }
}
