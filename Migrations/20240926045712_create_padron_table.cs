using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEV.PagoLinea.Migrations
{
    /// <inheritdoc />
    public partial class create_padron_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CuentasPadron",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idLocalidad = table.Column<int>(type: "int", nullable: false),
                    localidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idPadron = table.Column<int>(type: "int", nullable: false),
                    idCuenta = table.Column<int>(type: "int", nullable: false),
                    razonSocial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    localizacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    iVA = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    total = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    periodoFactura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentasPadron", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuentasPadron");
        }
    }
}
