using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    public class WeeklyPlanService
    {
        private readonly ApplicationDbContext _context;
        private readonly DataService _dataService;
        private DateTime? lastPlanDate = null;
        private List<Recipe> currentWeeklyPlan = new List<Recipe>();
        private User userProfile = new();
        public WeeklyPlanService(DataService dataService, ApplicationDbContext context)
        {
            _dataService = dataService;
            _context = context;
        }

        // Generates a weekly plan based on the user's favorite recipes
        public async Task<List<Recipe>> GenerateWeeklyPlanAsync()
        {
            // Fetch the authenticated user
            var userProfile = await _dataService.GetAuthenticatedUserAsync();

            if (userProfile.User == null)
            {
                throw new Exception("User not authenticated.");
            }

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
                _context.Update(userProfile); // Make sure the user is tracked by the context
                await _context.SaveChangesAsync();

                // Return the new weekly plan
                return newWeeklyPlan;
            }
            else
            {
                throw new Exception("No favorite recipes found.");
            }

        }


        // Allow the user to force a new plan manually
        public async Task<List<Recipe>> ForceNewPlanAsync()
        {
            var userProfile = await _dataService.GetAuthenticatedUserAsync();
            var favoriteRecipes = userProfile.User.FavoriteRecipes;
            var random = new Random();
            currentWeeklyPlan = favoriteRecipes.OrderBy(x => random.Next()).Take(Constants.WEEK_DAY_NUM).ToList();
            lastPlanDate = DateTime.Now;

            return currentWeeklyPlan;
        }

        // Check when the last plan was generated
        public DateTime? GetLastPlanDate()
        {
            return lastPlanDate;
        }
    }

}
