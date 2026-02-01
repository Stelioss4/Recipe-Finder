using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using RecipeFinder_WebApp.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RecipeFinderTest.Unit
{
    public class SaveScrapedRecipesTests
    {
        [Fact]
        public void FilterRecipesWithInstructions_ReturnsOnlyRecipesWithInstructions()
        {
            // Arrange
            var service = new RecipeFilterService();

            var recipes = new List<Recipe>
        {
            new Recipe { RecipeName = "R1", CookingInstructions = "step 1" },
            new Recipe { RecipeName = "R2", CookingInstructions = null },
            new Recipe { RecipeName = "R3", CookingInstructions = "" }
        };

            // Act
            var result = service.FilterRecipesWithInstructions(recipes);

            // Assert
            Assert.Single(result);
            Assert.Equal("R1", result[0].RecipeName);
        }
    }
}