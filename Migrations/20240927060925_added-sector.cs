using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEV.PagoLinea.Migrations
{
    /// <inheritdoc />
    public partial class addedsector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sector",
                table: "CuentasPadron",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sector",
                table: "CuentasPadron");
        }
    }
}
