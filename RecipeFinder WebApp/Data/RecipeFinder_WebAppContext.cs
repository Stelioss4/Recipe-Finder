using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data;

public class RecipeFinder_WebAppContext : IdentityDbContext<RecipeFinder_WebAppUser>
{
    public RecipeFinder_WebAppContext(DbContextOptions<RecipeFinder_WebAppContext> options)
        : base(options)
    {
    }

    public DbSet<Recipe> Recipes { get; set; }

    public DbSet<Address> Address { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RecipeFinder_WebAppUser>()
               .HasMany(u => u.FavoriteRecipes)
              .WithMany()
               .UsingEntity(j => j.ToTable("UserFavoriteRecipes"));

        builder.Entity<Recipe>()
               .HasKey(r => r.RecipeName);
        builder.Entity<User>()
              .HasKey(u => u.FirstName);
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


