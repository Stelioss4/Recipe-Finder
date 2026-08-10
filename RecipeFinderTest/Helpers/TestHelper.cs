using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Recipe_Finder;
using RecipeFinder_WebApp.Data;
using System.Security.Claims;

namespace RecipeFinderTest.Helpers
{
    public static class TestHelper
    {
        public static IDbContextFactory<ApplicationDbContext> CreateDbContextFactory()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TestDbContextFactory(options);
        }

        public static Recipe CreateRecipe(
            int id,
            string name,
            double calories,
            string recipeRoot)
        {
            return new Recipe
            {
                Id = id,
                RecipeName = name,
                RecipeRoot = recipeRoot,
                Url = $"https://test.com/recipe/{id}",
                NutritionValue = new NutritionValue
                {
                    Calories = calories
                }
            };
        }

        private class TestDbContextFactory
            : IDbContextFactory<ApplicationDbContext>
        {
            private readonly DbContextOptions<ApplicationDbContext> _options;

            public TestDbContextFactory(
                DbContextOptions<ApplicationDbContext> options)
            {
                _options = options;
            }

            public ApplicationDbContext CreateDbContext()
            {
                return new ApplicationDbContext(_options);
            }
        }

        public static Mock<UserManager<ApplicationUser>> CreateUserManagerMock(
       ApplicationUser applicationUser)
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);

            userManagerMock
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(applicationUser);

            return userManagerMock;
        }
    }
}