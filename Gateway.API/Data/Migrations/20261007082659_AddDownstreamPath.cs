using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDownstreamPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DownstreamPath",
                table: "ApiDefinitions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ApiDefinitions",
                keyColumn: "Id",
                keyValue: 1,
                column: "DownstreamPath",
                value: "api/users");

            migrationBuilder.UpdateData(
                table: "ApiDefinitions",
                keyColumn: "Id",
                keyValue: 2,
                column: "DownstreamPath",
                value: "/api/order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownstreamPath",
                table: "ApiDefinitions");
        }
    }
}
