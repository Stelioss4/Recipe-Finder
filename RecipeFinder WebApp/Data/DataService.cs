using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Recipe_Finder;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace RecipeFinder_WebApp.Data
{
    public class DataService
    {
        private Recipe recipe { get; set; } = new();
        private User? UserProfile { get; set; } = new User();
        private List<Recipe> Recipes { get; set; } = new List<Recipe>();
        private Review review { get; set; } = new();
        private Rating rating { get; set; } = new();

        private readonly IHttpClientFactory _clientFactory;
        //        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationStateProvider AuthenticationStateProvider;
        private readonly NavigationManager _navigation;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public DataService(NavigationManager Navigation, IHttpClientFactory clientFactory, IDbContextFactory<ApplicationDbContext> contextFactory, UserManager<ApplicationUser> userManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _clientFactory = clientFactory;
            //       context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            AuthenticationStateProvider = authenticationStateProvider;
            _navigation = Navigation;
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Add Ingredient from user's recipes to a shopping list
        /// </summary>
        /// <returns></returns>
        public async Task AddIngredientsToShoppingList(Ingredient ingredient)
        {
            using var context = _contextFactory.CreateDbContext();

            var appUser = await GetAuthenticatedUserAsync();

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

                    await context.SaveChangesAsync();

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
            using var context = _contextFactory.CreateDbContext();

            try
            {
                var appUser = await GetAuthenticatedUserAsync();

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
                        await context.SaveChangesAsync();

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
            using var context = _contextFactory.CreateDbContext();

            var appUser = await GetAuthenticatedUserAsync();

            if (appUser?.User != null)
            {
                // Access the user's favorite recipes directly due to AutoInclude
                if (appUser.User.FavoriteRecipes.Any(r => r.Id == recipe.Id))
                {
                    Console.WriteLine("Recipe is already in your favorites.");
                    return;
                }

                // Retrieve the tracked recipe
                var trackedRecipe = await context.Recipes.FirstOrDefaultAsync(r => r.Id == recipe.Id);
                if (trackedRecipe == null)
                {
                    Console.WriteLine("Recipe not found in the database.");
                    return;
                }

                // Add the recipe to favorites
                appUser.User.FavoriteRecipes.Add(trackedRecipe);
                await context.SaveChangesAsync();

                Console.WriteLine("Recipe added to your favorites successfully.");
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
            using var _context = _contextFactory.CreateDbContext();

            try
            {
                var appUser = await GetAuthenticatedUserAsync();



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

        /// <summary>
        /// Retrieves saved recipes from database based on search terms and source.
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public async Task<List<Recipe>> GetRecipesFromDatabaseAsync(string searchQuery, string source)
        {

            using var _context = _contextFactory.CreateDbContext();

            // Normalize the search query
            searchQuery = searchQuery.Trim().ToLowerInvariant();

            // Query the database for existing recipes that match the search query or URL
            var existingRecipes = await _context.Recipes
                .Where(r => r.SearchTerms.Any(st => st.Term == searchQuery) && r.SourceDomain == source)
                .ToListAsync();

            // If any matching recipes exist in the database, return them
            if (existingRecipes.Any())
            {
                return existingRecipes;
            }
            return new List<Recipe>();
        }

        /// <summary>
        /// Get an Authenticate User with all of their properties.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<ApplicationUser> GetAuthenticatedUserAsync()
        {
            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity.IsAuthenticated)
                {
                    var appUser = await _userManager.GetUserAsync(user);

                    if (appUser == null)
                    {
                        throw new NullReferenceException("ApplicationUser is null.");
                    }

                    if (appUser != null)
                    {
                        return appUser; // Return the authenticated user
                    }
                    else
                    {
                        throw new NullReferenceException("User associated with ApplicationUser is null.");
                    }
                }
                else
                {
                    Console.WriteLine("User is not authenticated.");
                    _navigation.NavigateTo("account/login");
                    return null; // No authenticated user, return null
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving authenticated user: {ex.Message}");
                // Handle the error (you might want to log or display an error message)
                return null; // Return null in case of an error
            }
        }

        public async Task<(double AverageRating, List<Review> Reviews)> ShowRecipesReviewsAndRatings(Recipe recipe)
        {
            using var _context = _contextFactory.CreateDbContext();

            recipe = await _context.Recipes
               .Include(r => r.Reviews)
               .Include(r => r.Ratings)
               .FirstOrDefaultAsync(r => r.Id == recipe.Id);

            if (recipe == null)
            {
                return (0, new List<Review>());
            }

            double averageRating = recipe.Rating; // Rating property now calculates the average
            List<Review> reviews = recipe.Reviews;

            return (averageRating, reviews);
        }
    }
}
