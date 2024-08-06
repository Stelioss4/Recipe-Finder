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

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);

    //    builder.Entity<RecipeFinder_WebAppUser>()
    //        .HasMany(u => u.FavoriteRecipes)
    //        .WithMany(r => r.UsersWhoFavorited)
    //        .UsingEntity(j => j.ToTable("UserFavoriteRecipes"));
    //}
}
