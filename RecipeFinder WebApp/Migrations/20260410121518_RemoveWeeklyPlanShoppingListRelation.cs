using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeFinderWebApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWeeklyPlanShoppingListRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientUser1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
