using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEV.PagoLinea.Migrations
{
    /// <inheritdoc />
    public partial class created_oficinas_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cat_Oficinas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    oficina = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    servidor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    baseDatos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    actualizable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cat_Oficinas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cat_Localidades",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    localidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    oficinaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cat_Localidades", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cat_Localidades_Cat_Oficinas_oficinaId",
                        column: x => x.oficinaId,
                        principalTable: "Cat_Oficinas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cat_Localidades_oficinaId",
                table: "Cat_Localidades",
                column: "oficinaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cat_Localidades");

            migrationBuilder.DropTable(
                name: "Cat_Oficinas");
        }
    }
}
