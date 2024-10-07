using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEV.PagoLinea.Migrations
{
    /// <inheritdoc />
    public partial class created_order_payment_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedAt",
                table: "SystemUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getDate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "getDate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "SystemUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getDate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "getDate()");

            migrationBuilder.CreateTable(
                name: "Opr_OrdenPago",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idLocalidad = table.Column<int>(type: "int", nullable: false),
                    idPadron = table.Column<int>(type: "int", nullable: false),
                    ammount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    concept = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    node = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    responseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    authorization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    responseAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opr_OrdenPago", x => x.code);
                });

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$XkM1dmV7Nt9GauA0u615f.g/IrlZPpFQRGPFPcwMJpthew0g9XwtS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opr_OrdenPago");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedAt",
                table: "SystemUsers",
                type: "datetime",
                nullable: false,
                defaultValueSql: "getDate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getDate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "SystemUsers",
                type: "datetime",
                nullable: false,
                defaultValueSql: "getDate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getDate()");

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$AfcrCArGfAU.P/haHseq6.3yo40Nrw72/Kmsyn8rsyA0p5ndU0N4y");
        }
    }
}
