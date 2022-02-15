using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.DataTransfer
{
    public class AddressDTO
    {
        public string Level1Id { get; set; }
        public string Level1 { get; set; }
        public string Level2Id { get; set; }
        public string Level2 { get; set; }
        public string Level3Id { get; set; }
        public string Level3 { get; set; }
        public string Detail { get; set; }
        public string FullAddress
        {
            get
            {
                return string.Concat(Detail, ", ", Level3, ", ", Level2, ", ", Level1);
            }
        }
    }
}
