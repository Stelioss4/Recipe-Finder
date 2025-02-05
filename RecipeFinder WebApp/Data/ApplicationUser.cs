using Microsoft.AspNetCore.Identity;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual User User { get; set; } = new();

    }

}
