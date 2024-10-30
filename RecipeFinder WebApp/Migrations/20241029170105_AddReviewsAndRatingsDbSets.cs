using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeFinderWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewsAndRatingsDbSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Recipes_RecipeId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_User_ProfileId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Recipes_RecipeId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_User_ProfileId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "Ratings");

            migrationBuilder.RenameIndex(
                name: "IX_Review_RecipeId",
                table: "Reviews",
                newName: "IX_Reviews_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ProfileId",
                table: "Reviews",
                newName: "IX_Reviews_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_RecipeId",
                table: "Ratings",
                newName: "IX_Ratings_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_ProfileId",
                table: "Ratings",
                newName: "IX_Ratings_ProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Recipes_RecipeId",
                table: "Ratings",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_User_ProfileId",
                table: "Ratings",
                column: "ProfileId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Recipes_RecipeId",
                table: "Reviews",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_User_ProfileId",
                table: "Reviews",
                column: "ProfileId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Recipes_RecipeId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_User_ProfileId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Recipes_RecipeId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_User_ProfileId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "Rating");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_RecipeId",
                table: "Review",
                newName: "IX_Review_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ProfileId",
                table: "Review",
                newName: "IX_Review_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_RecipeId",
                table: "Rating",
                newName: "IX_Rating_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_ProfileId",
                table: "Rating",
                newName: "IX_Rating_ProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Recipes_RecipeId",
                table: "Rating",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_User_ProfileId",
                table: "Rating",
                column: "ProfileId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Recipes_RecipeId",
                table: "Review",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_User_ProfileId",
                table: "Review",
                column: "ProfileId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
