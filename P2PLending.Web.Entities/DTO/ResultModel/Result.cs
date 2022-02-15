namespace P2PLending.Web.Entities.DTO.ResultModel
{
    public class Result<T>: BaseResult
    {
        public T Data { get; set; }
        public int TotalRows { get; set; }
    }
}
