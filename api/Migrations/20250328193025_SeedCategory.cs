using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3bb98091-8604-4f43-a6ef-72089edca813");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0b29218-0a74-4310-a0e1-60f1cd3a9bf5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13baf865-8249-4cb4-af1f-7d009b3a2857", null, "Admin", "ADMIN" },
                    { "6bcc619a-3a59-419d-9225-dcb6fa789151", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Groceries" },
                    { 2, "Electronics" },
                    { 3, "Utilities" },
                    { 4, "Clothing" },
                    { 5, "Health" },
                    { 6, "Leisure" },
                    { 7, "Others" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13baf865-8249-4cb4-af1f-7d009b3a2857");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6bcc619a-3a59-419d-9225-dcb6fa789151");

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3bb98091-8604-4f43-a6ef-72089edca813", null, "User", "USER" },
                    { "f0b29218-0a74-4310-a0e1-60f1cd3a9bf5", null, "Admin", "ADMIN" }
                });
        }
    }
}
