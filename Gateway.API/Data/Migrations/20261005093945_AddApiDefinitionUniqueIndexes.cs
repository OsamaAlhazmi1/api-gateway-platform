using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.API.Migrations
{
    /// <inheritdoc />
    public partial class AddApiDefinitionUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ApiDefinitions_Name",
                table: "ApiDefinitions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiDefinitions_RoutePrefix",
                table: "ApiDefinitions",
                column: "RoutePrefix",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApiDefinitions_Name",
                table: "ApiDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_ApiDefinitions_RoutePrefix",
                table: "ApiDefinitions");
        }
    }
}
