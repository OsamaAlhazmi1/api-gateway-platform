using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.API.Migrations
{
    /// <inheritdoc />
    public partial class HashApiClientKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApiKey",
                table: "ApiClients",
                newName: "ApiKeyHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApiKeyHash",
                table: "ApiClients",
                newName: "ApiKey");
        }
    }
}
