using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEV.PagoLinea.Migrations
{
    /// <inheritdoc />
    public partial class added_af_mf_to_padron : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "af",
                table: "CuentasPadron",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mf",
                table: "CuentasPadron",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$T/UeZUBStqrfHIHV6SHNZOnVwOdA0r7tLcWK3HKcWzatoX5QEoPSS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "af",
                table: "CuentasPadron");

            migrationBuilder.DropColumn(
                name: "mf",
                table: "CuentasPadron");

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$w9RlSrL0i.NcQ4ni7ftZV.atNbFOGUCcE1QrmftT5Y0jomWzVmlvG");
        }
    }
}
