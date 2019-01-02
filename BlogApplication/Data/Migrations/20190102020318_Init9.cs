using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApplication.Data.Migrations
{
    public partial class Init9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "Posts");
        }
    }
}
