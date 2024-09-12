using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for your entities
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> User { get; set; }  // Separate from ApplicationUser, for application logic

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationship between ApplicationUser and User
            builder.Entity<ApplicationUser>()
                .HasOne(a => a.Profile)     // ApplicationUser has one User profile
                .WithOne()                  // One-to-one relationship
                .HasForeignKey<ApplicationUser>(a => a.UserId); // Foreign key in ApplicationUser

            // Set UserId as the primary key for User entity
            builder.Entity<User>()
                .HasKey(u => u.UserId);     // Primary key

            // Configure a one-to-many relationship between User and Recipe
            builder.Entity<Recipe>()
                   .HasOne(r => r.User) // A Recipe has one User
                   .WithMany(u => u.FavoriteRecipes) // A User can have many Recipes
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Configure Address primary key
            builder.Entity<Address>()
                .HasKey(a => a.PostalCode);   // PostalCode as primary key

            // Configure Ingredient primary key
            builder.Entity<Ingredient>()
                .HasKey(i => i.IngredientsName);  // IngredientsName as primary key

            // Configure Rating primary key
            builder.Entity<Rating>()
                .HasKey(r => r.Value);    // Value as primary key

            // Configure Review primary key
            builder.Entity<Review>()
                .HasKey(r => r.ReviewText);  // ReviewText as primary key
        }
    }
}
