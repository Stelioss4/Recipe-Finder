using Recipe_Finder;
using Newtonsoft.Json;


namespace RecipeFinder_WebApp.Data
{
    public class SpoonacularService
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public SpoonacularService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Spoonacular:ApiKey"];
        }

        public async Task<List<Ingredient>> GetIngredientsForRecipe(Recipe recipe)
        {
            var response = await _httpClient.GetAsync($"https://api.spoonacular.com/recipes/{recipe}/ingredientWidget.json?apiKey={_apiKey}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var ingredients = JsonConvert.DeserializeObject<IngredientResponse>(jsonResponse);

                return ingredients.ingredients;
            }
            else
            {
                throw new Exception("Failed to retrieve ingredients from Spoonacular.");
            }
        }
    }
}
