using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedgst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GSTAmount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GSTPercentage",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "GSTAmount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "GSTPercentage",
                table: "Invoices");
        }
    }
}
