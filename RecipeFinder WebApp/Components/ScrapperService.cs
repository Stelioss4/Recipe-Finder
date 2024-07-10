using HtmlAgilityPack;
using Recipe_Finder;
using RecipeFinder_WebApp.Components;

namespace RecipeFinder_WebApp
{
    public class ScrapperService
    {
        private readonly HttpClient _httpClient;
        private DataService _dataService;

        public ScrapperService(HttpClient httpClient, DataService ds)
        {
            _httpClient = httpClient;
            _dataService = ds;
        }


        public async Task<List<Recipe>> ScrapeFromAllRecipe(string searchQuery)
        {
            // Check in _dataService.Recipies if recipes are already there, if yes return them
            var existingRecipes = _dataService.Recipies
                .Where(r => r.RecipeName != null && r.RecipeName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (existingRecipes.Any())
            {
                return existingRecipes;
            }
            // If not found, scrape new recipes
            var searchResults = await ScrapeSearchResultsFromAllRecipe(searchQuery);
            if (searchResults == null || !searchResults.Any())
            {
                return new List<Recipe>(); // Return an empty list if no search results are found
            }

            List<Recipe> detailedRecipes = new List<Recipe>();
            foreach (var recipe in searchResults)
            {
                if (recipe?.Url != null) // Check if the searchResultRecipie and its URL are not null
                {
                    Recipe detailedRecipe = await ScrapeAllRecipesDetailsAndUpdateRecipie(recipe);
                    if (detailedRecipe != null) // Ensure detailedRecipe is not null before adding
                    {
                        detailedRecipes.Add(detailedRecipe);
                    }
                }
            }

            if (detailedRecipes.Any())
            {
                _dataService.Recipies.AddRange(detailedRecipes);
            }

            return detailedRecipes;
        }

        public async Task<List<Recipe>> ScrapeSearchResultsFromAllRecipe(string searchQuery)
        {
            List<Recipe> recipes = new List<Recipe>();

            try
            {
                string searchUrl = $"https://www.allrecipes.com/search?q={Uri.EscapeDataString(searchQuery)}";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(searchUrl);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                var listNode = document.DocumentNode.SelectSingleNode("//*[@id=\"mntl-search-results__content_1-0\"]");
                if (listNode != null)
                {
                    var resultNodes = listNode.SelectNodes(".//span");
                    if (resultNodes != null)
                    {
                        foreach (var node in resultNodes)
                        {
                            var titleNode = node.SelectSingleNode(".//span");
                            var linkNode = node.SelectSingleNode(".//span");
                            var imageNode = node.SelectSingleNode("/html/body/main/div/div/div/a[1]/div[1]/div[1]");

                            if (titleNode != null && linkNode != null)
                            {
                                var recipe = new Recipe
                                {
                                    RecipeName = titleNode.InnerText.Trim(),
                                    Url = linkNode.GetAttributeValue("href", string.Empty),
                                    Image = imageNode?.GetAttributeValue("src", string.Empty)
                                };
                                recipes.Add(recipe);
                            }
                            else
                            {
                                Console.WriteLine("Title or link node is null for one of the recipies.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No result nodes found in the list node.");
                    }
                }
                else
                {
                    Console.WriteLine("List node is null. The structure of the page might have changed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return recipes;
        }

        public async Task<Recipe> ScrapeAllRecipesDetailsAndUpdateRecipie(Recipe searchResultRecipie)
        {
            try
            {
                var html = await _httpClient.GetStringAsync(searchResultRecipie.Url);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                // Parse Recipe Name
                var recipeNameNode = document.DocumentNode.SelectSingleNode("//h1");
                if (recipeNameNode != null)
                {
                    searchResultRecipie.RecipeName = recipeNameNode.InnerText.Trim();
                }
                else
                {
                    Console.WriteLine("Recipe's Name node is null");
                }

                // Parse Image
                var imageNode = document.DocumentNode.SelectSingleNode("//*[@id=\"article__photo-ribbon_1-0\"]");
                if (imageNode != null)
                {
                    searchResultRecipie.Image = imageNode.GetAttributeValue("src", string.Empty);
                }
                else
                {
                    Console.WriteLine("Image node is null");
                }

                // Parse Cooking Instructions
                var instructionsNode = document.DocumentNode.SelectSingleNode("//*[@id=\"mntl-sc-block_22-0\"]");
                if (instructionsNode != null)
                {
                    searchResultRecipie.CookingInstructions = instructionsNode.InnerHtml.Trim();
                }
                else
                {
                    Console.WriteLine("Instructions node is null");
                }

                // Parse Video URL if available
                var videoNode = document.DocumentNode.SelectSingleNode("//*[@id=\"article__primary-video-jw_1-0\"]/video");
                if (videoNode != null)
                {
                    searchResultRecipie.VideoUrl = videoNode.GetAttributeValue("src", string.Empty);
                }
                else
                {
                    Console.WriteLine("Video node is null");
                }

                // Parse Cuisine Type (assuming a placeholder node, as AllRecipes does not explicitly categorize cuisine type)
                var cuisineTypeNode = document.DocumentNode.SelectSingleNode("//span[@class='recipe-cuisine']");
                if (cuisineTypeNode != null)
                {
                    if (Enum.TryParse(cuisineTypeNode.InnerText.Trim(), true, out CuisineType cuisineType))
                    {
                        searchResultRecipie.CuisineType = cuisineType;
                    }
                }
                else
                {
                    Console.WriteLine("Cuisine Type node is null");
                }

                // Parse Difficulty Level (assuming a placeholder node, as AllRecipes does not explicitly categorize difficulty level)
                var difficultyLevelNode = document.DocumentNode.SelectSingleNode("//span[@class='recipe-difficulty']");
                if (difficultyLevelNode != null)
                {
                    if (Enum.TryParse(difficultyLevelNode.InnerText.Trim(), true, out DifficultyLevel difficultyLevel))
                    {
                        searchResultRecipie.DifficultyLevel = difficultyLevel;
                    }
                }
                else
                {
                    Console.WriteLine("Difficulty Level node is null");
                }

                // Parse Occasion Tags (assuming a placeholder node, as AllRecipes does not explicitly categorize occasion tags)
                var occasionTagsNode = document.DocumentNode.SelectSingleNode("//span[@class='recipe-occasion']");
                if (occasionTagsNode != null)
                {
                    if (Enum.TryParse(occasionTagsNode.InnerText.Trim(), true, out OccasionTags occasionTags))
                    {
                        searchResultRecipie.OccasionTags = occasionTags;
                    }
                }
                else
                {
                    Console.WriteLine("Occasion Tags node is null");
                }

                // Parse List of Ingredients
                var ingredientsNode = document.DocumentNode.SelectSingleNode("//*[@id=\"mm-recipes-structured-ingredients_1-0\"]/ul");
                if (ingredientsNode != null)
                {
                    var ingredientNodes = ingredientsNode.SelectNodes("//*[@id=\"mm-recipes-structured-ingredients_1-0\"]/ul/li");
                    if (ingredientNodes != null)
                    {
                        searchResultRecipie.ListofIngredients = ingredientNodes
                            .Select(li => new Ingredient { IngredientsName = li.InnerText.Trim() })
                            .ToList();
                    }
                    else
                    {
                        Console.WriteLine("Ingredient nodes are null");
                    }
                }
                else
                {
                    Console.WriteLine("Ingredients node is null");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scraping searchResultRecipie details: {ex.Message}");
            }

            return searchResultRecipie;
        }


        public async Task<List<Recipe>> ScrapeCKRecipies(string searchQuery)
        {
            // Check in _dataService.Recipies if recipes are already there, if yes return them
            var existingRecipes = _dataService.Recipies
                .Where(r => r.RecipeName != null && r.RecipeName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (existingRecipes.Any())
            {
                return existingRecipes;
            }

            // If not found, scrape new recipes
            var searchResults = await ScrapeSearchResultsFromChefkoch(searchQuery);
            if (searchResults == null || !searchResults.Any())
            {
                return new List<Recipe>(); // Return an empty list if no search results are found
            }

            List<Recipe> detailedRecipes = new List<Recipe>();

            foreach (var recipe in searchResults)
            {
                if (recipe?.Url != null) // Check if the searchResultRecipie and its URL are not null
                {
                    Recipe detailedRecipe = await ScrapeCKDetailsAndUpdateRecipie(recipe);
                    if (detailedRecipe != null) // Ensure detailedRecipe is not null before adding
                    {
                        detailedRecipes.Add(detailedRecipe);
                    }
                }
            }

            if (detailedRecipes.Any())
            {
                _dataService.Recipies.AddRange(detailedRecipes);
            }

            return detailedRecipes;
        }

        public async Task<List<Recipe>> ScrapeSearchResultsFromChefkoch(string searchQuery)
        {
            List<Recipe> recipies = new List<Recipe>();

            try
            {
                string searchUrl = $"https://www.chefkoch.de/rs/s0/{Uri.EscapeDataString(searchQuery)}/Rezepte.html";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(searchUrl);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                var listNode = document.DocumentNode.SelectSingleNode("//*[@id=\"__layout\"]");

                if (listNode != null)
                {
                    var resultNodes = listNode.SelectNodes(".//div");

                    if (resultNodes != null)
                    {
                        foreach (var node in resultNodes)
                        {
                            var titleNode = node.SelectSingleNode(".//h3");
                            var linkNode = node.SelectSingleNode(".//a");
                            var imageNode = node.SelectSingleNode(".//img");

                            if (titleNode != null && linkNode != null)
                            {
                                var recipe = new Recipe
                                {
                                    RecipeName = titleNode.InnerText.Trim(),
                                    Url = linkNode.GetAttributeValue("href", string.Empty),
                                    Image = imageNode?.GetAttributeValue("src", string.Empty)
                                };
                                recipies.Add(recipe);
                            }
                            else
                            {
                                Console.WriteLine("Title or link node is null for one of the recipies.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No result nodes found in the list node.");
                    }
                }
                else
                {
                    Console.WriteLine("List node is null. The structure of the page might have changed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return recipies;
        }

        public async Task<Recipe> ScrapeCKDetailsAndUpdateRecipie(Recipe searchResultRecipie)
        {
            try
            {
                var html = await _httpClient.GetStringAsync(searchResultRecipie.Url);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                // Parse Recipe Name
                var recipeNameNode = document.DocumentNode.SelectSingleNode("//h1");
                if (recipeNameNode != null)
                {
                    searchResultRecipie.RecipeName = recipeNameNode.InnerText.Trim();
                }
                else
                {
                    Console.WriteLine("Recipe's Name node is null");
                }

                // Parse Image
                var imageNode = document.DocumentNode.SelectSingleNode("//*[@id=\"i-amp-0\"]/img");
                if (imageNode != null)
                {
                    searchResultRecipie.Image = imageNode.GetAttributeValue("src", string.Empty);
                }

                // Parse Cooking Instructions
                var instructionsNode = document.DocumentNode.SelectSingleNode("/html/body/main/article[4]/div[1]");
                if (instructionsNode != null)
                {
                    searchResultRecipie.CookingInstructions = instructionsNode.InnerHtml.Trim();
                }
                else
                {
                    Console.WriteLine("Instructions node is null");
                }

                // Parse Video URL if available
                var videoNode = document.DocumentNode.SelectSingleNode("//video/source");
                if (videoNode != null)
                {
                    searchResultRecipie.VideoUrl = videoNode.GetAttributeValue("src", string.Empty);
                }
                else
                {
                    Console.WriteLine("Video node is null");
                }

                // Parse Cuisine Type
                var cuisineTypeNode = document.DocumentNode.SelectSingleNode("//span[@class='cuisine-type']");
                if (cuisineTypeNode != null)
                {
                    if (Enum.TryParse(cuisineTypeNode.InnerText.Trim(), true, out CuisineType cuisineType))
                    {
                        searchResultRecipie.CuisineType = cuisineType;
                    }
                }
                else
                {
                    Console.WriteLine("Cuisine Type node is null");
                }

                // Parse Difficulty Level
                var difficultyLevelNode = document.DocumentNode.SelectSingleNode("/html/body/main/article[1]/div/div[2]/small/span[2]");
                if (difficultyLevelNode != null)
                {
                    if (Enum.TryParse(difficultyLevelNode.InnerText.Trim(), true, out DifficultyLevel difficultyLevel))
                    {
                        searchResultRecipie.DifficultyLevel = difficultyLevel;
                    }
                }
                else
                {
                    Console.WriteLine("Difficulty Level node is null");
                }

                // Parse Occasion Tags
                var occasionTagsNode = document.DocumentNode.SelectSingleNode("//span[@class='occasion-tags']");
                if (occasionTagsNode != null)
                {
                    if (Enum.TryParse(occasionTagsNode.InnerText.Trim(), true, out OccasionTags occasionTags))
                    {
                        searchResultRecipie.OccasionTags = occasionTags;
                    }
                }
                else
                {
                    Console.WriteLine("Occasion Tags node is null");
                }

                // Parse List of Ingredients
                var ingredientsNode = document.DocumentNode.SelectSingleNode("/html/body/main/article/table");
                if (ingredientsNode != null)
                {
                    var ingredientNodes = ingredientsNode.SelectNodes("/html/body/main/article[2]/table/tbody/tr");
                    if (ingredientNodes != null)
                    {
                        searchResultRecipie.ListofIngredients = ingredientNodes
                            .Select(li => new Ingredient { IngredientsName = li.InnerText.Trim() })
                            .ToList();
                    }
                    else
                    {
                        Console.WriteLine("Ingredient nodes are null");
                    }
                }
                else
                {
                    Console.WriteLine("Ingredients node is null");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scraping searchResultRecipie details: {ex.Message}");
            }

            return searchResultRecipie;
        }

       
        public async Task<List<Recipe>> ScrapeCookpadRecipies(string searchQuery)
        {
            // Check in _dataService.Recipies if recipes are already there, if yes return them
            var existingRecipes = _dataService.Recipies
                .Where(r => r.RecipeName != null && r.RecipeName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (existingRecipes.Any())
            {
                return existingRecipes;
            }

            // If not found, scrape new recipes
            var searchResults = await ScrapeSearchResultsFromCookpad(searchQuery);
            if (searchResults == null || !searchResults.Any())
            {
                return new List<Recipe>(); // Return an empty list if no search results are found
            }

            List<Recipe> detailedRecipes = new List<Recipe>();

            foreach (var recipe in searchResults)
            {
                if (recipe?.Url != null) // Check if the searchResultRecipie and its URL are not null
                {
                    Recipe detailedRecipe = await ScrapeCookpadDetailsAndUpdateRecipie(recipe);
                    if (detailedRecipe != null) // Ensure detailedRecipe is not null before adding
                    {
                        detailedRecipes.Add(detailedRecipe);
                    }
                }
            }

            if (detailedRecipes.Any())
            {
                _dataService.Recipies.AddRange(detailedRecipes);
            }

            return detailedRecipes;
        }

        public async Task<List<Recipe>> ScrapeSearchResultsFromCookpad(string searchQuery)
        {
            List<Recipe> recipes = new List<Recipe>();

            try
            {
                string searchUrl = $"https://cookpad.com/de/suchen/{Uri.EscapeDataString(searchQuery)}";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(searchUrl);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                var listNode = document.DocumentNode.SelectSingleNode("//div[@class='search_results']");

                if (listNode != null)
                {
                    var resultNodes = listNode.SelectNodes(".//div[@class='recipe-preview']");

                    if (resultNodes != null)
                    {
                        foreach (var node in resultNodes)
                        {
                            var titleNode = node.SelectSingleNode(".//h2/a");
                            var linkNode = node.SelectSingleNode(".//a");
                            var imageNode = node.SelectSingleNode(".//img");

                            if (titleNode != null && linkNode != null)
                            {
                                var recipe = new Recipe
                                {
                                    RecipeName = titleNode.InnerText.Trim(),
                                    Url = linkNode.GetAttributeValue("href", string.Empty),
                                    Image = imageNode?.GetAttributeValue("data-original", string.Empty) // Cookpad uses lazy loading
                                };
                                recipes.Add(recipe);
                            }
                            else
                            {
                                Console.WriteLine("Title or link node is null for one of the recipes.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No result nodes found in the list node.");
                    }
                }
                else
                {
                    Console.WriteLine("List node is null. The structure of the page might have changed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return recipes;
        }

        public async Task<Recipe> ScrapeCookpadDetailsAndUpdateRecipie(Recipe searchResultRecipie)
        {
            try
            {
                var html = await _httpClient.GetStringAsync(searchResultRecipie.Url);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                // Parse Recipe Name
                var recipeNameNode = document.DocumentNode.SelectSingleNode("//h1[@class='recipe-title']");
                if (recipeNameNode != null)
                {
                    searchResultRecipie.RecipeName = recipeNameNode.InnerText.Trim();
                }
                else
                {
                    Console.WriteLine("Recipe's Name node is null");
                }

                // Parse Image
                var imageNode = document.DocumentNode.SelectSingleNode("//div[@class='recipe-image']/img");
                if (imageNode != null)
                {
                    searchResultRecipie.Image = imageNode.GetAttributeValue("data-original", string.Empty);
                }
                else
                {
                    Console.WriteLine("Image node is null");
                }

                // Parse Cooking Instructions
                var instructionsNode = document.DocumentNode.SelectSingleNode("//div[@class='step']");
                if (instructionsNode != null)
                {
                    searchResultRecipie.CookingInstructions = instructionsNode.InnerHtml.Trim();
                }
                else
                {
                    Console.WriteLine("Instructions node is null");
                }

                // Parse List of Ingredients
                var ingredientsNode = document.DocumentNode.SelectSingleNode("//div[@class='ingredient-list']");
                if (ingredientsNode != null)
                {
                    var ingredientNodes = ingredientsNode.SelectNodes(".//li[@class='ingredient']");
                    if (ingredientNodes != null)
                    {
                        searchResultRecipie.ListofIngredients = ingredientNodes
                            .Select(li => new Ingredient { IngredientsName = li.InnerText.Trim() })
                            .ToList();
                    }
                    else
                    {
                        Console.WriteLine("Ingredient nodes are null");
                    }
                }
                else
                {
                    Console.WriteLine("Ingredients node is null");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scraping searchResultRecipie details: {ex.Message}");
            }

            return searchResultRecipie;
        }


        public List<Recipe> GetRandomRecipes(List<Recipe> recipes, int count = 7)
        {
            var random = new Random();
            return recipes.OrderBy(x => random.Next()).Take(count).ToList();
        }

    }
}

