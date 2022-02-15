using P2PLending.Web.Entities.DTO.ResultModel;
using P2PLending.Web.Helper.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace P2PLending.Web.Helper.Helpers
{
    public static class PhoneHelper
    {
        public static readonly string[] OldPhonePrefix = {
            //Viettel
            "0162",
            "0163",
            "0164",
            "0165",
            "0166",
            "0167",
            "0168",
            "0169",
            //Mobifone
            "0120",
            "0121",
            "0122",
            "0126",
            "0128",
            //Vinaphone
            "0123",
            "0124",
            "0125",
            "0127",
            "0129",
            //Vietnammobile
            "0186",
            "0188",
            //GMobile
            "0199"
        };
        public static BaseResult IsCorrectPhoneNumberFormat(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return new BaseResult()
                {
                    IsSuccess = false,
                    Message = BorrowerAppMessage.MSG02,
                    MessageCode = nameof(BorrowerAppMessage.MSG02)
                };
            }
            else
            {
                phoneNumber = phoneNumber.Trim();
                // check phone string 10 or 11 characters long
                if (phoneNumber.Length < 10 || phoneNumber.Length > 11)
                {
                    return new BaseResult()
                    {
                        IsSuccess = false,
                        Message = BorrowerAppMessage.MSG03,
                        MessageCode = nameof(BorrowerAppMessage.MSG03)
                    };
                }
                // check phone string all number and start with 0
                var allNumberRegex = @"^0\d+$";
                if (!Regex.IsMatch(phoneNumber, allNumberRegex))
                {
                    return new BaseResult()
                    {
                        IsSuccess = false,
                        Message = BorrowerAppMessage.MSG03,
                        MessageCode = nameof(BorrowerAppMessage.MSG03)
                    };
                }
                // check phone string use old format
                for (int i = 0; i < OldPhonePrefix.Length; i++)
                {
                    var regex = $@"^{OldPhonePrefix[i]}\d+$";
                    if(Regex.IsMatch(phoneNumber, regex))
                    {
                        return new BaseResult()
                        {
                            IsSuccess = false,
                            Message = BorrowerAppMessage.MSG03,
                            MessageCode = nameof(BorrowerAppMessage.MSG03)
                        };
                    }
                }
            }

            return new BaseResult()
            {
                IsSuccess = true,
            };
        }
    }
}
