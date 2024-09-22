﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Recipe_Finder;
using System.Xml.Serialization;

namespace RecipeFinder_WebApp.Data
{
    public class DataService
    {
        public User? userProfile { get; set; } = new User();
        public Address Address { get; set; } = new Address();
        public List<User> users { get; set; } = new List<User>();
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();

        private readonly IHttpClientFactory _clientFactory;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationStateProvider AuthenticationStateProvider;
        private readonly NavigationManager Navigation;
        private bool showSuccessMessage;
        private bool showAlreadyExistsMessage;

        public DataService(IHttpClientFactory clientFactory, ApplicationDbContext context, UserManager<ApplicationUser> userManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _clientFactory = clientFactory;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            AuthenticationStateProvider = authenticationStateProvider;
            Recipes = LoadRecipesFromXmlFile(Constants.XML_CACHE_PATH);
        }

        public async Task InitializeUserDataAsync()
        {
            // Get the current authenticated ClaimUser
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var ClaimUser = authState.User;

            if (ClaimUser.Identity.IsAuthenticated)
            {
                // Find the ApplicationUser based on the current ClaimUser's identity
                var appUser = await _userManager.GetUserAsync(ClaimUser);

                if (appUser != null)
                {
                    if (appUser.User == null)
                    {
                        // Initialize the custom User entity if it doesn't exist
                        appUser.User = new User
                        {
                            UserId = appUser.Id,
                            Name = appUser.UserName,
                            Email = appUser.Email,
                            FavoriteRecipes = new List<Recipe>()
                        };
                    }

                    userProfile = appUser.User;
                }
            }
        }
        /// <summary>
        /// Adds a recipe to the ClaimUser's favorites and saves the changes to the database.
        /// </summary>
        public async Task AddFavoriteRecipeAsync(string userId, Recipe recipe)
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

                    userProfile = appUser.User;

                    if (userProfile != null)
                    {
                        // Check if the recipe already exists in the user's favorite list
                        bool isRecipeInFavorites = userProfile.FavoriteRecipes
                            .Any(r => r.RecipeName == recipe.RecipeName && r.Url == recipe.Url);

                        if (isRecipeInFavorites)
                        {
                            // Notify the user that the recipe is already in their favorites
                            Console.WriteLine("Recipe is already in your favorites.");
                            showSuccessMessage = false;
                            showAlreadyExistsMessage = true;
                        }
                        else
                        {
                            // Add the recipe to the user's favorite list
                            userProfile.FavoriteRecipes.Add(recipe);

                            // Save changes to the database
                            await _context.SaveChangesAsync();

                            // Notify the user that the recipe was successfully added
                            Console.WriteLine("Recipe added to your favorites successfully.");
                            showSuccessMessage = true;
                            showAlreadyExistsMessage = false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("User is not authenticated.");
                    Navigation.NavigateTo("account/login");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding recipe to favorites: {ex.Message}");
                // Handle the error appropriately (e.g., display an error message)
            }
        }
        /// <summary>
        /// Gets the ClaimUser's favorite recipes. If none exist, returns an empty list.
        /// </summary>
        public static List<Recipe> GetUserFavorites(User user)
        {
            return user.FavoriteRecipes ?? new List<Recipe>();
        }

        /// <summary>
        /// Saves the list of recipes to an XML file.
        /// </summary>
        public static void SaveRecipesToXmlFile(List<Recipe> recipes, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, recipes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving recipes to XML file: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the list of recipes from an XML file.
        /// </summary>
        public static List<Recipe> LoadRecipesFromXmlFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        var recipes = (List<Recipe>)serializer.Deserialize(reader);
                        return recipes ?? new List<Recipe>(); // Return an empty list if deserialization returns null
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading recipes from XML file: {ex.Message}");
            }
            return new List<Recipe>();
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
    }
}
