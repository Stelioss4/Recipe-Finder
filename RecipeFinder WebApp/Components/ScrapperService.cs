using HtmlAgilityPack;
using Recipe_Finder;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecipeFinder_WebApp.Components
{
    public class ScrapperService
    {
        public async Task<List<Recipe>> ScrappingOnAllRecipe(string searchQuery)
        {
            List<Recipe> recipes = new List<Recipe>();

            try
            {
                HtmlWeb web = new HtmlWeb();
                string searchUrl = $"https://www.allrecipes.com/search?q={Uri.EscapeDataString(searchQuery)}";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(searchUrl);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                // Select nodes containing recipe details               
                var listNode = document.DocumentNode.SelectSingleNode("//*[@id=\"searchTemplate_1-0\"]/body");

                var resultNodes = listNode.ChildNodes;
                if (listNode != null)
                {
                    foreach (var node in resultNodes)
                    {
                        var titleNode = node.SelectSingleNode(".//h3[@class='card__title']");
                        var linkNode = node.SelectSingleNode(".//a[@class='card__titleLink']");
                        var imageNode = node.SelectSingleNode(".//img[@class='card__img']");

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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return recipes;
        }

        public async Task<List<Recipe>> ScrappingOnChefKoch(string searchQuery)
        {
            List<Recipe> recipes = new List<Recipe>();

            try
            {
                HtmlWeb web = new HtmlWeb();
                string searchUrl = $"https://www.chefkoch.de/rs/s0/{Uri.EscapeDataString(searchQuery)}";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(searchUrl);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                // Select nodes containing recipe details               
                var listNode = document.DocumentNode.SelectSingleNode("//*[@id=\"__layout\"]/div/div[1]/main/section");

                var resultNodes = listNode.ChildNodes;
                if (listNode != null)
                {
                    foreach (var node in resultNodes)
                    {
                        var titleNode = node.SelectSingleNode(".//h1[@class='card__title']");
                        var linkNode = node.SelectSingleNode(".//a[@class='card__titleLink']");
                        var imageNode = node.SelectSingleNode(".//img[@class='card__img']");

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
                    }
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
