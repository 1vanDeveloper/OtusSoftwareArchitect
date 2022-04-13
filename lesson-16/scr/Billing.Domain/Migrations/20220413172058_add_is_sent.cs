using Microsoft.EntityFrameworkCore.Migrations;

namespace Billing.Domain.Migrations
{
    public partial class add_is_sent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "NotificationEvents",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "NotificationEvents");
        }
    }
}
