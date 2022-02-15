using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Entities.Base;
using System;

namespace P2PLending.Web.Entities.Entities.Borrower
{
    public partial class BorrowerProfile: BaseEntity
    {
        public int account_id { get; set; }
        public AccountMobile account_mobile { get; set; }
        public string phone { get; set; }
        public byte[] id_card_with_user_image { get; set; }
        public byte[] id_card_front_image { get; set; }
        public byte[] id_card_back_image { get; set; }
        public string full_name { get; set; }
        public string gender { get; set; }
        public DateTime date_of_birth { get; set; }
        public int resident_address_id { get; set; }
        public int current_address_id { get; set; }
        public string owned_type { get; set; }
        public string id_card_number { get; set; }
        public DateTime id_card_issue_date { get; set; }
        public string id_card_issue_by { get; set; }
        public int marital_status_id { get; set; }
        public string occupation { get; set; }
        public string occupation_position { get; set; }
        public string workplace_name { get; set; }
        public int workplace_address_id { get; set; }
        public int current_work_address_id { get; set; }
        public string workplace_phone { get; set; }
        public int income { get; set; }
    }
}
