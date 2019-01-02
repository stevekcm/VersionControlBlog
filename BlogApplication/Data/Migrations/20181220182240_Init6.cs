using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApplication.Data.Migrations
{
    public partial class Init6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "comment",
                table: "SubComments",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "comment",
                table: "MainComments",
                newName: "Message");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Message",
                table: "SubComments",
                newName: "comment");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "MainComments",
                newName: "comment");
        }
    }
}
