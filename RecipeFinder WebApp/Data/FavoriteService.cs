using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    public class FavoriteService
    {
        private User User { get; set; } = new User();

     //   public event Action OnFavoritesChanged;
        private readonly NavigationManager _navigation;
        private readonly DataService _dataService;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;


        public FavoriteService(DataService dataService, NavigationManager Navigation, IDbContextFactory<ApplicationDbContext> contextFactory/*, ApplicationDbContext context*/)
        {
            //_context = context ?? throw new ArgumentNullException(nameof(context));
            _navigation = Navigation;
            _dataService = dataService;
            _contextFactory = contextFactory;

        }

        /// <summary>
        /// Add Ingredient from user's recipes to a shopping list
        /// </summary>
        /// <returns></returns>
        public async Task AddIngredientsToShoppingList(Ingredient ingredient)
        {
            using var _context = _contextFactory.CreateDbContext();

            var appUser = await _dataService.GetAuthenticatedUserAsync();

            appUser = await _context.Users
               .Include(u => u.User.ShoppingList)
               .FirstOrDefaultAsync(u => u.Id == appUser.Id);

            //User = appUser.User;

            if (appUser != null)
            {
                if (appUser.User.ShoppingList.Contains(ingredient))
                {
                    Console.WriteLine("ingredient already in shopping list");
                }
                else
                {
                    appUser.User.ShoppingList.Add(ingredient);

                    await _context.SaveChangesAsync();

                    Console.WriteLine("Ingredient is successfully added to shopping list");

                }
            }
            else
            {
                Console.WriteLine("User is not authenticated.");
                _navigation.NavigateTo("account/login");
            }

        }

        /// <summary>
        /// Removes Ingredient from shopping list
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        public async Task RemoveIngredientFromShoppingListAsync(Ingredient ingredient)
        {
            try
            {
                using var _context = _contextFactory.CreateDbContext();

                var appUser = await _dataService.GetAuthenticatedUserAsync();

                appUser = await _context.Users
                .Include(u => u.User.ShoppingList)
                .FirstOrDefaultAsync(u => u.Id == appUser.Id);

                User = appUser.User;

                if (User != null)
                {
                    var ingredientToRemove = User.ShoppingList
                        .FirstOrDefault(i =>
                         i.Id == ingredient.Id && i.UserId == ingredient.UserId);

                    if (ingredientToRemove != null)
                    {
                        // Remove the recipe from the list
                        User.ShoppingList.Remove(ingredientToRemove);

                        // Save changes to the database
                        await _context.SaveChangesAsync();

                        Console.WriteLine("Ingredient removed from ShoppingList successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing ingredient from Shopping list: {ex.Message}");
                // Handle the error appropriately (e.g., display an error message)
            }
        }

        /// <summary>
        /// Adds a recipe to the ClaimUser's favorites and saves the changes to the database.
        /// </summary>
        public async Task AddFavoriteRecipeAsync(Recipe recipe)
        {
            using var _context = _contextFactory.CreateDbContext();

            var appUser = await _dataService.GetAuthenticatedUserAsync();

            if (appUser != null)
            {
                appUser = await _context.Users
                    .Include(u => u.User.FavoriteRecipes)
                    .FirstOrDefaultAsync(u => u.Id == appUser.Id);

                if (appUser.User != null)
                {
                    // Manually check if the recipe is already in the favorites list
                    var existingRecipe = appUser.User.FavoriteRecipes
                        .FirstOrDefault(r => r.Id == recipe.Id);

                    if (existingRecipe != null)
                    {
                        // Notify the user that the recipe is already in their favorites
                        Console.WriteLine("Recipe is already in your favorites.");
                    }
                    else
                    {
                        // Check for already tracked entity to avoid duplicate tracking
                        var trackedRecipe = _context.Recipes.Local
                            .FirstOrDefault(r => r.Id == recipe.Id);

                        if (trackedRecipe == null)
                        {
                            _context.Recipes.Attach(recipe);
                        }
                        else
                        {
                            recipe = trackedRecipe;
                        }

                        appUser.User.FavoriteRecipes.Add(recipe);
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Recipe added to your favorites successfully.");
                    }
                }
            }
            else
            {
                Console.WriteLine("User is not authenticated.");
                _navigation.NavigateTo("account/login");
            }
        }

        /// <summary>
        /// Removes a recipe from the authenticated user's list of favorite recipes.
        /// </summary>
        public async Task RemoveFavoriteRecipeAsync(Recipe recipe)
        {
            try
            {
                using var _context = _contextFactory.CreateDbContext();

                var appUser = await _dataService.GetAuthenticatedUserAsync();

                appUser = await _context.Users
                 .Include(u => u.User.FavoriteRecipes)
                 .FirstOrDefaultAsync(u => u.Id == appUser.Id);

                if (appUser.User != null)
                {

                    // Use SourceDomain and SearchTerms as additional criteria
                    var recipeToRemove = appUser.User.FavoriteRecipes
                     .FirstOrDefault(r =>
                         string.Equals(r.RecipeName, recipe.RecipeName, StringComparison.OrdinalIgnoreCase) &&
                         string.Equals(r.SourceDomain, recipe.SourceDomain, StringComparison.OrdinalIgnoreCase)
                     //r.SearchTerms != null &&
                     //recipe.SearchTerms != null &&
                     //r.SearchTerms
                     //    .OrderBy(t => t.Term) 
                     //    .Select(t => t.Term) 
                     //    .SequenceEqual(
                     //        recipe.SearchTerms.OrderBy(t => t.Term).Select(t => t.Term)
                     );

                    if (recipeToRemove != null)
                    {
                        // Remove the recipe from the list
                        appUser.User.FavoriteRecipes.Remove(recipeToRemove);

                        // Save changes to the database
                        await _context.SaveChangesAsync();

                        Console.WriteLine("Recipe removed from favorites successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Recipe not found in favorites.");
                    }
                }
                else
                {
                    Console.WriteLine("User is not authenticated.");
                    _navigation.NavigateTo("account/login");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing recipe from favorites: {ex.Message}");
                // Handle the error appropriately (e.g., display an error message)
            }
        }
        public async Task ReloadFavorites()
        {
            using var _context = _contextFactory.CreateDbContext();

            var appUser = await _dataService.GetAuthenticatedUserAsync();

            if (appUser != null)
            {
                // Reload the user's favorite recipes from the database
                var updatedUser = await _context.Users
                     .Include(u => u.User.FavoriteRecipes)
                     .FirstOrDefaultAsync(u => u.Id == appUser.Id);

                if (updatedUser != null)
                {
                    appUser.User.FavoriteRecipes = updatedUser.User.FavoriteRecipes;
                }
            }
            else
            {
                Console.WriteLine("User is not authenticated.");
                _navigation.NavigateTo("account/login");
            }
        }

    }
}