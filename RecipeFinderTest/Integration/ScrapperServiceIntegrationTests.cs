using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using RecipeFinder_WebApp.Data;
using RecipeFinderTest;
using RecipeFinderTest.Integration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RecipeFinderTest
{
    public class SaveScrapedRecipesTests
    {

        [Fact]
        public async Task SaveScrapedRecipesAsync_PersistsRecipesInDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var factory = new TestDbContextFactory(options);
            var service = new RecipePersistenceService(factory);


            using var context = factory.CreateDbContext();

            var recipes = new List<Recipe>
            {
                new Recipe { RecipeName = "Test1", CookingInstructions = "do it" , Id = 1 , Rating = 4.4 , ListOfIngredients = new List<Ingredient>{new Ingredient { IngredientsName = "something" },new Ingredient { IngredientsName = "something more" }}},
                new Recipe { RecipeName = "Test2", CookingInstructions = "do it again" , Id = 2 , Rating = 4.4 ,  ListOfIngredients = new List<Ingredient>{new Ingredient { IngredientsName = "something1" },new Ingredient { IngredientsName = "something more1" }}}
            };

            await service.SaveScrapedRecipesAsync(recipes);

            var saved = await context.Recipes.ToListAsync();
            Assert.Equal(2, saved.Count);
        }
    }
}