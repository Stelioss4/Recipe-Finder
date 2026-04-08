using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using System.Reflection.Metadata.Ecma335;

namespace RecipeFinder_WebApp.Data
{
    public class WeeklyPlanService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly NavigationManager _navigation;
        private readonly DataService _dataService;
        private List<Recipe> currentWeeklyPlan = new List<Recipe>();
        private User userProfile = new();
        public WeeklyPlanService(DataService dataService, NavigationManager navigation, IDbContextFactory<ApplicationDbContext> contextFactory /* ApplicationDbContext _context*/)
        {
            _dataService = dataService;
            _contextFactory = contextFactory;
            _navigation = navigation;
        }

        /// <summary>
        /// Generates a weekly plan based on the user's favorite recipes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Recipe>> GenerateWeeklyPlanAsync(int? maxCalories, int? maxPrepTime, int? preferredFavoriteRecipes)
        {
            using var context = _contextFactory.CreateDbContext();

            var userProfile = await _dataService.GetAuthenticatedUserAsync();

            if (userProfile == null || userProfile.User == null)
            {
                throw new Exception("User not authenticated.");
            }

            userProfile = await context.Users
                .Include(u => u.User.WeeklyPlan)
                .Include(u => u.User.FavoriteRecipes)
                .Include(u => u.User.UserPreferences)
                .FirstOrDefaultAsync(u => u.Id == userProfile.Id);

            if (userProfile == null || userProfile.User == null)
            {
                throw new Exception("Authenticated user could not be loaded.");
            }

            var favoriteRecipes = userProfile.User.FavoriteRecipes;
            var preferences = userProfile.User.UserPreferences;

            if (preferences == null)
            {
                userProfile.User.UserPreferences = new UserPreferences
                {
                    UserId = userProfile.User.Id
                };

                preferences = userProfile.User.UserPreferences;
            }

            preferences.MaxCaloriesPerRecipe = maxCalories;
            preferences.MaxPreparationTimeInMinutes = maxPrepTime;
            preferences.PreferredFavoriteRecipesPerWeek = preferredFavoriteRecipes;

            int weeklyPlanDays = preferences.WeeklyPlanDays ?? Constants.WEEK_DAY_NUM;
            int favoriteRecipesToTake = preferences.PreferredFavoriteRecipesPerWeek ?? 0;

            if (favoriteRecipesToTake < 0)
            {
                favoriteRecipesToTake = 0;
            }

            if (favoriteRecipesToTake > weeklyPlanDays)
            {
                favoriteRecipesToTake = weeklyPlanDays;
            }

            var allRecipes = await context.Recipes
                .Include(r => r.NutritionValue)
                .ToListAsync();

            var validRecipes = allRecipes
                .Where(r =>
                {
                    if (preferences.MaxCaloriesPerRecipe.HasValue)
                    {
                        if (r.NutritionValue == null || !r.NutritionValue.Calories.HasValue)
                            return false;

                        if (r.NutritionValue.Calories.Value > preferences.MaxCaloriesPerRecipe.Value)
                            return false;
                    }

                    if (preferences.MaxPreparationTimeInMinutes.HasValue)
                    {
                        var prepMinutes = ExtractMinutesFromTimeText(r.Time);

                        if (!prepMinutes.HasValue)
                            return false;

                        if (prepMinutes.Value > preferences.MaxPreparationTimeInMinutes.Value)
                            return false;
                    }

                    return true;
                })
                .ToList();

            if (!validRecipes.Any())
            {
                throw new Exception("No recipes match the selected weekly plan preferences.");
            }

            var validFavoriteRecipes = favoriteRecipes?
                .Where(fr => validRecipes.Any(vr => vr.Id == fr.Id))
                .ToList() ?? new List<Recipe>();

            var random = new Random();
            var newWeeklyPlan = new List<Recipe>();

            if (favoriteRecipesToTake > 0 && validFavoriteRecipes.Any())
            {
                var selectedFavoriteRecipes = validFavoriteRecipes
                    .OrderBy(x => random.Next())
                    .Take(Math.Min(favoriteRecipesToTake, validFavoriteRecipes.Count))
                    .ToList();

                newWeeklyPlan.AddRange(selectedFavoriteRecipes);
            }

            int remainingSlots = weeklyPlanDays - newWeeklyPlan.Count;

            if (remainingSlots > 0)
            {
                var remainingRecipes = validRecipes
                    .Where(r => newWeeklyPlan.All(wp => wp.Id != r.Id && NormalizeRecipeName(wp.RecipeName) != NormalizeRecipeName(r.RecipeName)))
                    .OrderBy(x => random.Next())
                    .Take(remainingSlots)
                    .ToList();

                newWeeklyPlan.AddRange(remainingRecipes);
            }

            if (newWeeklyPlan.Count < weeklyPlanDays)
            {
                throw new Exception("Not enough recipes match the selected preferences to create a full weekly plan.");
            }

            userProfile.User.WeeklyPlan = newWeeklyPlan;
            userProfile.User.LastWeeklyPlanDate = DateTime.Now;

            context.Update(userProfile);
            await context.SaveChangesAsync();

            Console.WriteLine("Your weekly plan is up-to-date and saved!");

            return newWeeklyPlan;
        }
        private int? ExtractMinutesFromTimeText(string timeText)
        {
            if (string.IsNullOrWhiteSpace(timeText))
                return null;

            var lower = HtmlAgilityPack.HtmlEntity.DeEntitize(timeText).ToLowerInvariant().Trim();

            var hourMatches = System.Text.RegularExpressions.Regex.Matches(lower, @"(\d+)\s*(std|h)");
            var minuteMatches = System.Text.RegularExpressions.Regex.Matches(lower, @"(\d+)\s*min");

            int totalMinutes = 0;

            foreach (System.Text.RegularExpressions.Match match in hourMatches)
            {
                if (int.TryParse(match.Groups[1].Value, out int hours))
                {
                    totalMinutes += hours * 60;
                }
            }

            foreach (System.Text.RegularExpressions.Match match in minuteMatches)
            {
                if (int.TryParse(match.Groups[1].Value, out int minutes))
                {
                    totalMinutes += minutes;
                }
            }

            if (totalMinutes == 0)
                return null;

            return totalMinutes;
        }

        private string NormalizeRecipeName(string recipeName)
        {
            if (string.IsNullOrWhiteSpace(recipeName))
                return string.Empty;

            recipeName = recipeName.Trim().ToLowerInvariant();

            recipeName = System.Text.RegularExpressions.Regex.Replace(recipeName, @"[^\w\s]", " ");
            recipeName = System.Text.RegularExpressions.Regex.Replace(recipeName, @"\s+", " ");

            return recipeName.Trim().ToLowerInvariant();
        }

        /// <summary>
        /// Allow the user to force a new plan manually
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Recipe>> ForceNewPlanAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            // Fetch the authenticated user
            var userProfile = await _dataService.GetAuthenticatedUserAsync();

            if (userProfile.User == null)
            {
                throw new Exception("User not authenticated.");
            }

            userProfile = await context.Users
            .Include(u => u.User.FavoriteRecipes)
            .FirstOrDefaultAsync(u => u.Id == userProfile.Id);

            var favoriteRecipes = userProfile.User.FavoriteRecipes;

            if (favoriteRecipes != null && favoriteRecipes.Count > Constants.LIMIT_DAYS)
            {
                // Randomly select 7 recipes for the weekly plan
                var random = new Random();
                var newWeeklyPlan = favoriteRecipes.OrderBy(x => random.Next()).Take(Constants.WEEK_DAY_NUM).ToList();

                // Update the user's profile with the new weekly plan and the current date
                userProfile.User.WeeklyPlan = newWeeklyPlan;
                userProfile.User.LastWeeklyPlanDate = DateTime.Now;

                // Save changes to the database
                context.Update(userProfile); // Make sure the user is tracked by the context
                await context.SaveChangesAsync();

                // Return the new weekly plan
                return newWeeklyPlan;
            }
            else
            {
                throw new Exception("No favorite recipes found.");
            }
        }

        /// <summary>
        /// Check when the last plan was generated
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DateTime?> CheckWeeklyPlanDate()
        {
            using var _context = _contextFactory.CreateDbContext();

            var appUser = await _dataService.GetAuthenticatedUserAsync();

            // Ensure appUser and User are not null before accessing properties
            if (appUser.User == null)
            {
                throw new Exception("User not authenticated.");
            }

            return appUser.User.LastWeeklyPlanDate;
        }
    }
}


