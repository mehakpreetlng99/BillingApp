using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BillingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class updated_dbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "378509ff-9144-413f-b1cc-6223b9163c9d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "91db2630-c30a-4c48-9a42-6c84e7cff55c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd5ac82d-97fd-4c75-b9ab-7edbf23bb66f");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "378509ff-9144-413f-b1cc-6223b9163c9d", null, "Agent", "AGENT" },
                    { "91db2630-c30a-4c48-9a42-6c84e7cff55c", null, "Admin", "ADMIN" },
                    { "bd5ac82d-97fd-4c75-b9ab-7edbf23bb66f", null, "SuperAdmin", "SUPERADMIN" }
                });
        }
    }
}
