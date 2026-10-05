using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gateway.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedApiDefinitions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApiDefinitions",
                columns: new[] { "Id", "DestinationAddress", "Name", "RoutePrefix" },
                values: new object[,]
                {
                    { 1, "http://localhost:5024/", "Users API", "/users" },
                    { 2, "http://localhost:5297/", "Orders API", "/orders" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApiDefinitions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ApiDefinitions",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
