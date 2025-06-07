using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace exemplu.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CONCURSURI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime", nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    nr_max_participanti = table.Column<int>(type: "int", nullable: false),
                    restrictie_varsta = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONCURSURI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CONCURENTI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Prenume = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    DataNasterii = table.Column<DateTime>(type: "datetime", nullable: false),
                    Tara = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Varsta = table.Column<int>(type: "int", nullable: false),
                    CONCURSId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONCURENTI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CONCURENTI_CONCURSURI_CONCURSId",
                        column: x => x.CONCURSId,
                        principalTable: "CONCURSURI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CONCURENTI_CONCURSId",
                table: "CONCURENTI",
                column: "CONCURSId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CONCURENTI");

            migrationBuilder.DropTable(
                name: "CONCURSURI");
        }
    }
}
