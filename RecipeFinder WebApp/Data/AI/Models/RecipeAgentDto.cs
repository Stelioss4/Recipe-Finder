using Recipe_Finder;

namespace RecipeFinder_WebApp.Data.AI.Models
{
    public class RecipeAgentDto
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _recipeName;

        public string RecipeName
        {
            get { return _recipeName; }
            set { _recipeName = value; }
        }
        private string _time;

        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }

        private string _difficultyLevel;

        public string DifficultyLevel
        {
            get { return _difficultyLevel; }
            set { _difficultyLevel = value; }
        }

        private CuisineType? _cuisineType;

        public CuisineType? CuisineType
        {
            get { return _cuisineType; }
            set { _cuisineType = value; }
        }

        private double? _calories;

        public double? Calories
        {
            get { return _calories; }
            set { _calories = value; }
        }

        private bool _isFavorite;

        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { _isFavorite = value; }
        }

        private List<string> _ingredients = new List<string>();

        public List<string> Ingredients
        {
            get { return _ingredients; }
            set { _ingredients = value; }
        }
    }
}