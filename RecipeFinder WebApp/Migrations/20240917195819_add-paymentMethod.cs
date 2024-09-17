using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeFinderWebApp.Migrations
{
    /// <inheritdoc />
    public partial class addpaymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodsId",
                table: "User",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AcountEmail = table.Column<string>(type: "TEXT", nullable: false),
                    Acountpasword = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_PaymentMethodsId",
                table: "User",
                column: "PaymentMethodsId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_PaymentMethod_PaymentMethodsId",
                table: "User",
                column: "PaymentMethodsId",
                principalTable: "PaymentMethod",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_PaymentMethod_PaymentMethodsId",
                table: "User");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_User_PaymentMethodsId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PaymentMethodsId",
                table: "User");
        }
    }
}
