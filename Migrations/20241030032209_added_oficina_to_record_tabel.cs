using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEV.PagoLinea.Migrations
{
    /// <inheritdoc />
    public partial class added_oficina_to_record_tabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "oficinaId",
                table: "CuentasPadron",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$4rb8atploisM1L7qOriNiel4iEUurXyLPkShTY.UD1KzW5T9EQw0C");

            migrationBuilder.CreateIndex(
                name: "IX_CuentasPadron_oficinaId",
                table: "CuentasPadron",
                column: "oficinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CuentasPadron_Cat_Oficinas_oficinaId",
                table: "CuentasPadron",
                column: "oficinaId",
                principalTable: "Cat_Oficinas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuentasPadron_Cat_Oficinas_oficinaId",
                table: "CuentasPadron");

            migrationBuilder.DropIndex(
                name: "IX_CuentasPadron_oficinaId",
                table: "CuentasPadron");

            migrationBuilder.DropColumn(
                name: "oficinaId",
                table: "CuentasPadron");

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$e2FQ82rluQeQ6wc1XmtW7OYu9sdl1Q/S/dlGMzJf5mWM0pngcSam6");
        }
    }
}
