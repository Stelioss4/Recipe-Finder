﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    public class DataService
    {


        private readonly IHttpClientFactory _clientFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationStateProvider AuthenticationStateProvider;
        private readonly NavigationManager _navigation;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public DataService(NavigationManager Navigation, IHttpClientFactory clientFactory, IDbContextFactory<ApplicationDbContext> contextFactory, UserManager<ApplicationUser> userManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _clientFactory = clientFactory;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            AuthenticationStateProvider = authenticationStateProvider;
            _navigation = Navigation;
            _contextFactory = contextFactory;
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

                var appUser = await _userManager.GetUserAsync(user);

                if (appUser == null)
                {
                    throw new NullReferenceException("ApplicationUser is null.");
                }

                if (appUser != null)
                {
                    return appUser; // Return the authenticated appUser
                }
                else
                {
                    throw new NullReferenceException("User associated with ApplicationUser is null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving authenticated appUser: {ex.Message}");
                // Handle the error (you might want to log or display an error message)
                return null; // Return null in case of an error
            }
        }

        public async Task<User> GetUserByIdAsync()
        {
            try
            {
                using var _context = _contextFactory.CreateDbContext();

                var appUser = await GetAuthenticatedUserAsync();
                if (appUser == null || appUser.User == null)
                {
                    throw new NullReferenceException("Authenticated ApplicationUser or its User is null.");
                }

                // Fetch the user from the database
                var user = await _context.User
                    .Include(u => u.ShoppingList)
                    .FirstOrDefaultAsync(u => u.Id == appUser.User.Id);

                if (user == null)
                {
                    throw new NullReferenceException("User not found in the database.");
                }

                return user; // Return the fetched user
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving authenticated User: {ex.Message}");
                return null; // Handle error gracefully
            }
        }


        /// <summary>
        /// Returns recipe's reviews and ratings average
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public async Task<(double AverageRating, List<Review> Reviews)> ShowRecipesReviewsAndRatings(Recipe recipe)
        {
            using var _context = _contextFactory.CreateDbContext();

            recipe = await _context.Recipes
               .Include(r => r.Reviews)
               .ThenInclude(review => review.Profile)
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

        public async Task SubmitRatingAndReview(Recipe recipe, Rating newRating, Review newReview)
        {

            using var _context = _contextFactory.CreateDbContext();

            var appUser = await GetAuthenticatedUserAsync();
            if (appUser != null)
            {
                if (recipe == null)
                {
                    throw new ArgumentNullException(nameof(recipe));
                }

                try
                {

                    // Check if the recipe exists in the database
                    var existingRecipe = await _context.Recipes
                       .FirstOrDefaultAsync(r => r.Id == recipe.Id);

                    if (existingRecipe == null)
                    {
                        throw new InvalidOperationException("The specified recipe does not exist in the database.");
                    }

                    var user = _context.User.Find(appUser.User.Id);

                    newRating.Profile = user;
                    newReview.Profile = user;
                    newRating.TimeStamp = DateTime.Now;
                    newReview.TimeStamp = DateTime.Now;

                    // Add the new rating and review to the existing recipe
                    existingRecipe.Ratings.Add(newRating);
                    existingRecipe.Reviews.Add(newReview);


                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error submitting review and rating: {ex.Message}");
                    throw;
                }
            }
            else
            {
                _navigation.NavigateTo("Account/Login");
            }
        }
    }
}


