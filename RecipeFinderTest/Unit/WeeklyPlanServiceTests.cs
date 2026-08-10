using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Moq;
using Recipe_Finder;
using RecipeFinder_WebApp.Data;
using RecipeFinderTest.Helpers;
using System.Security.Claims;
using Xunit;

namespace RecipeFinderTest.Unit
{
    public class WeeklyPlanServiceTests
    {
        [Fact]
        public async Task GenerateWeeklyPlanAsync_ExcludesRecipesAboveMaxCalories()
        {
            // Arrange
            var factory = TestHelper.CreateDbContextFactory();

            var recipes = new List<Recipe>
            {
                TestHelper.CreateRecipe(
                    1,
                    "Chicken Pasta",
                    350,
                    "chicken pasta"),

                TestHelper.CreateRecipe(
                    2,
                    "Beef Rice",
                    420,
                    "beef rice"),

                TestHelper.CreateRecipe(
                    3,
                    "Greek Salad",
                    280,
                    "greek salad"),

                TestHelper.CreateRecipe(
                    4,
                    "Chicken Soup",
                    310,
                    "chicken soup"),

                TestHelper.CreateRecipe(
                    5,
                    "Vegetable Curry",
                    450,
                    "vegetable curry"),

                TestHelper.CreateRecipe(
                    6,
                    "Fish Tacos",
                    490,
                    "fish tacos"),

                TestHelper.CreateRecipe(
                    7,
                    "Turkey Wrap",
                    400,
                    "turkey wrap"),

                // This recipe must be excluded
                TestHelper.CreateRecipe(
                    8,
                    "Mega Pizza",
                    750,
                    "mega pizza")
            };

            // Seed recipes
            using (var context = factory.CreateDbContext())
            {
                context.Recipes.AddRange(recipes);
                await context.SaveChangesAsync();
            }

            // Create authenticated identity
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
                new Claim(ClaimTypes.Name, "testuser")
            };

            var identity = new ClaimsIdentity(
                claims,
                "TestAuth");

            var claimsPrincipal =
                new ClaimsPrincipal(identity);

            // Mock AuthenticationStateProvider
            var authenticationStateProviderMock =
                new Mock<AuthenticationStateProvider>();

            authenticationStateProviderMock
                .Setup(x => x.GetAuthenticationStateAsync())
                .ReturnsAsync(
                    new AuthenticationState(claimsPrincipal));

            // Create test User
            var user = new User
            {
                Id = 1,
                Name = "Test User",

                FavoriteRecipes = new List<Recipe>(),

                WeeklyPlan = new List<Recipe>(),

                ShoppingList = new List<Ingredient>(),

                UserPreferences = new UserPreferences
                {
                    Id = 1,
                    UserId = 1,
                    WeeklyPlanDays = 7
                }
            };

            // Create ASP.NET Identity user
            var applicationUser = new ApplicationUser
            {
                Id = "test-user-id",
                UserName = "testuser",
                User = user
            };

            // Seed user
            using (var context = factory.CreateDbContext())
            {
                context.Users.Add(applicationUser);
                await context.SaveChangesAsync();
            }

            // Mock UserManager
            var userStoreMock =
                new Mock<IUserStore<ApplicationUser>>();

            var userManagerMock =
                new Mock<UserManager<ApplicationUser>>(
                    userStoreMock.Object,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

            userManagerMock
                .Setup(x => x.GetUserAsync(
                    It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(applicationUser);

            // Other DataService dependencies
            var navigationManagerMock =
                new Mock<NavigationManager>();

            var httpClientFactoryMock =
                new Mock<IHttpClientFactory>();

            var classificationService =
                new RecipeClassificationService();

            // Real DataService
            var dataService =
                new DataService(
                    navigationManagerMock.Object,
                    httpClientFactoryMock.Object,
                    factory,
                    userManagerMock.Object,
                    authenticationStateProviderMock.Object,
                    classificationService);

            // Real WeeklyPlanService
            var weeklyPlanService =
                new WeeklyPlanService(
                    dataService,
                    navigationManagerMock.Object,
                    factory);

            // Act
            var result =
                await weeklyPlanService.GenerateWeeklyPlanAsync(
                    maxCalories: 500,
                    maxPrepTime: null,
                    preferredFavoriteRecipes: 0);

            // Assert
            Assert.Equal(7, result.Count);

            Assert.DoesNotContain(
                result,
                recipe =>
                    recipe.NutritionValue?.Calories > 500);

            Assert.DoesNotContain(
                result,
                recipe =>
                    recipe.RecipeName == "Mega Pizza");
        }
    }
}