using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Only store User entity without referencing ApplicationUser


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                   .HasOne(a => a.User)
                   .WithOne()
                   .HasForeignKey<User>(u => u.UserId);

            // Configure the relationship between User and Recipe if needed
            builder.Entity<Recipe>()
                   .HasOne(r => r.User)
                   .WithMany(u => u.FavoriteRecipes)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
