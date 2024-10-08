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
        public WeeklyPlanService(DataService dataService , ApplicationDbContext context)
        {
            _dataService = dataService;
            _context = context;
        }

        // Generates a weekly plan based on the user's favorite recipes
        public async Task<List<Recipe>> GenerateWeeklyPlanAsync(List<Recipe> recipes)
        {
            if (currentWeeklyPlan.Any() && lastPlanDate.HasValue && (DateTime.Now - lastPlanDate.Value).TotalDays < 7)
            {
                return currentWeeklyPlan; // Return existing plan if less than 7 days have passed
            }

            // Fetch user's favorite recipes
            var userProfile = await _dataService.GetAuthenticatedUserAsync();
            var favoriteRecipes = userProfile.FavoriteRecipes;

            if (favoriteRecipes == null || !favoriteRecipes.Any())
            {
                throw new Exception("No favorite recipes found.");
            }

            // Randomly select 7 recipes for the weekly plan
            var random = new Random();
            currentWeeklyPlan = favoriteRecipes.OrderBy(x => random.Next()).Take(7).ToList();
            lastPlanDate = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return currentWeeklyPlan;
        }

        // Allow the user to force a new plan manually
        public async Task<List<Recipe>> ForceNewPlanAsync(Guid userId)
        {
            var userProfile = await _dataService.GetAuthenticatedUserAsync();
            var favoriteRecipes = userProfile.FavoriteRecipes;
            var random = new Random();
            currentWeeklyPlan = favoriteRecipes.OrderBy(x => random.Next()).Take(7).ToList();
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
