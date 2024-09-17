using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeFinderWebApp.Migrations
{
    /// <inheritdoc />
    public partial class addadress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressID",
                table: "User",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StreetsName = table.Column<string>(type: "TEXT", nullable: false),
                    Housenumber = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_AddressID",
                table: "User",
                column: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Address_AddressID",
                table: "User",
                column: "AddressID",
                principalTable: "Address",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Address_AddressID",
                table: "User");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropIndex(
                name: "IX_User_AddressID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "User");
        }
    }
}
