using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Recipe_Finder;
using RecipeFinder_WebApp.Data;
using RecipeFinderTest.Helpers;
using System.Security.Claims;

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
                TestHelper.CreateUserManagerMock(applicationUser);

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
            var weeklyPlanService = TestHelper.CreateWeeklyPlanService(dataService, navigationManagerMock.Object, factory);

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

        [Fact]
        public async Task GenerateWeeklyPlanAsync_ExcludesRecipesAboveMaxPreparationTime()
        {
            // Arrange
            var factory = TestHelper.CreateDbContextFactory();

            var recipes = new List<Recipe>
    {
        TestHelper.CreateRecipe(1, "Chicken Pasta", 350, "chicken pasta"),
        TestHelper.CreateRecipe(2, "Beef Rice", 420, "beef rice"),
        TestHelper.CreateRecipe(3, "Greek Salad", 280, "greek salad"),
        TestHelper.CreateRecipe(4, "Chicken Soup", 310, "chicken soup"),
        TestHelper.CreateRecipe(5, "Vegetable Curry", 450, "vegetable curry"),
        TestHelper.CreateRecipe(6, "Fish Tacos", 490, "fish tacos"),
        TestHelper.CreateRecipe(7, "Turkey Wrap", 400, "turkey wrap"),
        TestHelper.CreateRecipe(8, "Slow Roast", 450, "slow roast")
    };

            recipes[0].Time = "30 min";
            recipes[1].Time = "45 min";
            recipes[2].Time = "1 std";
            recipes[3].Time = "1h 15min";
            recipes[4].Time = "50 min";
            recipes[5].Time = "40 min";
            recipes[6].Time = "55 min";

            // This one must be excluded
            recipes[7].Time = "2h";

            using (var context = factory.CreateDbContext())
            {
                context.Recipes.AddRange(recipes);
                await context.SaveChangesAsync();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
        new Claim(ClaimTypes.Name, "testuser")
    };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var authenticationStateProviderMock =
                new Mock<AuthenticationStateProvider>();

            authenticationStateProviderMock
                .Setup(x => x.GetAuthenticationStateAsync())
                .ReturnsAsync(new AuthenticationState(claimsPrincipal));

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

            var applicationUser = new ApplicationUser
            {
                Id = "test-user-id",
                UserName = "testuser",
                User = user
            };

            using (var context = factory.CreateDbContext())
            {
                context.Users.Add(applicationUser);
                await context.SaveChangesAsync();
            }

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            var userManagerMock =
                TestHelper.CreateUserManagerMock(applicationUser);

            userManagerMock
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(applicationUser);

            var navigationManagerMock = new Mock<NavigationManager>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var classificationService = new RecipeClassificationService();

            var dataService = new DataService(
                navigationManagerMock.Object,
                httpClientFactoryMock.Object,
                factory,
                userManagerMock.Object,
                authenticationStateProviderMock.Object,
                classificationService);

            var weeklyPlanService = TestHelper.CreateWeeklyPlanService(dataService, navigationManagerMock.Object, factory);

            // Act
            var result = await weeklyPlanService.GenerateWeeklyPlanAsync(
                maxCalories: null,
                maxPrepTime: 75,
                preferredFavoriteRecipes: 0);

            // Assert
            Assert.Equal(7, result.Count);

            Assert.DoesNotContain(
                result,
                recipe => recipe.RecipeName == "Slow Roast");
        }

        [Fact]
        public async Task GenerateWeeklyPlanAsync_DoesNotIncludeRecipesWithDuplicateRoots()
        {
            // Arrange
            var factory = TestHelper.CreateDbContextFactory();

            var recipes = new List<Recipe>
    {
        TestHelper.CreateRecipe(1, "Chicken Burger", 400, "burger"),
        TestHelper.CreateRecipe(2, "Cheeseburger", 450, "burger"),

        TestHelper.CreateRecipe(3, "Chicken Pasta", 350, "chicken pasta"),
        TestHelper.CreateRecipe(4, "Beef Rice", 420, "beef rice"),
        TestHelper.CreateRecipe(5, "Greek Salad", 280, "greek salad"),
        TestHelper.CreateRecipe(6, "Chicken Soup", 310, "chicken soup"),
        TestHelper.CreateRecipe(7, "Vegetable Curry", 450, "vegetable curry"),
        TestHelper.CreateRecipe(8, "Fish Tacos", 490, "fish tacos")
    };

            using (var context = factory.CreateDbContext())
            {
                context.Recipes.AddRange(recipes);
                await context.SaveChangesAsync();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
        new Claim(ClaimTypes.Name, "testuser")
    };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var authenticationStateProviderMock =
                new Mock<AuthenticationStateProvider>();

            authenticationStateProviderMock
                .Setup(x => x.GetAuthenticationStateAsync())
                .ReturnsAsync(new AuthenticationState(claimsPrincipal));

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

            var applicationUser = new ApplicationUser
            {
                Id = "test-user-id",
                UserName = "testuser",
                User = user
            };

            using (var context = factory.CreateDbContext())
            {
                context.Users.Add(applicationUser);
                await context.SaveChangesAsync();
            }

            var userManagerMock =
                TestHelper.CreateUserManagerMock(applicationUser);

            var navigationManagerMock = new Mock<NavigationManager>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var classificationService =
                new RecipeClassificationService();

            var dataService = new DataService(
                navigationManagerMock.Object,
                httpClientFactoryMock.Object,
                factory,
                userManagerMock.Object,
                authenticationStateProviderMock.Object,
                classificationService);

            var weeklyPlanService = TestHelper.CreateWeeklyPlanService(dataService, navigationManagerMock.Object, factory);

            // Act
            var result = await weeklyPlanService.GenerateWeeklyPlanAsync(
                maxCalories: null,
                maxPrepTime: null,
                preferredFavoriteRecipes: 0);

            // Assert
            Assert.Equal(7, result.Count);

            var roots = result
                .Select(recipe => recipe.RecipeRoot)
                .ToList();

            Assert.Equal(
                roots.Count,
                roots.Distinct().Count());
        }

        [Fact]
        public async Task GenerateWeeklyPlanAsync_IncludesPreferredNumberOfFavoriteRecipes()
        {
            // Arrange
            var factory = TestHelper.CreateDbContextFactory();

            var favorite1 = TestHelper.CreateRecipe(1, "Chicken Pasta", 350, "chicken pasta");
            var favorite2 = TestHelper.CreateRecipe(2, "Beef Rice", 420, "beef rice");
            var favorite3 = TestHelper.CreateRecipe(3, "Greek Salad", 280, "greek salad");

            var recipes = new List<Recipe>
    {
        favorite1,
        favorite2,
        favorite3,
        TestHelper.CreateRecipe(4, "Chicken Soup", 310, "chicken soup"),
        TestHelper.CreateRecipe(5, "Vegetable Curry", 450, "vegetable curry"),
        TestHelper.CreateRecipe(6, "Fish Tacos", 490, "fish tacos"),
        TestHelper.CreateRecipe(7, "Turkey Wrap", 400, "turkey wrap"),
        TestHelper.CreateRecipe(8, "Lentil Bowl", 380, "lentil bowl"),
        TestHelper.CreateRecipe(9, "Salmon Plate", 430, "salmon plate")
    };

            using (var context = factory.CreateDbContext())
            {
                context.Recipes.AddRange(recipes);
                await context.SaveChangesAsync();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
        new Claim(ClaimTypes.Name, "testuser")
    };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var authenticationStateProviderMock =
                new Mock<AuthenticationStateProvider>();

            authenticationStateProviderMock
                .Setup(x => x.GetAuthenticationStateAsync())
                .ReturnsAsync(new AuthenticationState(claimsPrincipal));

            var user = new User
            {
                Id = 1,
                Name = "Test User",
                FavoriteRecipes = new List<Recipe>
        {
            favorite1,
            favorite2,
            favorite3
        },
                WeeklyPlan = new List<Recipe>(),
                ShoppingList = new List<Ingredient>(),
                UserPreferences = new UserPreferences
                {
                    Id = 1,
                    UserId = 1,
                    WeeklyPlanDays = 7
                }
            };

            var applicationUser = new ApplicationUser
            {
                Id = "test-user-id",
                UserName = "testuser",
                User = user
            };

            using (var context = factory.CreateDbContext())
            {
                var favoriteRecipes = await context.Recipes
                    .Where(r =>
                        r.Id == favorite1.Id ||
                        r.Id == favorite2.Id ||
                        r.Id == favorite3.Id)
                    .ToListAsync();

                applicationUser.User.FavoriteRecipes = favoriteRecipes;

                context.Users.Add(applicationUser);

                await context.SaveChangesAsync();
            }

            var userManagerMock =
                TestHelper.CreateUserManagerMock(applicationUser);

            var navigationManagerMock = new Mock<NavigationManager>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var classificationService = new RecipeClassificationService();

            var dataService = new DataService(
                navigationManagerMock.Object,
                httpClientFactoryMock.Object,
                factory,
                userManagerMock.Object,
                authenticationStateProviderMock.Object,
                classificationService);

            var weeklyPlanService = TestHelper.CreateWeeklyPlanService(dataService, navigationManagerMock.Object, factory);

            // Act
            var result = await weeklyPlanService.GenerateWeeklyPlanAsync(
                maxCalories: null,
                maxPrepTime: null,
                preferredFavoriteRecipes: 2);

            // Assert
            Assert.Equal(7, result.Count);

            var favoriteIds = new[]
            {
        favorite1.Id,
        favorite2.Id,
        favorite3.Id
    };

            var favoritesInPlan = result
                .Count(recipe => favoriteIds.Contains(recipe.Id));

            Assert.True(favoritesInPlan >= 2);
        }

        [Fact]
        public async Task GenerateWeeklyPlanAsync_ClampsPreferredFavoritesToWeeklyPlanDays()
        {
            // Arrange
            var factory = TestHelper.CreateDbContextFactory();

            var recipes = new List<Recipe>
    {
        TestHelper.CreateRecipe(1, "Chicken Pasta", 350, "chicken pasta"),
        TestHelper.CreateRecipe(2, "Beef Rice", 420, "beef rice"),
        TestHelper.CreateRecipe(3, "Greek Salad", 280, "greek salad"),
        TestHelper.CreateRecipe(4, "Chicken Soup", 310, "chicken soup"),
        TestHelper.CreateRecipe(5, "Vegetable Curry", 450, "vegetable curry"),
        TestHelper.CreateRecipe(6, "Fish Tacos", 490, "fish tacos"),
        TestHelper.CreateRecipe(7, "Turkey Wrap", 400, "turkey wrap")
    };

            using (var context = factory.CreateDbContext())
            {
                context.Recipes.AddRange(recipes);
                await context.SaveChangesAsync();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
        new Claim(ClaimTypes.Name, "testuser")
    };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var authenticationStateProviderMock =
                new Mock<AuthenticationStateProvider>();

            authenticationStateProviderMock
                .Setup(x => x.GetAuthenticationStateAsync())
                .ReturnsAsync(new AuthenticationState(claimsPrincipal));

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

            var applicationUser = new ApplicationUser
            {
                Id = "test-user-id",
                UserName = "testuser",
                User = user
            };

            using (var context = factory.CreateDbContext())
            {
                var favoriteRecipes = await context.Recipes.ToListAsync();

                applicationUser.User.FavoriteRecipes = favoriteRecipes;

                context.Users.Add(applicationUser);

                await context.SaveChangesAsync();
            }

            var userManagerMock =
                TestHelper.CreateUserManagerMock(applicationUser);

            var navigationManagerMock =
                new Mock<NavigationManager>();

            var httpClientFactoryMock =
                new Mock<IHttpClientFactory>();

            var classificationService =
                new RecipeClassificationService();

            var dataService = new DataService(
                navigationManagerMock.Object,
                httpClientFactoryMock.Object,
                factory,
                userManagerMock.Object,
                authenticationStateProviderMock.Object,
                classificationService);

            var weeklyPlanService = TestHelper.CreateWeeklyPlanService(
     dataService,
     navigationManagerMock.Object,
     factory);

            // Act
            var result = await weeklyPlanService.GenerateWeeklyPlanAsync(
                maxCalories: null,
                maxPrepTime: null,
                preferredFavoriteRecipes: 10);

            // Assert
            Assert.Equal(7, result.Count);

            var favoriteIds = recipes
                .Select(r => r.Id)
                .ToHashSet();

            var favoritesInPlan = result
                .Count(r => favoriteIds.Contains(r.Id));

            Assert.Equal(7, favoritesInPlan);
        }

        [Fact]
        public async Task GenerateWeeklyPlanAsync_TreatsNegativePreferredFavoritesAsZero()
        {
            // Arrange
            var factory = TestHelper.CreateDbContextFactory();

            var favorite1 = TestHelper.CreateRecipe(
                1,
                "Chicken Pasta",
                350,
                "chicken pasta");

            var favorite2 = TestHelper.CreateRecipe(
                2,
                "Beef Rice",
                420,
                "beef rice");

            var favorite3 = TestHelper.CreateRecipe(
                3,
                "Greek Salad",
                280,
                "greek salad");

            var recipes = new List<Recipe>
    {
        favorite1,
        favorite2,
        favorite3,
        TestHelper.CreateRecipe(4, "Chicken Soup", 310, "chicken soup"),
        TestHelper.CreateRecipe(5, "Vegetable Curry", 450, "vegetable curry"),
        TestHelper.CreateRecipe(6, "Fish Tacos", 490, "fish tacos"),
        TestHelper.CreateRecipe(7, "Turkey Wrap", 400, "turkey wrap"),
        TestHelper.CreateRecipe(8, "Lentil Bowl", 380, "lentil bowl"),
        TestHelper.CreateRecipe(9, "Salmon Plate", 430, "salmon plate")
    };

            using (var context = factory.CreateDbContext())
            {
                context.Recipes.AddRange(recipes);
                await context.SaveChangesAsync();
            }

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

            var authenticationStateProviderMock =
                new Mock<AuthenticationStateProvider>();

            authenticationStateProviderMock
                .Setup(x => x.GetAuthenticationStateAsync())
                .ReturnsAsync(
                    new AuthenticationState(claimsPrincipal));

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

            var applicationUser = new ApplicationUser
            {
                Id = "test-user-id",
                UserName = "testuser",
                User = user
            };

            using (var context = factory.CreateDbContext())
            {
                var favoriteRecipes = await context.Recipes
                    .Where(r =>
                        r.Id == favorite1.Id ||
                        r.Id == favorite2.Id ||
                        r.Id == favorite3.Id)
                    .ToListAsync();

                applicationUser.User.FavoriteRecipes = favoriteRecipes;

                context.Users.Add(applicationUser);

                await context.SaveChangesAsync();
            }

            var userManagerMock =
                TestHelper.CreateUserManagerMock(applicationUser);

            var navigationManagerMock =
                new Mock<NavigationManager>();

            var httpClientFactoryMock =
                new Mock<IHttpClientFactory>();

            var classificationService =
                new RecipeClassificationService();

            var dataService = new DataService(
                navigationManagerMock.Object,
                httpClientFactoryMock.Object,
                factory,
                userManagerMock.Object,
                authenticationStateProviderMock.Object,
                classificationService);

            var weeklyPlanService = TestHelper.CreateWeeklyPlanService(dataService, navigationManagerMock.Object, factory);


            // Act
            var result =
                await weeklyPlanService.GenerateWeeklyPlanAsync(
                    maxCalories: null,
                    maxPrepTime: null,
                    preferredFavoriteRecipes: -5);

            // Assert
            Assert.Equal(7, result.Count);

            Assert.Equal(
                result.Count,
                result.Select(r => r.RecipeRoot)
                      .Distinct()
                      .Count());
        }

        [Fact]
        public async Task GetWeeklyPlanShoppingListAsync_ReturnsUniqueIngredients()
        {
            // Arrange
            var factory = TestHelper.CreateDbContextFactory();

            var recipe1 = TestHelper.CreateRecipe(
                1,
                "Chicken Pasta",
                350,
                "chicken pasta");

            recipe1.ListOfIngredients = new List<Ingredient>
    {
        new Ingredient { IngredientsName = "Chicken" },
        new Ingredient { IngredientsName = "Tomato" },
        new Ingredient { IngredientsName = "Salt" }
    };

            var recipe2 = TestHelper.CreateRecipe(
                2,
                "Beef Rice",
                420,
                "beef rice");

            recipe2.ListOfIngredients = new List<Ingredient>
    {
        new Ingredient { IngredientsName = "  chicken  " },
        new Ingredient { IngredientsName = "Rice" },
        new Ingredient { IngredientsName = "SALT" }
    };

            using (var context = factory.CreateDbContext())
            {
                context.Recipes.AddRange(recipe1, recipe2);
                await context.SaveChangesAsync();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
        new Claim(ClaimTypes.Name, "testuser")
    };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var authenticationStateProviderMock =
                new Mock<AuthenticationStateProvider>();

            authenticationStateProviderMock
                .Setup(x => x.GetAuthenticationStateAsync())
                .ReturnsAsync(new AuthenticationState(claimsPrincipal));

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

            var applicationUser = new ApplicationUser
            {
                Id = "test-user-id",
                UserName = "testuser",
                User = user
            };

            using (var context = factory.CreateDbContext())
            {
                var weeklyPlanRecipes = await context.Recipes
                    .Where(r => r.Id == recipe1.Id || r.Id == recipe2.Id)
                    .ToListAsync();

                applicationUser.User.WeeklyPlan = weeklyPlanRecipes;

                context.Users.Add(applicationUser);

                await context.SaveChangesAsync();
            }

            var userManagerMock =
                TestHelper.CreateUserManagerMock(applicationUser);

            var navigationManagerMock =
                new Mock<NavigationManager>();

            var httpClientFactoryMock =
                new Mock<IHttpClientFactory>();

            var classificationService =
                new RecipeClassificationService();

            var dataService = new DataService(
                navigationManagerMock.Object,
                httpClientFactoryMock.Object,
                factory,
                userManagerMock.Object,
                authenticationStateProviderMock.Object,
                classificationService);

            var weeklyPlanService = TestHelper.CreateWeeklyPlanService(dataService, navigationManagerMock.Object, factory);

            // Act
            var result =
                await weeklyPlanService.GetWeeklyPlanShoppingListAsync();

            // Assert
            Assert.Equal(4, result.Count);

            var normalizedIngredients = result
                .Select(i => i.IngredientsName.Trim().ToLowerInvariant())
                .ToList();

            Assert.Contains("chicken", normalizedIngredients);
            Assert.Contains("tomato", normalizedIngredients);
            Assert.Contains("salt", normalizedIngredients);
            Assert.Contains("rice", normalizedIngredients);

            Assert.Equal(
                normalizedIngredients.Count,
                normalizedIngredients.Distinct().Count());
        }
    }
}