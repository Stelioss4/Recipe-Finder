using Microsoft.AspNetCore.Identity;
using Recipe_Finder;
using System.Reflection.Metadata;


namespace RecipeFinder_WebApp.Data
{
    public class RecipeFinder_WebAppUser : IdentityUser
    {
        User User { get; set; } = new User();

        public string UserName { get; set; }

        public List<Recipe> FavoriteRecipes { get; set; }

    }
}
