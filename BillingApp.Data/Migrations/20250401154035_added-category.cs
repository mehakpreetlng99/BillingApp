using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BillingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e9e2729-1bc3-4ea0-87bb-2b4638e3939c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e74b687-c732-42ff-af00-a371af2337da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfbcdbf7-e911-4ada-b92b-cd89851d4381");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e9e2729-1bc3-4ea0-87bb-2b4638e3939c", null, "SuperAdmin", "SUPERADMIN" },
                    { "9e74b687-c732-42ff-af00-a371af2337da", null, "Agent", "AGENT" },
                    { "bfbcdbf7-e911-4ada-b92b-cd89851d4381", null, "Admin", "ADMIN" }
                });
        }
    }
}
