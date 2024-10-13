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
        public User? UserProfile { get; set; } = new User();
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();

        private readonly IHttpClientFactory _clientFactory;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationStateProvider AuthenticationStateProvider;
        private readonly NavigationManager _navigation;

        public DataService(NavigationManager Navigation, IHttpClientFactory clientFactory, ApplicationDbContext context, UserManager<ApplicationUser> userManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _clientFactory = clientFactory;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            AuthenticationStateProvider = authenticationStateProvider;
            //Recipes = LoadRecipesFromXmlFile(Constants.XML_CACHE_PATH);
            _navigation = Navigation;
        }

        /// <summary>
        /// Adds a recipe to the ClaimUser's favorites and saves the changes to the database.
        /// </summary>
        public async Task AddFavoriteRecipeAsync(Recipe recipe)
        {

            UserProfile = await GetAuthenticatedUserAsync();

            if (UserProfile != null)
            {

                if (UserProfile.FavoriteRecipes.Contains(recipe))
                {
                    // Notify the user that the recipe is already in their favorites
                    Console.WriteLine("Recipe is already in your favorites.");
                }
                else
                {
                    // Add the recipe to the user's favorite list
                    UserProfile.FavoriteRecipes.Add(recipe);

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
                UserProfile = await GetAuthenticatedUserAsync();

                if (UserProfile != null)
                {
                    // Use SourceDomain and SearchTerms as additional criteria
                    var recipeToRemove = UserProfile.FavoriteRecipes
                    .FirstOrDefault(r =>
                        r.RecipeName == recipe.RecipeName &&
                        r.Url == recipe.Url &&
                        r.SourceDomain == recipe.SourceDomain &&
                        r.SearchTerms != null && recipe.SearchTerms != null &&
                        r.SearchTerms.OrderBy(t => t).SequenceEqual(recipe.SearchTerms.OrderBy(t => t)));


                    if (recipeToRemove != null)
                    {
                        // Remove the recipe from the list
                        UserProfile.FavoriteRecipes.Remove(recipeToRemove);

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
        /// Retrieves cached recipes based on search terms and source.
        /// </summary>
        public List<Recipe> GetCachedRecipes(List<string> searchTerms, string source)
        {
            var normalizedSearchTerms = searchTerms.Select(term => term.Trim().ToLowerInvariant()).ToList();
            source = source.Trim().ToLowerInvariant();

            var existingRecipes = Recipes
                .Where(r => r.RecipeName != null &&
                            r.SourceDomain != null &&
                            r.SourceDomain.Trim().ToLowerInvariant().Equals(source, StringComparison.OrdinalIgnoreCase) &&
                            r.SearchTerms != null)
                .Where(r => normalizedSearchTerms.Any(term => r.RecipeName.Contains(term, StringComparison.OrdinalIgnoreCase)) &&
                            normalizedSearchTerms.Any(term => r.SearchTerms.Any(st => st.Trim().ToLowerInvariant().Equals(term, StringComparison.OrdinalIgnoreCase))))
                .ToList();

            return existingRecipes;
        }
        /// <summary>
        /// Retrieves saved recipes from database based on search terms and source.
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public async Task<List<Recipe>> GetRecipesFromDatabaseAsync(string searchQuery, string source)
        {
            // Normalize the search query
            searchQuery = searchQuery.Trim().ToLowerInvariant();

            // Query the database for existing recipes that match the search query or URL
            var existingRecipes = await _context.Recipes
                .Where(r => r.SearchTerms.Contains(searchQuery) && r.SourceDomain == source)
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
        public async Task<User> GetAuthenticatedUserAsync()
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

                    UserProfile = appUser.User;
                    UserProfile.Name = appUser.UserName;

                    if (UserProfile != null)
                    {
                        return UserProfile; // Return the authenticated user
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
    }
}
