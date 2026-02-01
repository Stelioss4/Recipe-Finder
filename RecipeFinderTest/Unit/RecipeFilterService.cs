using Recipe_Finder;

namespace RecipeFinderTest.Unit
{
    public class RecipeFilterService
    {
        public List<Recipe> FilterRecipesWithInstructions(List<Recipe> recipes)
        {
            if (recipes == null)
                return new List<Recipe>();

            return recipes
                .Where(r => !string.IsNullOrWhiteSpace(r.CookingInstructions))
                .ToList();
        }
    }
}