using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewAPI.Migrations
{
    public partial class Update_RefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
