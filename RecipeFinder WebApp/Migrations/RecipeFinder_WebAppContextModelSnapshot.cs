﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecipeFinder_WebApp.Data;

#nullable disable

namespace RecipeFinder_WebApp.Migrations
{
    [DbContext(typeof(RecipeFinder_WebAppContext))]
    partial class RecipeFinder_WebAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("RecipeFinder_WebApp.Data.RecipeFinder_WebAppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Recipe_Finder.Address", b =>
                {
                    b.Property<string>("PostalCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Housenumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StreetsName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PostalCode");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Recipe_Finder.Ingredient", b =>
                {
                    b.Property<string>("IngredientsName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Calories")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Carbohydrate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Fat")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Protein")
                        .HasColumnType("TEXT");

                    b.Property<string>("RecipeId")
                        .HasColumnType("TEXT");

                    b.Property<double>("Unit")
                        .HasColumnType("REAL");

                    b.HasKey("IngredientsName");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Recipe_Finder.Rating", b =>
                {
                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.Property<string>("ProfileUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RecipeId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TimeStam")
                        .HasColumnType("TEXT");

                    b.HasKey("Value");

                    b.HasIndex("ProfileUserId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Recipe_Finder.Recipe", b =>
                {
                    b.Property<string>("RecipeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CookingInstructions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("CookingTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("CuisineType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DifficultyLevel")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LinksForDrinkPairing")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OccasionTags")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Rating")
                        .HasColumnType("REAL");

                    b.Property<string>("RecipeFinder_WebAppUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RecipeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SearchTerms")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SourceDomain")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Time")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId1")
                        .HasColumnType("TEXT");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("RecipeId");

                    b.HasIndex("RecipeFinder_WebAppUserId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Recipe_Finder.Review", b =>
                {
                    b.Property<string>("ReviewText")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfileUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RecipeId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TimeStam")
                        .HasColumnType("TEXT");

                    b.HasKey("ReviewText");

                    b.HasIndex("ProfileUserId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Recipe_Finder.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("AddressPostalCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConfirmPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("RememberMe")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId");

                    b.HasIndex("AddressPostalCode");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RecipeFinder_WebApp.Data.RecipeFinder_WebAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RecipeFinder_WebApp.Data.RecipeFinder_WebAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecipeFinder_WebApp.Data.RecipeFinder_WebAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RecipeFinder_WebApp.Data.RecipeFinder_WebAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Recipe_Finder.Ingredient", b =>
                {
                    b.HasOne("Recipe_Finder.Recipe", null)
                        .WithMany("ListofIngredients")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("Recipe_Finder.Rating", b =>
                {
                    b.HasOne("Recipe_Finder.User", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileUserId");

                    b.HasOne("Recipe_Finder.Recipe", null)
                        .WithMany("Ratings")
                        .HasForeignKey("RecipeId");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Recipe_Finder.Recipe", b =>
                {
                    b.HasOne("RecipeFinder_WebApp.Data.RecipeFinder_WebAppUser", null)
                        .WithMany("FavoriteRecipes")
                        .HasForeignKey("RecipeFinder_WebAppUserId");

                    b.HasOne("Recipe_Finder.User", "User")
                        .WithMany("FavoriteRecipes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Recipe_Finder.User", null)
                        .WithMany("WeeklyPlan")
                        .HasForeignKey("UserId1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Recipe_Finder.Review", b =>
                {
                    b.HasOne("Recipe_Finder.User", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileUserId");

                    b.HasOne("Recipe_Finder.Recipe", null)
                        .WithMany("Reviews")
                        .HasForeignKey("RecipeId");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Recipe_Finder.User", b =>
                {
                    b.HasOne("Recipe_Finder.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressPostalCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("RecipeFinder_WebApp.Data.RecipeFinder_WebAppUser", b =>
                {
                    b.Navigation("FavoriteRecipes");
                });

            modelBuilder.Entity("Recipe_Finder.Recipe", b =>
                {
                    b.Navigation("ListofIngredients");

                    b.Navigation("Ratings");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Recipe_Finder.User", b =>
                {
                    b.Navigation("FavoriteRecipes");

                    b.Navigation("WeeklyPlan");
                });
#pragma warning restore 612, 618
        }
    }
}
