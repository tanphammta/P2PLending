using System.Text.Json;

namespace P2PLending.Web.Entities.DTO.ResultModel
{
    public class ErrorDetail
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string MessageCode { get; set; }
        public string InnerException { get; set; }
        public bool IsSuccess { get; set; } = false;
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
