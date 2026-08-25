using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using RecipeFinder_WebApp.Data.AI.Models;

namespace RecipeFinder_WebApp.Data.AI
{
    public class RecipeAgentService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly DataService _dataService;

        public RecipeAgentService(IDbContextFactory<ApplicationDbContext> contextFactory, DataService dataService)
        {
            _contextFactory = contextFactory;
            _dataService = dataService;
        }

        public async Task<List<RecipeAgentDto>> GetAvailableRecipesAsync()
        {
            using var context = _contextFactory.CreateDbContext();

            var appUser = await _dataService.GetAuthenticatedUserAsync();

            if (appUser == null || appUser.User == null)
            {
                throw new Exception("User not authenticated.");
            }

            var user = await context.Users
                 .Include(u => u.User.FavoriteRecipes)
                 .FirstOrDefaultAsync(u => u.Id == appUser.Id);

            if (user == null || user.User == null)
            {
                throw new Exception("Authenticated user could not be loaded.");
            }

            var favoriteIds = user.User.FavoriteRecipes
                .Select(recipe => recipe.Id)
                .ToHashSet();

            var recipes = await context.Recipes
                .Include(recipe => recipe.ListOfIngredients)
                .Include(recipe => recipe.NutritionValue)
                .ToListAsync();

            List<RecipeAgentDto> recipeDtos = new List<RecipeAgentDto>();

            foreach (var recipe in recipes)
            {
                RecipeAgentDto recipeDto = MapRecipeToDto(recipe, favoriteIds);

                recipeDtos.Add(recipeDto);
            }
            return recipeDtos;  
        }

        private RecipeAgentDto MapRecipeToDto(Recipe recipe, HashSet<int> favoriteIds)
        {
            RecipeAgentDto recipeDto = new RecipeAgentDto
            {
                Id = recipe.Id,
                RecipeName = recipe.RecipeName,
                Time = recipe.Time,
                DifficultyLevel = recipe.DifficultyLevel,
                CuisineType = recipe.CuisineType,
                Calories = recipe.NutritionValue?.Calories,
                IsFavorite = favoriteIds.Contains(recipe.Id),
                Ingredients = recipe.ListOfIngredients
                    .Select(ingredient => ingredient.IngredientsName)
                    .ToList()
            };

            return recipeDto;
        }
    }
}