namespace Recipe_Finder
{
    public class Recipe
    {
		private string _cookingInstructions;

		public string CookingInstructions
		{
			get { return _cookingInstructions; }
			set { _cookingInstructions = value; }
		}

		private string _videolink;

		public string Videolink
		{
			get { return _videolink; }
			set { _videolink = value; }
		}

		private TimeSpan _cookingTime;

		public TimeSpan CookingTime
		{
			get { return _cookingTime; }
			set { _cookingTime = value; }
		}

		private string _cuisineType;

		public string CuisineType
		{
			get { return _cuisineType; }
			set { _cuisineType = value; }
		}

		private OccasionTags _occasionTags;

		public OccasionTags OccasionTags
		{
			get { return _occasionTags; }
			set { _occasionTags = value; }
		}

		private int _difficultyLevel;

		public int DifficultyLevel
        {
			get { return _difficultyLevel; }
			set { _difficultyLevel = value; }
		}

        public List<Ingredient> ListofIngredients { get; set; }

	}
}
