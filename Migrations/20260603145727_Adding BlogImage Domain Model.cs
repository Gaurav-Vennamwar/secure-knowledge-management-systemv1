using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureKnowledgeManagementSystemv1.API.Migrations
{
    /// <inheritdoc />
    public partial class AddingBlogImageDomainModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostCategory_BlogPosts_BlogPostsid",
                table: "BlogPostCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostCategory_Categories_Categoriesid",
                table: "BlogPostCategory");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "BlogPosts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Categoriesid",
                table: "BlogPostCategory",
                newName: "CategoriesId");

            migrationBuilder.RenameColumn(
                name: "BlogPostsid",
                table: "BlogPostCategory",
                newName: "BlogPostsId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogPostCategory_Categoriesid",
                table: "BlogPostCategory",
                newName: "IX_BlogPostCategory_CategoriesId");

            migrationBuilder.CreateTable(
                name: "BlogImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tittle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogImages", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostCategory_BlogPosts_BlogPostsId",
                table: "BlogPostCategory",
                column: "BlogPostsId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostCategory_Categories_CategoriesId",
                table: "BlogPostCategory",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostCategory_BlogPosts_BlogPostsId",
                table: "BlogPostCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostCategory_Categories_CategoriesId",
                table: "BlogPostCategory");

            migrationBuilder.DropTable(
                name: "BlogImages");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BlogPosts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "BlogPostCategory",
                newName: "Categoriesid");

            migrationBuilder.RenameColumn(
                name: "BlogPostsId",
                table: "BlogPostCategory",
                newName: "BlogPostsid");

            migrationBuilder.RenameIndex(
                name: "IX_BlogPostCategory_CategoriesId",
                table: "BlogPostCategory",
                newName: "IX_BlogPostCategory_Categoriesid");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostCategory_BlogPosts_BlogPostsid",
                table: "BlogPostCategory",
                column: "BlogPostsid",
                principalTable: "BlogPosts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostCategory_Categories_Categoriesid",
                table: "BlogPostCategory",
                column: "Categoriesid",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
