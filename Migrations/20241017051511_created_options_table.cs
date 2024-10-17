using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEV.PagoLinea.Migrations
{
    /// <inheritdoc />
    public partial class created_options_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Options",
                columns: new[] { "id", "key", "value" },
                values: new object[] { 1, "ON-MAINTENANCE", "0" });

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$e2FQ82rluQeQ6wc1XmtW7OYu9sdl1Q/S/dlGMzJf5mWM0pngcSam6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$T/UeZUBStqrfHIHV6SHNZOnVwOdA0r7tLcWK3HKcWzatoX5QEoPSS");
        }
    }
}
