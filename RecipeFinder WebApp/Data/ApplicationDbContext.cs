using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Recipe> Recipes { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(u => u.FavoriteRecipes)
                .WithMany(r => r.FavoriteByUsers);

            builder.Entity<User>()
                .HasMany(u => u.WeeklyPlan)
                .WithMany(r => r.WeeklyPlanUsers);
        }
    }
}