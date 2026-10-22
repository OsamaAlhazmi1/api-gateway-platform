using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.API.Migrations
{
    /// <inheritdoc />
    public partial class AddApiReqestLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ApiRequestLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ApiRequestLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
