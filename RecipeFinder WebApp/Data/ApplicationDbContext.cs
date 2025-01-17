using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using System.Reflection.Emit;

namespace RecipeFinder_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<User> User { get; set; }

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

            builder.Entity<User>()
                .HasMany(u => u.ShoppingList)
                .WithMany(i => i.UserId);

            // AutoInclude the User navigation property in ApplicationUser
            builder.Entity<ApplicationUser>()
                .Navigation(e => e.User)
                .AutoInclude();

            // Configure User entity to include FavoriteRecipes and WeeklyPlan
            builder.Entity<User>()
                .Navigation(u => u.FavoriteRecipes)
                .AutoInclude();

            builder.Entity<User>()
                .Navigation(u => u.WeeklyPlan)
                .AutoInclude();
        }
    }
}