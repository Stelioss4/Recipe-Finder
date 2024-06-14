using HtmlAgilityPack;
using Recipe_Finder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeFinder_WebApp.Components
{
    public class ScrapperService
    {
        public async Task<List<Recipe>> Scrapping(string searchQuery)
        {
            List<Recipe> recipes = new List<Recipe>();

            try
            {
                HtmlWeb web = new HtmlWeb();
                string searchUrl = "https://www.allrecipes.com/";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(searchUrl);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                // Select nodes containing recipe details
                var nodes = document.DocumentNode.SelectNodes("//input[@class='mntl-search-form--open__search-input']");
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        var titleNode = node.SelectSingleNode(".//span[@class='card__title-text']");
                        var linkNode = node.SelectSingleNode(".//span[@class='link__wrapper']");
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
