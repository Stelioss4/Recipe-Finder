using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using System.Reflection.Emit;

namespace RecipeFinder_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Review> Reviews { get; set; } 
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
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

            builder.Entity<Review>()
                .HasOne(r => r.Recipe)
                .WithMany(r => r.Reviews);

            builder.Entity<Rating>()
                .HasOne(r => r.Recipe)
                .WithMany(r => r.Ratings);


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

            builder.Entity<Recipe>()
                .Navigation(r => r.ListOfIngredients)
                .AutoInclude(); 

        }
    }
}