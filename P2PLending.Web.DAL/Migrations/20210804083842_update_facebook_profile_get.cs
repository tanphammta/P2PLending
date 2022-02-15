using Microsoft.EntityFrameworkCore.Migrations;

namespace P2PLending.Web.DAL.Migrations
{
    public partial class update_facebook_profile_get : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "account_mobile",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "account_mobile",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar",
                table: "account_mobile");

            migrationBuilder.DropColumn(
                name: "email",
                table: "account_mobile");
        }
    }
}
