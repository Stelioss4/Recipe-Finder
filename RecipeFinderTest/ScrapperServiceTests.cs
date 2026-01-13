//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Recipe_Finder;
//using RecipeFinder_WebApp.Data;
//using RecipeFinder_WebApp;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;


//namespace RecipeFinderTest
//{
//    public class ScrapperServiceTests
//    {
//        private sealed class TestDbContextFactory : IDbContextFactory<ApplicationDbContext>
//        {
//            private readonly DbContextOptions<ApplicationDbContext> _options;

//            public TestDbContextFactory(DbContextOptions<ApplicationDbContext> options)
//            {
//                _options = options;
//            }

//            public ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);
//        }

//        private sealed class FakeScrapperService : ScrapperService
//        {
//            private readonly List<Recipe> _searchResults;
//            private readonly Func<Recipe, Recipe?> _detailsFunc;

//            public FakeScrapperService(
//                HttpClient httpClient,
//                DataService ds,
//                IDbContextFactory<ApplicationDbContext> contextFactory,
//                IEmailSender emailSender,
//                List<Recipe> searchResults,
//                Func<Recipe, Recipe?> detailsFunc)
//                : base(httpClient, ds, contextFactory, emailSender)
//            {
//                _searchResults = searchResults;
//                _detailsFunc = detailsFunc;
//            }

//            //public Task<List<Recipe>> ScrapeSearchResultsFromChefKoch(string searchQuery)
//            //{
//            //    return Task.FromResult(_searchResults);
//            //}

//            //public Task<Recipe> ScrapeCKDetailsAndUpdateRecipe(Recipe recipe)
//            //{
//            //    var r = _detailsFunc(recipe);
//            //    return Task.FromResult(r!);
//            //}
//        }

//        [Fact]
//        public async Task ScrapeCKRecipes_WhenDbHasExisting_ReturnsExisting_AndDoesNotSave()
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                //.UseInMemoryDatabase(Guid.NewGuid().ToString())
//                .Options;

//            var factory = new TestDbContextFactory(options);

//            var existing = new List<Recipe>
//        {
//            new Recipe { Url = "u1", SourceDomain = Constants.CHEFKOCH_URL },
//            new Recipe { Url = "u2", SourceDomain = Constants.CHEFKOCH_URL }
//        };

//            var dataServiceMock = new Mock<DataService>();
//            dataServiceMock
//                .Setup(x => x.GetRecipesFromDatabaseAsync(It.IsAny<string>(), Constants.CHEFKOCH_URL))
//                .ReturnsAsync(existing);

//            var emailMock = new Mock<IEmailSender>();

//            var service = new FakeScrapperService(
//                httpClient: new HttpClient(),
//                ds: dataServiceMock.Object,
//                contextFactory: factory,
//                emailSender: emailMock.Object,
//                searchResults: new List<Recipe>(),
//                detailsFunc: r => r
//            );

//            var result = await service.ScrapeCKRecipes("chicken");

//            Assert.Equal(2, result.Count);

//            await using var verify = factory.CreateDbContext();
//            Assert.Empty(verify.Recipes);

//            emailMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
//        }
//    }
//}
