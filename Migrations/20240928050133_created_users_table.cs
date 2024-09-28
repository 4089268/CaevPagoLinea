using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEV.PagoLinea.Migrations
{
    /// <inheritdoc />
    public partial class created_users_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemUsers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemUsers", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "SystemUsers",
                columns: new[] { "id", "email", "name", "password" },
                values: new object[] { 1, "admin@email.com", "Administrador", "$2a$11$iHbU57ouP/Mx0tS1hvfNfezOONyNnT.n89nFWHfbEnireMfdtoUhO" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemUsers");
        }
    }
}
