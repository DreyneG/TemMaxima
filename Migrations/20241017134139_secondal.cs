using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace APITEMPERATURAMAXIMA.Migrations
{
    /// <inheritdoc />
    public partial class secondal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorarioTemp",
                table: "Temperaturas");

            migrationBuilder.RenameColumn(
                name: "cpf",
                table: "AspNetUsers",
                newName: "Cpf");

            migrationBuilder.AlterColumn<string>(
                name: "cpf",
                table: "Funcionarios",
                type: "text",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateTable(
                name: "Estado_ar",
                columns: table => new
                {
                    IdEstado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    estado = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estado_ar", x => x.IdEstado);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estado_ar");

            migrationBuilder.RenameColumn(
                name: "Cpf",
                table: "AspNetUsers",
                newName: "cpf");

            migrationBuilder.AddColumn<DateTime>(
                name: "HorarioTemp",
                table: "Temperaturas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "cpf",
                table: "Funcionarios",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "cpf",
                table: "AspNetUsers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
