using Microsoft.EntityFrameworkCore;
using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.Entities;
using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Entities.AccountLinkedPayment;
using P2PLending.Web.Entities.Entities.AddressEntity;
using P2PLending.Web.Entities.Entities.Borrower;
using P2PLending.Web.Entities.Entities.Loans;
using P2PLending.Web.Entities.Entities.MasterData;
using P2PLending.Web.Entities.Entities.OperatorDepartment;
using P2PLending.Web.Entities.Entities.OTP;
using P2PLending.Web.Entities.Entities.Relative;
using P2PLending.Web.Helper.Constants;

namespace P2PLending.Web.DAL.DataContext
{
    public class P2PLendingDbContext : DbContext
    {
        public P2PLendingDbContext(DbContextOptions<P2PLendingDbContext> options) : base(options) { }
        public DbSet<AccountOperation> OperationAccounts { get; set; }
        public DbSet<AccountMobile> MobileAccounts { get; set; }
        public DbSet<PhoneVerification> PhoneVerifications { get; set; }
        public DbSet<SMSOTP> SMSOTPs { get; set; }
        public DbSet<BorrowerProfile> BorrowerProfiles { get; set; }
        public DbSet<RelativePerson> RelativePersons { get; set; }
        public DbSet<MobileLinkedPayment> MobileLinkedPayments { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<LoanApplicationAdditionalInfo> LoanApplicationAdditionalInfos { get; set; }
        public DbSet<LoanApplicationValidateAttribute> LoanApplicationAttributeValidates { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<RelativePersonType> RelativePersonTypes { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<LoanProduct> LoanProducts { get; set; }
        public DbSet<LoanProductAdditionalInfo> LoanProductAdditionalInfos { get; set; }
        public DbSet<AddressLevel1> AddressLevel1s { get; set; }
        public DbSet<AddressLevel2> AddressLevel2s { get; set; }
        public DbSet<AddressLevel3> AddressLevel3s { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<CreditRankConfig> CreditRankConfigs { get; set; }
        public DbSet<DPDCollectFeeConfig> DPDCollectFeeConfigs { get; set; }
        public DbSet<FeesParameterConfig> FeesParameterConfigs { get; set; }
        public DbSet<LoanManagementParameterConfig> LoanManagementParameterConfigs { get; set; }
        public DbSet<LoanPeriodManagementFeeConfig> LoanPeriodManagementFeeConfigs { get; set; }
        public DbSet<PaymentService> PaymentServices { get; set; }
        public DbSet<LoanValidateAttributeConfig> LoanValidateAttributes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region AccountOperation
            //AccountOperation
            builder.Entity<AccountOperation>().ToTable(TableName.AccountOperation);
            builder.Entity<AccountOperation>().Property(p => p.username).HasMaxLength(50).IsRequired(true);
            builder.Entity<AccountOperation>().HasIndex(p => p.username).IsUnique(true);
            builder.Entity<AccountOperation>().Property(p => p.email).HasMaxLength(255).IsRequired(true);
            builder.Entity<AccountOperation>().HasIndex(p => p.email).IsUnique(true);
            builder.Entity<AccountOperation>().Property(p => p.phone).HasMaxLength(20);
            builder.Entity<AccountOperation>().Property(p => p.full_name);

            #endregion
            #region AccountMobile
            //AccountMobile
            builder.Entity<AccountMobile>().ToTable(TableName.AccountMobile);
            builder.Entity<AccountMobile>().Property(p => p.phone).HasMaxLength(20).IsRequired(true);
            builder.Entity<AccountMobile>().Property(p => p.current_device_id).HasMaxLength(100);
            builder.Entity<AccountMobile>().Property(p => p.current_device_name).HasMaxLength(100);
            builder.Entity<AccountMobile>().Property(p => p.fcm_token);
            #endregion
            #region PhoneVerification
            //PhoneVerification
            builder.Entity<PhoneVerification>().ToTable(TableName.PhoneVerifications);
            builder.Entity<PhoneVerification>().Property(p => p.phone).HasMaxLength(20).IsRequired(true);
            builder.Entity<PhoneVerification>().Property(p => p.verification_type).IsRequired(true);
            builder.Entity<PhoneVerification>().Property(p => p.verified).IsRequired(true).HasDefaultValue(false);
            builder.Entity<PhoneVerification>().Property(p => p.retry_times).IsRequired(true).HasDefaultValue(0);
            builder.Entity<PhoneVerification>().Property(p => p.is_lock).IsRequired(true).HasDefaultValue(false);
            builder.Entity<PhoneVerification>().Property(p => p.current_sms_otp_id).IsRequired(true).HasDefaultValue(0);
            #endregion
            #region SMSOTP
            //SMSOTP
            builder.Entity<SMSOTP>().ToTable(TableName.SMSOTPS);
            builder.Entity<SMSOTP>()
                .HasOne(p => p.phone_verification)
                .WithMany(p => p.smsOTPs)
                .HasForeignKey(p => p.phone_verification_id);
            builder.Entity<SMSOTP>().Property(p => p.phone_verification_id).IsRequired(true);
            builder.Entity<SMSOTP>().Property(p => p.phone).IsRequired(true);
            builder.Entity<SMSOTP>().Property(p => p.value).IsRequired(true);
            builder.Entity<SMSOTP>().Property(p => p.status).IsRequired(true);
            builder.Entity<SMSOTP>().Property(p => p.expired_date).IsRequired(true);
            #endregion
            #region BorrowerProfile
            //BorrowerProfile
            builder.Entity<BorrowerProfile>().ToTable(TableName.BorrowerProfiles);
            builder.Entity<BorrowerProfile>()
                .HasOne(p => p.account_mobile)
                .WithMany()
                .HasForeignKey(p => p.account_id);
            builder.Entity<BorrowerProfile>().Property(p => p.full_name).HasMaxLength(255).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.gender).HasMaxLength(10).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.date_of_birth).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.resident_address_id).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.current_address_id).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.id_card_number).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.id_card_issue_date).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.id_card_issue_by).HasMaxLength(255).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.occupation).HasMaxLength(255).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.workplace_name).HasMaxLength(255).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.workplace_address_id).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.workplace_phone).HasMaxLength(20).IsRequired(true);
            builder.Entity<BorrowerProfile>().Property(p => p.income).IsRequired(true);
            #endregion
            #region RelativePerson
            //RelativePerson
            builder.Entity<RelativePerson>().ToTable(TableName.BorrowerRelativePersons);
            builder.Entity<RelativePerson>()
                .HasOne(p => p.borrower_profile)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(p => p.borrower_profile_id); ;
            builder.Entity<RelativePerson>()
                .HasOne(p => p.relative_person_type)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(p => p.type_id);
            #endregion
            #region RelativePersonType
            //RelativePersonType
            builder.Entity<RelativePersonType>().ToTable(TableName.RelativePersonTypes);
            builder.Entity<RelativePersonType>().Property(p => p.type_name).HasMaxLength(50).IsRequired(true);
            #endregion
            #region MaritalStatus
            //MaritalStatus
            builder.Entity<MaritalStatus>().ToTable(TableName.MaritalStatuses);
            builder.Entity<MaritalStatus>().HasIndex(p => p.status).IsUnique(true);
            builder.Entity<MaritalStatus>().Property(p => p.status).HasMaxLength(30).HasColumnType("nvarchar");
            #endregion
            #region Occupation
            //Occupation
            builder.Entity<Occupation>().ToTable(TableName.Occupations);
            builder.Entity<Occupation>().HasIndex(p => p.name).IsUnique(true);
            builder.Entity<Occupation>().Property(p => p.name).HasMaxLength(255).HasColumnType("nvarchar");
            #endregion
            #region LoanProduct
            //LoanProduct
            builder.Entity<LoanProduct>().ToTable(TableName.LoanProducts);
            builder.Entity<LoanProduct>().Property(p => p.product_code).HasMaxLength(50).IsRequired(true);
            builder.Entity<LoanProduct>().HasIndex(p => p.product_code).IsUnique(true);
            builder.Entity<LoanProduct>().Property(p => p.name).HasMaxLength(100).HasColumnType("nvarchar").IsRequired(true);
            builder.Entity<LoanProduct>().Property(p => p.description).HasMaxLength(255).HasColumnType("nvarchar");
            builder.Entity<LoanProduct>().Property(p => p.minimum_amount).IsRequired(true);
            builder.Entity<LoanProduct>().Property(p => p.maximum_amount).IsRequired(true);
            builder.Entity<LoanProduct>().Property(p => p.amount_unit).HasMaxLength(20).IsRequired(true).HasDefaultValue("million");
            builder.Entity<LoanProduct>().Property(p => p.minimum_duration).IsRequired(true);
            builder.Entity<LoanProduct>().Property(p => p.maximum_duration).IsRequired(true);
            builder.Entity<LoanProduct>().Property(p => p.duration_unit).HasMaxLength(20).IsRequired(true).HasDefaultValue("month");
            #endregion
            #region LoanProductAdditionalInfo
            //LoanProductAdditionalInfo
            builder.Entity<LoanProductAdditionalInfo>().ToTable(TableName.LoanProductAdditionalInfos);
            builder.Entity<LoanProductAdditionalInfo>()
                .HasOne(p => p.loan_type)
                .WithMany()
                .HasForeignKey(p => p.loan_product_id);
            builder.Entity<LoanProductAdditionalInfo>().Property(p => p.info_name).HasMaxLength(50).IsRequired(true).HasColumnType("nvarchar");
            builder.Entity<LoanProductAdditionalInfo>().Property(p => p.info_format).HasMaxLength(50).IsRequired(true);
            #endregion
            #region AddressLevel1
            //AddressLevel1
            builder.Entity<AddressLevel1>().ToTable(TableName.AddressLevel1).HasKey(p => p.level1_id);
            builder.Entity<AddressLevel1>().Property(p => p.level1_id).HasMaxLength(10);
            builder.Entity<AddressLevel1>().Property(p => p.name).HasMaxLength(100).IsRequired(true);
            builder.Entity<AddressLevel1>().Property(p => p.type).HasMaxLength(20);
            #endregion
            #region AddressLevel2
            //AddressLevel2
            builder.Entity<AddressLevel2>().ToTable(TableName.AddressLevel2).HasKey(p => p.level2_id);
            builder.Entity<AddressLevel2>()
                .HasOne(p => p.address_level_1)
                .WithMany(p => p.level2s)
                .HasForeignKey(p => p.level1_id);
            builder.Entity<AddressLevel2>().Property(p => p.level1_id).HasMaxLength(10);
            builder.Entity<AddressLevel2>().Property(p => p.level2_id).HasMaxLength(10);
            builder.Entity<AddressLevel2>().Property(p => p.name).HasMaxLength(100).IsRequired(true);
            builder.Entity<AddressLevel2>().Property(p => p.type).HasMaxLength(20);
            #endregion
            #region AddressLevel3
            //AddressLevel3
            builder.Entity<AddressLevel3>().ToTable(TableName.AddressLevel3).HasKey(p => p.level3_id);
            builder.Entity<AddressLevel3>()
                .HasOne(p => p.address_level_2)
                .WithMany(p => p.level3s)
                .HasForeignKey(p => p.level2_id);
            builder.Entity<AddressLevel3>().Property(p => p.level2_id).HasMaxLength(10);
            builder.Entity<AddressLevel3>().Property(p => p.level3_id).HasMaxLength(10);
            builder.Entity<AddressLevel3>().Property(p => p.name).HasMaxLength(100).IsRequired(true);
            builder.Entity<AddressLevel3>().Property(p => p.type).HasMaxLength(20);
            #endregion
            #region Address
            //Address
            builder.Entity<Address>().ToTable(TableName.Addresses);
            builder.Entity<Address>().Property(p => p.address_detail);
            #endregion
            #region MobileLinkedPayment
            //MobileLinkedPayment
            builder.Entity<MobileLinkedPayment>().ToTable(TableName.MobileLinkedPayments);
            builder.Entity<MobileLinkedPayment>().Property(p => p.service_account_id).HasMaxLength(50).IsRequired(true);
            #endregion
            #region LoanApplication
            //LoanApplication
            builder.Entity<LoanApplication>().ToTable(TableName.LoanApplication);
            builder.Entity<LoanApplication>().Property(p => p.borrower_profile_id).IsRequired(true);
            builder.Entity<LoanApplication>().Property(p => p.loan_amount).IsRequired(true);
            builder.Entity<LoanApplication>().Property(p => p.loan_duration).IsRequired(true);
            builder.Entity<LoanApplication>().Property(p => p.borrower_linked_payment_service_id).IsRequired(true);
            #endregion
            #region LoanApplicationAdditionalInfo
            //LoanApplicationAdditionalInfo
            builder.Entity<LoanApplicationAdditionalInfo>().ToTable(TableName.LoanApplicationAdditionalInfos);
            builder.Entity<LoanApplicationAdditionalInfo>().Property(p => p.value).IsRequired(true);
            #endregion
            #region LoanApplicationAttributeValidate
            //LoanApplicationAttributeValidate
            builder.Entity<LoanApplicationValidateAttribute>().ToTable(TableName.LoanApplicationValidateAttributes);
            builder.Entity<LoanApplicationValidateAttribute>().Property(p => p.attribute_name).IsRequired(true);
            builder.Entity<LoanApplicationValidateAttribute>().Property(p => p.is_validate).IsRequired(true);
            builder.Entity<LoanApplicationValidateAttribute>().Property(p => p.reason).HasMaxLength(255);
            #endregion
            #region Department
            builder.Entity<Department>().ToTable(TableName.Departments);
            builder.Entity<Department>().Property(p => p.code).HasMaxLength(50);
            builder.Entity<Department>().Property(p => p.name).HasMaxLength(50);
            builder.Entity<Department>().HasIndex(p => p.code).IsUnique(true);
            #endregion
            #region Position
            builder.Entity<Position>().ToTable(TableName.Positions);
            builder.Entity<Position>().Property(p => p.code).HasMaxLength(50);
            builder.Entity<Position>().Property(p => p.name).HasMaxLength(50);
            builder.Entity<Position>().HasIndex(p => p.code).IsUnique(true);
            #endregion
            #region CreditRankConfig
            builder.Entity<CreditRankConfig>().ToTable(TableName.CreditRankConfigs);
            #endregion
            #region DPDCollectFeeConfig
            builder.Entity<DPDCollectFeeConfig>().ToTable(TableName.DPDCollectFeeConfigs);
            #endregion
            #region FeesParameterConfig
            builder.Entity<FeesParameterConfig>().ToTable(TableName.FeesParameterConfigs);
            #endregion
            #region LoanManagementParameterConfig
            builder.Entity<LoanManagementParameterConfig>().ToTable(TableName.LoanManagementParameterConfigs);
            #endregion
            #region LoanPeriodManagementFeeConfig
            builder.Entity<LoanPeriodManagementFeeConfig>().ToTable(TableName.LoanPeriodManagementFeeConfigs);
            #endregion
            #region PaymentService
            builder.Entity<PaymentService>().ToTable(TableName.PaymentServices);
            #endregion
            #region LoanValidateAttribute
            builder.Entity<LoanValidateAttributeConfig>().ToTable(TableName.LoanValidateAttributeConfigs);
            #endregion
        }
    }
}
