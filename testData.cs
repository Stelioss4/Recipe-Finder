namespace Recipe_Finder
{
    public static class testData
    {
        public static List<Recipe> RecipeList()
        {
            List<Recipe> result = new List<Recipe>();

            List<Ingredient> ingList = new List<Ingredient>();

            Ingredient ing1 = new Ingredient();

            ing1.Name = "egg";
            ing1.Protein = 12.9m;
            ing1.Fat = 50;
            ing1.Carbohydrate = 280.5m;
            ing1.Calories = 200;
            ing1.Amount = IngredientAmount.pieces;

            Ingredient ing2 = new Ingredient();

            ing2.Name = "water";
            ing2.Protein = 0m;
            ing2.Fat = 0;
            ing2.Carbohydrate = 0;
            ing2.Calories = 0;
            ing2.Amount = IngredientAmount.cups;


            ingList.Add(ing1);
            ingList.Add(ing2);

            Recipe recipe = new Recipe();

            recipe.CookingInstructions = "boil an egg";
            recipe.Videolink = "www.howtoBoilanEgg.com";
            recipe.DifficultyLevel = "easy";
            recipe.ListofIngredients = ingList;
            recipe.CookingTime = TimeSpan.FromMinutes(14);
            recipe.CuisineType = "global";
            recipe.LinksForDrinkPairing = "www.drinkwithfood.com";
            recipe.OccasionTags = OccasionTags.brunch;

            Console.WriteLine(ingList[0].Name);
            Console.WriteLine(ingList[1].Name);
            Console.WriteLine(recipe.CookingInstructions);
            Console.WriteLine(recipe.CookingTime);
            Console.WriteLine(recipe.Videolink);
            Console.WriteLine(recipe.CuisineType);
            Console.WriteLine(recipe.DifficultyLevel);
            Console.WriteLine(recipe.LinksForDrinkPairing);



            ing1.Name = "nudles";
            ing1.Protein = 12.9m;
            ing1.Fat = 50;
            ing1.Carbohydrate = 280.5m;
            ing1.Calories = 200;
            ing1.Amount = IngredientAmount.pieces;

            ingList.Add(ing1);

            ing2.Name = "water";
            ing2.Protein = 0m;
            ing2.Fat = 0;
            ing2.Carbohydrate = 0;
            ing2.Calories = 0;
            ing2.Amount = IngredientAmount.cups;


            ingList.Add(ing2);

            Recipe recipe2 = new Recipe();

            recipe2.CookingInstructions = "boil nudles";
            recipe2.Videolink = "www.howtocooknudles.com";
            recipe2.DifficultyLevel = "easy";
            recipe2.ListofIngredients = ingList;
            recipe2.CookingTime = TimeSpan.FromMinutes(10);
            recipe2.CuisineType = "italian";
            recipe2.LinksForDrinkPairing = "www.drinkwithfood.com";
            recipe2.OccasionTags = OccasionTags.lunch;

            result.Add(recipe);
            result.Add(recipe2);

            Console.WriteLine();

            Console.WriteLine(ingList[0].Name);
            Console.WriteLine(ingList[1].Name);
            Console.WriteLine(recipe2.CookingInstructions);
            Console.WriteLine(recipe2.CookingTime);
            Console.WriteLine(recipe2.Videolink);
            Console.WriteLine(recipe2.CuisineType);
            Console.WriteLine(recipe2.DifficultyLevel);
            Console.WriteLine(recipe2.LinksForDrinkPairing);

            Console.WriteLine();

            return result;
        }

    }
}
