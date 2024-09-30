using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEV.PagoLinea.Migrations
{
    /// <inheritdoc />
    public partial class added_datetime_to_cat_oficinas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ultimaActualizacion",
                table: "Cat_Oficinas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$AfcrCArGfAU.P/haHseq6.3yo40Nrw72/Kmsyn8rsyA0p5ndU0N4y");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ultimaActualizacion",
                table: "Cat_Oficinas");

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$iHbU57ouP/Mx0tS1hvfNfezOONyNnT.n89nFWHfbEnireMfdtoUhO");
        }
    }
}
