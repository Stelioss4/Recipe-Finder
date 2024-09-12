using Microsoft.AspNetCore.Identity;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string UserId { get; set; } // Foreign key to User entity
        public ApplicationUser()
        {
            UserId = Guid.NewGuid().ToString();
        }
        public User Profile { get; set; }
        public List<Recipe> FavoriteRecipes { get; set; }
    }
}


