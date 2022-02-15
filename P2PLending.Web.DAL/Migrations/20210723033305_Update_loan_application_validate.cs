using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace P2PLending.Web.DAL.Migrations
{
    public partial class Update_loan_application_validate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_borrower_relative_persons_relative_person_types_relative_per~",
                table: "borrower_relative_persons");

            migrationBuilder.DropForeignKey(
                name: "FK_loan_application_additional_infos_loan_applications_loan_app~",
                table: "loan_application_additional_infos");

            migrationBuilder.DropForeignKey(
                name: "FK_loan_applications_account_mobile_account_id",
                table: "loan_applications");

            migrationBuilder.DropForeignKey(
                name: "FK_loan_applications_borrower_profiles_borrower_profileid",
                table: "loan_applications");

            migrationBuilder.DropForeignKey(
                name: "FK_loan_applications_loan_products_loan_productid",
                table: "loan_applications");

            migrationBuilder.DropForeignKey(
                name: "FK_loan_product_additional_info_loan_products_loan_type_id",
                table: "loan_product_additional_info");

            migrationBuilder.DropTable(
                name: "loan_application_attribute_validate");

            migrationBuilder.DropIndex(
                name: "IX_loan_applications_account_id",
                table: "loan_applications");

            migrationBuilder.DropIndex(
                name: "IX_loan_applications_borrower_profileid",
                table: "loan_applications");

            migrationBuilder.DropIndex(
                name: "IX_loan_applications_loan_productid",
                table: "loan_applications");

            migrationBuilder.DropIndex(
                name: "IX_loan_application_additional_infos_loan_application_id",
                table: "loan_application_additional_infos");

            migrationBuilder.DropColumn(
                name: "borrower_profileid",
                table: "loan_applications");

            migrationBuilder.DropColumn(
                name: "loan_productid",
                table: "loan_applications");

            migrationBuilder.DropColumn(
                name: "info_format",
                table: "loan_application_additional_infos");

            migrationBuilder.DropColumn(
                name: "info_name",
                table: "loan_application_additional_infos");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sms_otps",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "relative_person_types",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "positions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "phone_verifications",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "payment_services",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "occupations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "mobile_linked_payments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "marital_statuses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "loan_products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "loan_product_additional_info",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "loan_type_id",
                table: "loan_product_additional_info",
                newName: "loan_product_info");

            migrationBuilder.RenameIndex(
                name: "IX_loan_product_additional_info_loan_type_id",
                table: "loan_product_additional_info",
                newName: "IX_loan_product_additional_info_loan_product_info");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "loan_period_management_fee_configs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "loan_management_parameter_configs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "loan_applications",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "loan_product_additional_infoid",
                table: "loan_application_additional_infos",
                newName: "loan_product_additional_infoId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "loan_application_additional_infos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "fees_parameter_configs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "dpd_collect_fee_configs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "departments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "credit_rank_configs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "borrower_relative_persons",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "relative_person_type_id",
                table: "borrower_relative_persons",
                newName: "type_id");

            migrationBuilder.RenameColumn(
                name: "relative_person_phone",
                table: "borrower_relative_persons",
                newName: "phone");

            migrationBuilder.RenameIndex(
                name: "IX_borrower_relative_persons_relative_person_type_id",
                table: "borrower_relative_persons",
                newName: "IX_borrower_relative_persons_type_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "borrower_profiles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "borrower_profiles",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "assigned_operator_id",
                table: "borrower_profiles",
                newName: "current_work_address_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "addresses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "account_operation",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "account_mobile",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "loan_applications",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "format",
                table: "loan_application_additional_infos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "loan_applicationId",
                table: "loan_application_additional_infos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "loan_application_additional_infos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "workplace_address_id",
                table: "borrower_profiles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "income",
                table: "borrower_profiles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "occupation_position",
                table: "borrower_profiles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "borrower_profiles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "level3_id",
                table: "addresses",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "level2_id",
                table: "addresses",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "level1_id",
                table: "addresses",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "loan_application_validate_attributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    loan_application_id = table.Column<int>(type: "int", nullable: false),
                    attribute_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    table_name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    column_name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    reference_id = table.Column<int>(type: "int", nullable: false),
                    is_validate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    reason = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
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
                    table.PrimaryKey("PK_loan_application_validate_attributes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "loan_validate_attribute_configs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    attribute_name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    display_name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    table_name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    column_name = table.Column<string>(type: "longtext", nullable: true)
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
                    table.PrimaryKey("PK_loan_validate_attribute_configs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_loan_application_additional_infos_loan_applicationId",
                table: "loan_application_additional_infos",
                column: "loan_applicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_borrower_relative_persons_relative_person_types_type_id",
                table: "borrower_relative_persons",
                column: "type_id",
                principalTable: "relative_person_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_application_additional_infos_loan_applications_loan_app~",
                table: "loan_application_additional_infos",
                column: "loan_applicationId",
                principalTable: "loan_applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_product_additional_info_loan_products_loan_product_info",
                table: "loan_product_additional_info",
                column: "loan_product_info",
                principalTable: "loan_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_borrower_relative_persons_relative_person_types_type_id",
                table: "borrower_relative_persons");

            migrationBuilder.DropForeignKey(
                name: "FK_loan_application_additional_infos_loan_applications_loan_app~",
                table: "loan_application_additional_infos");

            migrationBuilder.DropForeignKey(
                name: "FK_loan_product_additional_info_loan_products_loan_product_info",
                table: "loan_product_additional_info");

            migrationBuilder.DropTable(
                name: "loan_application_validate_attributes");

            migrationBuilder.DropTable(
                name: "loan_validate_attribute_configs");

            migrationBuilder.DropIndex(
                name: "IX_loan_application_additional_infos_loan_applicationId",
                table: "loan_application_additional_infos");

            migrationBuilder.DropColumn(
                name: "status",
                table: "loan_applications");

            migrationBuilder.DropColumn(
                name: "format",
                table: "loan_application_additional_infos");

            migrationBuilder.DropColumn(
                name: "loan_applicationId",
                table: "loan_application_additional_infos");

            migrationBuilder.DropColumn(
                name: "name",
                table: "loan_application_additional_infos");

            migrationBuilder.DropColumn(
                name: "occupation_position",
                table: "borrower_profiles");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "borrower_profiles");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sms_otps",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "relative_person_types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "positions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "phone_verifications",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "payment_services",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "occupations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "mobile_linked_payments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "marital_statuses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "loan_products",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "loan_product_additional_info",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "loan_product_info",
                table: "loan_product_additional_info",
                newName: "loan_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_loan_product_additional_info_loan_product_info",
                table: "loan_product_additional_info",
                newName: "IX_loan_product_additional_info_loan_type_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "loan_period_management_fee_configs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "loan_management_parameter_configs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "loan_applications",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "loan_product_additional_infoId",
                table: "loan_application_additional_infos",
                newName: "loan_product_additional_infoid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "loan_application_additional_infos",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "fees_parameter_configs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "dpd_collect_fee_configs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "departments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "credit_rank_configs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "borrower_relative_persons",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "type_id",
                table: "borrower_relative_persons",
                newName: "relative_person_type_id");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "borrower_relative_persons",
                newName: "relative_person_phone");

            migrationBuilder.RenameIndex(
                name: "IX_borrower_relative_persons_type_id",
                table: "borrower_relative_persons",
                newName: "IX_borrower_relative_persons_relative_person_type_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "borrower_profiles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "borrower_profiles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "current_work_address_id",
                table: "borrower_profiles",
                newName: "assigned_operator_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "addresses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "account_operation",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "account_mobile",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "borrower_profileid",
                table: "loan_applications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "loan_productid",
                table: "loan_applications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "info_format",
                table: "loan_application_additional_infos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "info_name",
                table: "loan_application_additional_infos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "workplace_address_id",
                table: "borrower_profiles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "income",
                table: "borrower_profiles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "level3_id",
                table: "addresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "level2_id",
                table: "addresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "level1_id",
                table: "addresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "loan_application_attribute_validate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    attribute_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_validate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    loan_application_id = table.Column<int>(type: "int", nullable: false),
                    page_name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    reason = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    update_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    update_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loan_application_attribute_validate", x => x.id);
                    table.ForeignKey(
                        name: "FK_loan_application_attribute_validate_loan_applications_loan_a~",
                        column: x => x.loan_application_id,
                        principalTable: "loan_applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_loan_applications_account_id",
                table: "loan_applications",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_loan_applications_borrower_profileid",
                table: "loan_applications",
                column: "borrower_profileid");

            migrationBuilder.CreateIndex(
                name: "IX_loan_applications_loan_productid",
                table: "loan_applications",
                column: "loan_productid");

            migrationBuilder.CreateIndex(
                name: "IX_loan_application_additional_infos_loan_application_id",
                table: "loan_application_additional_infos",
                column: "loan_application_id");

            migrationBuilder.CreateIndex(
                name: "IX_loan_application_attribute_validate_loan_application_id",
                table: "loan_application_attribute_validate",
                column: "loan_application_id");

            migrationBuilder.AddForeignKey(
                name: "FK_borrower_relative_persons_relative_person_types_relative_per~",
                table: "borrower_relative_persons",
                column: "relative_person_type_id",
                principalTable: "relative_person_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_application_additional_infos_loan_applications_loan_app~",
                table: "loan_application_additional_infos",
                column: "loan_application_id",
                principalTable: "loan_applications",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_applications_account_mobile_account_id",
                table: "loan_applications",
                column: "account_id",
                principalTable: "account_mobile",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_applications_borrower_profiles_borrower_profileid",
                table: "loan_applications",
                column: "borrower_profileid",
                principalTable: "borrower_profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_applications_loan_products_loan_productid",
                table: "loan_applications",
                column: "loan_productid",
                principalTable: "loan_products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_product_additional_info_loan_products_loan_type_id",
                table: "loan_product_additional_info",
                column: "loan_type_id",
                principalTable: "loan_products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
