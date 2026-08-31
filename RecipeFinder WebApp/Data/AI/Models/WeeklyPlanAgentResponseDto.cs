namespace RecipeFinder_WebApp.Data.AI.Models
{
    public class WeeklyPlanAgentResponseDto
    {
        private List<int> _recipeIds = new List<int>();

        public List<int> RecipeIds
        {
            get { return _recipeIds; }
            set { _recipeIds = value; }
        }
    }
}