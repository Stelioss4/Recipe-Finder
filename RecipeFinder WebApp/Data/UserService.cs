using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using RecipeFinder_WebApp.Data;

public class UserService
{
    private readonly UserManager<RecipeFinder_WebAppUser> _userManager;
    private readonly RecipeFinder_WebAppContext _context;

    public UserService(UserManager<RecipeFinder_WebAppUser> userManager, RecipeFinder_WebAppContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task AddFavoriteRecipeAsync(string userId, string recipeUrl)
    {
        var user = await _userManager.Users.Include(u => u.FavoriteRecipes).FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Url == recipeUrl);
            if (recipe != null && !user.FavoriteRecipes.Contains(recipe))
            {
                user.FavoriteRecipes.Add(recipe);
                await _userManager.UpdateAsync(user);
            }
        }
    }

    public async Task<List<Recipe>> GetFavoritesAsync(string userId)
    {
        var user = await _userManager.Users
            .Include(u => u.FavoriteRecipes)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.FavoriteRecipes.ToList() ?? new List<Recipe>();
    }
}
