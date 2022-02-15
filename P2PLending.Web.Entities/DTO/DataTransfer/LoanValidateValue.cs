namespace P2PLending.Web.Entities.DTO.DataTransfer
{
    public class LoanValidateValue<T>
    {
        public string AttributeName { get; set; }
        public T Value { get; set; }
        public bool IsValidate { get; set; }
        public string Reason { get; set; }
    }
}
