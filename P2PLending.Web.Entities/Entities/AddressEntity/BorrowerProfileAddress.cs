using P2PLending.Web.Entities.Entities.Base;
using P2PLending.Web.Entities.Entities.Borrower;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.AddressEntity
{
    [Table("borrower_profile_address")]
    public class BorrowerProfileAddress : BaseEntity
    {
        [ForeignKey("borrower_profiles")]
        public int borrower_profile_id { get; set; }
        public int city_id { get; set; }
        public int county_id { get; set; }
        public int district_id { get; set; }
        [NotMapped]
        public string city_name { get; set; }
        [NotMapped]
        public string county_name { get; set; }
        [NotMapped]
        public string district_name { get; set; }
        public string address_detail { get; set; }
        [NotMapped]
        public string full_address { get
            {
                return $"{address_detail}, {district_name}, {county_name}, {city_name}";
            }
        }

        public virtual BorrowerProfile borrower_profile { get; set; }
    }
}
