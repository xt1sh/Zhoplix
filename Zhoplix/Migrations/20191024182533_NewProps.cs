using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zhoplix.Migrations
{
    public partial class NewProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Titles");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CreditsStart",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasOpening",
                table: "Episodes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpeningFinish",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpeningStart",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThumbnailsAmount",
                table: "Episodes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailsLocation",
                table: "Episodes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    TitleId = table.Column<int>(nullable: false),
                    Liked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => new { x.UserId, x.TitleId });
                    table.ForeignKey(
                        name: "FK_Ratings_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TitleGenre",
                columns: table => new
                {
                    TitleId = table.Column<int>(nullable: false),
                    GenreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleGenre", x => new { x.TitleId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_TitleGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TitleGenre_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_TitleId",
                table: "Ratings",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_TitleGenre_GenreId",
                table: "TitleGenre",
                column: "GenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "TitleGenre");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropColumn(
                name: "CreditsStart",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "Format",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "HasOpening",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "OpeningFinish",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "OpeningStart",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "ThumbnailsAmount",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "ThumbnailsLocation",
                table: "Episodes");

            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Titles",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
