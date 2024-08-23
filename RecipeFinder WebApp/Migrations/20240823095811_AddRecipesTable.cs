using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeFinder_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                    StreetsName = table.Column<string>(type: "TEXT", nullable: false),
                    Housenumber = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.PostalCode);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    ConfirmPassword = table.Column<string>(type: "TEXT", nullable: false),
                    RememberMe = table.Column<bool>(type: "INTEGER", nullable: false),
                    AddressPostalCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Addresses_AddressPostalCode",
                        column: x => x.AddressPostalCode,
                        principalTable: "Addresses",
                        principalColumn: "PostalCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<string>(type: "TEXT", nullable: false),
                    SourceDomain = table.Column<string>(type: "TEXT", nullable: false),
                    SearchTerms = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RecipeName = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    CookingInstructions = table.Column<string>(type: "TEXT", nullable: false),
                    VideoUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<string>(type: "TEXT", nullable: false),
                    CookingTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    CuisineType = table.Column<int>(type: "INTEGER", nullable: false),
                    OccasionTags = table.Column<int>(type: "INTEGER", nullable: false),
                    DifficultyLevel = table.Column<string>(type: "TEXT", nullable: false),
                    LinksForDrinkPairing = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<double>(type: "REAL", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    RecipeFinder_WebAppUserId = table.Column<string>(type: "TEXT", nullable: true),
                    UserId1 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_Recipes_AspNetUsers_RecipeFinder_WebAppUserId",
                        column: x => x.RecipeFinder_WebAppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Recipes_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recipes_User_UserId1",
                        column: x => x.UserId1,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientsName = table.Column<string>(type: "TEXT", nullable: false),
                    Calories = table.Column<decimal>(type: "TEXT", nullable: false),
                    Fat = table.Column<decimal>(type: "TEXT", nullable: false),
                    Carbohydrate = table.Column<decimal>(type: "TEXT", nullable: false),
                    Protein = table.Column<decimal>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    Unit = table.Column<double>(type: "REAL", nullable: false),
                    RecipeId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientsName);
                    table.ForeignKey(
                        name: "FK_Ingredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId");
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    TimeStam = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProfileUserId = table.Column<string>(type: "TEXT", nullable: true),
                    RecipeId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Value);
                    table.ForeignKey(
                        name: "FK_Ratings_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId");
                    table.ForeignKey(
                        name: "FK_Ratings_User_ProfileUserId",
                        column: x => x.ProfileUserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewText = table.Column<string>(type: "TEXT", nullable: false),
                    TimeStam = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProfileUserId = table.Column<string>(type: "TEXT", nullable: true),
                    RecipeId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewText);
                    table.ForeignKey(
                        name: "FK_Reviews_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId");
                    table.ForeignKey(
                        name: "FK_Reviews_User_ProfileUserId",
                        column: x => x.ProfileUserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ProfileUserId",
                table: "Ratings",
                column: "ProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RecipeId",
                table: "Ratings",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_RecipeFinder_WebAppUserId",
                table: "Recipes",
                column: "RecipeFinder_WebAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UserId",
                table: "Recipes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UserId1",
                table: "Recipes",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProfileUserId",
                table: "Reviews",
                column: "ProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RecipeId",
                table: "Reviews",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AddressPostalCode",
                table: "User",
                column: "AddressPostalCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
