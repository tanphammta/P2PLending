using Microsoft.EntityFrameworkCore.Migrations;

namespace P2PLending.Web.DAL.Migrations
{
    public partial class change_mobile_linked_payment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mobile_linked_payments_account_mobile_account_id",
                table: "mobile_linked_payments");

            migrationBuilder.DropIndex(
                name: "IX_mobile_linked_payments_account_id",
                table: "mobile_linked_payments");

            migrationBuilder.DropColumn(
                name: "service_code",
                table: "mobile_linked_payments");

            migrationBuilder.DropColumn(
                name: "service_name",
                table: "mobile_linked_payments");

            migrationBuilder.DropColumn(
                name: "service_owner_name",
                table: "mobile_linked_payments");

            migrationBuilder.AddColumn<string>(
                name: "service_account_name",
                table: "mobile_linked_payments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "service_id",
                table: "mobile_linked_payments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "service_account_name",
                table: "mobile_linked_payments");

            migrationBuilder.DropColumn(
                name: "service_id",
                table: "mobile_linked_payments");

            migrationBuilder.AddColumn<string>(
                name: "service_code",
                table: "mobile_linked_payments",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "service_name",
                table: "mobile_linked_payments",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "service_owner_name",
                table: "mobile_linked_payments",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_mobile_linked_payments_account_id",
                table: "mobile_linked_payments",
                column: "account_id");

            migrationBuilder.AddForeignKey(
                name: "FK_mobile_linked_payments_account_mobile_account_id",
                table: "mobile_linked_payments",
                column: "account_id",
                principalTable: "account_mobile",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
