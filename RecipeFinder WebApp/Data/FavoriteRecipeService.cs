using Microsoft.EntityFrameworkCore;
using Recipe_Finder;


namespace RecipeFinder_WebApp.Data
{
    public class FavoriteRecipeService
    {
        private readonly RecipeFinder_WebAppContext _context;

        public FavoriteRecipeService(RecipeFinder_WebAppContext context)
        {
            _context = context;
        }

        public static async Task AddFavoriteRecipeAsync(string userId, string recipeId)
        {
            // Load the existing favorite recipes from the XML file
            List<Recipe> favoriteRecipes = DataService.LoadRecipesFromXmlFile(Constants.XML_FAVORITE_REC_PATH);

            // Create a new favorite recipe entry
            var favorite = new Recipe
            {
                UserId = userId,
                RecipeId = recipeId
            };

            // Add the new favorite to the list
            favoriteRecipes.Add(favorite);

            // Save the updated list back to the XML file
            DataService.SaveRecipesToXmlFile(favoriteRecipes, Constants.XML_FAVORITE_REC_PATH);
        }


        public static async Task<List<Recipe>> GetFavoriteRecipesAsync(string userId)
        {
            // Load all favorite recipes from the XML file
            List<Recipe> userFavoriteRecipes = DataService.LoadRecipesFromXmlFile(Constants.XML_FAVORITE_REC_PATH);

            return userFavoriteRecipes;
        }

    }
}
