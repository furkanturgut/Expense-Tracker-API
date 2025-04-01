using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ExpenseModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e8b733f-9901-4778-9388-a8f801c600d9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd950a99-1f44-4ba9-bc9e-08dc236229f9");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDayTime",
                table: "expenses",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c5bf8766-c80f-411f-84d0-0d697b9e92bc", null, "Admin", "ADMIN" },
                    { "f50374f0-e72b-4e01-a629-fa77036cc9ce", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5bf8766-c80f-411f-84d0-0d697b9e92bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f50374f0-e72b-4e01-a629-fa77036cc9ce");

            migrationBuilder.DropColumn(
                name: "CreatedDayTime",
                table: "expenses");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e8b733f-9901-4778-9388-a8f801c600d9", null, "User", "USER" },
                    { "dd950a99-1f44-4ba9-bc9e-08dc236229f9", null, "Admin", "ADMIN" }
                });
        }
    }
}
