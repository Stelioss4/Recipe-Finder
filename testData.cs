namespace Recipe_Finder
{
    public static class TestData
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
            ing1.Amount = IngredientAmount.Pieces;

            Ingredient ing2 = new Ingredient();

            ing2.Name = "water";
            ing2.Protein = 0m;
            ing2.Fat = 0;
            ing2.Carbohydrate = 0;
            ing2.Calories = 0;
            ing2.Amount = IngredientAmount.Cups;


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
            recipe.OccasionTags = OccasionTags.Brunch;

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
            ing1.Amount = IngredientAmount.Pieces;

            ingList.Add(ing1);

            ing2.Name = "water";
            ing2.Protein = 0m;
            ing2.Fat = 0;
            ing2.Carbohydrate = 0;
            ing2.Calories = 0;
            ing2.Amount = IngredientAmount.Cups;


            ingList.Add(ing2);

            Recipe recipe2 = new Recipe();

            recipe2.CookingInstructions = "boil nudles";
            recipe2.Videolink = "www.howtocooknudles.com";
            recipe2.DifficultyLevel = "easy";
            recipe2.ListofIngredients = ingList;
            recipe2.CookingTime = TimeSpan.FromMinutes(10);
            recipe2.CuisineType = "italian";
            recipe2.LinksForDrinkPairing = "www.drinkwithfood.com";
            recipe2.OccasionTags = OccasionTags.Lunch;

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

        public static UsersProfile UserProfil()
        {
            Address address = new Address();

            address.StreetsName = "onstreet";
            address.Housenumber = "11";
            address.City = "Genk";
            address.PostalCode = "12312";

            PaymentMethod paymentMethods = new PaymentMethod();
            paymentMethods.AcountEmail = "jack@email.strom";
            paymentMethods.Acountpasword = "acountpassword";

            UsersProfile user1 = new UsersProfile();

            user1.Name = "Jack";
            user1.Email = "jack@email.strom";
            user1.Password = "Password";
            user1.Address = address;
            user1.PaymentMethods = paymentMethods;
            //user1.WeeklyPlan = RecipeList();
            //user1.FavoriteRecipes = RecipeList();
           

            Console.WriteLine(user1.Name);
            Console.WriteLine(user1.Email);
            Console.WriteLine(user1.Password);
            Console.WriteLine($"{user1.Address.StreetsName} {user1.Address.Housenumber} {user1.Address.PostalCode} {user1.Address.City}");
            Console.WriteLine(user1.PaymentMethods.AcountEmail + "\n" + user1.PaymentMethods.Acountpasword);
            //Console.WriteLine(user1.WeeklyPlan);
            //Console.WriteLine(user1.FavoriteRecipes);
           
            Console.WriteLine();
            return user1;

        }

        public static ProductInformation MarketInformation()
        {
            ProductInformation productInformation = new ProductInformation();

            List<Ingredient> Ingrediants = new List<Ingredient>();

            Ingredient ing1 = new Ingredient();

            ing1.Name = "egg";
            ing1.Protein = 12.9m;
            ing1.Fat = 50;
            ing1.Carbohydrate = 280.5m;
            ing1.Calories = 200;
            ing1.Amount = IngredientAmount.Pieces;

            Ingredient ing2 = new Ingredient();

            ing2.Name = "water";
            ing2.Protein = 0m;
            ing2.Fat = 0;
            ing2.Carbohydrate = 0;
            ing2.Calories = 0;
            ing2.Amount = IngredientAmount.Cups;

            Ingrediants.Add(ing1);
            Ingrediants.Add(ing2);

            PaymentMethod paymentMethods = new PaymentMethod();
            paymentMethods.AcountEmail = "jack@email.strom";
            paymentMethods.Acountpasword = "acountpassword";

            productInformation.AvailableProduct = Ingrediants;
            productInformation.PaymentMethods = paymentMethods;
            productInformation.NumberOfServings = 4;
            productInformation.Price = 30;
            productInformation.MarketLink = "www.market.com";

            foreach (Ingredient ing in productInformation.AvailableProduct)
            {
                Console.WriteLine(ing.Name);
            }
            Console.WriteLine(productInformation.PaymentMethods.AcountEmail + " " + productInformation.PaymentMethods.Acountpasword);
            Console.WriteLine(productInformation.NumberOfServings);
            Console.WriteLine(productInformation.Price);
            Console.WriteLine(productInformation.MarketLink);
            Console.WriteLine();

            return productInformation;

        }

    }
}
