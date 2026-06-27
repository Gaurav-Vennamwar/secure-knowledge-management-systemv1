using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureKnowledgeManagementSystemv1.API.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class AddRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "10a7620f-01e4-482c-a211-a52f503476a1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6284af17-314a-40ea-9cca-13061b2ccbf5", "AQAAAAIAAYagAAAAEP9t2EITToVWFH5Z1Z7mhULoVw1L8hDxGXV310yYTXQwyhtRf+zZdOma9jTJVR7Edg==", "8242d3b0-7ebd-437a-9f03-bba1de813c4e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "10a7620f-01e4-482c-a211-a52f503476a1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "110c7253-5958-4a8e-8319-51162db223be", "AQAAAAIAAYagAAAAEOFn7YvRoILmLrMo4oJvXTB78HWYZakKXLJzxKi2D1fvaYOwVwCQOurtU8QlGBSm/w==", "72a078b8-10a7-4609-8709-b64911c6e492" });
        }
    }
}
