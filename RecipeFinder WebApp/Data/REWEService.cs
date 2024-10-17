using Recipe_Finder;

public class REWEService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public REWEService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["REWE:ApiKey"];
    }

    public async Task<List<Ingredient>> GetIngredientsForPurchase(List<Ingredient> ingredients)
    {
        var ingredientLinks = new List<Ingredient>();

        foreach (var ingredient in ingredients)
        {
            // Replace spaces with URL encoded format
            string searchQuery = Uri.EscapeDataString(ingredient.IngredientsName);

            // Create the request URL based on REWE's API requirements
            var requestUri = $"https://api.rewe.de/products?search={searchQuery}";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                // Process responseContent (deserialize it, extract the product URL)
                var productLink = ExtractProductLink(responseContent);

                // Add the product link for the ingredient
                ingredientLinks.Add(new Ingredient
                {
                    IngredientsName = ingredient.IngredientsName,
                    Amount = ingredient.Amount,
                    ProductLink = productLink
                });
            }
            else
            {
                // Handle failed API request
                ingredientLinks.Add(new Ingredient
                {
                    IngredientsName = ingredient.IngredientsName,
                    Amount = ingredient.Amount,
                    ProductLink = "Not Found"
                });
            }
        }

        return ingredientLinks;
    }

    private string ExtractProductLink(string responseContent)
    {
        // Implement logic to parse responseContent and extract the product URL
        // This will depend on REWE's API response format
        return "example-product-link";
    }
}
