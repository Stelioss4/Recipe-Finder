using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Recipe_Finder;
using RecipeFinder_WebApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using RecipeFinder_WebApp.Data.AI;
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

        public static WeeklyPlanService CreateWeeklyPlanService(DataService dataService, NavigationManager navigationManager, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            var recipeAgentService = new RecipeAgentService(
                contextFactory,
                dataService);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();

            var httpClient = new HttpClient();

            var openRouterService = new OpenRouterService(
                httpClient,
                configuration);

            return new WeeklyPlanService(
                dataService,
                navigationManager,
                contextFactory,
                recipeAgentService,
                openRouterService);
        }
    }
}