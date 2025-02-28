using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    public class WeeklyPlanService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly NavigationManager _navigation;
        private readonly DataService _dataService;
        private DateTime? lastPlanDate = null;
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
        public async Task<List<Recipe>> GenerateWeeklyPlanAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            // Fetch the authenticated user
            var userProfile = await _dataService.GetAuthenticatedUserAsync();

            if (userProfile.User == null)
            {
                throw new Exception("User not authenticated.");
            }

            userProfile = await context.Users
            .Include(u => u.User.WeeklyPlan)
            .Include(u => u.User.FavoriteRecipes)
            .FirstOrDefaultAsync(u => u.Id == userProfile.Id);

            // Check if the user already has a weekly plan and the date of the last plan
            if (userProfile.User.WeeklyPlan != null && userProfile.User.WeeklyPlan.Any() && userProfile.User.LastWeeklyPlanDate.HasValue)
            {
                // Calculate the number of days since the last plan was created
                var daysSinceLastPlan = (DateTime.Now - userProfile.User.LastWeeklyPlanDate.Value).TotalDays;

                if (daysSinceLastPlan < Constants.WEEK_DAY_NUM)
                {
                    // Return the existing weekly plan if it's less than 7 days old
                    return userProfile.User.WeeklyPlan;
                }
            }

            // Fetch user's favorite recipes from the database
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


