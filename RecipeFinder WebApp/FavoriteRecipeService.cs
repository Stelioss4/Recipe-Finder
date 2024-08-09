using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using RecipeFinder_WebApp.Components;
using RecipeFinder_WebApp.Data;


namespace RecipeFinder_WebApp
{
    public class FavoriteRecipeService
    {
        private readonly RecipeFinder_WebAppContext _context;

        public FavoriteRecipeService(RecipeFinder_WebAppContext context)
        {
            _context = context;
        }

        public async Task AddFavoriteRecipeAsync(string userId, string recipeId)
        {
            var favorite = new Recipe
            {
                UserId = userId,
                RecipeId = recipeId
            };

            _context.Recipes.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Recipe>> GetFavoriteRecipesAsync(string userId)
        {
            var favoriteRecipeIds = await _context.Recipes
                .Where(f => f.UserId == userId)
                .Select(f => f.RecipeId)
                .ToListAsync();

            // Load recipes from XML
            var allRecipes = ScrapperService.LoadRecipesFromXmlFile(Constants.XML_CACHE_PATH);
            var favoriteRecipes = allRecipes.Where(r => favoriteRecipeIds.Contains(r.RecipeId)).ToList();

            return favoriteRecipes;

        }


    }
}
