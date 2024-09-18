using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeFinderWebApp.Migrations
{
    /// <inheritdoc />
    public partial class addweeklyplan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Recipe",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_UserId1",
                table: "Recipe",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_User_UserId1",
                table: "Recipe",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_User_UserId1",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_UserId1",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Recipe");
        }
    }
}
