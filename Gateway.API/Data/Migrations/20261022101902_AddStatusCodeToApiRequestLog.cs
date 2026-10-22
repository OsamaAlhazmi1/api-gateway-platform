using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.API.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusCodeToApiRequestLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "ApiRequestLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "ApiRequestLogs");
        }
    }
}
