namespace RecipeFinder_WebApp.Data.AI.Models
{
    public class WeeklyPlanAgentRequestDto
    {
        private int? _maxCaloriesPerRecipe;

        public int? MaxCaloriesPerRecipe
        {
            get { return _maxCaloriesPerRecipe; }
            set { _maxCaloriesPerRecipe = value; }
        }

        private int? _maxPreparationTimeInMinutes;

        public int? MaxPreparationTimeInMinutes
        {
            get { return _maxPreparationTimeInMinutes; }
            set { _maxPreparationTimeInMinutes = value; }
        }

        private int _preferredFavoriteRecipesPerWeek;

        public int PreferredFavoriteRecipesPerWeek
        {
            get { return _preferredFavoriteRecipesPerWeek; }
            set { _preferredFavoriteRecipesPerWeek = value; }
        }

        private int _weeklyPlanDays;

        public int WeeklyPlanDays
        {
            get { return _weeklyPlanDays; }
            set { _weeklyPlanDays = value; }
        }

        private List<RecipeAgentDto> _candidateRecipes = new List<RecipeAgentDto>();

        public List<RecipeAgentDto> CandidateRecipes
        {
            get { return _candidateRecipes; }
            set { _candidateRecipes = value; }
        }
    }
}