using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PhotoAlbum.Migrations
{
    public partial class PictureAlbum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PeopleReferenceIDs",
                table: "Pictures",
                newName: "People");

            migrationBuilder.AddColumn<int>(
                name: "AlbumID",
                table: "Pictures",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PictureAlbums",
                columns: table => new
                {
                    AlbumID = table.Column<int>(nullable: false),
                    PictureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PictureAlbums", x => new { x.AlbumID, x.PictureID });
                    table.ForeignKey(
                        name: "FK_PictureAlbums_Albums_AlbumID",
                        column: x => x.AlbumID,
                        principalTable: "Albums",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PictureAlbums_Pictures_PictureID",
                        column: x => x.PictureID,
                        principalTable: "Pictures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_AlbumID",
                table: "Pictures",
                column: "AlbumID");

            migrationBuilder.CreateIndex(
                name: "IX_PictureAlbums_PictureID",
                table: "PictureAlbums",
                column: "PictureID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Albums_AlbumID",
                table: "Pictures",
                column: "AlbumID",
                principalTable: "Albums",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Albums_AlbumID",
                table: "Pictures");

            migrationBuilder.DropTable(
                name: "PictureAlbums");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_AlbumID",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "AlbumID",
                table: "Pictures");

            migrationBuilder.RenameColumn(
                name: "People",
                table: "Pictures",
                newName: "PeopleReferenceIDs");
        }
    }
}
