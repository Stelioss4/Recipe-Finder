using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeFinderWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddWeeklyPlanShoppingListRelationToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientUser_User_UserIdId",
                table: "IngredientUser");

            migrationBuilder.RenameColumn(
                name: "UserIdId",
                table: "IngredientUser",
                newName: "ShoppingListUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientUser_UserIdId",
                table: "IngredientUser",
                newName: "IX_IngredientUser_ShoppingListUsersId");

            migrationBuilder.CreateTable(
                name: "IngredientUser1",
                columns: table => new
                {
                    WeeklyPlanShoppingListId = table.Column<int>(type: "int", nullable: false),
                    WeeklyPlanShoppingListUsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientUser1", x => new { x.WeeklyPlanShoppingListId, x.WeeklyPlanShoppingListUsersId });
                    table.ForeignKey(
                        name: "FK_IngredientUser1_Ingredients_WeeklyPlanShoppingListId",
                        column: x => x.WeeklyPlanShoppingListId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientUser1_User_WeeklyPlanShoppingListUsersId",
                        column: x => x.WeeklyPlanShoppingListUsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientUser1_WeeklyPlanShoppingListUsersId",
                table: "IngredientUser1",
                column: "WeeklyPlanShoppingListUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientUser_User_ShoppingListUsersId",
                table: "IngredientUser",
                column: "ShoppingListUsersId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientUser_User_ShoppingListUsersId",
                table: "IngredientUser");

            migrationBuilder.DropTable(
                name: "IngredientUser1");

            migrationBuilder.RenameColumn(
                name: "ShoppingListUsersId",
                table: "IngredientUser",
                newName: "UserIdId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientUser_ShoppingListUsersId",
                table: "IngredientUser",
                newName: "IX_IngredientUser_UserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientUser_User_UserIdId",
                table: "IngredientUser",
                column: "UserIdId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
