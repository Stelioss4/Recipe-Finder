namespace Recipe_Finder
{
    public static class TestData
    {
        public static List<Recipe> RecipeList()
        {
            const int RECIPE_INGREDIENTS = 3;

            List<Recipe> recipes = new List<Recipe>();

            List<Ingredient> ingredients = new List<Ingredient>();

            List<Rating> ratings = new List<Rating>();

            List<Review> reviews = new List<Review>();

            List<UsersProfile> users = UserProfil();

            Rating rating = new Rating();
            Rating rating1 = new Rating();
            Rating rating2 = new Rating();

            Review review = new Review();
            Review review1 = new Review();
            Review review2 = new Review();

            review.ReviewText = "very good and easy";
            review.TimeStam = rating.TimeStam;
            review.Profile = rating.Profile;

            rating.Value = 4.8;
            rating.TimeStam = new DateTime(2024, 4, 11, 12, 30, 0);
            rating.Profile = users[0];

            review1.ReviewText = "Nice and easy";
            review1.TimeStam = rating1.TimeStam;
            review1.Profile = rating1.Profile;

            rating1.Value = 4.2;
            rating1.TimeStam = new DateTime(2024, 4, 15, 18, 28, 0);
            rating1.Profile = users[1];

            review2.ReviewText = "easy for everyone to do, but not so good";
            review2.TimeStam = rating2.TimeStam;
            review2.Profile = rating2.Profile;

            rating2.Value = 3.9;
            rating2.TimeStam = new DateTime(2024, 2, 19, 17, 30, 0);
            rating2.Profile = users[2];

            Ingredient ingredient = new Ingredient();

            ingredient.Name = "egg";
            ingredient.Protein = 12.9m;
            ingredient.Fat = 50;
            ingredient.Carbohydrate = 280.5m;
            ingredient.Calories = 200;
            ingredient.Unit = 1;
            ingredient.Amount = IngredientAmount.Pieces;

            Ingredient ingredient1 = new Ingredient();

            ingredient1.Name = "water";
            ingredient1.Protein = 0m;
            ingredient1.Fat = 0;
            ingredient1.Carbohydrate = 0;
            ingredient1.Calories = 0;
            ingredient1.Unit = 2;
            ingredient1.Amount = IngredientAmount.Cups;

            Ingredient ingredient2 = new Ingredient();

            ingredient2.Name = "salt";
            ingredient2.Protein = 0m;
            ingredient2.Fat = 0;
            ingredient2.Carbohydrate = 0;
            ingredient2.Calories = 0;
            ingredient2.Unit = 1 / 2;
            ingredient2.Amount = IngredientAmount.Teaspoons;

            ingredients.Add(ingredient);
            ingredients.Add(ingredient1);
            ingredients.Add(ingredient2);

            Recipe recipe = new Recipe();

            ratings.Add(rating);
            ratings.Add(rating1);
            ratings.Add(rating2);

            reviews.Add(review);
            reviews.Add(review1);
            reviews.Add(review2);

            recipe.CookingInstructions = "boil an egg";
            recipe.Videolink = "www.howtoBoilanEgg.com";
            recipe.DifficultyLevel = "easy";
            recipe.ListofIngredients = ingredients;
            recipe.CookingTime = TimeSpan.FromMinutes(14);
            recipe.CuisineType = "global";
            recipe.LinksForDrinkPairing = "www.drinkwithfood.com";
            recipe.OccasionTags = OccasionTags.Brunch;

            for (int i = 0; i < RECIPE_INGREDIENTS; i++)
            {
                Console.WriteLine(ingredients[i].Name);
            }
            Console.WriteLine(recipe.CookingInstructions);
            Console.WriteLine(recipe.CookingTime);
            Console.WriteLine(recipe.Videolink);
            Console.WriteLine(recipe.CuisineType);
            Console.WriteLine(recipe.DifficultyLevel);
            Console.WriteLine(recipe.LinksForDrinkPairing);
            for (int i = 0; i < reviews.Count; i++)
            {
                Console.WriteLine(ratings[i].Value);
                Console.WriteLine(reviews[i].ReviewText);
            }


            ingredient.Name = "nudles";
            ingredient.Protein = 12.9m;
            ingredient.Fat = 50;
            ingredient.Carbohydrate = 280.5m;
            ingredient.Calories = 200;
            ingredient.Unit = 500;
            ingredient.Amount = IngredientAmount.Grams;

            ingredient1.Name = "water";
            ingredient1.Protein = 0m;
            ingredient1.Fat = 0;
            ingredient1.Carbohydrate = 0;
            ingredient1.Calories = 0;
            ingredient1.Unit = 2000;
            ingredient1.Amount = IngredientAmount.Milliliters;

            ingredient2.Name = "salt";
            ingredient2.Protein = 0m;
            ingredient2.Fat = 0;
            ingredient2.Carbohydrate = 0;
            ingredient2.Calories = 0;
            ingredient2.Unit = 2;
            ingredient2.Amount = IngredientAmount.Teaspoons;

            Ingredient ingredient3 = new Ingredient();

            ingredient3.Name = "Tomato sauce";
            ingredient3.Protein = 11;
            ingredient3.Fat = 5;
            ingredient3.Carbohydrate = 97;
            ingredient3.Calories = 450;
            ingredient3.Unit = 500;
            ingredient3.Amount = IngredientAmount.Milliliters;

            Ingredient ingredient4 = new Ingredient();

            ingredient4.Name = "Olive oil";
            ingredient4.Protein = 0;
            ingredient4.Fat = 9;
            ingredient4.Carbohydrate = 0;
            ingredient4.Calories = 9;
            ingredient4.Unit = 2;
            ingredient4.Amount = IngredientAmount.Tablespoons;

            Ingredient ingredient5 = new Ingredient();

            ingredient5.Name = "onion";
            ingredient5.Protein = 1;
            ingredient5.Fat = 1;
            ingredient5.Carbohydrate = 12;
            ingredient5.Calories = 1;
            ingredient5.Unit = 1;
            ingredient5.Amount = IngredientAmount.Pieces;

            Ingredient ingredient6 = new Ingredient();

            ingredient6.Name = "Garlic";
            ingredient6.Protein = 2;
            ingredient6.Fat = 2;
            ingredient6.Carbohydrate = 12;
            ingredient6.Calories = 1;
            ingredient6.Unit = 2;
            ingredient6.Amount = IngredientAmount.Pieces;

            ingredients.Clear();
            ingredients.Add(ingredient);
            ingredients.Add(ingredient1);
            ingredients.Add(ingredient2);
            ingredients.Add(ingredient3);
            ingredients.Add(ingredient4);
            ingredients.Add(ingredient5);
            ingredients.Add(ingredient6);

            Recipe recipe2 = new Recipe();

            //rating.Value = 4.7;
            //rating.TimeStam = new DateTime(2024, 2, 28, 16, 32, 0);
            //rating.Profile = users[0];
            //review.ReviewText = "very nice easy and delicius!!";
            //review.TimeStam = rating.TimeStam;
            //review.Profile = users[0];

            //rating1.Value = 3.8;
            //rating1.TimeStam = new DateTime(2024, 4, 8, 09, 26, 0);
            //rating1.Profile = users[1];
            //review1.ReviewText = "very easy, not so good description.";
            //review1.TimeStam = rating1.TimeStam;
            //rating1.Profile = users[1];

            //rating2.Value = 5;
            //rating2.TimeStam = new DateTime(2024, 1, 19, 19, 34, 0);
            //rating2.Profile = users[2];
            //review2.ReviewText = "amazing recipe! super easy and tasty!";
            //review2.TimeStam = rating2.TimeStam;
            //rating2.Profile = users[2];

            //reviews.Clear();
            //reviews.Add(review);
            //reviews.Add(review1);
            //reviews.Add(review2);

            //ratings.Clear();
            //ratings.Add(rating);
            //ratings.Add(rating1);
            //ratings.Add(rating2);

            recipe2.CookingInstructions = "boil nudles";
            recipe2.Videolink = "www.howtocooknudles.com";
            recipe2.DifficultyLevel = "easy";
            recipe2.ListofIngredients = ingredients;
            recipe2.CookingTime = TimeSpan.FromMinutes(10);
            recipe2.CuisineType = "italian";
            recipe2.LinksForDrinkPairing = "www.drinkwithfood.com";
            recipe2.OccasionTags = OccasionTags.Lunch;
            recipe2.Rating = 4.6;

            recipes.Add(recipe);
            recipes.Add(recipe2);

            Console.WriteLine();

            for (int i = 0; i < ingredients.Count; i++)
            {
                Console.WriteLine(ingredients[i].Name);
            }
            Console.WriteLine(recipe2.CookingInstructions);
            Console.WriteLine(recipe2.CookingTime);
            Console.WriteLine(recipe2.Videolink);
            Console.WriteLine(recipe2.CuisineType);
            Console.WriteLine(recipe2.DifficultyLevel);
            Console.WriteLine(recipe2.LinksForDrinkPairing);
            //for (int i = 0; i < ingredients.Count; i++)
            //{
            //    Console.WriteLine(ratings[i]);
            //    Console.WriteLine(reviews[i]);
            //}
            UIMethods.RatingAndReview();

            Console.WriteLine();

            return recipes;
        }

        public static List<UsersProfile> UserProfil()
        {
            List<UsersProfile> users = new List<UsersProfile>();

            Address address = new Address();
            Address address1 = new Address();
            Address address2 = new Address();

            address.StreetsName = "onstreet";
            address.Housenumber = "11";
            address.City = "Genk";
            address.PostalCode = "12312";

            address1.StreetsName = "outsidestreet";
            address1.Housenumber = "32B";
            address1.City = "NY city";
            address1.PostalCode = "56430";

            address2.StreetsName = "insidethestreet";
            address2.Housenumber = "100";
            address2.City = "Athens";
            address2.PostalCode = "10000";

            PaymentMethod paymentMethods = new PaymentMethod();
            PaymentMethod paymentMethods1 = new PaymentMethod();
            PaymentMethod paymentMethods2 = new PaymentMethod();

            paymentMethods.AcountEmail = "john@email.ccm";
            paymentMethods.Acountpasword = "acountpassword";

            paymentMethods1.AcountEmail = "jack@email.strom";
            paymentMethods1.Acountpasword = "ountpassword";

            paymentMethods2.AcountEmail = "maria.mar@email.de";
            paymentMethods2.Acountpasword = "passsssssword";

            UsersProfile user = new UsersProfile();
            UsersProfile user1 = new UsersProfile();
            UsersProfile user2 = new UsersProfile();

            user.Name = "John";
            user.Email = "john@email.ccm";
            user.Password = "password";
            user.Address = address1;
            user.PaymentMethods = paymentMethods;


            user1.Name = "Jack";
            user1.Email = "jack@email.strom";
            user1.Password = "Password";
            user1.Address = address;
            user1.PaymentMethods = paymentMethods1;

            user2.Name = "Maria";
            user2.Email = "maria.mar@email.de";
            user2.Password = "password";
            user2.Address = address2;
            user2.PaymentMethods = paymentMethods2;

            users.Add(user);
            users.Add(user1);
            users.Add(user2);

            return users;
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
