namespace P2PLending.Web.Entities.DTO.ResultModel
{
    public class RestOutput<T>
    {
        public T Data { get; set; }
        public int ResultCode { get; set; }
        public int TotalRows { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
