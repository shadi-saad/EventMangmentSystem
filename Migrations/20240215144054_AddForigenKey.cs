using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMangmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddForigenKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_ArtistId",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "ArtistId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_ArtistId",
                table: "Events",
                column: "ArtistId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_ArtistId",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "ArtistId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_ArtistId",
                table: "Events",
                column: "ArtistId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
