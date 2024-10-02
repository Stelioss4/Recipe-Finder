using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using RecipeFinder_WebApp.Data;
using System.Net;

public class MealDbService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "1";
    private readonly string _baseUrl = "https://www.themealdb.com/api/json/v1/";
    private readonly ApplicationDbContext _dbContext;

    public MealDbService(HttpClient httpClient, string apiKey, ApplicationDbContext dbContext)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
        _dbContext = dbContext;
    }

    public async Task<List<Recipe>> GetRecipesAsync(string searchQuery)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{_apiKey}/search.php?s={Uri.EscapeDataString(searchQuery)}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<MealDbResponse>();

            if (result?.Meals != null)
            {
                var recipes = result.Meals.Select(meal => new Recipe
                {
                    RecipeName = meal.StrMeal,
                    Image = ConvertImageUrlToByteArray(meal.StrMealThumb),
                    VideoUrl = meal.StrYoutube,
                    CookingInstructions = meal.StrInstructions,
                    ListOfIngredients = ParseIngredients(meal),
                    SearchTerms = new List<string> { searchQuery }, // You can add this for tracking search queries
                    SourceDomain = "themealdb.com"
                }).ToList();

                // Save recipes to the database
                await SaveRecipesToDatabase(recipes);

                return recipes;
            }

            return new List<Recipe>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching recipes from TheMealDB: {ex.Message}");
            return new List<Recipe>();
        }
    }

    private List<Ingredient> ParseIngredients(Meal meal)
    {
        var ingredients = new List<Ingredient>();

        for (int i = 1; i <= 20; i++)
        {
            var ingredientProperty = typeof(Meal).GetProperty($"StrIngredient{i}");
            var measureProperty = typeof(Meal).GetProperty($"StrMeasure{i}");

            var ingredientName = (string)ingredientProperty?.GetValue(meal);
            var measure = (string)measureProperty?.GetValue(meal);

            if (!string.IsNullOrEmpty(ingredientName))
            {
                ingredients.Add(new Ingredient
                {
                    IngredientsName = ingredientName.Trim(),
                    Amount = measure?.Trim()
                });
            }
        }

        return ingredients;
    }

    // Method to save recipes to the database
    private async Task SaveRecipesToDatabase(List<Recipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            // Check if the recipe already exists in the database
            var existingRecipe = await _dbContext.Recipes
                .FirstOrDefaultAsync(r => r.RecipeName == recipe.RecipeName && r.SourceDomain == recipe.SourceDomain);

            if (existingRecipe == null)
            {
                // Recipe doesn't exist, add it to the database
                await _dbContext.Recipes.AddAsync(recipe);
            }
            else
            {
                // Recipe exists, update its details
                existingRecipe.CookingInstructions = recipe.CookingInstructions;
                existingRecipe.Image = recipe.Image;
                existingRecipe.VideoUrl = recipe.VideoUrl;
                existingRecipe.ListOfIngredients = recipe.ListOfIngredients;
                _dbContext.Recipes.Update(existingRecipe);
            }
        }

        // Save all changes to the database
        await _dbContext.SaveChangesAsync();
    }

    private byte[] ConvertImageUrlToByteArray(string imageUrl)
    {
        using (var webClient = new WebClient())
        {
            return webClient.DownloadData(imageUrl);
        }
    }
}
