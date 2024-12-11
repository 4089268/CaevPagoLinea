using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CAEV.PagoLinea.Migrations
{
    /// <inheritdoc />
    public partial class added_column_to_catoficina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "visible",
                table: "Cat_Oficinas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Options",
                columns: new[] { "id", "key", "value" },
                values: new object[,]
                {
                    { 2, "MAINTENANCE-TEXT", "Disculpa las molestias, Estamos realizando algunas mejoras en nuestro sitio para brindarte un mejor servicio. Por favor, vuelve a intentarlo pronto o contáctanos si tienes alguna duda." },
                    { 3, "OFFICE-INACTIVE-TEXT", "La oficina está desactivada actualmente. Por favor, inténtelo más tarde." }
                });

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$TTk4M0X90dH31alige9qUek5tH.DYvHRueAlw3kvZhbmDUKCrHmH6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "visible",
                table: "Cat_Oficinas");

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$GcCJXKM6EN9E8p3orpRhseyWK1LoxyliwsM6wn3B8MQWYJrPoWGCK");
        }
    }
}
