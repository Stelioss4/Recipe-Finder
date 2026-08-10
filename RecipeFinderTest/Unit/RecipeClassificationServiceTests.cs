using RecipeFinder_WebApp.Data;
using Xunit;

namespace RecipeFinderTest.Unit
{
    public class RecipeClassificationServiceTests
    {
        private readonly RecipeClassificationService _service = new();

        [Fact]
        public void GetRecipeRoot_ReturnsBurger_WhenRecipeContainsCheeseburger()
        {
            // Arrange
            var recipeName = "Classic Cheeseburger";

            // Act
            var result = _service.GetRecipeRoot(recipeName);

            // Assert
            Assert.Equal("burger", result);
        }

        [Fact]
        public void GetRecipeRoot_ReturnsBananaBread_WhenRecipeIsBananenbrot()
        {
            // Arrange
            var recipeName = "Bananenbrot";

            // Act
            var result = _service.GetRecipeRoot(recipeName);

            // Assert
            Assert.Equal("banana bread", result);
        }

        [Fact]
        public void NormalizeRecipeName_NormalizesGermanCharacters()
        {
            // Arrange
            var recipeName = "  Käse Brötchen für Spaß  ";

            // Act
            var result = _service.NormalizeRecipeName(recipeName);

            // Assert
            Assert.Equal("kaese broetchen fuer spass", result);
        }

        [Fact]
        public void HaveSameRoot_ReturnsTrue_WhenRecipesBelongToSameFamily()
        {
            // Arrange
            var firstRecipe = "American Cheeseburger";
            var secondRecipe = "Chicken Burger";

            // Act
            var result = _service.HaveSameRoot(firstRecipe, secondRecipe);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetRecipeRoot_ReturnsEmptyString_WhenRecipeNameIsEmpty()
        {
            // Arrange
            var recipeName = "";

            // Act
            var result = _service.GetRecipeRoot(recipeName);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetRecipeRoot_ReturnsFirstTwoRelevantWords_WhenNoKnownPatternMatches()
        {
            // Arrange
            var recipeName = "Creamy Lemon Chicken";

            // Act
            var result = _service.GetRecipeRoot(recipeName);

            // Assert
            Assert.Equal("lemon chicken", result);
        }

        [Fact]
        public void GetRecipeRoot_ReturnsSingleRelevantWord_WhenOnlyOneRemains()
        {
            // Arrange
            var recipeName = "Homemade Risotto";

            // Act
            var result = _service.GetRecipeRoot(recipeName);

            // Assert
            Assert.Equal("risotto", result);
        }

        [Fact]
        public void GetRecipeRoot_ReturnsEmptyString_WhenAllWordsAreIgnored()
        {
            // Arrange
            var recipeName = "Classic Homemade Best";

            // Act
            var result = _service.GetRecipeRoot(recipeName);

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}