﻿using HtmlAgilityPack;
using Recipe_Finder;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeFinder_WebApp
{
    public class ScrapperService
    {
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

                var listNode = document.DocumentNode.SelectSingleNode("//div[@id='mntl-search-results__content']");

                if (listNode != null)
                {
                    var resultNodes = listNode.SelectNodes(".//div[contains(@class, 'card__detailsContainer')]");

                    if (resultNodes != null)
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

                var listNode = document.DocumentNode.SelectSingleNode("//div[@class='search-list__results']");

                if (listNode != null)
                {
                    var resultNodes = listNode.SelectNodes(".//article");

                    if (resultNodes != null)
                    {
                        foreach (var node in resultNodes)
                        {
                            var titleNode = node.SelectSingleNode(".//a[@class='search-list__headline']");
                            var linkNode = node.SelectSingleNode(".//a[@class='search-list__headline']");
                            var imageNode = node.SelectSingleNode(".//img[@class='search-list__image']");

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
