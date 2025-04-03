using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedinvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Invoices");
        }
    }
}
