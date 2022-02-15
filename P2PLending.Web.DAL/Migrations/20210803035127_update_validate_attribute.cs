using Microsoft.EntityFrameworkCore.Migrations;

namespace P2PLending.Web.DAL.Migrations
{
    public partial class update_validate_attribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_loan_product_additional_info_loan_products_loan_product_info",
                table: "loan_product_additional_info");

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
                table: "loan_validate_attribute_configs",
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
                newName: "loan_product_id");

            migrationBuilder.RenameIndex(
                name: "IX_loan_product_additional_info_loan_product_info",
                table: "loan_product_additional_info",
                newName: "IX_loan_product_additional_info_loan_product_id");

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
                name: "Id",
                table: "loan_application_validate_attributes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "loan_product_additional_infoId",
                table: "loan_application_additional_infos",
                newName: "loan_product_additional_infoid");

            migrationBuilder.RenameColumn(
                name: "loan_applicationId",
                table: "loan_application_additional_infos",
                newName: "loan_applicationid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "loan_application_additional_infos",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_loan_application_additional_infos_loan_applicationId",
                table: "loan_application_additional_infos",
                newName: "IX_loan_application_additional_infos_loan_applicationid");

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
                name: "Id",
                table: "borrower_profiles",
                newName: "id");

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

            migrationBuilder.AlterColumn<int>(
                name: "loan_amount",
                table: "loan_applications",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "fees",
                table: "loan_applications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "interest_rate",
                table: "loan_applications",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "borrower_relative_persons",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "no",
                table: "borrower_relative_persons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "owned_type",
                table: "borrower_profiles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_loan_product_additional_info_loan_products_loan_product_id",
                table: "loan_product_additional_info",
                column: "loan_product_id",
                principalTable: "loan_products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_loan_product_additional_info_loan_products_loan_product_id",
                table: "loan_product_additional_info");

            migrationBuilder.DropColumn(
                name: "fees",
                table: "loan_applications");

            migrationBuilder.DropColumn(
                name: "interest_rate",
                table: "loan_applications");

            migrationBuilder.DropColumn(
                name: "name",
                table: "borrower_relative_persons");

            migrationBuilder.DropColumn(
                name: "no",
                table: "borrower_relative_persons");

            migrationBuilder.DropColumn(
                name: "owned_type",
                table: "borrower_profiles");

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
                table: "loan_validate_attribute_configs",
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
                name: "loan_product_id",
                table: "loan_product_additional_info",
                newName: "loan_product_info");

            migrationBuilder.RenameIndex(
                name: "IX_loan_product_additional_info_loan_product_id",
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
                name: "id",
                table: "loan_application_validate_attributes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "loan_product_additional_infoid",
                table: "loan_application_additional_infos",
                newName: "loan_product_additional_infoId");

            migrationBuilder.RenameColumn(
                name: "loan_applicationid",
                table: "loan_application_additional_infos",
                newName: "loan_applicationId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "loan_application_additional_infos",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_loan_application_additional_infos_loan_applicationid",
                table: "loan_application_additional_infos",
                newName: "IX_loan_application_additional_infos_loan_applicationId");

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
                name: "id",
                table: "borrower_profiles",
                newName: "Id");

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

            migrationBuilder.AlterColumn<float>(
                name: "loan_amount",
                table: "loan_applications",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_loan_product_additional_info_loan_products_loan_product_info",
                table: "loan_product_additional_info",
                column: "loan_product_info",
                principalTable: "loan_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
