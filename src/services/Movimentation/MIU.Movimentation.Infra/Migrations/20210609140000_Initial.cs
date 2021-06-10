using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MIU.Movimentations.Infra.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movimentation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TributeCode = table.Column<string>(type: "VARCHAR(1000)", nullable: false),
                    CustomerName = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    Cpf = table.Column<string>(type: "VARCHAR(11)", maxLength: 11, nullable: true),
                    MovimentationDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    TributeDescription = table.Column<string>(type: "VARCHAR(1000)", nullable: true),
                    TributeAliquot = table.Column<int>(type: "INT", nullable: false),
                    MovimentationGain = table.Column<decimal>(type: "DECIMAL", nullable: false),
                    MovimentationLoss = table.Column<decimal>(type: "DECIMAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentation", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimentation");
        }
    }
}
