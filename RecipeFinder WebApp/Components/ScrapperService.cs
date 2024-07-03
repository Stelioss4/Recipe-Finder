using HtmlAgilityPack;
using Recipe_Finder;
using RecipeFinder_WebApp.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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

        public async Task<Recipe> GetRecipeDetails(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var pageContent = await response.Content.ReadAsStringAsync();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(pageContent);

                var recipe = new Recipe
                {
                    RecipeName = htmlDocument.DocumentNode.SelectSingleNode("//h1")?.InnerText,
                    Image = htmlDocument.DocumentNode.SelectSingleNode("//img[@class='main-image']")?.GetAttributeValue("src", ""),
                    CookingInstructions = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='instructions']")?.InnerHtml,
                    VideoUrl = htmlDocument.DocumentNode.SelectSingleNode("//video/source")?.GetAttributeValue("src", ""),

                };
                return recipe;
            }
            return null;
        }


        public async Task<List<Recipe>> ScrappingOnAllRecipe(string searchQuery)
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
                            var imageNode = node.SelectSingleNode(".//img");

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

        public async Task<List<Recipe>> ScrapeCKRecipies(string searchQuery)
        {
            var SearchResults = ScrapeSearchResultsFromChefkoch(searchQuery).Result;
            foreach (var r in SearchResults)
            {
                var detail = scrapeCKDetails(r.Url);
            }

            //put all the results in _ds.Reciepies
            return SearchResults;

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

        public Recipe scrapeCKDetails(string url)
        {
            Recipe recipe = new Recipe();
            List<Ingredient> ingredients = new List<Ingredient>();

            try
            {
                var html = _httpClient.GetStringAsync(url).Result;

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                // Parse Recipe Name
                var recipeNameNode = document.DocumentNode.SelectSingleNode("//h1[@class='recipe-title']");
                recipe.RecipeName = recipeNameNode?.InnerText.Trim();

                // Parse Recipe Image
                var imageNode = document.DocumentNode.SelectSingleNode("//img[@class='recipe-image']");
                recipe.Image = imageNode?.GetAttributeValue("src", string.Empty);

                // Parse Cooking Instructions
                var instructionsNode = document.DocumentNode.SelectSingleNode("//div[@class='instructions']");
                recipe.CookingInstructions = instructionsNode?.InnerHtml.Trim();

                // Parse Video URL if available
                var videoNode = document.DocumentNode.SelectSingleNode("//video/source");
                recipe.VideoUrl = videoNode?.GetAttributeValue("src", string.Empty);

                // Parse Cuisine Type
                var cuisineTypeNode = document.DocumentNode.SelectSingleNode("//span[@class='cuisine-type']");
                if (cuisineTypeNode != null)
                {
                    Enum.TryParse(cuisineTypeNode.InnerText.Trim(), true, out CuisineType cuisineType);
                    recipe.CuisineType = cuisineType;
                }

                // Parse Difficulty Level
                var difficultyLevelNode = document.DocumentNode.SelectSingleNode("//span[@class='difficulty-level']");
                if (difficultyLevelNode != null)
                {
                    Enum.TryParse(difficultyLevelNode.InnerText.Trim(), true, out DifficultyLevel difficultyLevel);
                    recipe.DifficultyLevel = difficultyLevel;
                }

                // Parse Occasion Tags
                var occasionTagsNode = document.DocumentNode.SelectSingleNode("//span[@class='occasion-tags']");
                if (occasionTagsNode != null)
                {
                    Enum.TryParse(occasionTagsNode.InnerText.Trim(), true, out OccasionTags occasionTags);
                    recipe.OccasionTags = occasionTags;
                }

                // Parse List of Ingredients
                var ingredientsNode = document.DocumentNode.SelectSingleNode("//ul[@class='ingredients-list']");
                if (ingredientsNode != null)
                {
                    recipe.ListofIngredients = ingredientsNode.SelectNodes(".//li")
                        .Select(li => new Ingredient { Name = li.InnerText.Trim() })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine($"Error scraping recipe details: {ex.Message}");
            }

            return recipe;
        }

        public List<Recipe> GetRandomRecipes(List<Recipe> recipes, int count = 7)
        {
            var random = new Random();
            return recipes.OrderBy(x => random.Next()).Take(count).ToList();
        }
    }
}

