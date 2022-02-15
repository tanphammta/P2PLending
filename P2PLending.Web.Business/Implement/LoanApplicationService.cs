using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;
using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Entities.AddressEntity;
using P2PLending.Web.Entities.Entities.Borrower;
using P2PLending.Web.Entities.Entities.Loans;
using P2PLending.Web.Entities.Entities.MasterData;
using P2PLending.Web.Entities.Entities.Relative;
using P2PLending.Web.Helper.Constants;
using P2PLending.Web.Helper.Extension;
using P2PLending.Web.Helper.Helpers;
using P2PLending.Web.Repositories.Repositories.Interface;
using P2PLending.Web.Repositories.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2PLending.Web.Business.Implement
{
    public class LoanApplicationService: ILoanApplicationService
    {
        private ILoanProductRepository _loanProductRepository;
        private ILoanTypeAdditionalInfoRepository _loanProductAdditionalInfoRepository;
        private ILoanApplicationRepository _loanApplicationRepository;
        private ILoanApplicationAdditionalInfoRepository _loanApplicationAdditionalInfoRepository;
        private IBorrowerRelativePersonRepository _borrowerRelativePersonRepository;
        private IMapper _mapper;
        private IMasterDataRepository _masterDataRepository;
        private IUnitOfWork _unitOfWork;
        private IAddressRepository _addressRepository;
        private IBorrowerProfileRepository _borrowerProfileRepository;
        private ILoanApplicationValidateAttributeRepository _loanApplicationValidateAttributeRepository;
        private IMobileLinkedPaymentRepository _linkedPaymentRepository;
        public LoanApplicationService(
            ILoanProductRepository loanProductRepository, 
            ILoanTypeAdditionalInfoRepository loanTypeAdditionalInfoRepository,
            ILoanApplicationRepository loanApplicationRepository,
            ILoanApplicationAdditionalInfoRepository loanApplicationAdditionalInfoRepository,
            IBorrowerRelativePersonRepository borrowerRelativePersonRepository,
            IBorrowerProfileRepository borrowerProfileRepository,
            IMapper mapper,
            IMasterDataRepository masterDataRepository,
            IUnitOfWork unitOfWork,
            IAddressRepository addressRepository,
            ILoanApplicationValidateAttributeRepository loanApplicationValidateAttributeRepository,
            IMobileLinkedPaymentRepository linkedPaymentRepository)
        {
            _loanProductRepository = loanProductRepository;
            _loanProductAdditionalInfoRepository = loanTypeAdditionalInfoRepository;
            _loanApplicationAdditionalInfoRepository = loanApplicationAdditionalInfoRepository;
            _loanApplicationRepository = loanApplicationRepository;
            _borrowerProfileRepository = borrowerProfileRepository;
            _mapper = mapper;
            _masterDataRepository = masterDataRepository;
            _unitOfWork = unitOfWork;
            _addressRepository = addressRepository;
            _borrowerRelativePersonRepository = borrowerRelativePersonRepository;
            _loanApplicationValidateAttributeRepository = loanApplicationValidateAttributeRepository;
            _linkedPaymentRepository = linkedPaymentRepository;
        }

        public List<LoanProductDTO> GetLoanProducts()
        {
            var loanProductAdditionalInfos = _loanProductAdditionalInfoRepository.Queryable().ToList();
            var loanProducts = _loanProductRepository.Queryable().ToList();

            var loanTypeAdditionalInfoDTO = _mapper.Map<List<LoanProductAdditionalInfo>, List<LoanTypeAdditionalInfoDTO>>(loanProductAdditionalInfos);

            var loanProductDTO = loanProducts.Select(type =>
            {
                var dto = _mapper.Map<LoanProduct, LoanProductDTO>(type);
                dto.AdditionalInfos = loanTypeAdditionalInfoDTO.Where(info => info.LoanTypeId == dto.Id).ToList();
                return dto;
            }).ToList();

            return loanProductDTO;
        }

        public async Task<LoanApplicationMasterDataDTO> GetMasterDatas(MobileUserDTO user)
        {
            var masterDatas = new LoanApplicationMasterDataDTO();
            var addressLevel1s = new List<AddressLevel1>();
            var addressLevel2s = new List<AddressLevel2>();
            var addressLevel3s = new List<AddressLevel3>();
            var creditRankConfigs = new List<CreditRankConfig>();
            var loadDataTasks = new Task[]
            {
                Task.Run(async () => masterDatas.Occupations = await _masterDataRepository.ListOccupations()),
                Task.Run(async () => masterDatas.MaritalStatuses = await _masterDataRepository.ListMaritalStatuses()),
                Task.Run(async () => addressLevel1s = await _masterDataRepository.ListAddressLevel1()),
                Task.Run(async () => addressLevel2s = await _masterDataRepository.ListAddressLevel2()),
                Task.Run(async () => addressLevel3s = await _masterDataRepository.ListAddressLevel3()),
                Task.Run(async () => masterDatas.RelativePersonTypes = await _masterDataRepository.ListRelativePersonTypes()),
                Task.Run(async () => creditRankConfigs = await _masterDataRepository.ListCreditRankConfigs()),
            };

            try
            {
                await Task.WhenAll(loadDataTasks);

                foreach(var level1 in addressLevel1s)
                {
                    var level2s = addressLevel2s.Where(lvl2 => lvl2.level1_id == level1.level1_id).ToList();

                    foreach(var level2 in level2s)
                    {
                        var level3s = addressLevel3s.Where(lvl3 => lvl3.level2_id == level2.level2_id).ToList();
                        level2.level3s = level3s;
                    }

                    level1.level2s = level2s;
                }

                masterDatas.Addresses = new AddressList()
                {
                    Data = addressLevel1s
                };

                var userCreditPoint = GetCreditPoint(user.AccountId);
                masterDatas.UserCreditRank = GetCreditRank(userCreditPoint, creditRankConfigs);

                masterDatas.CreditRanks = _mapper.Map<List<CreditRankConfigDTO>>(creditRankConfigs);
            }
            catch
            {
                throw;
            }

            return masterDatas;
        }

        public LoanApplicationResult CreateLoanApplication(LoanApplicationRequest request, MobileUserDTO user)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var currentAddress = new Address()
                {
                    level1_id = request.CurrentAddress.Level1_Id,
                    level2_id = request.CurrentAddress.Level2_Id,
                    level3_id = request.CurrentAddress.Level3_Id,
                    address_detail = request.CurrentAddress.Detail,
                    create_by = user.Phone,
                    create_date = DateTime.Now
                };
                _addressRepository.Add(currentAddress);
                _unitOfWork.SaveChanges();

                var residentAddress = new Address()
                {
                    level1_id = request.ResidentAddress.Level1_Id,
                    level2_id = request.ResidentAddress.Level2_Id,
                    level3_id = request.ResidentAddress.Level3_Id,
                    address_detail = request.ResidentAddress.Detail,
                    create_by = user.Phone,
                    create_date = DateTime.Now
                };

                _addressRepository.Add(residentAddress);
                _unitOfWork.SaveChanges();

                var workplaceAddress = new Address()
                {
                    level1_id = request.Occupation.OccupationAddress.Level1_Id,
                    level2_id = request.Occupation.OccupationAddress.Level2_Id,
                    level3_id = request.Occupation.OccupationAddress.Level3_Id,
                    address_detail = request.Occupation.OccupationAddress.Detail,
                    create_by = user.Phone,
                    create_date = DateTime.Now
                };
                _addressRepository.Add(workplaceAddress);
                _unitOfWork.SaveChanges();

                var borrowerProfile = new BorrowerProfile()
                {
                    account_id = user.AccountId,
                    create_by = user.Phone,
                    create_date = DateTime.Now,
                    resident_address_id = residentAddress.id,
                    current_address_id = currentAddress.id,
                    owned_type = request.OwnedType,
                    date_of_birth = request.Profile.DateOfBirth.ToDateTime(),
                    full_name = request.Profile.FullName,
                    gender = request.Profile.Gender,
                    id_card_back_image = Convert.FromBase64String(request.IdCardBackImage),
                    id_card_front_image = Convert.FromBase64String(request.IdCardFrontImage),
                    id_card_with_user_image = Convert.FromBase64String(request.IdCardWithUserImage),
                    id_card_issue_by = request.Profile.IdCardIssuedBy,
                    id_card_issue_date = request.Profile.IdCardIssuedDate.ToDateTime(),
                    id_card_number = request.Profile.IdCardNumber,
                    marital_status_id = request.MaritalStatusId,
                    occupation = request.Occupation.OccupationName,
                    workplace_address_id = workplaceAddress.id,
                    workplace_name = request.Occupation.WorkplaceName,
                    workplace_phone = request.Occupation.WorkplacePhone,
                    income = request.Occupation.MonthlyIncome,
                    phone = user.Phone
                };

                _borrowerProfileRepository.Add(borrowerProfile);
                _unitOfWork.SaveChanges();

                var relativePerson1 = new RelativePerson()
                {
                    borrower_profile_id = borrowerProfile.id,
                    create_by = user.Phone,
                    create_date = DateTime.Now,
                    type_id = request.RelativePerson1.TypeId,
                    phone = request.RelativePerson1.Phone,
                    name = request.RelativePerson1.Name,
                    no = 1,
                };
                _borrowerRelativePersonRepository.Add(relativePerson1);

                var relativePerson2 = new RelativePerson()
                {
                    borrower_profile_id = borrowerProfile.id,
                    create_by = user.Phone,
                    create_date = DateTime.Now,
                    type_id = request.RelativePerson2.TypeId,
                    phone = request.RelativePerson2.Phone,
                    name = request.RelativePerson2.Name,
                    no = 2
                };
                _borrowerRelativePersonRepository.Add(relativePerson2);
                _unitOfWork.SaveChanges();

                var loanApplication = new LoanApplication()
                {
                    account_id = user.AccountId,
                    borrower_linked_payment_service_id = request.LinkedPaymentId,
                    borrower_profile_id = borrowerProfile.id,
                    create_by = user.Phone,
                    create_date = DateTime.Now,
                    loan_amount = request.LoanAmount,
                    loan_duration = request.LoanDuration,
                    loan_product_id = request.LoanProductId,
                    interest_rate = request.ExpectedInterestRate,
                    fees = request.ServiceFees,
                    status = LoanStatus.Submitted
                };
                _loanApplicationRepository.Add(loanApplication);
                _unitOfWork.SaveChanges();
                var loanProductAdditionalInfos = _loanProductAdditionalInfoRepository
                    .Queryable()
                    .Where(info => info.loan_product_id == request.LoanProductId)
                    .ToList();
                var additionalInfos = new List<LoanApplicationAdditionalInfo>();
                foreach (var aInfo in request.AdditionalInfos)
                {
                    var additionalInfoModel = loanProductAdditionalInfos.FirstOrDefault(a => a.id == aInfo.LoanProductAdditionalInfoId);
                    var additionalInfo = new LoanApplicationAdditionalInfo()
                    {
                        create_by = user.Phone,
                        create_date = DateTime.Now,
                        loan_application_id = loanApplication.id,
                        loan_product_additional_info_id = aInfo.LoanProductAdditionalInfoId,
                        name = additionalInfoModel?.info_name,
                        format = additionalInfoModel?.info_format,
                    };

                    if(additionalInfo.format == "Text")
                    {
                        additionalInfo.value = aInfo.value;
                        additionalInfo.binary_value = null;
                    }
                    else
                    {
                        additionalInfo.binary_value = Convert.FromBase64String(aInfo.value);
                        additionalInfo.value = "";
                    }

                    additionalInfos.Add(additionalInfo);
                }
                _loanApplicationAdditionalInfoRepository.AddRange(additionalInfos);
                _unitOfWork.SaveChanges();
                // add validate attribute column by config
                var validateConfigs = _masterDataRepository.ListValidateAttributes();
                var listValidates = new List<LoanApplicationValidateAttribute>();
                foreach (var attribute in validateConfigs)
                {
                    var applicationValidateAttribute = new LoanApplicationValidateAttribute()
                    {
                        attribute_name = attribute.attribute_name,
                        column_name = attribute.column_name,
                        table_name = attribute.table_name,
                        is_validate = true,
                        create_by = user.Phone,
                        create_date = DateTime.Now,
                        loan_application_id = loanApplication.id
                    };

                    if (attribute.table_name == TableName.BorrowerProfiles)
                    {
                        applicationValidateAttribute.reference_id = borrowerProfile.id;
                    }
                    else if (attribute.table_name == TableName.MobileLinkedPayments)
                    {
                        applicationValidateAttribute.reference_id = loanApplication.borrower_linked_payment_service_id;
                    }
                    else if (attribute.column_name == nameof(BorrowerProfile.current_address_id))
                    {
                        applicationValidateAttribute.reference_id = currentAddress.id;
                    }
                    else if (attribute.column_name == nameof(BorrowerProfile.resident_address_id))
                    {
                        applicationValidateAttribute.reference_id = residentAddress.id;
                    }
                    else if (attribute.attribute_name.Contains("RelativePerson1"))
                    {
                        applicationValidateAttribute.reference_id = relativePerson1.id;
                    }
                    else if (attribute.attribute_name.Contains("RelativePerson2"))
                    {
                        applicationValidateAttribute.reference_id = relativePerson2.id;
                    }

                    listValidates.Add(applicationValidateAttribute);
                }
                // add validate attribute column by additional infos
                foreach (var info in additionalInfos)
                {
                    var applicationValidateAttribute = new LoanApplicationValidateAttribute()
                    {
                        attribute_name = info.value,
                        column_name = nameof(info.value),
                        table_name = TableName.LoanApplicationAdditionalInfos,
                        is_validate = true,
                        create_by = user.Phone,
                        create_date = DateTime.Now,
                        loan_application_id = loanApplication.id,
                        reference_id = info.id
                    };

                    listValidates.Add(applicationValidateAttribute);
                }
                _loanApplicationValidateAttributeRepository.AddRange(listValidates);
                _unitOfWork.SaveChanges();

                var product = _loanProductRepository.AsNoTracking().FirstOrDefault(p => p.id == loanApplication.loan_product_id);

                var data = new LoanApplicationDTO()
                {
                    Id = loanApplication.id,
                    LoanAmount = loanApplication.loan_amount,
                    LoanDuration = loanApplication.loan_duration,
                    DurationUnit = product.duration_unit,
                    LoanProduct = product.name,
                    Status = loanApplication.status,
                    StatusDisplay = LoanApplicationHelper.StatusMappings.GetValueOrDefault(loanApplication.status),
                    InterestRate = loanApplication.interest_rate,
                    ServiceFees = loanApplication.fees,
                    Phone = borrowerProfile.phone,
                    CreateDate = loanApplication.create_date.Value.ToUnixSeconds()
                };

                _unitOfWork.Commit();
                return new LoanApplicationResult()
                {
                    IsSuccess = true,
                    Data = data
                };
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public LoanApplicationResult UpdateLoanApplication(LoanApplicationRequest request, MobileUserDTO user)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var loanApplication = _loanApplicationRepository.Get(request.Id);
                loanApplication.borrower_linked_payment_service_id = request.LinkedPaymentId;
                loanApplication.loan_amount = request.LoanAmount;
                loanApplication.loan_duration = request.LoanDuration;
                loanApplication.loan_product_id = request.LoanProductId;
                loanApplication.interest_rate = request.ExpectedInterestRate;
                loanApplication.fees = request.ServiceFees;
                loanApplication.status = LoanStatus.Submitted;
                loanApplication.update_date = DateTime.Now;
                loanApplication.update_by = user.Phone;
                _loanApplicationRepository.Update(loanApplication);
                _unitOfWork.SaveChanges();

                var borrowerProfile = _borrowerProfileRepository.Get(loanApplication.borrower_profile_id);
                borrowerProfile.owned_type = request.OwnedType;
                borrowerProfile.date_of_birth = request.Profile.DateOfBirth.ToDateTime();
                borrowerProfile.full_name = request.Profile.FullName;
                borrowerProfile.gender = request.Profile.Gender;
                borrowerProfile.id_card_back_image = Convert.FromBase64String(request.IdCardBackImage);
                borrowerProfile.id_card_front_image = Convert.FromBase64String(request.IdCardFrontImage);
                borrowerProfile.id_card_with_user_image = Convert.FromBase64String(request.IdCardWithUserImage);
                borrowerProfile.id_card_issue_by = request.Profile.IdCardIssuedBy;
                borrowerProfile.id_card_issue_date = request.Profile.IdCardIssuedDate.ToDateTime();
                borrowerProfile.id_card_number = request.Profile.IdCardNumber;
                borrowerProfile.marital_status_id = request.MaritalStatusId;
                borrowerProfile.occupation = request.Occupation.OccupationName;
                borrowerProfile.workplace_name = request.Occupation.WorkplaceName;
                borrowerProfile.workplace_phone = request.Occupation.WorkplacePhone;
                borrowerProfile.income = request.Occupation.MonthlyIncome;
                borrowerProfile.update_date = DateTime.Now;
                borrowerProfile.update_by = user.Phone;
                _borrowerProfileRepository.Update(borrowerProfile);
                _unitOfWork.SaveChanges();

                var currentAddress = _addressRepository.Get(borrowerProfile.current_address_id);
                currentAddress.level1_id = request.CurrentAddress.Level1_Id;
                currentAddress.level2_id = request.CurrentAddress.Level2_Id;
                currentAddress.level3_id = request.CurrentAddress.Level3_Id;
                currentAddress.update_date = DateTime.Now;
                currentAddress.update_by = user.Phone;
                currentAddress.address_detail = request.CurrentAddress.Detail;
                _addressRepository.Update(currentAddress);

                var residentAddress = _addressRepository.Get(borrowerProfile.resident_address_id);
                residentAddress.level1_id = request.ResidentAddress.Level1_Id;
                residentAddress.level2_id = request.ResidentAddress.Level2_Id;
                residentAddress.level3_id = request.ResidentAddress.Level3_Id;
                residentAddress.address_detail = request.ResidentAddress.Detail;
                residentAddress.update_date = DateTime.Now;
                residentAddress.update_by = user.Phone;
                _addressRepository.Update(residentAddress);

                var workplaceAddress = _addressRepository.Get(borrowerProfile.workplace_address_id);
                workplaceAddress.level1_id = request.Occupation.OccupationAddress.Level1_Id;
                workplaceAddress.level2_id = request.Occupation.OccupationAddress.Level2_Id;
                workplaceAddress.level3_id = request.Occupation.OccupationAddress.Level3_Id;
                workplaceAddress.address_detail = request.Occupation.OccupationAddress.Detail;
                workplaceAddress.update_date = DateTime.Now;
                workplaceAddress.update_by = user.Phone;
                _addressRepository.Update(workplaceAddress);
                _unitOfWork.SaveChanges();

                var relativePersons = _borrowerRelativePersonRepository.AsNoTracking().Where(p => p.borrower_profile_id == borrowerProfile.id).ToList();

                var relativePerson1 = relativePersons.FirstOrDefault(p => p.no == 1);
                relativePerson1.type_id = request.RelativePerson1.TypeId;
                relativePerson1.phone = request.RelativePerson1.Phone;
                relativePerson1.name = request.RelativePerson1.Name;
                relativePerson1.update_date = DateTime.Now;
                relativePerson1.update_by = user.Phone;
                _borrowerRelativePersonRepository.Update(relativePerson1);

                var relativePerson2 = relativePersons.FirstOrDefault(p => p.no == 2);
                relativePerson2.type_id = request.RelativePerson2.TypeId;
                relativePerson2.phone = request.RelativePerson2.Phone;
                relativePerson2.name = request.RelativePerson2.Name;
                relativePerson2.update_date = DateTime.Now;
                relativePerson2.update_by = user.Phone;
                _borrowerRelativePersonRepository.Update(relativePerson2);

                var loanProductAdditionalInfos = _loanProductAdditionalInfoRepository
                    .Queryable()
                    .Where(info => info.loan_product_id == request.LoanProductId)
                    .ToList();
                var loanAdditionalInfos = _loanApplicationAdditionalInfoRepository.DeleteWhere(p => p.loan_application_id == loanApplication.id);
                var additionalInfos = new List<LoanApplicationAdditionalInfo>();

                if (request.AdditionalInfos != null)
                {
                    foreach (var aInfo in request.AdditionalInfos)
                    {
                        var additionalInfoModel = loanProductAdditionalInfos.FirstOrDefault(a => a.id == aInfo.LoanProductAdditionalInfoId);
                        var additionalInfo = new LoanApplicationAdditionalInfo()
                        {
                            update_date = DateTime.Now,
                            update_by = user.Phone,
                            loan_application_id = loanApplication.id,
                            loan_product_additional_info_id = aInfo.LoanProductAdditionalInfoId,
                            name = additionalInfoModel?.info_name,
                            format = additionalInfoModel?.info_format,
                            value = aInfo.value
                        };
                        if (additionalInfo.format == "Text")
                        {
                            additionalInfo.value = aInfo.value;
                        }
                        else
                        {
                            additionalInfo.binary_value = Convert.FromBase64String(aInfo.value);
                        }


                        additionalInfos.Add(additionalInfo);
                    }
                }
                
                _loanApplicationAdditionalInfoRepository.AddRange(additionalInfos);
                _unitOfWork.SaveChanges();

                // add validate attribute column by config
                var validateConfigs = _masterDataRepository.ListValidateAttributes();
                var listValidates = new List<LoanApplicationValidateAttribute>();

                _loanApplicationValidateAttributeRepository.DeleteWhere(p => p.loan_application_id == loanApplication.id && p.table_name == TableName.LoanApplicationAdditionalInfos);
                _unitOfWork.SaveChanges();
                var validateAttributes = _loanApplicationValidateAttributeRepository.Queryable().Where(p => p.loan_application_id == loanApplication.id).ToList();
                foreach(var attribute in validateAttributes)
                {
                    attribute.is_validate = true;
                    attribute.update_date = DateTime.Now;
                    attribute.update_by = user.Phone;
                    attribute.reason = String.Empty;
                }
                _loanApplicationValidateAttributeRepository.UpdateRange(validateAttributes);
                // add validate attribute column by additional infos
                foreach (var info in additionalInfos)
                {
                    var applicationValidateAttribute = new LoanApplicationValidateAttribute()
                    {
                        attribute_name = info.value,
                        column_name = nameof(info.value),
                        table_name = TableName.LoanApplicationAdditionalInfos,
                        is_validate = true,
                        create_date = DateTime.Now,
                        create_by = user.Phone,
                        loan_application_id = loanApplication.id,
                        reference_id = info.id
                    };

                    listValidates.Add(applicationValidateAttribute);
                }
                _loanApplicationValidateAttributeRepository.AddRange(listValidates);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
                var product = _loanProductRepository.AsNoTracking().FirstOrDefault(p => p.id == loanApplication.loan_product_id);

                var data = new LoanApplicationDTO()
                {
                    Id = loanApplication.id,
                    LoanAmount = loanApplication.loan_amount,
                    LoanDuration = loanApplication.loan_duration,
                    DurationUnit = product.duration_unit,
                    LoanProduct = product.name,
                    Status = loanApplication.status,
                    StatusDisplay = LoanApplicationHelper.StatusMappings.GetValueOrDefault(loanApplication.status),
                    InterestRate = loanApplication.interest_rate,
                    ServiceFees = loanApplication.fees,
                    Phone = borrowerProfile.phone,
                    CreateDate = loanApplication.create_date.Value.ToUnixSeconds()
                };

                return new LoanApplicationResult()
                {
                    IsSuccess = true,
                    Data = data
                };
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public bool BorrowerSubmitLoanApplication(int loanApplicationId)
        {

            return true;
        }

        public List<LoanApplicationDTO> GetLoanApplicationByBorrower(int borrowerId) {
            var result = new List<LoanApplicationDTO>();
            var loanProducts = _loanProductRepository.AsNoTracking().ToList();
            var loans = _loanApplicationRepository.AsNoTracking().Where(loan => loan.account_id == borrowerId && loan.status != LoanStatus.Cancel);
            var bpIds = loans.Select(p => p.borrower_profile_id).Distinct().ToList();
            var borrowerProfiles = _borrowerProfileRepository.AsNoTracking().Where(b => bpIds.Contains(b.id)).ToList();
            foreach(var loan in loans)
            {
                var product = loanProducts.FirstOrDefault(p => p.id == loan.loan_product_id);
                var borrowerProfile = borrowerProfiles.FirstOrDefault(p => p.id == loan.borrower_profile_id);
                result.Add(new LoanApplicationDTO()
                {
                    Id = loan.id,
                    LoanAmount = loan.loan_amount,
                    LoanDuration = loan.loan_duration,
                    DurationUnit = product.duration_unit,
                    LoanProduct = product.name,
                    Status = loan.status,
                    StatusDisplay = LoanApplicationHelper.StatusMappings.GetValueOrDefault(loan.status),
                    InterestRate = loan.interest_rate,
                    ServiceFees = loan.fees,
                    Phone = borrowerProfile.phone,
                    CreateDate = loan.create_date.Value.ToUnixSeconds(),
                    Icon = product.icon
                });
            }

            return result;
        }

        public async Task<LoanApplicationDetail> GetLoanApplicationDetailById(int id)
        {
            var detail = new LoanApplicationDetail();
            var loan = _loanApplicationRepository.Queryable().FirstOrDefault(l => l.id == id);

            if(loan != null)
            {
                detail.Id = loan.id;
                detail.LoanProductId = loan.loan_product_id;
                // get related datas
                var validateValues = _loanApplicationValidateAttributeRepository.AsNoTracking().Where(p => p.loan_application_id == loan.id).ToList();
                var loanProduct = _loanProductRepository.AsNoTracking().FirstOrDefault(l => l.id == loan.loan_product_id);
                var borrowerProfile = _borrowerProfileRepository.AsNoTracking().FirstOrDefault(p => p.id == loan.borrower_profile_id);
                var relativePersons = _borrowerRelativePersonRepository.AsNoTracking().Where(p => p.borrower_profile_id == borrowerProfile.id).ToList();
                var relativePersonType = await _masterDataRepository.ListRelativePersonTypes();
                var linkedPayment = _linkedPaymentRepository.AsNoTracking().FirstOrDefault(p => p.id == loan.borrower_linked_payment_service_id);
                var productAdditionalInfos = _loanProductAdditionalInfoRepository.ListAdditionalInfoByProductId(loanProduct.id);

                detail.LoanProduct = loanProduct.name;
                detail.Fees = loan.fees;
                detail.InterestRate = loan.interest_rate;
                detail.ResidentAddress = _addressRepository.GetAddress(borrowerProfile.resident_address_id);
                detail.CurrentAddress = _addressRepository.GetAddress(borrowerProfile.current_address_id);
                detail.OwnedType = borrowerProfile.owned_type;
                detail.MaritalStatusId = borrowerProfile.marital_status_id;
                detail.IdCardWithUserImage = Convert.ToBase64String(borrowerProfile.id_card_with_user_image);
                detail.IdCardBackImage = Convert.ToBase64String(borrowerProfile.id_card_back_image);
                detail.IdCardFrontImage = Convert.ToBase64String(borrowerProfile.id_card_front_image);
                // Relative Persons
                foreach (var rPerson in relativePersons)
                {
                    if (rPerson.no == 1)
                    {
                        detail.RelativePerson1 = new LADRelativePerson()
                        {
                            Id = rPerson.id,
                            Name = rPerson.name,
                            Phone = rPerson.phone,
                            TypeId = rPerson.type_id,
                            Type = relativePersonType.FirstOrDefault(r => r.id == rPerson.type_id).type_name
                        };
                    }
                    else
                    {
                        detail.RelativePerson2 = new LADRelativePerson()
                        {
                            Id = rPerson.id,
                            Name = rPerson.name,
                            Phone = rPerson.phone,
                            TypeId = rPerson.type_id,
                            Type = relativePersonType.FirstOrDefault(r => r.id == rPerson.type_id).type_name
                        };
                    }
                }
                // Borrower Profile
                detail.Profile = new LADBorrowerProfile()
                {
                    Id = borrowerProfile.id,
                    DateOfBirth = borrowerProfile.date_of_birth.ToUnixSeconds(),
                    FullName = borrowerProfile.full_name,
                    Gender = borrowerProfile.gender,
                    IdCardNumber = borrowerProfile.id_card_number,
                    IdCardIssuedBy = borrowerProfile.id_card_issue_by,
                    IdCardIssuedDate = borrowerProfile.id_card_issue_date.ToUnixSeconds(),
                    Phone = borrowerProfile.phone
                };
                // Occupation
                detail.Occupation = new LADOccupation()
                {
                    OccupationName = borrowerProfile.occupation,
                    OccupationPosition = borrowerProfile.occupation_position,
                    MonthlyIncome = borrowerProfile.income,
                    WorkplaceName = borrowerProfile.workplace_name,
                    WorkplacePhone = borrowerProfile.workplace_phone,
                    OccupationAddress = _addressRepository.GetAddress(borrowerProfile.workplace_address_id)
                };

                // Linked Payment
                if(linkedPayment != null)
                {
                    var paymentService = _masterDataRepository.GetPaymentService(linkedPayment.service_id);
                    detail.LinkedPayment = new LADLinkedPayment()
                    {
                        Id = linkedPayment.id,
                        AccountId = linkedPayment.service_account_id,
                        AccountName = linkedPayment.service_account_name,
                        ServiceName = paymentService.name
                    };
                }
                
                // AdditionalInfos
                detail.AdditionalInfos = new List<LADAdditionalInfo>();
                var addInfos = _loanApplicationAdditionalInfoRepository.ListAdditionalInfoByApplicationId(loan.id);
                if (addInfos.Any())
                {
                    foreach (var info in addInfos)
                    {
                        var productInfo = productAdditionalInfos.FirstOrDefault(p => p.id == info.loan_product_additional_info_id);
                        detail.AdditionalInfos.Add(new LADAdditionalInfo()
                        {
                            Id = info.id,
                            Value = info.value,
                            BinaryValue = info.binary_value,
                            LoanProductAdditionalInfoId = info.loan_product_additional_info_id,
                            InfoName = productInfo.info_name,
                            InfoFormat = productInfo.info_format
                        });
                    }
                }
                
                // Validating info
                var invalidateDatas = _loanApplicationValidateAttributeRepository.ListInvalidateAttributeByLoanApplication(loan.id);
                if (invalidateDatas.Any())
                {
                    detail.InvalidateDatas = _mapper.Map<List<LoanApplicationValidateAttributeDTO>>(invalidateDatas);
                }
            }

            return detail;
        }

        private int GetCreditPoint(int accountId)
        {
            return 400 - accountId;
        }

        private CreditRankConfigDTO GetCreditRank(int creditPoints, List<CreditRankConfig> creditRankConfigs)
        {
            var config = creditRankConfigs.FirstOrDefault(c => (c.min_score == null || creditPoints >= c.min_score) && (c.max_score == null || creditPoints <= c.max_score));

            return _mapper.Map<CreditRankConfigDTO>(config);
        }
        
        private LoanApplicationValidateAttribute GetValidateValue(string attributeName, string tableName, List<LoanApplicationValidateAttribute> validateValues)
        {
            return validateValues.FirstOrDefault(value => value.attribute_name == attributeName && (string.IsNullOrEmpty(tableName) || value.table_name == tableName));
        }

        public BaseResult UpdateLoanApplicationStatus(int id, string status)
        {
            var loan = _loanApplicationRepository.Get(id);
            if (loan != null)
            {
                loan.status = status;
                _unitOfWork.SaveChanges();
                return new BaseResult()
                {
                    IsSuccess = true
                };
            }

            return new BaseResult()
            {
                IsSuccess = false,
                Message = "Không tìm thấy đơn vay"
            };
        }

        public BaseResult UpdateLoanValidateAttributes(UpdateLoanValidateRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var loan = _loanApplicationRepository.Get(request.Id);

                if (loan != null)
                {
                    if (request.InvalidateAttributes.Any())
                    {
                        loan.status = LoanStatus.UpdateRequired;
                        _unitOfWork.SaveChanges();

                        var validateAttributes = _loanApplicationValidateAttributeRepository.Queryable().Where(p => p.loan_application_id == request.Id).ToList();

                        foreach (var attribute in request.InvalidateAttributes)
                        {
                            var validateAttr = validateAttributes.FirstOrDefault(p => p.attribute_name == attribute.AttributeName);
                            if (validateAttr != null)
                            {
                                validateAttr.is_validate = false;
                                validateAttr.reason = attribute.Reason;
                                validateAttr.update_date = DateTime.Now;
                            }
                        }

                        _loanApplicationValidateAttributeRepository.UpdateRange(validateAttributes);
                        _unitOfWork.SaveChanges();
                    }
                    _unitOfWork.Commit();
                    return new BaseResult()
                    {
                        IsSuccess = true
                    };
                }

                return new BaseResult()
                {
                    IsSuccess = false,
                    Message = "Không tìm thấy đơn vay"
                };
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public List<LoanApplicationValidateAttributeDTO> GetLoanApplicationValidateAttributeDTOs(int id)
        {
            var validateAttributes = _loanApplicationValidateAttributeRepository.AsNoTracking().Where(p => p.loan_application_id == id).ToList();
            return _mapper.Map<List<LoanApplicationValidateAttributeDTO>>(validateAttributes);
        }
    }
}