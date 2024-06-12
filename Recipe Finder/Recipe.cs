namespace Recipe_Finder
{
    public class Recipe
    {
		private string _recipeName;

		public string RecipeName
		{
			get { return _recipeName; }
			set { _recipeName = value; }
		}

		private string _image;

		public string Image
		{
			get { return _image; }
			set { _image = value; }
		}


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

		private DifficultyLevel _difficultyLevel;    //difficulty will be measured by easy, medium and hard.

        public DifficultyLevel DifficultyLevel
        {
			get { return _difficultyLevel; }
			set { _difficultyLevel = value; }
		}

        public List<Ingredient> ListofIngredients { get; set; }

        private string _linksForDrinkPairing;

        public string LinksForDrinkPairing
        {
            get { return _linksForDrinkPairing; }
            set { _linksForDrinkPairing = value; }
        }

		private List<Rating> _ratings;

		public List<Rating> Ratings
        {
			get { return _ratings; }
			set { _ratings = value; }
		}

        public double Rating
        {
            get
            {
                if (Ratings != null && Ratings.Count > 0)
                {
                    return Ratings.Average(r => r.Value);
                }
                else
                {
                    return 0;
                }
            }
            set { }
        }

        private List<Review> _reviews;

        public List<Review> Reviews
        {
            get { return _reviews; }
            set { _reviews = value; }
        }

		private string _url;

		public string Url
		{
			get { return  _url; }
			set {  _url = value; }
		}


	}
}
