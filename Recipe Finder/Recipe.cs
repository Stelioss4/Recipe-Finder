﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Finder
{
    public class Recipe
    {
        private string _sourceDomain;

        public string SourceDomain
        {
            get
            {
                if (_sourceDomain == null && !string.IsNullOrEmpty(Url))
                {
                    Uri u = new Uri(Url);
                    _sourceDomain = u.Host.ToLowerInvariant(); // Ensure consistency with case-insensitive comparison;
                }
                return _sourceDomain;
            }
            set { _sourceDomain = value.ToLowerInvariant(); } // Normalize the value being set; 
        }

        private List<string> _searchTerms;

        public List<string> SearchTerms
        {
            get { return _searchTerms; }
            set { _searchTerms = value; }
        }

        private string _userId;
        public string UserId 
        { 
            get { return _userId; } 
            set { _userId = value; } 
        }

        private string _recipeId;
        public string RecipeId 
        {
            get { return _recipeId; }
            set { _recipeId = value; } 
        }

        public User User { get; set; }


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

        private string _videoUrl;

        public string VideoUrl
        {
            get { return _videoUrl; }
            set { _videoUrl = value; }
        }

        private string _time;

        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }

        private TimeSpan _cookingTime;

        public TimeSpan CookingTime
        {
            get { return _cookingTime; }
            set { _cookingTime = value; }
        }

        private CuisineType _cuisineType;
        [NotMapped]
        public CuisineType CuisineType
        {
            get { return _cuisineType; }
            set { _cuisineType = value; }
        }

        private OccasionTags _occasionTags;
        [NotMapped]
        public OccasionTags OccasionTags
        {
            get { return _occasionTags; }
            set { _occasionTags = value; }
        }

        private string _difficultyLevel;    //difficulty will be measured by easy, medium and hard.

        public string DifficultyLevel
        {
            get { return _difficultyLevel; }
            set { _difficultyLevel = value; }
        }
        [NotMapped]
        public List<Ingredient>? ListofIngredients { get; set; } = new List<Ingredient>();

        private string _linksForDrinkPairing;

        public string LinksForDrinkPairing
        {
            get { return _linksForDrinkPairing; }
            set { _linksForDrinkPairing = value; }
        }

        private List<Rating> _ratings;
        [NotMapped]
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
        [NotMapped]
        public List<Review> Reviews
        {
            get { return _reviews; }
            set { _reviews = value; }
        }

        private string _url;

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
    }
}
