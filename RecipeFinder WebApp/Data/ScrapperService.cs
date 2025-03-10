﻿using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    public class ScrapperService
    {
        private readonly HttpClient _httpClient;
        private DataService _dataService;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;


        public ScrapperService(HttpClient httpClient, DataService ds, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _httpClient = httpClient;
            _dataService = ds;
            _contextFactory = contextFactory;
        }

        public async Task<List<Recipe>> ScrapeFromAllRecipe(string searchQuery)
        {
            using var _context = _contextFactory.CreateDbContext();

            var existingRecipes = await _dataService.GetRecipesFromDatabaseAsync(searchQuery, Constants.ALLRECIPE_URL);

            if (existingRecipes.Count > 0)
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
                if (recipe?.Url != null) // Check if the searchResultRecipe and its URL are not null
                {
                    recipe.SearchTerms = new List<RecipeSearchTerm>
                    {
                        new RecipeSearchTerm { Term = searchQuery }
                    }; recipe.SourceDomain = Constants.ALLRECIPE_URL; // Set the SourceDomain
                    Recipe detailedRecipe = await ScrapeAllRecipesDetailsAndUpdateRecipe(recipe);
                    if (detailedRecipe != null) // Ensure detailedRecipe is not null before adding
                    {
                        detailedRecipes.Add(detailedRecipe);
                    }
                }
            }

            if (detailedRecipes.Count > 0)
            {
                _context.Recipes.AddRange(detailedRecipes);
                await _context.SaveChangesAsync();
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
                            var imageNode = node.SelectSingleNode("//img");

                            if (titleNode != null && linkNode != null)
                            {
                                var imageUrl = imageNode?.GetAttributeValue("src", string.Empty);

                                // Create a new Recipe object
                                var recipe = new Recipe
                                {
                                   RecipeName = titleNode.InnerText.Trim(),
                                   Url = linkNode.GetAttributeValue("href", string.Empty),
                                   Image = !string.IsNullOrEmpty(imageUrl) ? await DownloadImageAsByteArray(imageUrl) : null, // Convert image URL to byte[]
                                   SearchTerms = new List<RecipeSearchTerm>
                                   {
                                       new RecipeSearchTerm { Term = searchQuery }
                                   },
                                   SourceDomain = new Uri(Constants.ALLRECIPE_URL).Host.ToLowerInvariant() // Set SourceDomain and normalize
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

        public async Task<Recipe> ScrapeAllRecipesDetailsAndUpdateRecipe(Recipe searchResultRecipe)
        {
            try
            {
                //if (!Uri.IsWellFormedUriString(searchResultRecipe.Url, UriKind.Absolute))
                //{
                //    searchResultRecipe.Url = $"https://www.allrecipes.com {searchResultRecipe.Url}";
                //}

                var html = await _httpClient.GetStringAsync(searchResultRecipe.Url);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                // Parse Recipe Name
                var recipeNameNode = document.DocumentNode.SelectSingleNode("//*[@id=\"article-header--recipe_1-0\"]/h1");
                if (recipeNameNode != null)
                {
                    searchResultRecipe.RecipeName = recipeNameNode.InnerText.Trim();
                }
                else
                {
                    Console.WriteLine("Recipe's Name node is null");
                }

                // Parse Image
                var imageNode = document.DocumentNode.SelectSingleNode("//*[@id=\"article__photo-ribbon_1-0\"]");
                if (imageNode != null)
                {
                    var imageUrl = imageNode.GetAttributeValue("src", string.Empty);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        searchResultRecipe.Image = await DownloadImageAsByteArray(imageUrl);
                    }
                }
                else
                {
                    Console.WriteLine("Image node is null");
                }

                // Parse Cooking Instructions
                var instructionsNode = document.DocumentNode.SelectSingleNode("//*[@id=\"mm-recipes-intro__content_1-0\"]");
                if (instructionsNode != null)
                {
                    searchResultRecipe.CookingInstructions = instructionsNode.InnerHtml.Trim();
                }
                else
                {
                    Console.WriteLine("Instructions node is null");
                }

                // Parse Video URL if available
                var videoNode = document.DocumentNode.SelectSingleNode("//*[@id=\"article__primary-video-container_1-0\"]");
                if (videoNode != null)
                {
                    searchResultRecipe.VideoUrl = videoNode.GetAttributeValue("src", string.Empty);
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
                        searchResultRecipe.CuisineType = cuisineType;
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

                    searchResultRecipe.DifficultyLevel = difficultyLevelNode.InnerText.Trim();

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
                        searchResultRecipe.OccasionTags = occasionTags;
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
                        searchResultRecipe.ListOfIngredients = ingredientNodes
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
                Console.WriteLine($"Error scraping searchResultRecipe details: {ex.Message}");
            }

            return searchResultRecipe;
        }

        public async Task<List<Recipe>> ScrapeCKRecipes(string searchQuery)
        {
            using var _context = _contextFactory.CreateDbContext();

            var existingRecipes = await _dataService.GetRecipesFromDatabaseAsync(searchQuery, Constants.CHEFKOCH_URL);
            if (existingRecipes.Count > 0)
            {
                return existingRecipes;
            }
            // If not found, scrape new recipes
            var searchResults = await ScrapeSearchResultsFromChefKoch(searchQuery);
            if (searchResults == null || !searchResults.Any())
            {
                return new List<Recipe>(); // Return an empty list if no search results are found
            }

            List<Recipe> detailedRecipes = new List<Recipe>();

            foreach (var recipe in searchResults)
            {
                // Skip recipes that are already in the database
                if (existingRecipes.Any(r => r.Url == recipe.Url))
                {
                    continue;
                }

                // Scrape details and add new recipes
                recipe.SearchTerms = new List<RecipeSearchTerm> { new RecipeSearchTerm { Term = searchQuery } }; // Set the search terms
                recipe.SourceDomain = Constants.CHEFKOCH_URL; // Set the SourceDomain
                Recipe detailedRecipe = await ScrapeCKDetailsAndUpdateRecipe(recipe);
                if (detailedRecipe != null) // Ensure detailedRecipe is not null before adding
                {
                    detailedRecipes.Add(detailedRecipe);
                }
            }

            // Add new recipes to the database
            if (detailedRecipes.Count > 0)
            {
                _context.Recipes.AddRange(detailedRecipes);
                await _context.SaveChangesAsync();
            }

            return detailedRecipes;
        }

        public async Task<List<Recipe>> ScrapeSearchResultsFromChefKoch(string searchQuery)
        {
            List<Recipe> recipes = new List<Recipe>();

            try
            {
                string searchUrl = $"https://www.chefkoch.de/rs/s0/{Uri.EscapeDataString(searchQuery)}/Rezepte.html";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(searchUrl);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                var listNode = document.DocumentNode.SelectSingleNode("//*[@id=\"__layout\"]/div/div[1]/main/section/div[5]/div");

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
                                var imageUrl = imageNode?.GetAttributeValue("src", string.Empty);

                                // Create a new Recipe object
                                var recipe = new Recipe
                                {
                                    RecipeName = titleNode.InnerText.Trim(),
                                    Url = linkNode.GetAttributeValue("href", string.Empty),
                                    Image = !string.IsNullOrEmpty(imageUrl) ? await DownloadImageAsByteArray(imageUrl) : null, // Convert image URL to byte[]
                                    SearchTerms = new List<RecipeSearchTerm> { new RecipeSearchTerm { Term = searchQuery } },
                                    SourceDomain = new Uri(Constants.CHEFKOCH_URL).Host.ToLowerInvariant() // Set SourceDomain and normalize
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

        public async Task<Recipe> ScrapeCKDetailsAndUpdateRecipe(Recipe searchResultRecipe)
        {
            try
            {
                var html = await _httpClient.GetStringAsync(searchResultRecipe.Url);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                // Parse Recipe Name
                var recipeNameNode = document.DocumentNode.SelectSingleNode("//h1");
                if (recipeNameNode != null)
                {
                    searchResultRecipe.RecipeName = recipeNameNode.InnerText.Trim();
                }
                else
                {
                    Console.WriteLine("Recipe's Name node is null");
                }

                // Parse Image
                var imageNode = document.DocumentNode.SelectSingleNode("//*[@id=\"i-amp-0\"]/img");
                if (imageNode != null)
                {
                    var imageUrl = imageNode.GetAttributeValue("src", string.Empty);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        searchResultRecipe.Image = await DownloadImageAsByteArray(imageUrl);
                    }
                }

                // Parse Cooking Instructions
                var instructionsNodes = new List<HtmlNode>
        {
            document.DocumentNode.SelectSingleNode("/html/body/main/article[3]/div[1]"),
            document.DocumentNode.SelectSingleNode("/html/body/main/article[4]/div[1]")
        };

                HtmlNode correctInstructionsNode = instructionsNodes.FirstOrDefault(node => node != null && IsValidInstructionsNode(node));

                if (correctInstructionsNode != null)
                {
                    searchResultRecipe.CookingInstructions = correctInstructionsNode.InnerHtml.Trim();
                }
                else
                {
                    Console.WriteLine("Valid instructions node not found");
                }

                // Parse Video URL if available
                var videoNode = document.DocumentNode.SelectSingleNode("//video/source");
                if (videoNode != null)
                {
                    searchResultRecipe.VideoUrl = videoNode.GetAttributeValue("src", string.Empty);
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
                        searchResultRecipe.CuisineType = cuisineType;
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
                    searchResultRecipe.DifficultyLevel = difficultyLevelNode.InnerText.Trim();
                }
                else
                {
                    Console.WriteLine("Difficulty Level node is null");
                }

                // Parse Cooking Time
                var cookingTimeNode = document.DocumentNode.SelectSingleNode("/html/body/main/article[1]/div/div[2]/small/span[1]");
                if (cookingTimeNode != null)
                {
                    searchResultRecipe.Time = cookingTimeNode.InnerText.Trim();
                }
                else
                {
                    Console.WriteLine("Cooking time node is null");
                }

                // Parse Occasion Tags
                var occasionTagsNode = document.DocumentNode.SelectSingleNode("//span[@class='occasion-tags']");
                if (occasionTagsNode != null)
                {
                    if (Enum.TryParse(occasionTagsNode.InnerText.Trim(), true, out OccasionTags occasionTags))
                    {
                        searchResultRecipe.OccasionTags = occasionTags;
                    }
                }
                else
                {
                    Console.WriteLine("Occasion Tags node is null");
                }

                // Parse List of Ingredients
                var ingredientsNode = document.DocumentNode.SelectSingleNode("/html/body/main/article[2]/table/tbody");
                if (ingredientsNode != null)
                {
                    var ingredientRows = ingredientsNode.SelectNodes("tr");
                    if (ingredientRows != null)
                    {
                        searchResultRecipe.ListOfIngredients = ingredientRows
                            .Select(row =>
                            {
                                // Extract the amount and name from specific td elements
                                var amountNode = row.SelectSingleNode("td[1]");
                                var nameNode = row.SelectSingleNode("td[2]");

                                var amount = amountNode?.InnerText.Trim() ?? ""; // Get the amount or empty if null
                                var ingredientName = nameNode?.InnerText.Trim() ?? ""; // Get the name or empty if null

                                return new Ingredient
                                {
                                    IngredientsName = ingredientName,
                                    Amount = amount // Assuming you have an Amount property in the Ingredient class
                                };
                            })
                            .ToList();
                    }
                    else
                    {
                        Console.WriteLine("Ingredient rows are null");
                    }
                }
                else
                {
                    Console.WriteLine("Ingredients node is null");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scraping searchResultRecipe details: {ex.Message}");
            }

            return searchResultRecipe;
        }

        public async Task<byte[]> DownloadImageAsByteArray(string imageUrl, string baseUrl = null)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return null;

            // If the URL is relative and a base URL is provided, prepend the base URL
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute) && !string.IsNullOrWhiteSpace(baseUrl))
            {
                imageUrl = new Uri(new Uri(baseUrl), imageUrl).ToString();
            }

            try
            {
                return await _httpClient.GetByteArrayAsync(imageUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading image from {imageUrl}: {ex.Message}");
                return null;
            }
        }

        public List<Recipe> GetRandomFavoriteRecipes(List<Recipe> favoriteRecipes, int count = 7)
        {
            if (favoriteRecipes == null || favoriteRecipes.Count == 0)
            {
                return new List<Recipe>(); // Return an empty list if there are no favorite recipes
            }

            Random random = new Random();
            return favoriteRecipes
                    .OrderBy(x => random.Next())
                    .Take(count)
                    .ToList();
        }

        private bool IsValidInstructionsNode(HtmlNode node)
        {
            // Define the criteria for a valid instructions node
            // For example, check if the node contains a specific class or id, or contains a certain amount of text
            if (node != null)
            {
                // Example criteria: node must contain a minimum amount of text
                return node.InnerText.Trim().Length > 150;
            }

            return false;
        }
    }
}

