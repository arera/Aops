using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOps.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class orleveldb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_orgle",
                table: "orgle");

            migrationBuilder.RenameTable(
                name: "orgle",
                newName: "OrganisationLevels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganisationLevels",
                table: "OrganisationLevels",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganisationLevels",
                table: "OrganisationLevels");

            migrationBuilder.RenameTable(
                name: "OrganisationLevels",
                newName: "orgle");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orgle",
                table: "orgle",
                column: "Id");
        }
    }
}
