using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PhotoAlbum.Migrations
{
    public partial class MigrationOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorID",
                table: "Pictures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PictureEvents",
                columns: table => new
                {
                    EventID = table.Column<int>(nullable: false),
                    PictureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PictureEvents", x => new { x.EventID, x.PictureID });
                    table.ForeignKey(
                        name: "FK_PictureEvents_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PictureEvents_Pictures_PictureID",
                        column: x => x.PictureID,
                        principalTable: "Pictures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PictureLocations",
                columns: table => new
                {
                    LocationID = table.Column<int>(nullable: false),
                    PictureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PictureLocations", x => new { x.LocationID, x.PictureID });
                    table.ForeignKey(
                        name: "FK_PictureLocations_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PictureLocations_Pictures_PictureID",
                        column: x => x.PictureID,
                        principalTable: "Pictures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_AuthorID",
                table: "Pictures",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_PictureEvents_PictureID",
                table: "PictureEvents",
                column: "PictureID");

            migrationBuilder.CreateIndex(
                name: "IX_PictureLocations_PictureID",
                table: "PictureLocations",
                column: "PictureID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Authors_AuthorID",
                table: "Pictures",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Authors_AuthorID",
                table: "Pictures");

            migrationBuilder.DropTable(
                name: "PictureEvents");

            migrationBuilder.DropTable(
                name: "PictureLocations");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_AuthorID",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "AuthorID",
                table: "Pictures");
        }
    }
}
