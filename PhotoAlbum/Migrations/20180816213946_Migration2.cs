using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PhotoAlbum.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Authors_AuthorID",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_AuthorID",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "AuthorID",
                table: "Pictures");

            migrationBuilder.CreateTable(
                name: "PictureAuthors",
                columns: table => new
                {
                    AuthorID = table.Column<int>(nullable: false),
                    PictureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PictureAuthors", x => new { x.AuthorID, x.PictureID });
                    table.ForeignKey(
                        name: "FK_PictureAuthors_Authors_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "Authors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PictureAuthors_Pictures_PictureID",
                        column: x => x.PictureID,
                        principalTable: "Pictures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PictureAuthors_PictureID",
                table: "PictureAuthors",
                column: "PictureID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PictureAuthors");

            migrationBuilder.AddColumn<int>(
                name: "AuthorID",
                table: "Pictures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_AuthorID",
                table: "Pictures",
                column: "AuthorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Authors_AuthorID",
                table: "Pictures",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
