using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RecipeFinder_WebApp.Data
{
    public class RecipeFinder_WebAppContext(DbContextOptions<RecipeFinder_WebAppContext> options) : IdentityDbContext<IdentityUser>(options)
    {

    }
}
