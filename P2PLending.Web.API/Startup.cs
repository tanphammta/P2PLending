using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using P2PLending.Web.API.Mapper;
using P2PLending.Web.API.Middlewares;
using P2PLending.Web.Business.Implement;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities;
using P2PLending.Web.Entities.DTO.Setting;
using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Entities.AccountLinkedPayment;
using P2PLending.Web.Entities.Entities.AddressEntity;
using P2PLending.Web.Entities.Entities.Borrower;
using P2PLending.Web.Entities.Entities.Loans;
using P2PLending.Web.Entities.Entities.MasterData;
using P2PLending.Web.Entities.Entities.OperatorDepartment;
using P2PLending.Web.Entities.Entities.Relative;
using P2PLending.Web.Entities.Token;
using P2PLending.Web.Helper.Constants;
using P2PLending.Web.Helper.Extension;
using P2PLending.Web.Helper.JwtHandlers;
using P2PLending.Web.Repositories.Repositories.Implement;
using P2PLending.Web.Repositories.Repositories.Interface;
using P2PLending.Web.Repositories.UnitOfWork.Implement;
using P2PLending.Web.Repositories.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using static P2PLending.Web.Helper.Enums.Enums;

namespace P2PLending.Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            var jsonString = File.ReadAllText("versions.json");
            var versions = JsonConvert.DeserializeObject<AppVersion>(jsonString);
            var versionDesc = $@"
                AdminPortal: {versions.AdminPortal}
                Borrower: {versions.Borrower}
            ";
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContext<P2PLendingDbContext>(x =>
            x.UseMySql(
                connectionString, serverVersion, y => y.MigrationsAssembly("P2PLending.Web.DAL"))
            );
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "P2PLending APIs",
                    Version = "v1",
                    Description = versionDesc
                }
                );

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // configure strongly typed settings objects
            var tokensSection = Configuration.GetSection("Tokens");
            services.Configure<TokenSettings>(tokensSection);

            // configure jwt authentication
            var tokenSettings = tokensSection.Get<TokenSettings>();
            var key = System.Text.Encoding.ASCII.GetBytes(tokenSettings.Key);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Configure Firebase Admin SDK
            var privateKeyFile = Configuration.GetSection("Firebase").GetValue<string>("PrivateKeyFile");
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(privateKeyFile),
            });
            
            services.AddAutoMapper(typeof(MySQLToDTOProfile), typeof(DTOToMySQLProfile));

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<TokenSettings>(Configuration.GetSection("Tokens"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<SupportSetting>(Configuration.GetSection("Supports"));

            // DEPENDENCY INJECTIONS
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtHandler, JwtHandler>();
            services.AddScoped<P2PDBContextFactory, P2PDBContextFactory>();

            services.AddScoped<IOperationAccountService, OperationAccountService>();
            services.AddScoped<IMobileAccountService, MobileAccountService>();
            services.AddScoped<IOTPService, OTPService>();
            services.AddScoped<ILoanApplicationService, LoanApplicationService>();
            services.AddScoped<IFacebookService, FacebookService>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<ILinkedPaymentService, LinkedPaymentService>();

            services.AddScoped<IOperationAccountRepository, OperationAccountRepository>();
            services.AddScoped<IMobileAccountRepository, MobileAccountRepository>();
            services.AddScoped<IPhoneVerificationRepository, PhoneVerificationRepository>();
            services.AddScoped<ISMSOTPRepository, SMSOTPRepository>();
            services.AddScoped<IBorrowerProfileAddressRepository, BorrowerProfileAddressRepository>();
            services.AddScoped<IBorrowerProfileRepository, BorrowerProfileRepository>();
            services.AddScoped<IBorrowerRelativePersonRepository, BorrowerRelativePersonRepository>();
            services.AddScoped<ILoanApplicationAdditionalInfoRepository, LoanApplicationAdditionalInfoRepository>();
            services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
            services.AddScoped<ILoanTypeAdditionalInfoRepository, LoanTypeAdditionalInfoRepository>();
            services.AddScoped<ILoanProductRepository, LoanTypeRepository>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
            services.AddScoped<IMobileLinkedPaymentRepository, MobileLinkedPaymentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IFirebaseRepository, FirebaseRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ILoanApplicationValidateAttributeRepository, LoanApplicationValidateAttributeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, P2PLendingDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, @"Images\Icons")),
                RequestPath = "/icons"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, @"Images\Icons")),
                RequestPath = "/icons"
            });

            app.UseSwagger();

            var iisApplicationRoute = Configuration.GetSection("IIS").GetValue<string>("ApplicationRoute");
            app.UseSwaggerUI(c =>
            {
#if DEBUG
                // For Debug in Kestrel
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "P2P API V1");
#else
               // To deploy on IIS
               c.SwaggerEndpoint($"/{iisApplicationRoute}/swagger/v1/swagger.json", "P2P API V1");
#endif
                c.RoutePrefix = string.Empty;
            });

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            loggerFactory.AddLog4Net();
            // migrate latest
            context.Database.Migrate();
            SeedData(context, env);
        }


        private void SeedData(P2PLendingDbContext context, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                if (!context.MobileAccounts.Any())
                {
                    var data = new List<AccountMobile>()
                    {
                        new AccountMobile()
                        {
                            phone = "0969061760",
                            role = UserRole.Borrower,
                            create_date = DateTime.Now,
                            password = "Ws82LnhPq6PsFMkpfoPY0s6fybt/J23cek4C2ZMjhCI=",
                            password_salt = "+IlhAHZtfAAxi7dBQ8MBcQ==",
                            password_hash_algorithm = "HMACSHA1",
                            registration_time = DateTime.UtcNow.ToUnixSeconds(),
                            is_active = true
                        }
                    };

                    context.MobileAccounts.AddRange(data);
                    context.SaveChanges();
                }

                if (!context.OperationAccounts.Any())
                {
                    var data = new List<AccountOperation>()
                    {
                        new AccountOperation()
                        {
                            username = "tanpq",
                            email = "quangtan1701@gmail.com",
                            phone = "0969061760",
                            full_name = "aaaaaaa",
                            role = UserRole.Admin,
                            create_date = DateTime.Now,
                            password = "Ws82LnhPq6PsFMkpfoPY0s6fybt/J23cek4C2ZMjhCI=",
                            password_salt = "+IlhAHZtfAAxi7dBQ8MBcQ==",
                            password_hash_algorithm = "HMACSHA1",
                            registration_time = DateTime.UtcNow.ToUnixSeconds(),
                            is_active = true,
                        }
                    };

                    context.OperationAccounts.AddRange(data);
                    context.SaveChanges();
                }
            }
            if (!context.LoanProducts.Any())
            {
                var data = new List<LoanProduct>()
                    {
                        new LoanProduct
                        {
                            id = 1,
                            product_code = "StudentLoan",
                            name = "Vay cho sinh viên",
                            minimum_amount = 1,
                            maximum_amount = 15,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "student-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 2,
                            product_code = "MotorbikeLoan",
                            name = "Vay mua xe máy",
                            minimum_amount = 1,
                            maximum_amount = 30,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "motorbike-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 3,
                            product_code = "PhonePurchaseLoan",
                            name = "Vay mua điện thoại",
                            minimum_amount = 1,
                            maximum_amount = 20,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "phone-purchase-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 4,
                            product_code = "WeddingLoan",
                            name = "Vay cho đám cưới",
                            minimum_amount = 1,
                            maximum_amount = 15,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "wedding-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 5,
                            product_code = "PregnancyLoan",
                            name = "Vay cho bà bầu",
                            minimum_amount = 1,
                            maximum_amount = 15,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "pregnancy-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 6,
                            product_code = "BabyLoan",
                            name = "Vay cho em bé",
                            minimum_amount = 1,
                            maximum_amount = 15,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "baby-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 7,
                            product_code = "FurnitureLoan",
                            name = "Vay mua nội thất",
                            minimum_amount = 1,
                            maximum_amount = 15,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "furniture-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 8,
                            product_code = "MedicalLoan",
                            name = "Vay cho chi phí y tế",
                            minimum_amount = 1,
                            maximum_amount = 15,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "medical-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 9,
                            product_code = "DebtPaidLoan",
                            name = "Vay thanh toán nợ",
                            minimum_amount = 1,
                            maximum_amount = 30,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "debt-paid-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 10,
                            product_code = "SmallBusinessLoan",
                            name = "Vay hộ kinh doanh",
                            minimum_amount = 1,
                            maximum_amount = 10,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "small-business-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 11,
                            product_code = "GrabVietgoLoan",
                            name = "Vay của Grab, Vietgo",
                            minimum_amount = 1,
                            maximum_amount = 10,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "grab-vietgo-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 12,
                            product_code = "CashLoan",
                            name = "Vay tiền mặt",
                            minimum_amount = 1,
                            maximum_amount = 10,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "cash-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 13,
                            product_code = "AgriLoan",
                            name = "Vay p.triển nông thôn",
                            minimum_amount = 1,
                            maximum_amount = 10,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "agriculture-loan-icon.svg"
                        },
                        new LoanProduct
                        {
                            id = 14,
                            product_code = "Other",
                            name = "Vay khác",
                            minimum_amount = 1,
                            maximum_amount = 10,
                            minimum_duration = 1,
                            maximum_duration = 12,
                            create_date=DateTime.Now,
                            icon = "other-loan-icon.svg"
                        }
                    };

                context.LoanProducts.AddRange(data);
                context.SaveChanges();
            }

            if (!context.LoanProductAdditionalInfos.Any())
            {
                var data = new List<LoanProductAdditionalInfo>()
                    {
                        new LoanProductAdditionalInfo()
                        {
                            loan_product_id = 1,
                            info_name = "Bảng điểm",
                            info_format = AdditionalInfoFormat.Media,
                            create_date=DateTime.Now
                        },
                        new LoanProductAdditionalInfo()
                        {
                            loan_product_id = 1,
                            info_name = "Thẻ sinh viên",
                            info_format = AdditionalInfoFormat.Media,
                            create_date=DateTime.Now
                        },
                        new LoanProductAdditionalInfo()
                        {
                            loan_product_id = 2,
                            info_name = "Bảng lương",
                            info_format = AdditionalInfoFormat.Media,
                            create_date=DateTime.Now
                        },
                        new LoanProductAdditionalInfo()
                        {
                            loan_product_id = 2,
                            info_name = "Mức lương",
                            info_format = AdditionalInfoFormat.Text,
                            create_date=DateTime.Now
                        },
                        new LoanProductAdditionalInfo()
                        {
                            loan_product_id = 3,
                            info_name = "Bảng lương",
                            info_format = AdditionalInfoFormat.Media,
                            create_date=DateTime.Now
                        },
                        new LoanProductAdditionalInfo()
                        {
                            loan_product_id = 3,
                            info_name = "Mức lương",
                            info_format = AdditionalInfoFormat.Text,
                            create_date=DateTime.Now
                        }
                    };

                context.LoanProductAdditionalInfos.AddRange(data);
                context.SaveChanges();
            }
            if (!context.MaritalStatuses.Any())
            {
                var data = new List<MaritalStatus>()
                {
                    new MaritalStatus()
                    {
                        id = 1,
                        status = "Độc thân",
                        create_date = DateTime.Now
                    },
                    new MaritalStatus()
                    {
                        id = 2,
                        status = "Kết hôn",
                        create_date = DateTime.Now
                    }
                };

                context.MaritalStatuses.AddRange(data);
                context.SaveChanges();
            }

            if (!context.RelativePersonTypes.Any())
            {
                var data = new List<RelativePersonType>()
                {
                    new RelativePersonType()
                    {
                        id = 1,
                        type_name = "Bố",
                        create_date = DateTime.Now
                    },
                    new RelativePersonType()
                    {
                        id = 2,
                        type_name = "Mẹ",
                        create_date = DateTime.Now
                    },new RelativePersonType()
                    {
                        id = 3,
                        type_name = "Vợ",
                        create_date = DateTime.Now
                    },
                    new RelativePersonType()
                    {
                        id = 4,
                        type_name = "Chồng",
                        create_date = DateTime.Now
                    }
                };

                context.RelativePersonTypes.AddRange(data);
                context.SaveChanges();
            }

            if (!context.Occupations.Any())
            {
                var data = new List<Occupation>()
                {
                    new Occupation()
                    {
                        id = 1,
                        code = "Student",
                        name = "Sinh viên",
                        create_date = DateTime.Now
                    },
                    new Occupation()
                    {
                        id = 2,
                        code = "Bank",
                        name = "Ngân hàng",
                        create_date = DateTime.Now
                    },
                    new Occupation()
                    {
                        id = 3,
                        code = "Office",
                        name = "Nhân viên văn phòng",
                        create_date = DateTime.Now
                    },
                    new Occupation()
                    {
                        id = 4,
                        code = "Business",
                        name = "Kinh doanh",
                        create_date = DateTime.Now
                    },
                    new Occupation()
                    {
                        id = 5,
                        code = "Freelance",
                        name = "Tự do",
                        create_date = DateTime.Now
                    }
                };

                context.Occupations.AddRange(data);
                context.SaveChanges();
            }

            if (!context.AddressLevel1s.Any()
                && !context.AddressLevel2s.Any()
                && !context.AddressLevel3s.Any())
            {
                var jsonString = File.ReadAllText("seed_data.json");
                var addressData = JObject.Parse(jsonString)["address"].ToObject<AddressList>();

                if (addressData.Data.Any())
                {
                    var level1s = addressData.Data;
                    var level2s = level1s.SelectMany(lvl1 => lvl1.level2s).ToList();
                    var level3s = level2s.SelectMany(lvl2 => lvl2.level3s).ToList();

                    context.AddressLevel1s.AddRange(level1s);
                    context.AddressLevel2s.AddRange(level2s);
                    context.AddressLevel3s.AddRange(level3s);
                    context.SaveChanges();

                }
            }

            if (!context.Departments.Any())
            {
                var data = new List<Department>()
                {
                    new Department()
                    {
                        id = 1,
                        code = "Admin",
                        name = "Admin",
                        create_date = DateTime.Now
                    },
                    new Department()
                    {
                        id = 2,
                        code = "sale",
                        name = "Kinh Doanh",
                        create_date = DateTime.Now
                    },
                    new Department()
                    {
                        id = 3,
                        code = "Appraisal",
                        name = "Thẩm định",
                        create_date = DateTime.Now
                    },
                    new Department()
                    {
                        id = 4,
                        code = "Accountant",
                        name = "Kế toán",
                        create_date = DateTime.Now
                    },
                    new Department()
                    {
                        id = 5,
                        code = "Collection",
                        name = "Collection",
                        create_date = DateTime.Now
                    },
                };

                context.Departments.AddRange(data);
                context.SaveChanges();
            }

            if (!context.Positions.Any())
            {
                var data = new List<Position>()
                {
                    new Position()
                    {
                        id = 1,
                        code = "Manager",
                        name = "Quản lý",
                        rank = (int)OperatorPosition.Manager
                    },
                    new Position()
                    {
                        id = 2,
                        code = "Team Leader",
                        name = "Trưởng nhóm",
                        rank = (int)OperatorPosition.TeamLeader
                    },
                    new Position()
                    {
                        id = 3,
                        code = "Staff",
                        name = "Nhân Viên",
                        rank = (int)OperatorPosition.Staff
                    },
                };

                context.Positions.AddRange(data);
                context.SaveChanges();
            }

            if (!context.CreditRankConfigs.Any())
            {
                var datas = new List<CreditRankConfig>()
                {
                    new CreditRankConfig()
                    {
                        id = 1,
                        min_score = 0,
                        max_score = 0,
                        rank = "0",
                        unit = "%",
                        create_date = DateTime.Now
                    },
                    new CreditRankConfig()
                    {
                        id = 2,
                        min_score = 1,
                        max_score = 50,
                        rank = "C3",
                        unit = "%",
                        create_date = DateTime.Now
                    },
                    new CreditRankConfig()
                    {
                        id = 3,
                        min_score = 51,
                        max_score = 100,
                        rank = "C2",
                        unit = "%",
                        create_date = DateTime.Now
                    },
                    new CreditRankConfig()
                    {
                        id = 4,
                        min_score = 101,
                        max_score = 150,
                        rank = "C1",
                        unit = "%",
                        create_date = DateTime.Now
                    },
                    new CreditRankConfig()
                    {
                        id = 5,
                        min_score = 151,
                        max_score = 200,
                        rank = "B3",
                        interest_rate = 36,
                        unit = "%",
                        create_date = DateTime.Now
                    },
                    new CreditRankConfig()
                    {
                        id = 6,
                        min_score = 201,
                        max_score = 250,
                        rank = "B2",
                        interest_rate = 32,
                        unit = "%",
                        create_date = DateTime.Now
                    },
                    new CreditRankConfig()
                    {
                        id = 7,
                        min_score = 251,
                        max_score = 300,
                        rank = "B1",
                        interest_rate = 28,
                        unit = "%",
                        create_date = DateTime.Now
                    },
                    new CreditRankConfig()
                    {
                        id = 8,
                        min_score = 301,
                        max_score = 350,
                        rank = "A3",
                        interest_rate = 24,
                        unit = "%",
                        create_date = DateTime.Now
                    },
                    new CreditRankConfig()
                    {
                        id = 9,
                        min_score = 351,
                        max_score = 400,
                        rank = "A2",
                        interest_rate = 20,
                        unit = "%",
                        create_date = DateTime.Now
                    },
                    new CreditRankConfig()
                    {
                        id = 10,
                        min_score = 401,
                        rank = "A1",
                        interest_rate = 16,
                        unit = "%",
                        create_date = DateTime.Now
                    }
                };

                context.CreditRankConfigs.AddRange(datas);
                context.SaveChanges();
            }

            if (!context.DPDCollectFeeConfigs.Any())
            {
                var datas = new List<DPDCollectFeeConfig>()
                {
                    new DPDCollectFeeConfig()
                    {
                        id = 1,
                        create_date = DateTime.Now,
                        from_days = 0,
                        to_days = 5,
                        paid_target = "Investor",
                        beneficial_target = "Mony",
                        value = 0,
                        unit = "%",
                        description = @"X% * [Repay (Gốc + Lãi + PE) + Phí tất toán nếu có]"
                    },
                    new DPDCollectFeeConfig()
                    {
                        id = 2,
                        create_date = DateTime.Now,
                        from_days = 6,
                        to_days = 30,
                        paid_target = "Investor",
                        beneficial_target = "Mony",
                        value = 2,
                        unit = "%",
                        description = @"X% * [Repay (Gốc + Lãi + PE) + Phí tất toán nếu có]"
                    },
                    new DPDCollectFeeConfig()
                    {
                        id = 3,
                        create_date = DateTime.Now,
                        from_days = 31,
                        to_days = 60,
                        paid_target = "Investor",
                        beneficial_target = "Mony",
                        value = 4,
                        unit = "%",
                        description = @"X% * [Repay (Gốc + Lãi + PE) + Phí tất toán nếu có]"
                    },
                    new DPDCollectFeeConfig()
                    {
                        id = 4,
                        create_date = DateTime.Now,
                        from_days = 61,
                        to_days = 90,
                        paid_target = "Investor",
                        beneficial_target = "Mony",
                        value = 7,
                        unit = "%",
                        description = @"X% * [Repay (Gốc + Lãi + PE) + Phí tất toán nếu có]"
                    },
                    new DPDCollectFeeConfig()
                    {
                        id = 5,
                        create_date = DateTime.Now,
                        from_days = 91,
                        to_days = 180,
                        paid_target = "Investor",
                        beneficial_target = "Mony",
                        value = 25,
                        unit = "%",
                        description = @"X% * [Repay (Gốc + Lãi + PE) + Phí tất toán nếu có]"
                    },
                    new DPDCollectFeeConfig()
                    {
                        id = 6,
                        create_date = DateTime.Now,
                        from_days = 181,
                        to_days = 360,
                        paid_target = "Investor",
                        beneficial_target = "Mony",
                        value = 35,
                        unit = "%",
                        description = @"X% * [Repay (Gốc + Lãi + PE) + Phí tất toán nếu có]"
                    },
                    new DPDCollectFeeConfig()
                    {
                        id = 7,
                        create_date = DateTime.Now,
                        from_days = 361,
                        paid_target = "Investor",
                        beneficial_target = "Mony",
                        value = 50,
                        unit = "%",
                        description = @"X% * [Repay (Gốc + Lãi + PE) + Phí tất toán nếu có]"
                    }
                };

                context.DPDCollectFeeConfigs.AddRange(datas);
                context.SaveChanges();
            }

            if (!context.FeesParameterConfigs.Any())
            {
                var datas = new List<FeesParameterConfig>()
                {
                    new FeesParameterConfig()
                    {
                        id = 1,
                        create_date = DateTime.Now,
                        name = "Phí khởi tạo khoản vay",
                        paid_target = "Borrower",
                        beneficial_target = "Finplus",
                        description = "Tỉ lệ",
                        value = 3,
                        unit = "%"
                    },
                    new FeesParameterConfig()
                    {
                        id = 2,
                        create_date = DateTime.Now,
                        name = "Phí quản lý khoản vay",
                        paid_target = "Investor",
                        beneficial_target = "Finplus",
                        description = "X% * Số tiền gốc borrower trả nợ thực tế từng lần cho investor",
                        value = 1,
                        unit = "%"
                    },
                    new FeesParameterConfig()
                    {
                        id = 3,
                        create_date = DateTime.Now,
                        name = "Lãi trả chậm",
                        description = "X% * (Gốc + Lãi) * số ngày trả chậm * Lãi suất / 365",
                        value = 1,
                        unit = "%"
                    },
                    new FeesParameterConfig()
                    {
                        id = 4,
                        create_date = DateTime.Now,
                        name = "t1",
                        description = "Mốc thời gian trả chậm",
                        value = 1,
                        unit = "ngày"
                    },
                    new FeesParameterConfig()
                    {
                        id = 5,
                        create_date = DateTime.Now,
                        name = "t2",
                        description = "Mốc thời gian trả chậm",
                        value = 2,
                        unit = "ngày"
                    },
                    new FeesParameterConfig()
                    {
                        id = 6,
                        create_date = DateTime.Now,
                        name = "Phí phạt trả chậm dưới t1 ngày",
                        paid_target = "Borrower",
                        beneficial_target = "Finplus",
                        description = "Tỉ lệ phạt",
                        value = 1,
                        unit = "%"
                    },
                    new FeesParameterConfig()
                    {
                        id = 7,
                        create_date = DateTime.Now,
                        name = "Phí phạt trả chậm từ t1 ngày đến t2 ngày",
                        paid_target = "Borrower",
                        beneficial_target = "Finplus",
                        description = "X% * (Gốc + Lãi)",
                        value = 1,
                        unit = "%"
                    },
                    new FeesParameterConfig()
                    {
                        id = 8,
                        create_date = DateTime.Now,
                        name = "Phí phạt trả chậm từ trên t2 ngày",
                        paid_target = "Borrower",
                        beneficial_target = "Finplus",
                        description = "X% * (Gốc + Lãi)",
                        value = 1,
                        unit = "%"
                    },
                    new FeesParameterConfig()
                    {
                        id = 9,
                        create_date = DateTime.Now,
                        name = "Phí trả nợ trước hạn",
                        paid_target = "Borrower",
                        beneficial_target = "Investor",
                        description = "X% * (Gốc * số tháng trước hạn)",
                        value = 1,
                        unit = "%"
                    },
                };

                context.FeesParameterConfigs.AddRange(datas);
                context.SaveChanges();
            }

            if (!context.LoanManagementParameterConfigs.Any())
            {
                var datas = new List<LoanManagementParameterConfig>()
                {
                    new LoanManagementParameterConfig()
                    {
                        id = 1,
                        create_date = DateTime.Now,
                        name = "d1",
                        value = 10,
                        unit = "ngày",
                        description = "Thời gian tối da nhận lãi suất, sau d1 ngày đơn tự chuyển từ Chờ xác nhận lãi suất sang Quá hạn xác nhận lãi suất"
                    },
                    new LoanManagementParameterConfig()
                    {
                        id = 2,
                        create_date = DateTime.Now,
                        name = "d2",
                        value = 10,
                        unit = "ngày",
                        description = "Thời gian trên chợ tối đa"
                    },
                    new LoanManagementParameterConfig()
                    {
                        id = 3,
                        create_date = DateTime.Now,
                        name = "d3",
                        value = 12,
                        unit = "ngày",
                        description = "Thời gian tối đa đơn ở trạng thái Quá hạn xác nhận lãi suất, sau thời gian này hệ thống tự hủy đơn"
                    },
                    new LoanManagementParameterConfig()
                    {
                        id = 4,
                        create_date = DateTime.Now,
                        name = "d4",
                        value = 42,
                        unit = "ngày",
                        description = "Thời gian tối đa đơn ở trạng thái Quá hạn huy động vốn, sau thời gian này hệ thống tự hủy đơn"
                    },
                    new LoanManagementParameterConfig()
                    {
                        id = 5,
                        create_date = DateTime.Now,
                        name = "d5",
                        value = 72,
                        unit = "ngày",
                        description = "Thời gian tối đa đơn ở trạng thái Chờ ký hợp đồng, sau thời gian này đơn được chuyển sang trạng thái Quá hạn huy động vốn"
                    },
                    new LoanManagementParameterConfig()
                    {
                        id = 6,
                        create_date = DateTime.Now,
                        name = "m0",
                        value = 72,
                        unit = "%",
                        description = "Tỉ lệ (tính theo %) số tiền cần huy động tối thiểu để đủ điều kiện giải ngân"
                    }
                };

                context.LoanManagementParameterConfigs.AddRange(datas);
                context.SaveChanges();
            }

            if (!context.LoanPeriodManagementFeeConfigs.Any())
            {
                var datas = new List<LoanPeriodManagementFeeConfig>()
                {
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 1,
                        create_date = DateTime.Now,
                        period = 1,
                        name = "Kỳ thứ 1",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X1",
                        value = 1.5f,
                        unit = "%"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 2,
                        create_date = DateTime.Now,
                        period = 2,
                        name = "Kỳ thứ 2",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X3",
                        value = 1.5f,
                        unit = "%"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 3,
                        create_date = DateTime.Now,
                        period = 3,
                        name = "Kỳ thứ 3",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X3",
                        value = 1.5f,
                        unit = "%"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 4,
                        create_date = DateTime.Now,
                        period = 1,
                        name = "Kỳ thứ 4",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X4",
                        value = 1.5f,
                        unit = "%"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 5,
                        create_date = DateTime.Now,
                        period = 5,
                        name = "Kỳ thứ 5",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X5",
                        value = 1.5f,
                        unit = "%"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 6,
                        create_date = DateTime.Now,
                        period = 6,
                        name = "Kỳ thứ 6",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X6",
                        value = 1.5f,
                        unit = "%"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 7,
                        create_date = DateTime.Now,
                        period = 7,
                        name = "Kỳ thứ 7",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X7"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 8,
                        create_date = DateTime.Now,
                        period = 8,
                        name = "Kỳ thứ 8",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X8",
                        value = 1.5f,
                        unit = "%"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 9,
                        create_date = DateTime.Now,
                        period = 9,
                        name = "Kỳ thứ 9",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X9",
                        value = 1.5f,
                        unit = "%"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 10,
                        create_date = DateTime.Now,
                        period = 10,
                        name = "Kỳ thứ 10",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X10",
                        value = 1.5f,
                        unit = "%"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 11,
                        create_date = DateTime.Now,
                        period = 11,
                        name = "Kỳ thứ 11",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X11",
                        value = 1.5f,
                        unit = "%"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 12,
                        create_date = DateTime.Now,
                        period = 12,
                        name = "Kỳ thứ 12",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X12",
                        value = 1.5f,
                        unit = "%"
                    },
                    new LoanPeriodManagementFeeConfig()
                    {
                        id = 13,
                        create_date = DateTime.Now,
                        period = 13,
                        name = "Các kỳ sau kỳ 12",
                        paid_target = "Borrower",
                        beneficial_target = "Mony",
                        description = "Số tiền giải ngân * Tỉ lệ phí X13",
                        value = 1.5f,
                        unit = "%"
                    }
                };

                context.LoanPeriodManagementFeeConfigs.AddRange(datas);
                context.SaveChanges();
            }

            if (!context.PaymentServices.Any())
            {
                var datas = new List<PaymentService>()
                {
                    new PaymentService()
                    {
                        name = "Vietcombank",
                        full_name = "Ngân hàng TMCP Ngoại thương",
                        icon = "vietcombank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "Vietinbank",
                        full_name = "Ngân hàng TMCP Công thương",
                        icon = "vietinbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "Techcombank",
                        full_name = "Ngân hàng TMCP Kỹ Thương Việt Nam",
                        icon = "techcombank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "AGRIBANK",
                        full_name = "Ngân hàng TMCP Nông Nghiệp",
                        icon = "agribank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "BIDV",
                        full_name = "Ngân hàng TMCP Đầu Tư",
                        icon = "bidv.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "ABBank",
                        full_name = "Ngân hàng TMCP An Bình",
                        icon = "abbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "ACB",
                        full_name = "Ngân hàng TMCP Á Châu",
                        icon = "acb.png"
                    },
                    new PaymentService()
                    {
                        name = "Bac A Bank",
                        full_name = "Ngân hàng TMCP Bắc Á",
                        icon = "baca.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "Bac A Bank",
                        full_name = "Ngân hàng TMCP Bắc Á",
                        icon = "baca.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "Bao Viet Bank",
                        full_name = "Ngân hàng TMCP Bảo Việt",
                        icon = "baoviet.png"
                    },
                    new PaymentService()
                    {
                        name = "Dong A Bank",
                        full_name = "Ngân hàng TMCP Đông Á",
                        icon = "donga.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "Eximbank",
                        full_name = "Ngân hàng TMCP Xuất nhập khẩu",
                        icon = "eximbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "GPBank",
                        full_name = "Ngân hàng TMCP Dầu khí Toàn Cầu",
                        icon = "gpbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "HDBank",
                        full_name = "NH TMCP Phát Triển Nhà TPHCM",
                        icon = "hdbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "Kien Long Bank",
                        full_name = "Ngân hàng TMCP Kiên Long",
                        icon = "kienlong.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "Lien Viet Post Bank",
                        full_name = "Ngân hàng TMCP Bưu điện Liên Việt",
                        icon = "kienlong.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "Kien Long Bank",
                        full_name = "Ngân hàng TMCP Kiên Long",
                        icon = "lienviet.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "MB Bank",
                        full_name = "Ngân hàng TMCP Quân Đội",
                        icon = "mbbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "MDB",
                        full_name = "Ngân hàng TMCP Phát Triển Mê Kông",
                        icon = "mdb.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "MHB",
                        full_name = "NH TMCP Phát triển Nhà ĐBSCL",
                        icon = "mhb.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "MSB (Maritime Bank)",
                        full_name = "Ngân hàng TMCP Hàng Hải Việt Nam",
                        icon = "msb.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "Nam A Bank",
                        full_name = "Ngân hàng TMCP Nam Á",
                        icon = "nama.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "NCB",
                        full_name = "Ngân hàng TMCP Quốc Dân",
                        icon = "ncb.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "OCB",
                        full_name = "Ngân hàng TMCP Phương Đông",
                        icon = "ocb.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "OceanBank",
                        full_name = "Ngân hàng TMCP Đại Dương",
                        icon = "oceanbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "PGBank",
                        full_name = "Ngân hàng TMCP Xăng dầu Petrolimex",
                        icon = "pgbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "PNB (Phuong Nam Bank)",
                        full_name = "Ngân hàng TMCP Phương Nam",
                        icon = "pnb.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "PVCOMBANK",
                        full_name = "Ngân hàng TMCP Đại chúng",
                        icon = "pvcom.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "SACOMBANK",
                        full_name = "Ngân hàng TMCP Sài Gòn Thương Tín",
                        icon = "sacombank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "SaigonBank",
                        full_name = "NH TMCP Sài Gòn Công Thương",
                        icon = "saigonbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "SCB",
                        full_name = "Ngân hàng TMCP Sài Gòn",
                        icon = "scb.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "SeaBank",
                        full_name = "Ngân hàng TMCP Đông Nam Á",
                        icon = "seabank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "SHB",
                        full_name = "Ngân hàng TMCP Sài Gòn - Hà Nội",
                        icon = "shb.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "TPBank",
                        full_name = "Ngân hàng TMCP Tiên Phong",
                        icon = "tpbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "VCCB (Viet Capital)",
                        full_name = "Ngân hàng TMCP Bản Việt",
                        icon = "vccb.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "VCCB (Viet Capital)",
                        full_name = "Ngân hàng TMCP Bản Việt",
                        icon = "vccb.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "VIB",
                        full_name = "Ngân hàng TMCP Quốc tế",
                        icon = "vib.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "Viet A Bank",
                        full_name = "Ngân hàng TMCP Việt Á",
                        icon = "vieta.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "VietBank",
                        full_name = "Ngân hàng TMCP Việt Nam Thương Tín",
                        icon = "vietbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "VNCB",
                        full_name = "Ngân hàng TMCP Xây dựng",
                        icon = "vncb.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "VPBank",
                        full_name = "NH TMCP Việt Nam Thịnh Vượng",
                        icon = "vpbank.png",
                        type = PaymentType.Bank
                    },
                    new PaymentService()
                    {
                        name = "Momo",
                        full_name = "Ví điện tử Momo",
                        icon = "momo.png",
                        type = PaymentType.VirtualWallet
                    }
                };

                context.PaymentServices.AddRange(datas);
                context.SaveChanges();
            }

            if (!context.LoanValidateAttributes.Any())
            {
                var datas = new List<LoanValidateAttributeConfig>()
                {
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "FullName",
                        display_name = "Họ tên",
                        table_name = TableName.BorrowerProfiles,
                        column_name = nameof(BorrowerProfile.full_name),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "Gender",
                        display_name = "Giới tính",
                        table_name = TableName.BorrowerProfiles,
                        column_name = nameof(BorrowerProfile.gender),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "DateOfBirth",
                        display_name = "Ngày sinh",
                        table_name = TableName.BorrowerProfiles,
                        column_name = nameof(BorrowerProfile.date_of_birth),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "IdCardNumber",
                        display_name = "Số CMND",
                        table_name = TableName.BorrowerProfiles,
                        column_name = nameof(BorrowerProfile.id_card_number),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "Phone",
                        display_name = "Điện thoại",
                        table_name = TableName.BorrowerProfiles,
                        column_name = nameof(BorrowerProfile.phone),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "CurrentAddressId",
                        display_name = "Tạm trú",
                        table_name = TableName.Addresses,
                        column_name = nameof(BorrowerProfile.current_address_id),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "ResidentAddressId",
                        display_name = "Thường trú",
                        table_name = TableName.Addresses,
                        column_name = nameof(BorrowerProfile.resident_address_id),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "Facebook",
                        display_name = "Facebook",
                        table_name = TableName.BorrowerProfiles,
                        column_name = "facebook",
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "RelativePerson1Name",
                        display_name = "Người thân 1",
                        table_name = TableName.BorrowerRelativePersons,
                        column_name = nameof(RelativePerson.relative_person_type),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "RelativePerson1Phone",
                        display_name = "SĐT Người thân 1",
                        table_name = TableName.BorrowerRelativePersons,
                        column_name = nameof(RelativePerson.phone),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "RelativePerson2Name",
                        display_name = "Người thân 2",
                        table_name = TableName.BorrowerRelativePersons,
                        column_name = nameof(RelativePerson.relative_person_type),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "RelativePerson2Phone",
                        display_name = "SĐT Người thân 2",
                        table_name = TableName.BorrowerRelativePersons,
                        column_name = nameof(RelativePerson.phone),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "LinkedPaymentAccountId",
                        display_name = "Số tài khoản",
                        table_name = TableName.MobileLinkedPayments,
                        column_name = nameof(MobileLinkedPayment.service_account_id),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "LinkedPaymentAccountName",
                        display_name = "Tên tài khoản",
                        table_name = TableName.MobileLinkedPayments,
                        column_name = nameof(MobileLinkedPayment.service_account_name),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "Occupation",
                        display_name = "Nghề nghiệp",
                        table_name = TableName.BorrowerProfiles,
                        column_name = nameof(BorrowerProfile.occupation_position),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "OccupationPosition",
                        display_name = "Vị trí nghề nghiệp",
                        table_name = TableName.BorrowerProfiles,
                        column_name = nameof(BorrowerProfile.occupation_position),
                        create_date = DateTime.Now
                    },
                    new LoanValidateAttributeConfig()
                    {
                        attribute_name = "WorkplaceName",
                        display_name = "Công ty",
                        table_name = TableName.BorrowerProfiles,
                        column_name = nameof(BorrowerProfile.workplace_name),
                        create_date = DateTime.Now
                    },
                     new LoanValidateAttributeConfig()
                    {
                        attribute_name = "IdCardWithUserImage",
                        display_name = "Ảnh cầm CMND",
                        table_name = TableName.BorrowerProfiles,
                        column_name = nameof(BorrowerProfile.id_card_with_user_image),
                        create_date = DateTime.Now
                    },
                      new LoanValidateAttributeConfig()
                    {
                        attribute_name = "IdCardBackImage",
                        display_name = "Mặt sau CMND",
                        table_name = TableName.BorrowerProfiles,
                        column_name = nameof(BorrowerProfile.id_card_back_image),
                        create_date = DateTime.Now
                    },
                       new LoanValidateAttributeConfig()
                    {
                        attribute_name = "IdCardFrontImage",
                        display_name = "Mặt trước CMND",
                        table_name = TableName.BorrowerProfiles,
                        column_name = nameof(BorrowerProfile.id_card_front_image),
                        create_date = DateTime.Now
                    },
                };

                context.LoanValidateAttributes.AddRange(datas);
                context.SaveChanges();
            }
        }
    }
}
