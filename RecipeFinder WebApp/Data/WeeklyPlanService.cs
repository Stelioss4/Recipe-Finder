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
        /// Generates a weekly plan based on the user's favorite recipes & users preferences (max calories, max prep time, preferred number of favorite recipes in the plan) and saves it to the database.
        /// It ensures that the same recipe root is not repeated within the same weekly plan and that all recipes in the plan meet the specified preferences.
        /// If there are not enough valid recipes to fill the weekly plan, an exception is thrown. The method returns the generated weekly plan as a list of Recipe objects.
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
                var shuffledFavoriteRecipes = validFavoriteRecipes
                    .OrderBy(x => random.Next())
                    .ToList();

                AddRecipesToWeeklyPlan(
                    sourceRecipes: shuffledFavoriteRecipes,
                    targetPlan: newWeeklyPlan,
                    maxToAdd: favoriteRecipesToTake);
            }

            int remainingSlots = weeklyPlanDays - newWeeklyPlan.Count;

            if (remainingSlots > 0)
            {
                var shuffledValidRecipes = validRecipes
                    .OrderBy(x => random.Next())
                    .ToList();

                AddRecipesToWeeklyPlan(
                    sourceRecipes: shuffledValidRecipes,
                    targetPlan: newWeeklyPlan,
                    maxToAdd: remainingSlots);
            }

            if (newWeeklyPlan.Count < weeklyPlanDays)
            {
                throw new Exception("Not enough unique recipe roots match the selected preferences to create a full weekly plan.");
            }

            userProfile.User.WeeklyPlan = newWeeklyPlan;
            userProfile.User.LastWeeklyPlanDate = DateTime.Now;

            context.Update(userProfile);
            await context.SaveChangesAsync();

            Console.WriteLine("Your weekly plan is up-to-date and saved!");

            return newWeeklyPlan;
        }

        public async Task<List<Ingredient>> GetWeeklyPlanShoppingListAsync()
        {
            using var context = _contextFactory.CreateDbContext();

            var userProfile = await _dataService.GetAuthenticatedUserAsync();

            if (userProfile?.User == null)
                throw new Exception("User not authenticated.");

            userProfile = await context.Users
                .Include(u => u.User.WeeklyPlan)
                    .ThenInclude(r => r.ListOfIngredients)
                .FirstOrDefaultAsync(u => u.Id == userProfile.Id);

            if (userProfile?.User == null)
                throw new Exception("User not found.");

            var weeklyPlan = userProfile.User.WeeklyPlan;

            if (weeklyPlan == null || !weeklyPlan.Any())
                throw new Exception("No weekly plan found.");

            List<Ingredient> weeklyPlanShoppingList = new List<Ingredient>();
            var addedIngredientNames = new HashSet<string>();

            foreach (var recipe in weeklyPlan)
            {
                if (recipe.ListOfIngredients == null)
                    continue;

                foreach (var ingredient in recipe.ListOfIngredients)
                {
                    if (string.IsNullOrWhiteSpace(ingredient.IngredientsName))
                        continue;

                    var normalizedName = ingredient.IngredientsName.Trim().ToLower();

                    if (!addedIngredientNames.Contains(normalizedName))
                    {
                        weeklyPlanShoppingList.Add(ingredient);
                        addedIngredientNames.Add(normalizedName);
                    }
                }
            }

            return weeklyPlanShoppingList;
        }



        /// <summary>
        /// Adds unique recipes from the source collection to the weekly plan, up to the specified maximum number.
        /// </summary>
        /// <remarks>A recipe is considered a duplicate and will not be added if another recipe with the
        /// same Id or, if specified, the same RecipeRoot already exists in the target plan.</remarks>
        /// <param name="sourceRecipes">The list of recipes to consider for addition to the weekly plan. Recipes that are null or already present in
        /// the target plan (by Id or RecipeRoot) are ignored.</param>
        /// <param name="targetPlan">The list representing the current weekly plan. Recipes are added to this list if they are not already
        /// present.</param>
        /// <param name="maxToAdd">The maximum number of recipes to add from the source collection. Must be zero or greater.</param>
        private void AddRecipesToWeeklyPlan(List<Recipe> sourceRecipes, List<Recipe> targetPlan, int maxToAdd)
        {
            var addedCount = 0;

            foreach (var recipe in sourceRecipes)
            {
                if (addedCount >= maxToAdd)
                    break;

                if (recipe == null)
                    continue;

                if (targetPlan.Any(wp => wp.Id == recipe.Id))
                    continue;

                if (!string.IsNullOrWhiteSpace(recipe.RecipeRoot) &&
                    targetPlan.Any(wp => wp.RecipeRoot == recipe.RecipeRoot))
                    continue;

                targetPlan.Add(recipe);
                addedCount++;
            }
        }

        /// <summary>
        /// Extracts the total number of minutes represented in a time string containing hours and/or minutes.
        /// </summary>
        /// <remarks>The method recognizes both 'h' and 'std' as hour indicators and 'min' as the minute
        /// indicator. The input is case-insensitive and HTML entities are decoded before parsing.</remarks>
        /// <param name="timeText">The input string containing a textual representation of time, such as hours and minutes. Can include formats
        /// like '2h 30min', '1 std', or '45 min'.</param>
        /// <returns>The total number of minutes parsed from the input string, or null if no valid time information is found.</returns>
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


