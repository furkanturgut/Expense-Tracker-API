using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_expenses_AspNetUsers_UserId",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_categories_CategoryId",
                table: "expenses");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13baf865-8249-4cb4-af1f-7d009b3a2857");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6bcc619a-3a59-419d-9225-dcb6fa789151");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "expenses",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "expenses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e8b733f-9901-4778-9388-a8f801c600d9", null, "User", "USER" },
                    { "dd950a99-1f44-4ba9-bc9e-08dc236229f9", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_AspNetUsers_UserId",
                table: "expenses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_categories_CategoryId",
                table: "expenses",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_expenses_AspNetUsers_UserId",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_categories_CategoryId",
                table: "expenses");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e8b733f-9901-4778-9388-a8f801c600d9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd950a99-1f44-4ba9-bc9e-08dc236229f9");

            migrationBuilder.UpdateData(
                table: "expenses",
                keyColumn: "UserId",
                keyValue: null,
                column: "UserId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "expenses",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "expenses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13baf865-8249-4cb4-af1f-7d009b3a2857", null, "Admin", "ADMIN" },
                    { "6bcc619a-3a59-419d-9225-dcb6fa789151", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_AspNetUsers_UserId",
                table: "expenses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_categories_CategoryId",
                table: "expenses",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
