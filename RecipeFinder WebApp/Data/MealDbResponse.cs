namespace RecipeFinder_WebApp.Data
{
    public class MealDbResponse
    {
        public List<Meal> Meals { get; set; }
    }

    public class Meal
    {
        // Basic meal info
        public string IdMeal { get; set; }  // Meal ID
        public string StrMeal { get; set; }  // Meal Name
        public string StrInstructions { get; set; }  // Instructions
        public string StrMealThumb { get; set; }  // Meal Thumbnail URL (image)
        public string StrYoutube { get; set; }  // YouTube video URL

        // Ingredients (up to 20)
        public string StrIngredient1 { get; set; }
        public string StrIngredient2 { get; set; }
        public string StrIngredient3 { get; set; }
        public string StrIngredient4 { get; set; }
        public string StrIngredient5 { get; set; }
        public string StrIngredient6 { get; set; }
        public string StrIngredient7 { get; set; }
        public string StrIngredient8 { get; set; }
        public string StrIngredient9 { get; set; }
        public string StrIngredient10 { get; set; }
        public string StrIngredient11 { get; set; }
        public string StrIngredient12 { get; set; }
        public string StrIngredient13 { get; set; }
        public string StrIngredient14 { get; set; }
        public string StrIngredient15 { get; set; }
        public string StrIngredient16 { get; set; }
        public string StrIngredient17 { get; set; }
        public string StrIngredient18 { get; set; }
        public string StrIngredient19 { get; set; }
        public string StrIngredient20 { get; set; }

        // Corresponding measures for the ingredients
        public string StrMeasure1 { get; set; }
        public string StrMeasure2 { get; set; }
        public string StrMeasure3 { get; set; }
        public string StrMeasure4 { get; set; }
        public string StrMeasure5 { get; set; }
        public string StrMeasure6 { get; set; }
        public string StrMeasure7 { get; set; }
        public string StrMeasure8 { get; set; }
        public string StrMeasure9 { get; set; }
        public string StrMeasure10 { get; set; }
        public string StrMeasure11 { get; set; }
        public string StrMeasure12 { get; set; }
        public string StrMeasure13 { get; set; }
        public string StrMeasure14 { get; set; }
        public string StrMeasure15 { get; set; }
        public string StrMeasure16 { get; set; }
        public string StrMeasure17 { get; set; }
        public string StrMeasure18 { get; set; }
        public string StrMeasure19 { get; set; }
        public string StrMeasure20 { get; set; }

        // Category and Area (optional fields)
        public string StrCategory { get; set; }  // E.g., "Beef", "Chicken"
        public string StrArea { get; set; }  // E.g., "American", "Italian"
    }

}
