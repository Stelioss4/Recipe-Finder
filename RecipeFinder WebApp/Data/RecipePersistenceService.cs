using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeFinder_WebApp.Data
{
    public class RecipePersistenceService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public RecipePersistenceService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task SaveScrapedRecipesAsync(List<Recipe> detailedRecipes)
        {
            if (detailedRecipes == null || detailedRecipes.Count == 0)
                return;

            using var context = _contextFactory.CreateDbContext();
            context.Recipes.AddRange(detailedRecipes);
            await context.SaveChangesAsync();
        }
    }
}
