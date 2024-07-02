using HtmlAgilityPack;
using Recipe_Finder;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeFinder_WebApp
{
    public class ScrapperService
    {
        private readonly HttpClient _httpClient;

        public ScrapperService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Recipe> GetRecipeDetails(string recipeUrl)
        {
            var response = await _httpClient.GetAsync(recipeUrl);
            if (response.IsSuccessStatusCode)
            {
                var pageContent = await response.Content.ReadAsStringAsync();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(pageContent);

                var recipe = new Recipe
                {
                    RecipeName = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='page-title']")?.InnerText,
                    Image = htmlDocument.DocumentNode.SelectSingleNode("//img[@class='i-amphtml-fill-content i-amphtml-replaced-content']")?.GetAttributeValue("src", ""),
                    CookingInstructions = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='ds-box ds-box--fixed ds-grid-float ds-col-12']")?.InnerHtml,
                    VideoUrl = htmlDocument.DocumentNode.SelectSingleNode("//video/source")?.GetAttributeValue("src", "") // Adjust the XPath as needed
                };

                // Ensure to decode HTML entities in the extracted text
                if (recipe != null)
                {
                    recipe.RecipeName = System.Net.WebUtility.HtmlDecode(recipe.RecipeName);
                    recipe.CookingInstructions = System.Net.WebUtility.HtmlDecode(recipe.CookingInstructions);
                }
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

        public async Task<List<Recipe>> ScrapeRecipesFromChefkoch(string searchQuery)
        {
            List<Recipe> recipes = new List<Recipe>();

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
    }


}

