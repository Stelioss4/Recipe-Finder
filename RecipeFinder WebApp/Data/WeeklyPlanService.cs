using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using RecipeFinder_WebApp.Data.AI;
using RecipeFinder_WebApp.Data.AI.Models;
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
        private readonly RecipeAgentService _recipeAgentService;
        private readonly OpenRouterService _openRouterService;

        public WeeklyPlanService(DataService dataService, NavigationManager navigation, IDbContextFactory<ApplicationDbContext> contextFactory, RecipeAgentService recipeAgentService, OpenRouterService openRouterService)
        {
            _dataService = dataService;
            _contextFactory = contextFactory;
            _navigation = navigation;
            _recipeAgentService = recipeAgentService;
            _openRouterService = openRouterService;
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

            var favoriteIds = favoriteRecipes
                .Select(recipe => recipe.Id)
                .ToHashSet();

            var validRecipes = await GetCandidateRecipesAsync(
                preferences,
                favoriteIds,
                Constants.DEAFULT_CANDIDATE_COUNT);

            if (!validRecipes.Any())
            {
                throw new Exception(
                    "No recipes match the selected weekly plan preferences.");
            }

            var validFavoriteRecipes = validRecipes
                .Where(recipe => favoriteIds.Contains(recipe.Id))
                .ToList();

            var random = new Random();

            var newWeeklyPlan = new List<Recipe>();

            if (favoriteRecipesToTake > 0 && validFavoriteRecipes.Any())
            {
                var shuffledFavoriteRecipes = validFavoriteRecipes
                    .OrderBy(recipe => random.Next())
                    .ToList();

                AddRecipesToWeeklyPlan(
                    shuffledFavoriteRecipes,
                    newWeeklyPlan,
                    favoriteRecipesToTake);
            }

            int remainingSlots =
                weeklyPlanDays - newWeeklyPlan.Count;

            if (remainingSlots > 0)
            {
                var shuffledValidRecipes = validRecipes
                    .OrderBy(recipe => random.Next())
                    .ToList();

                AddRecipesToWeeklyPlan(
                    shuffledValidRecipes,
                    newWeeklyPlan,
                    remainingSlots);
            }

            if (newWeeklyPlan.Count < weeklyPlanDays)
            {
                throw new Exception("Not enough unique recipe roots match the selected preferences to create a full weekly plan.");
            }

            var weeklyPlanIds = newWeeklyPlan
                .Select(recipe => recipe.Id)
                .ToList();

            var trackedWeeklyPlan = await context.Recipes
                .Where(recipe => weeklyPlanIds.Contains(recipe.Id))
                .ToListAsync();

            trackedWeeklyPlan = weeklyPlanIds
                .Select(id => trackedWeeklyPlan
                    .First(recipe => recipe.Id == id))
                .ToList();

            userProfile.User.WeeklyPlan = trackedWeeklyPlan;
            userProfile.User.LastWeeklyPlanDate = DateTime.Now;

            context.Update(userProfile);

            await context.SaveChangesAsync();

            Console.WriteLine(
                "Your weekly plan is up-to-date and saved!");

            return trackedWeeklyPlan;
        }

        public async Task<List<Recipe>> GenerateSmartWeeklyPlanAsync(int? maxCalories, int? maxPrepTime, int? preferredFavoriteRecipes)
        {
            try
            {
                Console.WriteLine(
                    "Smart Weekly Planner: attempting AI generation.");

                var aiPlan = await GenerateAiWeeklyPlanAsync(
                    maxCalories,
                    maxPrepTime,
                    preferredFavoriteRecipes);

                Console.WriteLine(
                    "Smart Weekly Planner: AI generation successful.");

                return aiPlan;
            }
            catch (AiServiceUnavailableException ex)
            {
                Console.WriteLine(
                    "========== SMART PLANNER FALLBACK ==========");

                Console.WriteLine(
                    $"AI unavailable: {ex.Message}");

                Console.WriteLine(
                    "Falling back to standard weekly plan generator.");

                Console.WriteLine(
                    "============================================");

                return await GenerateWeeklyPlanAsync(
                    maxCalories,
                    maxPrepTime,
                    preferredFavoriteRecipes);
            }
        }

        public async Task<List<Recipe>> GenerateAiWeeklyPlanAsync(int? maxCalories, int? maxPrepTime, int? preferredFavoriteRecipes)
        {
            using var context = _contextFactory.CreateDbContext();

            var appUser = await _dataService.GetAuthenticatedUserAsync();

            if (appUser == null || appUser.User == null)
            {
                throw new Exception("User not authenticated.");
            }

            var userProfile = await context.Users
                .Include(u => u.User.WeeklyPlan)
                .Include(u => u.User.FavoriteRecipes)
                .Include(u => u.User.UserPreferences)
                .FirstOrDefaultAsync(u => u.Id == appUser.Id);

            if (userProfile == null || userProfile.User == null)
            {
                throw new Exception("Authenticated user could not be loaded.");
            }

            var preferences = userProfile.User.UserPreferences;

            if (preferences == null)
            {
                preferences = new UserPreferences
                {
                    UserId = userProfile.User.Id
                };

                userProfile.User.UserPreferences = preferences;
            }

            preferences.MaxCaloriesPerRecipe = maxCalories;
            preferences.MaxPreparationTimeInMinutes = maxPrepTime;
            preferences.PreferredFavoriteRecipesPerWeek = preferredFavoriteRecipes;

            int weeklyPlanDays =
                preferences.WeeklyPlanDays ?? Constants.WEEK_DAY_NUM;

            int preferredFavorites =
                preferences.PreferredFavoriteRecipesPerWeek ?? 0;

            var favoriteIds = userProfile.User.FavoriteRecipes
                .Select(recipe => recipe.Id)
                .ToHashSet();

            var candidateRecipes = await GetCandidateRecipesAsync(
                preferences,
                favoriteIds,
                Constants.DEAFULT_CANDIDATE_COUNT);

            if (candidateRecipes.Count < weeklyPlanDays)
            {
                throw new Exception(
                    "Not enough candidate recipes were found to generate a weekly plan.");
            }

            var candidateDtos = _recipeAgentService.MapRecipesToDtos(
                candidateRecipes,
                favoriteIds);

            WeeklyPlanAgentRequestDto agentRequest =
                new WeeklyPlanAgentRequestDto
                {
                    MaxCaloriesPerRecipe =
                        preferences.MaxCaloriesPerRecipe,

                    MaxPreparationTimeInMinutes =
                        preferences.MaxPreparationTimeInMinutes,

                    PreferredFavoriteRecipesPerWeek =
                        preferredFavorites,

                    WeeklyPlanDays =
                        weeklyPlanDays,

                    CandidateRecipes =
                        candidateDtos
                };

            var aiResponse =
                await _openRouterService.GenerateWeeklyPlanAsync(agentRequest);

            var selectedIds = aiResponse.RecipeIds;

            if (selectedIds == null)
            {
                throw new Exception("AI returned no recipe IDs.");
            }

            if (selectedIds.Count != weeklyPlanDays)
            {
                throw new Exception(
                    $"AI returned {selectedIds.Count} recipes instead of {weeklyPlanDays}.");
            }

            if (selectedIds.Distinct().Count() != weeklyPlanDays)
            {
                throw new Exception(
                    "AI returned duplicate recipe IDs.");
            }

            var candidateIds = candidateRecipes
                .Select(recipe => recipe.Id)
                .ToHashSet();

            if (selectedIds.Any(id => !candidateIds.Contains(id)))
            {
                throw new Exception(
                    "AI returned a recipe that was not part of the candidate recipes.");
            }

            var weeklyPlanRecipes = await context.Recipes
                .Where(recipe => selectedIds.Contains(recipe.Id))
                .ToListAsync();

            if (weeklyPlanRecipes.Count != weeklyPlanDays)
            {
                throw new Exception(
                    "One or more AI-selected recipes could not be loaded.");
            }

            weeklyPlanRecipes = selectedIds
                .Select(id => weeklyPlanRecipes
                    .First(recipe => recipe.Id == id))
                .ToList();

            userProfile.User.WeeklyPlan = weeklyPlanRecipes;
            userProfile.User.LastWeeklyPlanDate = DateTime.Now;

            await context.SaveChangesAsync();

            return weeklyPlanRecipes;
        }

        /// <summary>
        /// Create a weekly plan shopping list by aggregating the ingredients from all recipes in the user's current weekly plan, 
        /// ensuring that each ingredient is listed only once, even if it appears in multiple recipes.
        /// The method retrieves the user's weekly plan from the database, extracts the ingredients from each recipe, 
        /// and compiles a unique list of ingredients to be used as a shopping list for the week.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        public async Task<List<Recipe>> GetWeeklyPlanAsync(int userId)
        {
            using var context = _contextFactory.CreateDbContext();

            return await context.User
                .Where(u => u.Id == userId)
                .SelectMany(u => u.WeeklyPlan)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task ClearExpiredWeeklyPlanAsync()
        {
            using var context = _contextFactory.CreateDbContext();

            var appUser = await _dataService.GetAuthenticatedUserAsync();

            if (appUser?.User == null)
                return;

            var dbUser = await context.User
                .Include(u => u.WeeklyPlan)
                .FirstOrDefaultAsync(u => u.Id == appUser.User.Id);

            if (dbUser == null)
                return;

            dbUser.WeeklyPlan.Clear();
            dbUser.LastWeeklyPlanDate = null;

            await context.SaveChangesAsync();
        }


        public async Task<List<Recipe>> GetCandidateRecipesAsync(UserPreferences preferences, HashSet<int> favoriteIds, int candidateCount = Constants.DEAFULT_CANDIDATE_COUNT)
        {
            if (preferences == null)
            {
                throw new ArgumentNullException(nameof(preferences));
            }

            if (favoriteIds == null)
            {
                throw new ArgumentNullException(nameof(favoriteIds));
            }

            if (candidateCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(candidateCount));
            }

            using var context = _contextFactory.CreateDbContext();

            var query = context.Recipes
                .AsNoTracking()
                .AsQueryable();

            if (preferences.MaxCaloriesPerRecipe.HasValue)
            {
                query = query.Where(recipe =>
                    recipe.NutritionValue != null &&
                    recipe.NutritionValue.Calories.HasValue &&
                    recipe.NutritionValue.Calories.Value <= preferences.MaxCaloriesPerRecipe.Value);
            }

            var filteredRecipes = await query
                .Select(recipe => new Recipe
                {
                    Id = recipe.Id,
                    RecipeName = recipe.RecipeName,
                    RecipeRoot = recipe.RecipeRoot,
                    Time = recipe.Time
                })
                .ToListAsync();

            if (preferences.MaxPreparationTimeInMinutes.HasValue)
            {
                filteredRecipes = filteredRecipes
                    .Where(recipe =>
                    {
                        var preparationTime = ExtractMinutesFromTimeText(recipe.Time);

                        return preparationTime.HasValue &&
                               preparationTime.Value <= preferences.MaxPreparationTimeInMinutes.Value;
                    })
                    .ToList();
            }

            var favoriteRecipes = filteredRecipes
                .Where(recipe => favoriteIds.Contains(recipe.Id))
                .ToList();

            var nonFavoriteRecipes = filteredRecipes
                .Where(recipe => !favoriteIds.Contains(recipe.Id))
                .ToList();

            int preferredFavorites = preferences.PreferredFavoriteRecipesPerWeek ?? 0;

            int favoriteCandidateCount = Math.Min(
                Math.Max(preferredFavorites * 3, preferredFavorites),
                candidateCount / 2);

            var random = new Random();

            var selectedFavoriteCandidates = favoriteRecipes
                .OrderBy(recipe => random.Next())
                .Take(favoriteCandidateCount)
                .ToList();

            int remainingCandidateSlots =
                candidateCount - selectedFavoriteCandidates.Count;

            var selectedNonFavoriteCandidates = nonFavoriteRecipes
                .OrderBy(recipe => random.Next())
                .Take(remainingCandidateSlots)
                .ToList();

            var candidateRecipes = selectedFavoriteCandidates
                .Concat(selectedNonFavoriteCandidates)
                .ToList();

            if (candidateRecipes.Count < candidateCount)
            {
                var selectedIds = candidateRecipes
                    .Select(recipe => recipe.Id)
                    .ToHashSet();

                var additionalRecipes = filteredRecipes
                    .Where(recipe => !selectedIds.Contains(recipe.Id))
                    .OrderBy(recipe => random.Next())
                    .Take(candidateCount - candidateRecipes.Count)
                    .ToList();

                candidateRecipes.AddRange(additionalRecipes);
            }

            var candidateIds = candidateRecipes
                .Select(recipe => recipe.Id)
                .ToList();

            var fullCandidateRecipes = await context.Recipes
                .AsNoTracking()
                .Where(recipe => candidateIds.Contains(recipe.Id))
                .Include(recipe => recipe.NutritionValue)
                .Include(recipe => recipe.ListOfIngredients)
                .ToListAsync();

            var recipesById = fullCandidateRecipes
                .ToDictionary(recipe => recipe.Id);

            var orderedCandidateRecipes = candidateIds
                .Where(id => recipesById.ContainsKey(id))
                .Select(id => recipesById[id])
                .ToList();

            return orderedCandidateRecipes;
        }
    }
}


