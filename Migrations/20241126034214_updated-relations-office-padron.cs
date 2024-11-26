using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEV.PagoLinea.Migrations
{
    /// <inheritdoc />
    public partial class updatedrelationsofficepadron : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$GcCJXKM6EN9E8p3orpRhseyWK1LoxyliwsM6wn3B8MQWYJrPoWGCK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$HmyWkfvdhNagMxgq0Y9e0.v8qBrGiND/4gBqQ1RGqO18aMJhDSbP.");
        }
    }
}
