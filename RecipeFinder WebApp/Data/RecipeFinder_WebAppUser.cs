using Microsoft.AspNetCore.Identity;
using Recipe_Finder;


namespace RecipeFinder_WebApp.Data
{
    public class RecipeFinder_WebAppUser : IdentityUser
    {
        User User { get; set; } = new User();
        public ICollection<Recipe> FavoriteRecipes { get; set; } = new List<Recipe>();

    }
}
