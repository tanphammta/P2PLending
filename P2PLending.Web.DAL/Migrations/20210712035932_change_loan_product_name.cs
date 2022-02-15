using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace P2PLending.Web.DAL.Migrations
{
    public partial class change_loan_product_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_loan_application_additional_infos_loan_type_additional_info_~",
                table: "loan_application_additional_infos");

            migrationBuilder.DropForeignKey(
                name: "FK_loan_applications_loan_types_loan_typeid",
                table: "loan_applications");

            migrationBuilder.DropTable(
                name: "loan_type_additional_info");

            migrationBuilder.DropTable(
                name: "loan_types");

            migrationBuilder.RenameColumn(
                name: "loan_typeid",
                table: "loan_applications",
                newName: "loan_productid");

            migrationBuilder.RenameColumn(
                name: "loan_type_id",
                table: "loan_applications",
                newName: "loan_product_id");

            migrationBuilder.RenameIndex(
                name: "IX_loan_applications_loan_typeid",
                table: "loan_applications",
                newName: "IX_loan_applications_loan_productid");

            migrationBuilder.RenameColumn(
                name: "loan_type_additional_infoid",
                table: "loan_application_additional_infos",
                newName: "loan_product_additional_infoid");

            migrationBuilder.RenameColumn(
                name: "loan_type_additional_info_id",
                table: "loan_application_additional_infos",
                newName: "loan_product_additional_info_id");

            migrationBuilder.RenameIndex(
                name: "IX_loan_application_additional_infos_loan_type_additional_infoid",
                table: "loan_application_additional_infos",
                newName: "IX_loan_application_additional_infos_loan_product_additional_in~");

            migrationBuilder.CreateTable(
                name: "loan_products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    product_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    minimum_amount = table.Column<int>(type: "int", nullable: false),
                    maximum_amount = table.Column<int>(type: "int", nullable: false),
                    amount_unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "million")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    minimum_duration = table.Column<int>(type: "int", nullable: false),
                    maximum_duration = table.Column<int>(type: "int", nullable: false),
                    duration_unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "month")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    update_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    update_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loan_products", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "loan_product_additional_info",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    loan_type_id = table.Column<int>(type: "int", nullable: false),
                    info_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    info_format = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    update_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    update_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loan_product_additional_info", x => x.id);
                    table.ForeignKey(
                        name: "FK_loan_product_additional_info_loan_products_loan_type_id",
                        column: x => x.loan_type_id,
                        principalTable: "loan_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_loan_product_additional_info_loan_type_id",
                table: "loan_product_additional_info",
                column: "loan_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_loan_products_product_code",
                table: "loan_products",
                column: "product_code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_application_additional_infos_loan_product_additional_in~",
                table: "loan_application_additional_infos",
                column: "loan_product_additional_infoid",
                principalTable: "loan_product_additional_info",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_applications_loan_products_loan_productid",
                table: "loan_applications",
                column: "loan_productid",
                principalTable: "loan_products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_loan_application_additional_infos_loan_product_additional_in~",
                table: "loan_application_additional_infos");

            migrationBuilder.DropForeignKey(
                name: "FK_loan_applications_loan_products_loan_productid",
                table: "loan_applications");

            migrationBuilder.DropTable(
                name: "loan_product_additional_info");

            migrationBuilder.DropTable(
                name: "loan_products");

            migrationBuilder.RenameColumn(
                name: "loan_productid",
                table: "loan_applications",
                newName: "loan_typeid");

            migrationBuilder.RenameColumn(
                name: "loan_product_id",
                table: "loan_applications",
                newName: "loan_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_loan_applications_loan_productid",
                table: "loan_applications",
                newName: "IX_loan_applications_loan_typeid");

            migrationBuilder.RenameColumn(
                name: "loan_product_additional_infoid",
                table: "loan_application_additional_infos",
                newName: "loan_type_additional_infoid");

            migrationBuilder.RenameColumn(
                name: "loan_product_additional_info_id",
                table: "loan_application_additional_infos",
                newName: "loan_type_additional_info_id");

            migrationBuilder.RenameIndex(
                name: "IX_loan_application_additional_infos_loan_product_additional_in~",
                table: "loan_application_additional_infos",
                newName: "IX_loan_application_additional_infos_loan_type_additional_infoid");

            migrationBuilder.CreateTable(
                name: "loan_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    amount_unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "million")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    duration_unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "month")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    maximum_amount = table.Column<int>(type: "int", nullable: false),
                    maximum_duration = table.Column<int>(type: "int", nullable: false),
                    minimum_amount = table.Column<int>(type: "int", nullable: false),
                    minimum_duration = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    type_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    update_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    update_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loan_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "loan_type_additional_info",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    create_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    info_format = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    info_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    loan_type_id = table.Column<int>(type: "int", nullable: false),
                    update_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    update_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loan_type_additional_info", x => x.id);
                    table.ForeignKey(
                        name: "FK_loan_type_additional_info_loan_types_loan_type_id",
                        column: x => x.loan_type_id,
                        principalTable: "loan_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_loan_type_additional_info_loan_type_id",
                table: "loan_type_additional_info",
                column: "loan_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_loan_types_type_code",
                table: "loan_types",
                column: "type_code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_application_additional_infos_loan_type_additional_info_~",
                table: "loan_application_additional_infos",
                column: "loan_type_additional_infoid",
                principalTable: "loan_type_additional_info",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_applications_loan_types_loan_typeid",
                table: "loan_applications",
                column: "loan_typeid",
                principalTable: "loan_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
