using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PhotoAlbum.Migrations
{
    public partial class Test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PicturePeopleDb");

            migrationBuilder.AddColumn<string>(
                name: "People",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "People",
                table: "Pictures");

            migrationBuilder.CreateTable(
                name: "PicturePeopleDb",
                columns: table => new
                {
                    PeopleID = table.Column<int>(nullable: false),
                    PictureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PicturePeopleDb", x => new { x.PeopleID, x.PictureID });
                    table.ForeignKey(
                        name: "FK_PicturePeopleDb_PeopleDb_PeopleID",
                        column: x => x.PeopleID,
                        principalTable: "PeopleDb",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PicturePeopleDb_Pictures_PictureID",
                        column: x => x.PictureID,
                        principalTable: "Pictures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PicturePeopleDb_PictureID",
                table: "PicturePeopleDb",
                column: "PictureID");
        }
    }
}
