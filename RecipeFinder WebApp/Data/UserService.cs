using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using RecipeFinder_WebApp.Data;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly DataService _dataService;

    public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, DataService dataService)
    {
        _userManager = userManager;
        _context = context;
        _dataService = dataService;  
    }

    public async Task AddFavoriteRecipeAsync(string userId, string recipeId)
    {
        // Load all recipes from the XML file
        List<Recipe> allRecipes = _dataService.Recipes;

        // Load all favorite recipes from the XML file
        List<Recipe> allFavoriteRecipes = await Task.Run(() => DataService.LoadRecipesFromXmlFile(Constants.XML_FAVORITE_REC_PATH));
        if (allFavoriteRecipes.Count == 0)
        {
            allFavoriteRecipes = new List<Recipe>();
        }

        // Check if the recipe exists in all recipes
        var recipe = allRecipes.FirstOrDefault(r => r.RecipeId == recipeId);
        if (recipe == null)
        {
            // Recipe not found in the list of all recipes
            return;
        }

        // Check if the recipe is already in the user's favorites
        var userFavorites = allFavoriteRecipes.Where(r => r.UserId == userId).ToList();
        if (userFavorites.Any(r => r.RecipeId == recipeId))
        {
            // Recipe is already a favorite, no need to add it again
            return;
        }

        // Assign the UserId to the recipe and add it to the user's favorites
        recipe.UserId = userId;
        allFavoriteRecipes.Add(recipe);

        // Save the updated list of favorite recipes back to the XML file
        await Task.Run(() => DataService.SaveRecipesToXmlFile(allFavoriteRecipes, Constants.XML_FAVORITE_REC_PATH));
    }




    public async Task<List<Recipe>> GetFavoritesAsync(string userId)
    {
        // Load all favorite recipes from the XML file
        List<Recipe> allFavoriteRecipes = await Task.Run(() => DataService.LoadRecipesFromXmlFile(Constants.XML_FAVORITE_REC_PATH));

        // Filter the recipes by the userId
        List<Recipe> userFavoriteRecipes = allFavoriteRecipes.Where(r => r.UserId == userId).ToList();

        return userFavoriteRecipes;
    }

}
