using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using System.Reflection.Emit;

namespace RecipeFinder_WebApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {


        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure a one-to-many relationship between User and Recipe
            builder.Entity<Recipe>()
                   .HasOne(r => r.User) // One User can have many Recipes
                   .WithMany(u => u.FavoriteRecipes) // One Recipe belongs to one User
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                     .HasKey(u => u.UserId);

            builder.Entity<Address>()
                   .HasKey(a => a.PostalCode);

            builder.Entity<Ingredient>()
                   .HasKey(i => i.IngredientsName);

            builder.Entity<Rating>()
                   .HasKey(r => r.Value);

            builder.Entity<Review>()
                   .HasKey(r => r.ReviewText);
        }
    }
}
