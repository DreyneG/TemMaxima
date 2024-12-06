using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace APITEMPERATURAMAXIMA.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Estado_ar",
                table: "Estado_ar");

            migrationBuilder.DropColumn(
                name: "DataAlteracao",
                table: "MudancaTemps");

            migrationBuilder.DropColumn(
                name: "HorarioAlteracao",
                table: "MudancaTemps");

            migrationBuilder.DropColumn(
                name: "NomeAmbiente",
                table: "MudancaTemps");

            migrationBuilder.DropColumn(
                name: "NomeUsuario",
                table: "MudancaTemps");

            migrationBuilder.RenameColumn(
                name: "id_ambiente",
                table: "Temperaturas",
                newName: "IdAmbiente");

            migrationBuilder.RenameColumn(
                name: "IdEstado",
                table: "Estado_ar",
                newName: "IdAmbiente");

            migrationBuilder.AddColumn<DateTime>(
                name: "HorarioTemp",
                table: "Temperaturas",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HorarioMudanca",
                table: "MudancaTemps",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdAmbiente",
                table: "Estado_ar",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "IdEstado_ar",
                table: "Estado_ar",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estado_ar",
                table: "Estado_ar",
                column: "IdEstado_ar");

            migrationBuilder.CreateTable(
                name: "UsuarioAmbientes",
                columns: table => new
                {
                    IdUsuarioAmbiente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAmbiente = table.Column<int>(type: "integer", nullable: false),
                    IdFuncionario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioAmbientes", x => x.IdUsuarioAmbiente);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioAmbientes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estado_ar",
                table: "Estado_ar");

            migrationBuilder.DropColumn(
                name: "HorarioTemp",
                table: "Temperaturas");

            migrationBuilder.DropColumn(
                name: "HorarioMudanca",
                table: "MudancaTemps");

            migrationBuilder.DropColumn(
                name: "IdEstado_ar",
                table: "Estado_ar");

            migrationBuilder.RenameColumn(
                name: "IdAmbiente",
                table: "Temperaturas",
                newName: "id_ambiente");

            migrationBuilder.RenameColumn(
                name: "IdAmbiente",
                table: "Estado_ar",
                newName: "IdEstado");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DataAlteracao",
                table: "MudancaTemps",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HorarioAlteracao",
                table: "MudancaTemps",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "NomeAmbiente",
                table: "MudancaTemps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeUsuario",
                table: "MudancaTemps",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdEstado",
                table: "Estado_ar",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estado_ar",
                table: "Estado_ar",
                column: "IdEstado");
        }
    }
}
