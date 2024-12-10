using Microsoft.AspNetCore.Components;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    public class FavoriteService
    {
        private User? UserProfile { get; set; } = new User();

        private readonly ApplicationDbContext _context;
        private readonly NavigationManager _navigation;
        private readonly DataService _dataService;

        public FavoriteService(DataService dataService, NavigationManager Navigation, ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _navigation = Navigation;
            _dataService = dataService;

        }

        /// <summary>
        /// Add Ingredient from user's recipes to a shopping list
        /// </summary>
        /// <returns></returns>
        public async Task AddIngredientsToShoppingList(Ingredient ingredient)
        {
            var appUser = await _dataService.GetAuthenticatedUserAsync();

            UserProfile = appUser.User;

            if (UserProfile != null)
            {
                if (UserProfile.ShoppingList.Contains(ingredient))
                {
                    Console.WriteLine("ingredient already in shopping list");
                }
                else
                {
                    UserProfile.ShoppingList.Add(ingredient);

                    await _context.SaveChangesAsync();

                    Console.WriteLine("Ingredient is successfully added to shopping list");

                }
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
                var appUser = await _dataService.GetAuthenticatedUserAsync();

                UserProfile = appUser.User;

                if (UserProfile != null)
                {
                    var ingredientToRemove = UserProfile.ShoppingList
                        .FirstOrDefault(i =>
                         i.Id == ingredient.Id && i.UserId == ingredient.UserId);

                    if (ingredientToRemove != null)
                    {
                        // Remove the recipe from the list
                        UserProfile.ShoppingList.Remove(ingredientToRemove);

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

            var appUser = await _dataService.GetAuthenticatedUserAsync();


            if (appUser.User != null)
            {

                if (appUser.User.FavoriteRecipes.Contains(recipe))
                {
                    // Notify the user that the recipe is already in their favorites
                    Console.WriteLine("Recipe is already in your favorites.");
                }
                else
                {
                    // Add the recipe to the user's favorite list
                    appUser.User.FavoriteRecipes.Add(recipe);

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    // Notify the user that the recipe was successfully added
                    Console.WriteLine("Recipe added to your favorites successfully.");
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
                var appUser = await _dataService.GetAuthenticatedUserAsync();



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
    }
}