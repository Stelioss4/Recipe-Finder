using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeFinder_WebApp.Data;

namespace RecipeFinder_WebApp.Data
{
    public class RecipeFinder_WebAppContext(DbContextOptions<RecipeFinder_WebAppContext> options) : IdentityDbContext<RecipeFinder_WebAppUser>(options)
    {
    }
}
